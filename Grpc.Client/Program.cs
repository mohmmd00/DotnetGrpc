using Common.Proto;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MPS.Application;
using MPS.Domian.Interfaces;
using Grpc.Client.Services;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // تنظیمات کلاینت gRPC
        builder.Services.AddGrpcClient<MessageExchange.MessageExchangeClient>(options =>
        {
            options.Address = new Uri("https://localhost:7085"); // آدرس سرور رو درست وارد کن
        });

        builder.Services.AddTransient<IMessageProcessApplication, MessageProcessApplication>();
        builder.Services.AddTransient<IMessageProcessServiceImpl, MessageProcessServiceImpl>();

        var provider = builder.Services.BuildServiceProvider();
        var messageService = provider.GetRequiredService<IMessageProcessServiceImpl>();

        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            Console.WriteLine("\nCancellation requested. Stopping...");
            cts.Cancel();
            eventArgs.Cancel = true;
        };

        bool isServerReady = false;
        while (!isServerReady && !cts.Token.IsCancellationRequested)
        {
            try
            {
                Console.WriteLine($"{DateTime.Now:HH:mm:ss} - Trying to connect to gRPC server...");
                var testMessage = messageService.ReceiveDefaultMessage(); 
                isServerReady = true; 
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully connected to gRPC server!");
                Console.ResetColor();
            }
            catch (RpcException ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{DateTime.Now:HH:mm:ss} - Waiting for server: {ex.Status.Detail}");
                Console.ResetColor();
                await Task.Delay(5000, cts.Token); 
            }
        }

        try
        {
            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine($"{DateTime.Now:HH:mm:ss} - Calling ReceiveDefaultMessageAsync...");
                    var defaultMessage = messageService.ReceiveDefaultMessage();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Message Received from Server:\n\tPrimaryId -> {defaultMessage.PrimaryId}\n\tSender -> {defaultMessage.Sender}\n\tMessageText -> {defaultMessage.MessageText}");
                    Console.ResetColor();

                    Console.WriteLine($"{DateTime.Now:HH:mm:ss} - Calling SendProcessedMessageAsync...");
                    var result = messageService.SendProcessedMessage(defaultMessage);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Message Sent to Server:\n\tPrimaryId -> {result.MessageId}\n\tMessageLength -> {result.MessageLength}\n\tEngineType -> {result.EngineType}\n\tRegexFilter -> {result.RegexFilter}\n\tIsValid -> {result.IsValid}");
                    Console.ResetColor();

                    await Task.Delay(300, cts.Token);
                }
                catch (RpcException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{DateTime.Now:HH:mm:ss} - gRPC Error: {ex.Status.Detail}");
                    Console.ResetColor();
                    await Task.Delay(5000, cts.Token);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{DateTime.Now:HH:mm:ss} - General Error: {ex.Message}");
                    Console.ResetColor();
                    await Task.Delay(5000, cts.Token);
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation was cancelled");
        }
        finally
        {
            Console.WriteLine("Application shutting down...");
        }
    }
}