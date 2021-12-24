using ImageProcessing.Domain.Core.Models;

namespace ImageProcessing.Services.Interfaces
{
    public interface IAnalyzerService
    {
        Task<ImageAnalysis> Analyze(byte[] image);
    }
}