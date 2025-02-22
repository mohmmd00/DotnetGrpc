using MRS.Domain.Entities;

namespace MRS.Domain.Interfaces
{
    public interface IMessageRouterApplication
    {
        MRSMessage CreateMessage();
        void LogReceivedMessage(MRSMessage message);
        void LogReceivedProcessedMessage(MRSProcessedMessage message);

    }
}
