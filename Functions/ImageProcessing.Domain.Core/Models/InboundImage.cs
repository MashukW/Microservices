using ImageProcessing.Domain.Core.Enums;
using System.Text.Json.Serialization;

namespace ImageProcessing.Domain.Core.Models
{
    public class InboundImage
    {
        public InboundImage()
        {
            Id = Guid.Empty;
            Name = string.Empty;
            Description = string.Empty;
            Tags = Array.Empty<string>();
            File = string.Empty;
        }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("imageType")]
        public ImageType ImageType { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }

        [JsonPropertyName("file")]
        public string File { get; set; }
    }
}
