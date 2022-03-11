using Mango.Services.PaymentAPI.Services.Interfaces;

namespace Mango.Services.PaymentAPI.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<bool> HandlePayment()
        {
            // Implement payment logic via Stripe or Paddle
            return Task.FromResult(true);
        }
    }
}