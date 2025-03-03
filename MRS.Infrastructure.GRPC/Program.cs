using MRS.Infrastructure.Grpc.Services;
using MRS.Domain.Interfaces;
using MRS.Application;
using Framework_0.CustomLoggingService;
using Microsoft.Extensions.DependencyInjection;

namespace MRS.Infrastructure.Grpc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add gRPC and HttpClient services
            builder.Services.AddGrpc();
            builder.Services.AddHttpClient();

            // Add services to the container
            builder.Services.AddTransient<ILoggingService, LoggingService>(); // add loggin service
            builder.Services.AddTransient<IMessageRouterApplication, MessageRouterApplication>(); // add application service

            // Register MessageRouterServiceImpl as a transient service
            builder.Services.AddSingleton<MessageRouterServiceImpl>(); // add router service as singleton -- so background service be able to use scopefactory ...

            // Add the HealthMessageSender hosted service
            builder.Services.AddHostedService<HealthMessageSender>(); // add background service

            var app = builder.Build();

            // Configure the HTTP request pipeline
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            // Map MessageRouterServiceImpl as a gRPC service
            app.MapGrpcService<MessageRouterServiceImpl>();

            app.Run();
        }
    }
}