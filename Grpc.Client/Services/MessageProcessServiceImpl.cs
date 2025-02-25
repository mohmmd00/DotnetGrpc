using Common.Proto;
using Google.Protobuf.WellKnownTypes;
using MPS.Domian.Entities;
using MPS.Domian.Interfaces;

namespace Grpc.Client.Services
{
    public class MessageProcessServiceImpl : MessageExchange.MessageExchangeClient, IMessageProcessServiceImpl
    {
        private readonly IMessageProcessApplication _application;
        private readonly MessageExchange.MessageExchangeClient _client;


        public MessageProcessServiceImpl(IMessageProcessApplication application, MessageExchange.MessageExchangeClient client)
        {
            _application = application;
            _client = client;
        }

        public MPSMessage ReceiveDefaultMessage()
        {

            Empty Nothing = new Empty();


            var receivedmessage = _client.SendDefaultMessage(Nothing);
            var heartbeat = new HeartBeat() { PrimaryId = receivedmessage.PrimaryId,TimeCheck = DateTime.Now.ToString() };
            _client.AliveCheckMessage(heartbeat);

            MPSMessage mPSMessage = new MPSMessage(receivedmessage.PrimaryId, receivedmessage.Sender, receivedmessage.MessageText);

            return mPSMessage;
        }
        public ProcessedMessageFromproto SendProcessedMessage(MPSMessage mpsMessage)

        {
            var result = _application.ProcessMessage(mpsMessage);

            var ProtoProcessedMessage = new ProcessedMessageFromproto
            {
                MessageId = result.MessageId,
                MessageLength = result.MessageLength,
                EngineType = result.EngineType,
                IsValid = result.IsValid,
                // must bew created later its just a fixed data !!!
            };

            _client.ReceiveProcessedMessage(ProtoProcessedMessage);


            return ProtoProcessedMessage;
        }





    }
}
