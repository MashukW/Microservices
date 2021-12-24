using Azure.Storage.Blobs;
using ImageProcessing.Domain.Core.Enums;
using ImageProcessing.Domain.Core.Models;
using ImageProcessing.Domain.Core.Options;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public static class SaveImage
    {
        [FunctionName("save-image")]
        public static async Task<bool> Run(
            [ActivityTrigger] InboundImage uploadPhoto,
            [Blob(AppOptions.ImageBlobPath, FileAccess.ReadWrite, Connection = AzureOptions.ImageStorageConnectionSectionName)]
            BlobContainerClient blobContainerClient,
            ILogger logger)
        {
            var blobName = uploadPhoto.ImageType switch
            {
                ImageType.Dish => string.Format($"{AppOptions.DishBlobPath}/{AppOptions.ImageBlobJpgNameFormat}", uploadPhoto.Id),
                ImageType.Avatar => string.Format($"{AppOptions.AvatartBlobPath}/{AppOptions.ImageBlobJpgNameFormat}", uploadPhoto.Id),
                _ => throw new InvalidOperationException($"Image type '{uploadPhoto.ImageType}' does not supported.")
            };

            await blobContainerClient.CreateIfNotExistsAsync();

            var photoBytes = Convert.FromBase64String(uploadPhoto.File);
            await using (Stream photoStream = new MemoryStream(photoBytes))
            {
                await blobContainerClient.UploadBlobAsync(blobName, photoStream);
            }

            logger?.LogInformation($"Successfully uploaded {blobName} file and its metadata");

            return true;
        }
    }
}
