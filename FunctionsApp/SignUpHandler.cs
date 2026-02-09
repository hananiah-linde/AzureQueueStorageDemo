using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SharedLibrary;

namespace FunctionsApp;

public class SignUpHandler(ILogger<SignUpHandler> logger)
{
    [Function(nameof(SignUpHandler))]
    public void Run([QueueTrigger(QueueNames.Signup, Connection = "AzureWebJobsStorage")] QueueMessage message)
    {
        logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
    }
}