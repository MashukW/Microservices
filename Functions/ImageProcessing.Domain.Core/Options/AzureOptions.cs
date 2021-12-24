namespace ImageProcessing.Domain.Core.Options
{
    public class AzureOptions
    {
        public AzureOptions()
        {
            ComputerVisionUrl = string.Empty;
            ComputerVisionAccessKey = string.Empty;
        }

        public const string ImageStorageConnectionSectionName = "ImageStorageConnection";

        public const string CosmosDbConnectionSectionName = "CosmosDbConnection";

        public string ComputerVisionUrl { get; set; }

        public string ComputerVisionAccessKey { get; set; }
    }
}
