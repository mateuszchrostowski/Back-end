using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

using PSPiZK.Azure.Services.AzureBlobStorage;
using PSPiZK.Azure.Services.AzureComputerVision;
using PSPiZK.Azure.Services.AzureServiceBus;

var builder = WebApplication.CreateBuilder(args);

var vault = builder.Configuration["AzureKeyVault:Vault"];
var clientId = builder.Configuration["AzureKeyVault:ClientId"];
var clientSecret = builder.Configuration["AzureKeyVault:ClientSecret"];
var tenant = builder.Configuration["AzureKeyVault:TenantId"];

var credential = new ClientSecretCredential(tenant, clientId, clientSecret);
var client = new SecretClient(new Uri($"https://{vault}.vault.azure.net/"), credential);

builder.Configuration.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();
builder.Services.AddScoped<IAzureComputerVisionService, AzureComputerVisionService>();
builder.Services.AddSingleton<IAzureServiceBusService,
     AzureServiceBusService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
