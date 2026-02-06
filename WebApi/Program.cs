using Azure.Storage.Queues;
using SharedLibrary;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetValue<string>("AzureStorage:ConnectionString");
var options = new QueueClientOptions(QueueClientOptions.ServiceVersion.V2025_01_05)
{
    MessageEncoding = QueueMessageEncoding.Base64
};
builder.Services.AddSingleton(new QueueServiceClient(connectionString, options));
builder.Services.AddSingleton<IQueueService, QueueService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/signup", async (SignupRequest request, IQueueService queueService) =>
{
    await queueService.SendMessageAsync("signup", request);
    return Results.Ok(new { Message = "Signup request received" });
})
.WithName("Signup");

app.Run();