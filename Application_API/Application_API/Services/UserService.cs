using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Application_API.Data;
using Application_API.Data.Helpers;
using Application_API.Data.Static;
using Application_API.Exceptions;
using Application_API.Models;
using Application_API.Models.Dto;
using Application_API.Services.Interfaces;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Application_API.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext _context;
        private IEmailService _emailService;

        public UserService(ApplicationDbContext context, IEmailService emailService) {
            _context = context; 
            _emailService = emailService;
        }

        public async Task<TokenApiDto> Authenticate(User userObj)
        {
            if (userObj == null)
            {
                throw new NotFoundException("User was not found!");
            }

            var user = await _context.Users.
                FirstOrDefaultAsync(x => x.UserName == userObj.UserName);

            if (user == null)
            {
                throw new NotFoundException("User was not found!");
            }


            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                throw new IncorrectPasswordException("Password is incorrect!");
            }

            user.Token = CreateJWT(user);
            var newAccessToken = user.Token;
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
            await _context.SaveChangesAsync();

            return new TokenApiDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };
        }

        public async Task<string> RegisterUser(User userObj)
        {
            if (userObj == null)
            {
                throw new NotFoundException("User was not found!");
            }

            if (await CheckUserNameExistAsync(userObj.UserName))
            {
                throw new AlreadyExistsException("Username already exists!");
            }

            if (await CheckEmailExistAsync(userObj.Email))
            {
                throw new AlreadyExistsException("E-mail already exists!");
            }

            var pass = CheckPasswordStrength(userObj.Password);
            if (!string.IsNullOrEmpty(pass))
            {
                throw new IncorrectPasswordException(pass);
            }

            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";
            userObj.RefreshToken = "";

            await _context.Users.AddAsync(userObj);
            await _context.SaveChangesAsync();

            return ("User added successfully!");
            
        }

        public async Task<TokenApiDto> Refresh(TokenApiDto tokenApiDto)
        {
            if (string.IsNullOrEmpty(tokenApiDto.AccessToken) || string.IsNullOrEmpty(tokenApiDto.RefreshToken))
            {
                throw new NotFoundException("Tokens are missing or empty.");
            }

            if (tokenApiDto is null)
            {
                throw new InvalidRequestException("Invalid Client Request");
            }
            string AccessToken = tokenApiDto.AccessToken;
            string RefreshToken = tokenApiDto.RefreshToken;
            var principal = GetPrincipalFromExpiredToken(AccessToken);
            var username = principal.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user is null || user.RefreshToken != RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new InvalidRequestException("Invalid Request");
            }
            var newAccessToken = CreateJWT(user);
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();
            return new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };
        }

        public async Task<string> SendEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Email == email);
            if (user is null)
            {
                throw new NotFoundException("Email Does Not exist");
            }
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            var emailModel = new Email(email, "Reset Password", EmailBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return ("Email Sent!");

        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public Task<bool> CheckUserNameExistAsync(string userName)
    => _context.Users.AnyAsync(x => x.UserName == userName);

        public Task<bool> CheckEmailExistAsync(string email)
            => _context.Users.AnyAsync(x => x.Email == email);

        public string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();
            if (password.Length < 8)
            {
                sb.Append("Minimum password length should be 8!"+Environment.NewLine);
            }
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
            {
                sb.Append("Password should be AlphaNumeric!"+Environment.NewLine);
            }
            if (!Regex.IsMatch(password, "[<,@,#,,%,(,{,},!,?]"))
            {
                sb.Append("Password should contain special characters "+Environment.NewLine);
            }

            return sb.ToString();
        }

        public string CreateJWT(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysecret.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, $"{user.UserName}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(5),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);

        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("veryverysecret.....");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null  || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is invalid token");
            return principal;
        }

        public string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);
            var tokenInUser = _context.Users
                .Any(a => a.RefreshToken == refreshToken);
            if (tokenInUser)
            {
                return CreateRefreshToken();
            }
            return refreshToken;
        }

        public async Task<string> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == resetPasswordDto.Email);
            if (user is null)
            {
                throw new NotFoundException("Email does not exist!");
            }
            var tokenCode = user.ResetPasswordToken;
            DateTime? emailTokenExpiry = user.ResetPasswordExpiry;
            if (tokenCode != resetPasswordDto.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                throw new InvalidRequestException("Reset link is invalid!");
            }
            user.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return ("Password Reset Successfullly");
        }

    }
}
