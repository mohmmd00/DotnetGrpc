using MRS.Domain.Entities;

namespace MRS.Domain.Interfaces
{
    public interface IMessageRouterApplication
    {
        MRSMessage CreateMessage(string primaryId);
        MRSHealthMessage CreateHealthMessage(string primaryId);

    }
}
