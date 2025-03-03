using System.Text;
using System.Text.Json;

namespace MRS.Infrastructure.Grpc.Services
{
    public class HealthMessageSender : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly MessageRouterServiceImpl test;

        public HealthMessageSender(HttpClient httpClient, IServiceProvider serviceProvider, MessageRouterServiceImpl test)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7128/"); //webapi listen port/address
            _serviceProvider = serviceProvider;
            this.test = test;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {



                try
                {
                    using (var scope = _serviceProvider.CreateScope()/*create a new DI scope to resolve services that are not tied to the main application's request lifecycle.*/)
                    {


                        var messageRouterService = scope.ServiceProvider.GetService<MessageRouterServiceImpl>();

                        //create a secondery dictionary to fetch all active clients from RouterServiceImpl
                        var activeClients = test.ListOfActiveClients;

                        //create an onject taht holds the number of activeclients
                        int activeClientCount = activeClients.Count((kv => (DateTime.Now - kv.Value).TotalSeconds < 30));


                        // Check if there are any active clients
                        if ()
                        {
                            // Iterate over each client in the activeClients collection
                            foreach (var client in activeClients)
                            {
                                // Create an object with properties matching MMSMessage for the POST request
                                var postData = new
                                {
                                    PrimaryId = client.Key,           // Unique identifier of the client
                                    CurrentTime = client.Value.ToString("HH:mm:ss"), // Current time formatted as HH:mm:ss
                                    ActiveClients = activeClientCount // Total number of active clients
                                };

                                // Convert postData to JSON and wrap it in StringContent with UTF-8 encoding
                                var content = new StringContent(
                                    JsonSerializer.Serialize(postData)/*turn post data to serials using json serializer*/,
                                    Encoding.UTF8/*encode to this */,
                                    "application/json"/*json type*/);

                                // Send an asynchronous POST request to "api/health" with the JSON content
                                var response = await _httpClient.PostAsync("api/health", content, stoppingToken);

                                // Throw an exception if the response status code is not successful (not 200-299)
                                response.EnsureSuccessStatusCode();
                            }
                        }
                        else
                        {
                            var postData = new
                            {
                                PrimaryId = "00000000-0000-0000-0000-000000000000",     //if there is no primary id      
                                CurrentTime = DateTime.Now.ToString("HH:mm:ss"),
                                ActiveClients = activeClientCount,
                            };
                            var content = new StringContent(JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");
                            var response = await _httpClient.PostAsync("api/health", content, stoppingToken);
                            response.EnsureSuccessStatusCode();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{DateTime.Now:g} - Error sending health message:\n {ex.Message}");
                }
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
