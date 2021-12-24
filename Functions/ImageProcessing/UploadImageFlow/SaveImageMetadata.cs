using ImageProcessing.Domain.Core.Models;
using ImageProcessing.Domain.Core.Options;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ImageProcessing.UploadImageFlow
{
    public static class SaveImageMetadata
    {
        [FunctionName("save-image-metadata")]
        public static async Task<bool> Run(
            [ActivityTrigger] ImageMetadata imageMetadata,
            [CosmosDB(databaseName: "photos",
                collectionName: "metadata",
                ConnectionStringSetting = AzureOptions.CosmosDbConnectionSectionName,
                CreateIfNotExists = true)]
            IAsyncCollector<dynamic> cosmosMetadataCollection,
            ILogger logger)
        {
            logger.LogInformation("Saving metadata starting.");

            await cosmosMetadataCollection.AddAsync(imageMetadata);

            logger.LogInformation("Saving metadata finished.");

            return true;
        }
    }
}
