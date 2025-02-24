using Microsoft.Extensions.Logging;
namespace Framework_0.CustomLoggingService
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            this.logger = logger;
        }

        public void ProcessedMessageReceivedFromProcessByRouterToLog(string? primaryId, string? engineType, bool isValid, int? messageLength)
        {
            throw new NotImplementedException();
        }
    }
}
