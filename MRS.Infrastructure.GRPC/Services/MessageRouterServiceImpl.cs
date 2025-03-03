using Grpc.Core;
using Common.Proto;
using MRS.Domain.Entities;
using MRS.Domain.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Framework_0.CustomLoggingService;
using System;
using System.Collections.Concurrent;

namespace MRS.Infrastructure.Grpc.Services
{
    public class MessageRouterServiceImpl : MessageExchange.MessageExchangeBase,IMessageRouterServiceImpl
    {
        private readonly IMessageRouterApplication _routerApplication;
        private readonly HttpClient _httpClient;
        private readonly ILoggingService _loggingService;



        public ConcurrentDictionary<string, DateTime> ListOfActiveClients = new(); // create a cuncurrect dictionary fort keeping active clients 



        public MessageRouterServiceImpl(IMessageRouterApplication application, HttpClient httpClient, ILoggingService loggingService)
        {
            _routerApplication = application;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7128");
            _loggingService = loggingService;
        }
        public override Task<MessageFromproto> SendMessage(IntroduceMessageFromProto message, ServerCallContext context)
        {

            /*gets the intro messaqge thgen send a message from proto*/

            var result = _routerApplication.CreateMessage(message.PrimaryId); // send received id from client to application layer for creating a message and sender based on client mac adderss 

            return Task.FromResult(new MessageFromproto
            {
                PrimaryId = result.PrimaryId,
                MessageText = result.MessageText,
                Sender = result.Sender
            });
        }

        public override Task<Empty> ReceiveProcessedMessage(ProcessedMessageFromproto message, ServerCallContext context)
        {
            var processed = new MRSProcessedMessage
            {
                MessageId = message.PrimaryId,
                EngineType = message.EngineType,
                IsValid = message.IsValid,
                MessageLength = message.MessageLength
            };
            _loggingService.ProcessedMessageReceivedByRouterFromProcessToLog(processed.MessageId, processed.EngineType, processed.IsValid, processed.MessageLength);

            return Task.FromResult(new Empty());
        }


        public override Task<Empty> AliveCheckMessage(HeartBeat heartBeat, ServerCallContext context)
        {

            /*every time a clinet sends a heartbeat this function add it to concurrentdictionary */
            string clientId = heartBeat.PrimaryId;
            DateTime lastHeartbeat = DateTime.Parse(heartBeat.TimeCheck);// because time is string we need toi parse it 

            ListOfActiveClients.AddOrUpdate(
                clientId,
                lastHeartbeat,
                (key, oldValue) => lastHeartbeat
            );
            return Task.FromResult(new Empty());
        }


    }
}