using Grpc.Net.Client;
using Common.Proto;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MPS.Application;
using Grpc.Core;
using System.Threading.Tasks;
using MPS.Domian.Interfaces;
using Grpc.Client.Services;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure gRPC channel
        builder.Services.AddGrpcClient<MessageExchange.MessageExchangeClient>(options =>
        {
            options.Address = new Uri("https://localhost:7085");
            options.Address = new Uri("http://localhost:5121");
        });

        // Register gRPC client
        builder.Services.AddGrpcClient<MessageExchange.MessageExchangeClient>();

        // Register services
        builder.Services.AddTransient<IProcessLoggingService, ProcessLoggingService>();
        builder.Services.AddTransient<IMessageProcessApplication, MessageProcessApplication>();
        builder.Services.AddTransient<IMessageProcessServiceImpl, MessageProcessServiceImpl>();



        var provider = builder.Services.BuildServiceProvider();

        // Get the service instance from the provider
        var messageService = provider.GetRequiredService<IMessageProcessServiceImpl>();

        try
        {
            // Example usage - make sure your service methods are async if they perform I/O
            Console.WriteLine("Calling ReceiveDefaultMessageAsync...");
            var defaultMessage = messageService.ReceiveDefaultMessageAsync();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Message Received from Server:\n\tPrimaryId -> {defaultMessage.PrimaryId}\n\tSender -> {defaultMessage.Sender}\n\tMessageText -> {defaultMessage.MessageText}");
            Console.ForegroundColor = ConsoleColor.White;


            /*---------------------------------------------------*/


            Console.WriteLine("Calling SendProcessedMessageAsync...");
            var result = messageService.SendProcessedMessageAsync(defaultMessage);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Message Sent to Server:\n\tPrimaryId -> {result.MessageId}\n\tMessageLength -> {result.MessageLength}\n\tEngineType -> {result.EngineType}\n\tRegexFilter -> {result.RegexFilter}\n\tIsValid -> {result.IsValid}");
            Console.ForegroundColor = ConsoleColor.White;
            

        }
        catch (RpcException ex)
        {
            Console.WriteLine($"gRPC Error: {ex.Status.Detail}");
        }
    }
}