using Grpc.Net.Client;
using MessageService;

public class MessageRouterService
{
    private readonly MessageProcessService.MessageProcessServiceClient _client;

    public MessageRouterService(string serverAddress)
    {
        var channel = GrpcChannel.ForAddress(serverAddress);
        _client = new MessageProcessService.MessageProcessServiceClient(channel);
    }

    public ValidationResponse SendMessage(MessageRequest request)
    {
        // ارسال پیام به MessageProcessSystem
        return _client.ValidateMessage(request);
    }
}