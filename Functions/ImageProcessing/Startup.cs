using ImageProcessing;
using ImageProcessing.Domain.Core.Options;
using ImageProcessing.Services;
using ImageProcessing.Services.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace ImageProcessing
{
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;

            builder.Services.AddSingleton<IAnalyzerService, AzureAnalyzerService>();

            builder.Services.Configure<AzureOptions>(options =>
            {
                options.ComputerVisionUrl = configuration[nameof(AzureOptions.ComputerVisionUrl)];
                options.ComputerVisionAccessKey = configuration[nameof(AzureOptions.ComputerVisionAccessKey)];
            });
        }
    }
}