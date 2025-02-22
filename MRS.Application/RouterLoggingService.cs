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
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Message Sent to Client:\n\tPrimaryId -> {primaryId}\n\tSender -> {sender}\n\tMessageText -> {messageText}");
            Console.ResetColor();
        }
        public void MessageReceivedToLog(string? primaryId, string? engineType, bool isValid, int messageLength)
        {
            primaryId ??= "Unknown"; // Use "Unknown" if primaryId is null
            engineType ??= "Unknown"; // Use "Unknown" if engineType is null

            if (isValid) // Simplified condition (no need for == true)
            {
                _logger.LogInformation($"Message Received from Client:\n\tPrimaryId -> {primaryId}\n\tMessageLength -> {messageLength}\n\tEngineType -> {engineType}\n\tIsValid -> {isValid}");
            }
            else
            {
                _logger.LogWarning($"Message Received from Client:\n\tPrimaryId -> {primaryId}\n\tMessageLength -> {messageLength}\n\tEngineType -> {engineType}\n\tIsValid -> {isValid}");
            }
        }
    }
}
