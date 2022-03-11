namespace Mango.Services.PaymentAPI.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> HandlePayment();
    }
}
