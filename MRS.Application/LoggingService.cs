using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MRS.Domain.Interfaces;

namespace MRS.Application
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void LogMessageCreation(Guid primaryId, string sender, string messageText)
        {
            _logger.LogInformation($"messageCreatedBy {primaryId} -> sender : {sender} , messageText : {messageText}");
        }
    }
}
