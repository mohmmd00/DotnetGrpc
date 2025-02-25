using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using Common.Proto;
using Google.Protobuf.WellKnownTypes;
using MRS.Domain.Interfaces;

namespace MRS.Infrastructure.Grpc.Services
{
    public class HealthMessageSender : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly MessageRouterServiceImpl _messageRouterService;

        public HealthMessageSender(HttpClient httpClient, IServiceProvider serviceProvider , MessageRouterServiceImpl service)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7128/");
            _serviceProvider = serviceProvider;
            _messageRouterService = service;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {


                        var messageRouterService = scope.ServiceProvider.GetRequiredService<MessageRouterServiceImpl>();

                        //create a secondery dictionary to fetch all active clients from RouterServiceImpl
                        var activeClients = messageRouterService.GetActiveClients();

                        //create an onject taht holds the number of activeclients
                        int activeClientCount = activeClients.Count((kv => (DateTime.UtcNow - kv.Value).TotalSeconds < 30));


                        if (activeClientCount > 0)
                        {
                            foreach (var client in activeClients)
                            {
                                var postData = new
                                {
                                    PrimaryId = client.Key,
                                    CurrentTime = client.Value,
                                    ActiveClients = activeClientCount,
                                };
                                var content = new StringContent(JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");
                                var response = await _httpClient.PostAsync("api/health", content, stoppingToken);
                                response.EnsureSuccessStatusCode();
                            }
                        }
                        else
                        {
                            // در صورت صفر بودن کلاینت‌ها، یک درخواست POST ارسال می‌کنیم
                            var postData = new
                            {
                                PrimaryId = "N/A",           // یا مقدار مناسب برای زمانی که کلاینتی وجود ندارد
                                CurrentTime = DateTime.UtcNow,
                                ActiveClients = activeClientCount,
                            };
                            var content = new StringContent(JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");
                            var response = await _httpClient.PostAsync("api/health", content, stoppingToken);
                            response.EnsureSuccessStatusCode();
                        }

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Health message sent with {activeClientCount} active clients");
                        Console.ResetColor();


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending health message: {ex.Message}");
                }
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}