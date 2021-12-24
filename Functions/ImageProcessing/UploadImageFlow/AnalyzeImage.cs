using ImageProcessing.Domain.Core.Models;
using ImageProcessing.Services.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ImageProcessing.UploadImageFlow
{
    public class AnalyzeImage
    {
        private readonly IAnalyzerService _analyzerService;
        
        public AnalyzeImage(IAnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }

        [FunctionName("analyze-image")]
        public async Task<ImageAnalysis> Run(
            [ActivityTrigger] InboundImage uploadPhoto,
            ILogger logger)
        {
            logger.LogInformation("Image analysis starting.");

            var imageAnalysis = await _analyzerService.Analyze(Convert.FromBase64String(uploadPhoto.File));

            logger.LogInformation("Image analysis finished.");

            return imageAnalysis;
        }
    }
}
