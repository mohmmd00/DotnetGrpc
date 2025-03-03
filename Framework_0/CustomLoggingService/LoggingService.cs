using Microsoft.Extensions.Logging;
namespace Framework_0.CustomLoggingService
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void HealthMessageReceivedByApiFromRouterToLog(string? primaryId, string? currentTime , int activeClient)
        {
            _logger.LogInformation($"Health Message Received - PrimaryId: {primaryId}, CurrentTime: {currentTime}, ActiveClients: {activeClient}");
        }

        public void ProcessedMessageReceivedByRouterFromProcessToLog(string? primaryId, string? engineType, bool isValid, int? messageLength)
        {
            if (isValid)
            {
                _logger.LogInformation($"Message Received from Client:\n\tPrimaryId -> {primaryId}\n\tMessageLength -> {messageLength}\n\tEngineType -> {engineType}\n\tIsValid -> {isValid}");
            }
            else
            {
                _logger.LogWarning    ($"Message Received from Client:\n\tPrimaryId -> {primaryId}\n\tMessageLength -> {messageLength}\n\tEngineType -> {engineType}\n\tIsValid -> {isValid}");
            }
        }
    }
}
