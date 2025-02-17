using MPS.Domian.Entities;

namespace MPS.Domian.Interfaces
{
    public interface IMessageProcessApplication
    {
        MessageValidationResult ProcessMessageAsync(MessageInput message);
    }
}
