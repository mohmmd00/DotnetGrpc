using Grpc.Core;

namespace MRS.Infrastructure.GRPC.Services
{
    public class RouterService : ProcessInputMessage.ProcessInputMessageBase
    {

        public override async Task Introduce(
            IAsyncStreamReader<SendInputMessage> requestStream,
            IServerStreamWriter<GetResultMessage> responseStream,
            ServerCallContext context)
        {
            while (await requestStream.MoveNext(context.CancellationToken))
            {
                var inputMessage = requestStream.Current;

                // Process the incoming message (example logic)
                string engineName = "TextProcessingEngine";
                uint messageLength = (uint)(inputMessage.Textmessage?.Length ?? 0);
                bool isValid = messageLength > 0;

                // Create the response message
                var resultMessage = new GetResultMessage
                {
                    Id = inputMessage.Id,
                    Engine = engineName,
                    Messagelength = messageLength,
                    Isvalid = isValid
                };

                // Send the response back to the client
                await responseStream.WriteAsync(resultMessage);
            }
        }
    }
}
