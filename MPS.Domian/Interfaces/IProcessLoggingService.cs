namespace MPS.Domian.Interfaces
{
    public interface IProcessLoggingService
    {
        void LogMessageFiltered(string id, string? engineType, int messageLength , bool isValid);
    }
}
