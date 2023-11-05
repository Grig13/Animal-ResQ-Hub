using Microsoft.AspNetCore.Mvc;
using Application_API.Models;
using Application_API.Models.Dto;
using System.Security.Claims;

namespace Application_API.Services.Interfaces
{
    public interface IUserService
    {
        Task<TokenApiDto> Authenticate(User userObj);
        Task<string> RegisterUser(User userObj);
        Task<bool> CheckUserNameExistAsync(string userName);
        Task<bool> CheckEmailExistAsync(string email);
        string CheckPasswordStrength(string password);
        string CreateJWT(User user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string CreateRefreshToken();
        Task<IEnumerable<User>> GetAllUsers();
        Task<TokenApiDto> Refresh(TokenApiDto tokenApiDto);
        Task<string> SendEmail(string email);
        Task<string> ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}
