using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

public class HealthMessageSender : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHttpClientFactory _clientFactory;

    public HealthMessageSender(IServiceProvider serviceProvider, IHttpClientFactory clientFactory)
    {
        _serviceProvider = serviceProvider;
        _clientFactory = clientFactory; // Use IHttpClientFactory for HttpClient management
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Create a new scope to resolve scoped services
                using (var scope = _serviceProvider.CreateScope())
                {
                    var routerApplication = scope.ServiceProvider.GetRequiredService<MRS.Domain.Interfaces.IMessageRouterApplication>();

                    // Call business logic to create a health message
                    var result = routerApplication.CreateHealthMessage();

                    // Prepare the data to send to the Web API
                    var postData = new
                    {
                        PrimaryId = result.PrimaryId,
                        CurrentTime = result.CurrentTime.ToString() // Serialize DateTime to string if needed
                    };

                    // Convert the data to JSON and send it as the body of the POST request
                    var content = new StringContent(JsonSerializer.Serialize(postData), System.Text.Encoding.UTF8, "application/json");

                    // Create an HttpClient instance using IHttpClientFactory
                    var httpClient = _clientFactory.CreateClient(); // Properly use IHttpClientFactory here
                    httpClient.BaseAddress = new Uri("https://localhost:7128/"); // Set BaseAddress explicitly

                    // Make an HTTP POST request to the Web API
                    var apiResponse = await httpClient.PostAsync("api/health", content);
                    apiResponse.EnsureSuccessStatusCode(); // Throw exception if the request fails

                    Console.WriteLine($"Health message sent to Web API at {DateTime.UtcNow}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending health message: {ex.Message}");
            }

            // Wait for 30 seconds before sending the next health message
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}