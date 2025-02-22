using Common.Proto;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MPS.Domian.Entities;
using MPS.Domian.Interfaces;

namespace MPS.Infrastructure.Grpc.Client
{
    public class MessageProcessServiceImpl : MessageExchange.MessageExchangeClient
    {
        private readonly IMessageProcessApplication _application;
        private readonly MessageExchange.MessageExchangeClient _client;


        public MessageProcessServiceImpl(IMessageProcessApplication application, MessageExchange.MessageExchangeClient client)
        {
            _application = application;
            _client = client;

        }

        public ProcessedMessage DefaultMessageOperation()
        {
            try
            {
                MessageFromproto newmsg = _client.SendDefaultMessage(new Empty());
                // انجام عملیات مورد نظر (اگر وجود داشته باشد)
                ProcessedMessage thisa = _application.ProcessMessage(new MPSMessage(newmsg.PrimaryId, newmsg.MessageText, newmsg.Sender));

                // برگرداندن پاسخ خالی
                return thisa;
            }
            catch (Exception ex)
            {
                // مدیریت خطا و ارسال خطای gRPC به کلاینت
                throw new RpcException(new Status(StatusCode.Internal, $"خطا در پردازش: {ex.Message}"));
            }
        }

        public Empty ProcessedMessageOperation()
        {
            var process = DefaultMessageOperation();

            _client.ReceiveProcessedMessage(new ProcessedMessageFromproto { MessageId = process.MessageId, IsValid = process.IsValid, EngineType = process.EngineType, MessageLength = process.MessageLength });
            return new Empty();
        }


    }
}
