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
    public class MessageRouterServiceImpl : MessageExchange.MessageExchangeBase, IMessageRouterServiceImpl
    {
        private readonly IMessageRouterApplication _routerApplication;
        private readonly HttpClient _httpClient;
        private readonly ILoggingService _loggingService;






        public ConcurrentDictionary<string, DateTime> ListOfActiveClients = new();






        public MessageRouterServiceImpl(IMessageRouterApplication application, HttpClient httpClient, ILoggingService loggingService)
        {
            _routerApplication = application;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7128");
            _loggingService = loggingService;
        }

        public override Task<MessageFromproto> SendDefaultMessage(Empty request, ServerCallContext context)
        {
            var result = _routerApplication.CreateMessage();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Message Sent to Client:\n\tPrimaryId -> {result.PrimaryId}\n\tSender -> {result.Sender}\n\tMessageText -> {result.MessageText}");
            Console.ResetColor();

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
                MessageId = message.MessageId,
                EngineType = message.EngineType,
                IsValid = message.IsValid,
                MessageLength = message.MessageLength
            };
            _loggingService.ProcessedMessageReceivedFromProcessByRouterToLog(processed.MessageId, processed.EngineType, processed.IsValid, processed.MessageLength);

            return Task.FromResult(new Empty());
        }

        public ConcurrentDictionary<string, DateTime> GetActiveClients()
        {
            return ListOfActiveClients;
        }
        public override Task<Empty> AliveCheckMessage(HeartBeat heartBeat, ServerCallContext context)
        {
            string clientId = heartBeat.PrimaryId;
            DateTime lastHeartbeat = DateTime.Parse(heartBeat.TimeCheck);

            ListOfActiveClients.AddOrUpdate(
                clientId,
                lastHeartbeat,
                (key, oldValue) => lastHeartbeat
            );

            Console.ForegroundColor = ConsoleColor.Red;
            int clientcount = ListOfActiveClients.Count;
            Console.WriteLine($"There is/are {clientcount} client(s) Active connected to grpcServer");
            Console.ResetColor();

            foreach (var item in ListOfActiveClients)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"client number: {clientcount}  client id -> {item.Key} and lastheartbeat -> {item.Value}");
                Console.ResetColor();
            }


            return Task.FromResult(new Empty());
        }


    }
}