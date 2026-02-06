using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SharedLibrary;

namespace FunctionsApp;

public class SignupConsumer
{
    private readonly ILogger<SignupConsumer> _logger;

    public SignupConsumer(ILogger<SignupConsumer> logger)
    {
        _logger = logger;
    }

    [Function(nameof(SignupConsumer))]
    public void Run([QueueTrigger("signup", Connection = "AzureWebJobsStorage")] SignupRequest request)
    {
        _logger.LogInformation("Processing signup: {FirstName} {LastName} ({Email})",
            request.FirstName, request.LastName, request.Email);
    }
}
