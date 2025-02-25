using Common.Proto;
using MPS.Domian.Entities;

namespace Grpc.Client.Services
{
    public interface IMessageProcessServiceImpl
    {
        MPSMessage ReceiveDefaultMessage();
        ProcessedMessageFromproto SendProcessedMessage(MPSMessage mPSMessage);
    }
}
