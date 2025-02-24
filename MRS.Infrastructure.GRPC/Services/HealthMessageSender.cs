using System.Text.Json;
using MRS.Domain.Interfaces;

public class HealthMessageSender : BackgroundService /*component to use in background tasks without user intraction*/
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
                    // Create an HttpClient instance using IHttpClientFactory
                    var httpClient = _clientFactory.CreateClient(); // Properly use IHttpClientFactory here
                    httpClient.BaseAddress = new Uri("https://localhost:7128/"); // Set BaseAddress explicitly to match webapi 





                    var routerApplication = scope.ServiceProvider.GetRequiredService<IMessageRouterApplication>();

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



                    // Make an HTTP POST request to the Web API
                    var apiResponse = await httpClient.PostAsync("api/health", content);
                    apiResponse.EnsureSuccessStatusCode(); // Throw exception if the request fails
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"\nHealth message sent to Web API at {DateTime.UtcNow}\n");
                    Console.ResetColor();
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