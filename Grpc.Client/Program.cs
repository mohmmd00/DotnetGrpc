using Common.Proto;
using Grpc.Client.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MPS.Application;
using MPS.Domian.Interfaces;
class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddGrpcClient<MessageExchange.MessageExchangeClient>(options =>
            options.Address = new Uri("https://localhost:7085")); // grpc server address/port


        builder.Services.AddTransient<IMessageProcessApplication, MessageProcessApplication>();
        builder.Services.AddTransient<IMessageProcessServiceImpl, MessageProcessServiceImpl>();

        using var app = builder.Build();
        var messageService = app.Services.GetRequiredService<IMessageProcessServiceImpl>();

        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, @event) =>
        {
            cts.Cancel();
            @event.Cancel = true;
        };

        bool isServerReady = false;
        while (!isServerReady && !cts.Token.IsCancellationRequested)
        {
            try
            {
                messageService.ReceiveDefaultMessage();
                isServerReady = true;
                Console.WriteLine($"{DateTime.Now:HH:mm:ss} - Connected to gRPCC server");
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"{DateTime.Now:HH:mm:ss} - Waiting for server: {ex.Status.Detail}");
                await Task.Delay(5000, cts.Token);
            }
        }

        try
        {
            while (!cts.Token.IsCancellationRequested)
            {
                var defaultMessage = messageService.ReceiveDefaultMessage();
                Console.WriteLine($"Received:\n\tPrimaryId: {defaultMessage.PrimaryId}\n\tText: {defaultMessage.MessageText}\n\tSender: {defaultMessage.Sender}");

                var result = messageService.SendProcessedMessage(defaultMessage);
                Console.WriteLine($"Sent:\n\tPrimaryId: {result.PrimaryId}\n\tIsValid: {result.IsValid}");

                await Task.Delay(300, cts.Token);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation cancelled");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Critical error: {ex.Message}");
        }
    }
}