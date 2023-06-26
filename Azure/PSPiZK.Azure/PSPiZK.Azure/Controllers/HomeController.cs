using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using PSPiZK.Azure.Models;
using PSPiZK.Azure.Services.AzureBlobStorage;
using PSPiZK.Azure.Services.AzureComputerVision;
using PSPiZK.Azure.Services.AzureServiceBus;

using System.Diagnostics;

namespace PSPiZK.Azure.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAzureBlobStorageService _azureBlobStorageService;
        private readonly IAzureComputerVisionService _azureComputerVisionService;
        private readonly IAzureServiceBusService _azureServiceBusService;

        public HomeController(IAzureBlobStorageService azureBlobStorageService, IAzureComputerVisionService azureComputerVisionService, IAzureServiceBusService azureServiceBusService)
        {
            _azureBlobStorageService = azureBlobStorageService;
            _azureComputerVisionService = azureComputerVisionService;
            _azureServiceBusService = azureServiceBusService;
        }

        public async Task<ActionResult> Index()
        {
            var blobs = await _azureBlobStorageService.GetBlobsAsync();

            ViewBag.Blobs = blobs;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile file)
        {
            if(file != null && file.Length > 0)
            {
                if(!file.ContentType.StartsWith("image"))
                {
                    TempData["Message"] = "Only image files may be uploaded";
                }
                else
                {
                    var blob = await _azureBlobStorageService.UploadAsync(file);

                    var metadata = await _azureComputerVisionService.AnalyzeBlobAsync(blob);

                    var sbMessage = new ServiceBusMessage()
                    {
                        FileName = file.FileName,
                        Metadata = string.Join(", ", metadata.Where(x => x.Key != "caption").Select(x => x.Key).ToList()),
                        Caption = metadata["caption"]
                    };

                    var json = JsonConvert.SerializeObject(sbMessage, Formatting.Indented);

                    await _azureServiceBusService.SendAsync(json);

                    await _azureBlobStorageService.UploadMetadataAsync(blob, metadata);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}