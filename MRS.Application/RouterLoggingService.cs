using Microsoft.Extensions.Logging;
using MRS.Domain.Interfaces;

namespace MRS.Application
{
    public class RouterLoggingService : IRouterLoggingService
    {
        private readonly ILogger<RouterLoggingService> _logger;

        public RouterLoggingService(ILogger<RouterLoggingService> logger)
        {
            _logger = logger;
        }

        public void MessageSentToLog(string? primaryId, string? sender, string? messageText)
        {
            _logger.LogInformation($"Message Sent to Client:\n\tPrimaryId -> {primaryId}\n\tSender -> {sender}\n\tMessageText -> {messageText}");
        }
        public void MessageReceivedToLog(string? primaryId, string? engineType, bool isValid, int messageLength)
        {
            _logger.LogInformation($"Message Received from Client:\n\tPrimaryId -> {primaryId}\n\tMessageLength -> {messageLength}\n\tEngineType -> {engineType}\n\tIsValid -> {isValid}");
        }
    }
}
