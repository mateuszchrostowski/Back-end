using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

using PSPiZK.Azure.Models;

using System.Globalization;

namespace PSPiZK.Azure.Services.AzureComputerVision
{
    public class AzureComputerVisionService : IAzureComputerVisionService
    {
        private readonly string _subscriptionKey;
        private readonly string _endpoint;

        public AzureComputerVisionService(IConfiguration configuration)
        {
            _subscriptionKey = configuration.GetSection("AzureComputerVision").GetValue<string>("SubscriptionKey");
            _endpoint = configuration.GetSection("AzureComputerVision").GetValue<string>("Endpoint");
        }

        public async Task<Dictionary<string, string>> AnalyzeBlobAsync(BlobObject blob)
        {
            var client = GetComputerVisionClient();

            var features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Tags,
                VisualFeatureTypes.Description
            };

            try
            {
                var imageAnalysis = await client.AnalyzeImageAsync(blob.ImageUri, visualFeatures: features);
                if (imageAnalysis == null)
                    return new Dictionary<string, string>();

                var result = new Dictionary<string, string>();

                foreach(var item in imageAnalysis.Tags)
                {
                    result.Add(item.Name.Replace(' ', '_').Replace('-','_'),
                        item.Confidence.ToString(CultureInfo.InvariantCulture));
                }

                result.Add("caption", imageAnalysis.Description.Captions[0].Text);

                return result;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private ComputerVisionClient GetComputerVisionClient()
        {
            var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_subscriptionKey))
            {
                Endpoint = _endpoint
            };

            return client;
        }
    }
}
