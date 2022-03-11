using Mango.Services.OrderAPI.Services.Interfaces;

namespace Mango.Services.OrderAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IOrderMessageConsumer? _orderMessageConsumer;
        private static IUpdatePaymentStatusMessageConsumer? _updatePaymentStatusMessageConsumer;

        public static IApplicationBuilder UseOrderMessageConsumer(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                _orderMessageConsumer = scope.ServiceProvider.GetService<IOrderMessageConsumer>();
                _updatePaymentStatusMessageConsumer = scope.ServiceProvider.GetService<IUpdatePaymentStatusMessageConsumer>();

                var hostApplicationLife = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
                hostApplicationLife?.ApplicationStarted.Register(() => OnStartConsuming());
                hostApplicationLife?.ApplicationStarted.Register(() => OnStopConsuming());
            }

            return builder;
        }

        private static void OnStartConsuming()
        {
            _orderMessageConsumer?.Start();
            _updatePaymentStatusMessageConsumer?.Start();
        }

        private static void OnStopConsuming()
        {
            _orderMessageConsumer?.Stop();
            _updatePaymentStatusMessageConsumer?.Stop();
        }
    }
}
