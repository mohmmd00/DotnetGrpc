using MPS.Domian.Entities;

namespace MPS.Domian.Interfaces
{
    public interface IMessageProcessApplication
    {
        MPSProcessedMessage ProcessMessage(MPSMessage message);
    }
}
