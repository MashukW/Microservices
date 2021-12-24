using ImageProcessing.Domain.Core.Enums;
using ImageProcessing.Domain.Core.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public static class DownloadImage
    {
        [FunctionName("download-image")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "photos/{photoId}")]
            HttpRequest request,
            string photoId,
            [Blob("photos-small/{photoId}.jpg", FileAccess.Read, Connection = AzureOptions.ImageStorageConnectionSectionName)]
            Stream imageSmallStream,
            [Blob("photos-medium/{photoId}.jpg", FileAccess.Read, Connection = AzureOptions.ImageStorageConnectionSectionName)]
            Stream imageMediumStream,
            [Blob("photos/{photoId}.jpg", FileAccess.Read, Connection = AzureOptions.ImageStorageConnectionSectionName)]
            Stream imageOriginalStream,
            ILogger logger)
        {
            logger.LogInformation($"Downloading {photoId}...");

            var imageSizeType = request.Query["imageSizeType"].ToString().ToLowerInvariant();
            if (imageSizeType == ImageSizeType.Small.ToString().ToLowerInvariant())
            {
                logger.LogInformation($"Retrieving the small size");

                var smallImageBytes = await GetBytes(imageSmallStream);
                return new FileContentResult(smallImageBytes, "image/jpeg") { FileDownloadName = $"{photoId}.jpg" };
            }

            if (imageSizeType == ImageSizeType.Medium.ToString().ToLowerInvariant())
            {
                logger.LogInformation($"Retrieving the medium size");

                var mediumImageBytes = await GetBytes(imageMediumStream);
                return new FileContentResult(mediumImageBytes, "image/jpeg") { FileDownloadName = $"{photoId}.jpg" };
            }

            logger.LogInformation($"Retrieving the original size");

            var originalImageBytes = await GetBytes(imageOriginalStream);
            return new FileContentResult(originalImageBytes, "image/jpeg") { FileDownloadName = $"{photoId}.jpg" };
        }

        private static async Task<byte[]> GetBytes(Stream imageStream)
        {
            if (imageStream.Length > int.MaxValue)
                throw new InvalidOperationException("image is too big");

            byte[] imageBytes = new byte[imageStream.Length];
            await imageStream.ReadAsync(imageBytes, 0, (int)imageStream.Length);

            return imageBytes;
        }
    }
}
