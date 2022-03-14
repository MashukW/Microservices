namespace Mango.Services.EmailAPI.Services.Interfaces;

public interface IEmailService
{
    Task<bool> Send(string email);
}
