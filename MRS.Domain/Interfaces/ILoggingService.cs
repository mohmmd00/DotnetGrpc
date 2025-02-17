namespace MRS.Domain.Interfaces
{
    public interface ILoggingService
    {
        void LogMessageCreation(Guid primaryId, string? sender, string? messageText);
    }
}
