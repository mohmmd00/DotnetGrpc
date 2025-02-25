﻿using System.Collections.Concurrent;
using Common.Proto;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace MRS.Infrastructure.Grpc.Services
{
    public interface IMessageRouterServiceImpl
    {
        Task<MessageFromproto> SendDefaultMessage(Empty request, ServerCallContext context);
        Task<Empty> ReceiveProcessedMessage(ProcessedMessageFromproto message, ServerCallContext context);
        Task<Empty> AliveCheckMessage(HeartBeat heartBeat, ServerCallContext context);
         ConcurrentDictionary<string, DateTime> GetActiveClients();
    }
}
