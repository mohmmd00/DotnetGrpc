namespace Framework_0.CustomLoggingService
{
    public interface ILoggingService
    {
        void ProcessedMessageReceivedFromProcessByRouterToLog(string? primaryId, string? engineType, bool isValid, int? messageLength);
        void HealthMessageReceivedFromRouterByApi(string? primaryId, string? currentTime , int activeClient);
    }
}
