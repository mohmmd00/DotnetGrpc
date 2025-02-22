using Common.Proto;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MPS.Domian.Entities;

namespace Grpc.Client
{
    public interface IMessageProcessServiceImpl
    {
        MPSMessage ReceiveDefaultMessageAsync();
        ProcessedMessageFromproto SendProcessedMessageAsync(MPSMessage mPSMessage);
    }
}
