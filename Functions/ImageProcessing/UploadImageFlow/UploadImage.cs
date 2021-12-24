using ImageProcessing.Domain.Core.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public static class UploadImage
    {
        [FunctionName("upload-image")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage request,
            [DurableClient] IDurableOrchestrationClient orchestrationClient,
            ILogger logger)
        {
            var body = await request.Content.ReadAsStringAsync();
            var inboundImage = JsonSerializer.Deserialize<InboundImage>(body);

            var uploadImageOrchestratorId = await orchestrationClient.StartNewAsync("upload-image-orchestrator", inboundImage);

            logger.LogInformation($"Started upload image orchestration with ID => '{uploadImageOrchestratorId}'.");

            return orchestrationClient.CreateCheckStatusResponse(request, uploadImageOrchestratorId);
        }

        [FunctionName("upload-image-orchestrator")]
        public static async Task<bool> RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var inboundImage = context.GetInput<InboundImage>();

            var saveImageSuccess = await context.CallActivityAsync<bool>("save-image", inboundImage);
            if (saveImageSuccess)
            {
                var imageAnalysis = await context.CallActivityAsync<ImageAnalysis>("analyze-image", inboundImage);

                var imageMetadata = new ImageMetadata
                {
                    Id = inboundImage.Id,
                    Name = inboundImage.Name,
                    Description = inboundImage.Name,
                    Tags = inboundImage.Tags,
                    ImageAnalysis = imageAnalysis
                };
                var saveImageMetadataSuccess = await context.CallActivityAsync<bool>("save-image-metadata", imageMetadata);

                return saveImageMetadataSuccess;
            }

            return false;
        }
    }
}