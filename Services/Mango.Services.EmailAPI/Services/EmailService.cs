using Mango.Services.EmailAPI.Services.Interfaces;

namespace Mango.Services.PaymentAPI.Services;

public class EmailService : IEmailService
{
    public Task<bool> Send(string email)
    {
        // Implement send email logic via SMTP(S) provider
        return Task.FromResult(true);
    }
}
