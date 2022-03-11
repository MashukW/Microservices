using Mango.Services.OrderAPI.Services.Interfaces;

namespace Mango.Services.PaymentAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IPaymentMessageConsumer? _paymentMessageConsumer;

        public static IApplicationBuilder UsePaymentMessageConsumer(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                _paymentMessageConsumer = scope.ServiceProvider.GetService<IPaymentMessageConsumer>();

                var hostApplicationLife = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
                hostApplicationLife?.ApplicationStarted.Register(() => OnStartConsuming());
                hostApplicationLife?.ApplicationStarted.Register(() => OnStopConsuming());
            }

            return builder;
        }

        private static void OnStartConsuming()
        {
            _paymentMessageConsumer?.Start();
        }

        private static void OnStopConsuming()
        {
            _paymentMessageConsumer?.Stop();
        }
    }
}
