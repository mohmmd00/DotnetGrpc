using Grpc.Core;
using Common.Proto;
using MRS.Domain.Entities;
using MRS.Domain.Interfaces;
using Google.Protobuf.WellKnownTypes;

namespace MRS.Infrastructure.Grpc.Services
{
    public class MessageRouterServiceImpl : MessageExchange.MessageExchangeBase
    {
        private readonly IMessageRouterApplication _routerApplication;
        //private readonly MessageExchange.MessageExchangeBase _server;

        public MessageRouterServiceImpl(IMessageRouterApplication application)
        {
            _routerApplication = application;
        }
        public override Task<MessageFromproto> SendDefaultMessage(Empty request, ServerCallContext context)
        {
            // Call business logic asynchronously
            var result = _routerApplication.CreateMessage();
            _routerApplication.LogReceivedMessage(result); //make log in server side -- created message before sending to client side 


            // Map the result to the protocol buffer message
            return Task.FromResult(new MessageFromproto
            {
                PrimaryId = result.PrimaryId,
                MessageText = result.MessageText,
                Sender = result.Sender
            });
        }

        public override Task<Empty> ReceiveProcessedMessage(ProcessedMessageFromproto message , ServerCallContext context)
        {
            var processed =new MRSProcessedMessage
            {
                MessageId = message.MessageId,
                EngineType = message.EngineType,
                IsValid = message.IsValid,
                MessageLength = message.MessageLength

            };
            _routerApplication.LogReceivedProcessedMessage(processed); // make log in server side -- received processed message after processing in client side 

            return Task.FromResult(new Empty());
        }
    }
}