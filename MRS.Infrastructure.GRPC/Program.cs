using MRS.Infrastructure.Grpc.Services;
using MRS.Domain.Interfaces;
using MRS.Application;

namespace MRS.Infrastructure.Grpc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            
            builder.Services.AddGrpc();

            // Add services to the container.
            builder.Services.AddScoped<IRouterLoggingService, RouterLoggingService>();
            builder.Services.AddScoped<IMessageRouterApplication, MessageRouterApplication>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //app.MapGrpcService<GreeterService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.MapGrpcService<MessageRouterServiceImpl>();

            app.Run();
        }
    }
}