using ImageProcessing.Domain.Core.Models;
using ImageProcessing.Domain.Core.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public static class SearchImage
    {
        [FunctionName("search-image")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest request,
            [CosmosDB(databaseName: "photos",
                collectionName: "metadata",
                ConnectionStringSetting = AzureOptions.CosmosDbConnectionSectionName)]
            DocumentClient documentClient,
            ILogger logger)
        {
            logger.LogInformation("Searching...");

            string searchTerm = request.Query["searchTerm"];
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new NotFoundResult();

            var collectionUri = UriFactory.CreateDocumentCollectionUri("photos", "metadata");
            var feedOptions = new FeedOptions { EnableCrossPartitionQuery = true };
            var query = documentClient
                .CreateDocumentQuery<ImageMetadata>(collectionUri, feedOptions)
                .Where(p => p.Description.Contains(searchTerm))
                .AsDocumentQuery();

            var metadataResult = new List<ImageMetadata>();
            while (query.HasMoreResults)
            {
                var metadata = await query.ExecuteNextAsync<ImageMetadata>();

                metadataResult.AddRange(metadata.Select(x => x));
            }

            return new OkObjectResult(metadataResult);
        }
    }
}
