using Application_API.Models;

namespace Application_API.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Email email);   

    }
}
