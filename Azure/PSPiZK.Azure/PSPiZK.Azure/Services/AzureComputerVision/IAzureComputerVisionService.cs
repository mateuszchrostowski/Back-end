using PSPiZK.Azure.Models;

namespace PSPiZK.Azure.Services.AzureComputerVision
{
    public interface IAzureComputerVisionService
    {
        Task<Dictionary<string, string>> AnalyzeBlobAsync(BlobObject blob);
    }
}
