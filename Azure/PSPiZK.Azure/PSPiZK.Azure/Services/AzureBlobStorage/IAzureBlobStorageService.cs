using PSPiZK.Azure.Models;

namespace PSPiZK.Azure.Services.AzureBlobStorage
{
    public interface IAzureBlobStorageService
    {
        Task<List<BlobObject>> GetBlobsAsync();
        Task<BlobObject> UploadAsync(IFormFile formFile);
        Task UploadMetadataAsync(BlobObject blobObject, Dictionary<string, string> metadata);
    }
}
