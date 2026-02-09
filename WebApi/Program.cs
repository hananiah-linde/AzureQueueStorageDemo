using System.Text.Json;
using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;
using SharedLibrary;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureClients(azureClientBuilder =>
{
    azureClientBuilder.AddQueueServiceClient(builder.Configuration.GetConnectionString("AzureStorage")!)
        .WithName(StorageAccountNames.Main)
        .ConfigureOptions(options =>
        {
            options.MessageEncoding = QueueMessageEncoding.Base64;
        });

    azureClientBuilder.AddQueueServiceClient(builder.Configuration.GetConnectionString("AzureStorageSecondary")!)
        .WithName(StorageAccountNames.Secondary)
        .ConfigureOptions(options =>
        {
            options.MessageEncoding = QueueMessageEncoding.Base64;
        });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/signup", async (SignupRequest request, IAzureClientFactory<QueueServiceClient> clientFactory) =>
{
    var queueServiceClient = clientFactory.CreateClient(StorageAccountNames.Main);
    var queueClient = queueServiceClient.GetQueueClient(QueueNames.Signup);

    await queueClient.CreateIfNotExistsAsync();
    await queueClient.SendMessageAsync(JsonSerializer.Serialize(request));
    //await Task.Delay(1000); // Simulate processing time for signup
    return Results.Ok(new { Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c" });
})
.WithName("Signup");

app.Run();

public static class StorageAccountNames
{
    public const string Main = "Main";
    public const string Secondary = "Secondary";
}