using Azure.Messaging.ServiceBus;

namespace PSPiZK.Azure.Services.AzureServiceBus
{
    public class AzureServiceBusService : IAzureServiceBusService
    {
        private readonly ServiceBusSender _sender;

        public AzureServiceBusService(IConfiguration configuration)
        {
            var client = new ServiceBusClient(configuration["AzureServiceBus:ConnectionString"]);

            _sender = client.CreateSender(configuration["AzureServiceBus:Topic"]);
        }

        public async Task SendAsync(string message)
        {
            var sbMessage = new ServiceBusMessage(message);
            await _sender.SendMessageAsync(sbMessage);
        }
    }
}
