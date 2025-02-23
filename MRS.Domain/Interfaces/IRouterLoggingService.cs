namespace MRS.Domain.Interfaces
{
    public interface IRouterLoggingService
    {
        void MessageSentToLog(string? primaryId, string? sender, string? messageText);
        void MessageReceivedToLog(string? primaryId, string? engineType, bool isValid, int? messageLength);

    }
}
