using Mango.Services.EmailAPI.Services.Interfaces;

namespace Mango.Services.EmailAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IEmailMessageConsumer? _emailMessageConsumer;

        public static IApplicationBuilder UseSendEmailMessageConsumer(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                _emailMessageConsumer = scope.ServiceProvider.GetService<IEmailMessageConsumer>();

                var hostApplicationLife = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
                hostApplicationLife?.ApplicationStarted.Register(() => OnStartConsuming());
                hostApplicationLife?.ApplicationStarted.Register(() => OnStopConsuming());
            }

            return builder;
        }

        private static void OnStartConsuming()
        {
            _emailMessageConsumer?.Start();
        }

        private static void OnStopConsuming()
        {
            _emailMessageConsumer?.Stop();
        }
    }
}
