using ImageProcessing.Domain.Core.Models;
using ImageProcessing.Domain.Core.Options;
using ImageProcessing.Services.Interfaces;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace ImageProcessing.Services
{
    public class AzureAnalyzerService : IAnalyzerService
    {
        private readonly ComputerVisionClient _computerVisionClient;

        public AzureAnalyzerService(AzureOptions azureOptions)
        {
            var serviceClientCredentials = new ApiKeyServiceClientCredentials(azureOptions.ComputerVisionAccessKey);

            _computerVisionClient = new ComputerVisionClient(serviceClientCredentials)
            {
                Endpoint = azureOptions.ComputerVisionUrl
            };
        }

        public async Task<ImageAnalysis> Analyze(byte[] image)
        {
            using var imageStream = new MemoryStream(image);

            var imageAnalysis = await _computerVisionClient.AnalyzeImageInStreamAsync(imageStream);

            return new ImageAnalysis
            {
                Metadata = new ImageAnalysisMetadata
                {
                    Width = imageAnalysis.Metadata.Width,
                    Height = imageAnalysis.Metadata.Height,
                    Format = imageAnalysis.Metadata.Format
                },
                Categories = imageAnalysis.Categories.Select(x => x.Name).ToList(),
            };
        }
    }
}