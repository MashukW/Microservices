using Azure.Storage.Blobs;
using ImageProcessing.Domain.Core.Enums;
using ImageProcessing.Domain.Core.Options;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public static class ImageResize
    {
        [FunctionName("resize-image")]
        public static async Task Run(
            [BlobTrigger("photos/{imageName}", Connection = "")]
            Stream imageStream,
            string imageName,
            [Blob("photos-small", FileAccess.Write, Connection = AzureOptions.ImageStorageConnectionSectionName)]
            BlobContainerClient smallImageContainerClient,
            [Blob("photos-medium", FileAccess.Write, Connection = AzureOptions.ImageStorageConnectionSectionName)]
            BlobContainerClient mediumImageContainerClient,
            ILogger logger)
        {
            try
            {
                await ResizeImage(imageName, imageStream, smallImageContainerClient, ImageSizeType.Small);
                await ResizeImage(imageName, imageStream, mediumImageContainerClient, ImageSizeType.Medium);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
            }
        }

        private static async Task ResizeImage(
            string imageName,
            Stream imageStream,
            BlobContainerClient blobClient,
            ImageSizeType imageSizeType)
        {
            imageStream.Position = 0;

            using var image = await Image.LoadAsync(imageStream);
            var rotateSize = CalculateSize(image.Width, image.Height, imageSizeType);
            image.Mutate(x => x.Resize(rotateSize.Width, rotateSize.Height));
            var resizedImage = ImageToStream(image);

            await blobClient.CreateIfNotExistsAsync();
            await blobClient.UploadBlobAsync(imageName, resizedImage);
        }

        private static Size CalculateSize(int width, int height, ImageSizeType imageSize)
        {
            var newWidth = imageSize == ImageSizeType.Small
                ? width / 4
                : width / 2;

            var ratio = (float)newWidth / width;
            var newHeight = (int)Math.Floor(height * ratio);

            return new Size(newWidth, newHeight);
        }

        private static Stream ImageToStream(Image image)
        {
            var stream = new MemoryStream();

            var jpegEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(JpegFormat.Instance);
            image.Save(stream, jpegEncoder);
            stream.Position = 0;

            return stream;
        }
    }
}
