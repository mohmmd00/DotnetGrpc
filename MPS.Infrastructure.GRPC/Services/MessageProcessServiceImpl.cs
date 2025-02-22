using Grpc.Core;
using Common.Proto;
using Google.Protobuf.WellKnownTypes;
using MPS.Domian.Interfaces;
using MPS.Domian.Entities;


namespace MPS.Infrastructure.Grpc.Services
{
    public class MessageProcessServiceImpl : MessageExchange.MessageExchangeClient
    {
        private readonly IMessageProcessApplication _messageProcessApplication;

        public MessageProcessServiceImpl(IMessageProcessApplication messageProcessApplication)
        {
            _messageProcessApplication = messageProcessApplication;
        }

        public Task<ProcessingResult> SendToProcess(MPSMessage request, ServerCallContext context)
        {
            var processedMessage = _messageProcessApplication.ProcessMessageAsync(request);

            return Task.FromResult(new ProcessingResult
            {
                MessageId = processedMessage.MessageId,
                EngineType = processedMessage.EngineType,
                IsValid = processedMessage.IsValid,
                MessageLength = processedMessage.MessageLength,
                //RegexFilter = processedMessage.RegexFilter

            });
        }

        public Task<Empty> ReceiveProcessedMessage(ProcessingResult request, ServerCallContext context)
        {
            Console.WriteLine($"Received processing result from MRS: {request.MessageLength}");
            return Task.FromResult(new Empty());
        }
    }
}