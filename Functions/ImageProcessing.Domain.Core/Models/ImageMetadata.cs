using System.Text.Json.Serialization;

namespace ImageProcessing.Domain.Core.Models
{
    public class ImageMetadata
    {
        public ImageMetadata()
        {
            Name = string.Empty;
            Description = string.Empty;
            Tags = Array.Empty<string>();
            ImageAnalysis = new ImageAnalysis();
        }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }

        [JsonPropertyName("imageAnalysis")]
        public ImageAnalysis ImageAnalysis { get; set; }
    }
}
