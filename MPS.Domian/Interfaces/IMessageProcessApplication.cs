using MPS.Domian.Entities;

namespace MPS.Domian.Interfaces
{
    public interface IMessageProcessApplication
    {
        Guid GenerateSystemGuid();
        MPSProcessedMessage ProcessMessage(MPSMessage message);
    }
}
