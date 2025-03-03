using Common.Proto;
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
            var systemId =_application.GenerateSystemGuid().ToString();
            var Introductionmessage = new IntroduceMessageFromProto()
            {
                PrimaryId = systemId,
                EngineType = "RegexEngine"
            };
            var receivedmessage = _client.SendMessage(Introductionmessage);
            var heartbeat = new HeartBeat() 
            { 
                PrimaryId = receivedmessage.PrimaryId,
                TimeCheck = DateTime.Now.ToString() 
            };
                                                    // send a heartbeat so grpc server unsures
            _client.AliveCheckMessage(heartbeat);   // that client is alive every time client asks for new message to processs
                                                    // i dont know why i need this but ok 


            MPSMessage mPSMessage = new MPSMessage(receivedmessage.PrimaryId, receivedmessage.Sender, receivedmessage.MessageText);

            return mPSMessage;
        }
        public ProcessedMessageFromproto SendProcessedMessage(MPSMessage mpsMessage)

        {
            var result = _application.ProcessMessage(mpsMessage);

            var ProtoProcessedMessage = new ProcessedMessageFromproto
            {
                PrimaryId = result.MessageId,
                MessageLength = result.MessageLength,
                EngineType = result.EngineType,
                IsValid = result.IsValid,
                // engine type must bew created later its just a blank data !!!
            };

            _client.ReceiveProcessedMessage(ProtoProcessedMessage);


            return ProtoProcessedMessage;
        }





    }
}
