using Mango.Services.OrderAPI.Services.Interfaces;

namespace Mango.Services.OrderAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IOrderMessageConsumer? _orderMessageConsumer;

        public static IApplicationBuilder UseOrderMessageConsumer(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                _orderMessageConsumer = scope.ServiceProvider.GetService<IOrderMessageConsumer>();

                var hostApplicationLife = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
                hostApplicationLife?.ApplicationStarted.Register(() => OnStartConsuming());
                hostApplicationLife?.ApplicationStarted.Register(() => OnStopConsuming());
            }

            return builder;
        }

        private static void OnStartConsuming()
        {
            _orderMessageConsumer?.Start();
        }

        private static void OnStopConsuming()
        {
            _orderMessageConsumer?.Stop();
        }
    }
}
