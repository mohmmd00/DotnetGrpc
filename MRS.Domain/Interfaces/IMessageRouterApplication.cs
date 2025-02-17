using MRS.Domain.Entities;

namespace MRS.Domain.Interfaces
{
    public interface IMessageRouterApplication
    {
        Message CreateMessage();
    }
}
