namespace Framework_0.CustomLoggingService
{
    public interface ILoggingService
    {
        void ProcessedMessageReceivedByRouterFromProcessToLog(string? primaryId, string? engineType, bool isValid, int? messageLength);
        void HealthMessageReceivedByApiFromRouterToLog(string? primaryId, string? currentTime , int activeClient);
    }
}
