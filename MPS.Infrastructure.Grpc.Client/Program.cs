using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Common.Proto;
using Google.Protobuf.WellKnownTypes;

class Program
{
    static async Task Main(string[] args)
    {
        // Create a gRPC channel to the server
        using var channel = GrpcChannel.ForAddress("https://localhost:7085");
        var client = new MessageExchange.MessageExchangeClient(channel);

        try
        {
            // Call the SendDefaultMessage method
            Console.WriteLine("Calling SendDefaultMessage...");
            var defaultMessageResponse = await client.SendDefaultMessageAsync(new Empty());
            Console.WriteLine($"Received Default Message: {defaultMessageResponse.MessageText}");

            // Call the SendProcessedMessage method
            Console.WriteLine("Calling SendProcessedMessage...");
            var processedMessageResponse = await client.SendProcessedMessageAsync(new Empty());
            Console.WriteLine($"Received Processed Message: {processedMessageResponse.MessageId}");

            //// Call the ReceiveProcessedMessage method
            //Console.WriteLine("Calling ReceiveProcessedMessage...");
            //var processedMessage = new ProcessedMessageFromproto
            //{
            //    MessageId = "123",
            //    IsValid = true,
            //    EngineType = "EngineA",
            //    MessageLength = 10
            //};
            //await client.ReceiveProcessedMessageAsync(processedMessage);
            //Console.WriteLine("Processed Message Sent Successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}