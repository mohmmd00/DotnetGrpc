using Microsoft.Extensions.Logging;
using MPS.Domian.Interfaces;

namespace MPS.Application
{
    public class ProcessLoggingService : IProcessLoggingService
    {
        private readonly ILogger<ProcessLoggingService> _logger;

        public ProcessLoggingService(ILogger<ProcessLoggingService> logger)
        {
            _logger = logger;
        }
        public void LogMessageFiltered(string id, string? engineType, int messageLength, bool isValid)
        {
            _logger.LogInformation($"messageCreatedBy {id} -> enginetype : {engineType} , messageLength : {messageLength} , isvalid : {isValid} ");
        }
    }
}
