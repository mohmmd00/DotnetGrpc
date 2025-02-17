namespace MPS.Infrastructure.GRPC.Services
{
    using Grpc.Core;

    public class MessageProcessService : MessageProcessService.MessageProcessServiceBase
    {
        public override async Task ValidateMessageStream(
            IAsyncStreamReader<MessageRequest> requestStream,
            IServerStreamWriter<ValidationResponse> responseStream,
            ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync())
            {
                bool isValid = !string.IsNullOrEmpty(request.MessageText) && request.MessageText.Length <= 100;

                var response = new ValidationResponse
                {
                    Id = request.Id,
                    IsValid = isValid,
                    ValidationMessage = isValid ? "Message is valid." : "Message is invalid."
                };

                await responseStream.WriteAsync(response);
            }
        }
    }
}
