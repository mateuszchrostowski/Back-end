namespace PSPiZK.Azure.Services.AzureServiceBus
{
    public interface IAzureServiceBusService
    {
        Task SendAsync(string message);
    }
}
