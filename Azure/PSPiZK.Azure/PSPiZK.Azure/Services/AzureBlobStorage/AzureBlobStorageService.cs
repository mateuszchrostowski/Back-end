using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using PSPiZK.Azure.Models;

namespace PSPiZK.Azure.Services.AzureBlobStorage
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;

        public AzureBlobStorageService(IConfiguration configuration)
        {
            _storageConnectionString = configuration.GetSection("AzureBlobStorage").GetValue<string>("ConnectionString");
            _storageContainerName = configuration.GetSection("AzureBlobStorage").GetValue<string>("ContainerName");
        }

        public async Task<List<BlobObject>> GetBlobsAsync()
        {
            var container = GetBlobContainerClient();
            var blobs = new List<BlobObject>();

            await foreach(var blob in container.GetBlobsAsync())
            {
                var uri = container.Uri.ToString();
                var blobUri = $"{uri}/{blob.Name}";

                var blobClient = container.GetBlobClient(blob.Name);
                BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
                var metadata = blobProperties.Metadata;
                metadata.TryGetValue("caption", out var caption);

                blobs.Add(new BlobObject
                {
                    Name = blob.Name,
                    ImageUri = blobUri,
                    Caption = caption
                });
            }

            return blobs;
        }

        public async Task<BlobObject> UploadAsync(IFormFile formFile)
        {
            var container = GetBlobContainerClient();

            try
            {
                var client = container.GetBlobClient(formFile.FileName);
                await using var data = formFile.OpenReadStream();
                await client.UploadAsync(data);

                var uri = container.Uri.ToString();
                return new BlobObject
                {
                    Name = formFile.FileName,
                    ImageUri = $"{uri}/{formFile.FileName}"
                };
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UploadMetadataAsync(BlobObject blobObject, Dictionary<string, string> metadata)
        {
            var container = GetBlobContainerClient();
            var blobClient = container.GetBlobClient(blobObject.Name);
            if (blobClient == null)
                return;

            await blobClient.SetMetadataAsync(metadata);
        }

        private BlobContainerClient GetBlobContainerClient()
        {
            return new BlobContainerClient(_storageConnectionString, _storageContainerName);
        }
    }
}
