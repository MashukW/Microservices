namespace ImageProcessing.Domain.Core.Models
{
    public class ImageAnalysis
    {
        public ImageAnalysis()
        {
            Metadata = new ImageAnalysisMetadata();
            Categories = new List<string>();
        }

        public ImageAnalysisMetadata Metadata { get; set; }

        public List<string> Categories { get; set; }
    }

    public class ImageAnalysisMetadata
    {
        public ImageAnalysisMetadata()
        {
            Format = string.Empty;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Format { get; set; }
    }
}
