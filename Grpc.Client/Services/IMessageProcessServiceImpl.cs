using Common.Proto;
using MPS.Domian.Entities;

namespace Grpc.Client.Services
{
    public interface IMessageProcessServiceImpl
    {
        MPSMessage ReceiveDefaultMessageAsync();
        ProcessedMessageFromproto SendProcessedMessageAsync(MPSMessage mPSMessage);
    }
}
