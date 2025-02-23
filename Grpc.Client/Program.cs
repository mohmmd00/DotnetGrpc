using Common.Proto;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MPS.Application;
using Grpc.Core;
using MPS.Domian.Interfaces;
using Grpc.Client.Services;

class Program
{
    static async Task Main(string[] args)
    {
        // Create a builder for configuring the web application and its services
        var builder = WebApplication.CreateBuilder(args);

        // Configure the gRPC client to connect to the server
        // Only one address is used here (remove duplicate addresses)
        builder.Services.AddGrpcClient<MessageExchange.MessageExchangeClient>(options =>
        {
            options.Address = new Uri("https://localhost:7085"); // Replace with the correct server address
        });

        // Register services for dependency injection
        // These services will be used throughout the application :)
        builder.Services.AddTransient<IProcessLoggingService, ProcessLoggingService>();
        builder.Services.AddTransient<IMessageProcessApplication, MessageProcessApplication>();
        builder.Services.AddTransient<IMessageProcessServiceImpl, MessageProcessServiceImpl>();

        // Build the service provider to resolve registered services
        var provider = builder.Services.BuildServiceProvider();

        // Resolve the message service from the service provider
        var messageService = provider.GetRequiredService<IMessageProcessServiceImpl>();

        // Create a cancellation token source to handle graceful shutdown (e.g., Ctrl+C)
        using var cts = new CancellationTokenSource(); //with cntrl + c you can stop the program

        // Register an event handler for Ctrl+C to cancel the operation
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            Console.WriteLine("\nCancellation requested. Stopping...");
            cts.Cancel(); // Trigger cancellation
            eventArgs.Cancel = true; // Prevent the application from terminating immediately
        };

        try
        {
            // Main loop: Continuously send requests to the server every 200ms
            while (!cts.Token.IsCancellationRequested) // Check if cancellation is requested
            {
                try
                {
                    // Step 1: Receive a default message from the server
                    Console.WriteLine($"{DateTime.Now:HH:mm:ss} - Calling ReceiveDefaultMessageAsync...");
                    var defaultMessage = messageService.ReceiveDefaultMessageAsync(); // Call the gRPc and get defualt message
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Message Received from Server:\n\tPrimaryId -> {defaultMessage.PrimaryId}\n\tSender -> {defaultMessage.Sender}\n\tMessageText -> {defaultMessage.MessageText}");
                    Console.ResetColor();

                    // Step 2: Send the processed message back to the server
                    Console.WriteLine($"{DateTime.Now:HH:mm:ss} - Calling SendProcessedMessageAsync...");
                    var result = messageService.SendProcessedMessageAsync(defaultMessage); // Call the gRPC method
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Message Sent to Server:\n\tPrimaryId -> {result.MessageId}\n\tMessageLength -> {result.MessageLength}\n\tEngineType -> {result.EngineType}\n\tRegexFilter -> {result.RegexFilter}\n\tIsValid -> {result.IsValid}");
                    Console.ResetColor();

                    // Wait for 300ms before sending the next request
                    await Task.Delay(300, cts.Token); // wait 300 ms afgetr sending another
                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
                {
                    // Handle cancellation specifically (e.g., if the server cancels the request)
                    Console.WriteLine("Request was cancelled.");
                }
                catch (RpcException ex)
                {
                    // Handle gRPC-specific errors (e.g., server unavailable, network issues)
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"gRPC Error: {ex.Status.Detail}");
                    Console.ResetColor();
                    await Task.Delay(5000, cts.Token); // Wait 5 seconds before retrying
                }
                catch (Exception ex)
                {
                    // Handle any other unexpected errors
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"General Error: {ex.Message}");
                    Console.ResetColor();
                    await Task.Delay(5000, cts.Token); // Wait 5 secondss before retrying
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Handle the case where the operation is cancelled (e.g., Ctrl+C)
            Console.WriteLine("Operation was cancelled");
        }
        finally
        {
            // Cleanup: This block runs when the application is shutting down
            Console.WriteLine("Application shutting down...");
        }
    }
}