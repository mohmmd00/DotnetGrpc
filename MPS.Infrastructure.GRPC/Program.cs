//using MPS.Infrastructure.Grpc.Services;

using MPS.Application;
using MPS.Domian.Interfaces;
using MPS.Infrastructure.Grpc.Services;

namespace MPS.Infrastructure.Grpc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc();
            builder.Services.AddTransient<IMessageProcessApplication, MessageProcessApplication>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            app.MapGrpcService<MessageProcessServiceImpl>();

            app.Run("http://localhost:50051");
        }
    }
}