using MRS.Domain.Entities;

namespace MRS.Domain.Interfaces
{
    public interface IMessageRouterApplication
    {
        MRSMessage CreateMessage();
        MRSHealthMessage CreateHealthMessage();
        void LogSendMessage(MRSMessage message);
        void LogReceivedProcessedMessage(MRSProcessedMessage message);

    }
}
