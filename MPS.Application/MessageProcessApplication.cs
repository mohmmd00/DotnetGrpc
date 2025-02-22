using System.Text.RegularExpressions;
using MPS.Domian.Entities;
using MPS.Domian.Interfaces;

namespace MPS.Application
{
    public class MessageProcessApplication : IMessageProcessApplication
    {
        private readonly IProcessLoggingService _loggingService;

        public MessageProcessApplication(IProcessLoggingService loggingService)
        {
            _loggingService = loggingService;
        }
        public MPSProcessedMessage ProcessMessage(MPSMessage message)
        {

            // Perform regex validation and collect detailed results
            var regexDetails = new Dictionary<string, string>
            {
                { "HasDigits", Regex.IsMatch(message.MessageText, @"\d") ? "Valid" : "No digits found" },
                { "HasUppercase", Regex.IsMatch(message.MessageText, @"[A-Z]") ? "Valid" : "No uppercase letters found" }
            };

            // Determine if the message is valid based on all checks
            var isValid = regexDetails.Values.All(result => result == "Valid");

            // Create the processed message
            var result = new MPSProcessedMessage
            {
                MessageId = message.PrimaryId,
                EngineType = "RegexEngine",
                MessageLength = message.MessageText.Length,
                IsValid = isValid,
                RegexFilter = regexDetails,

            };

            return result;
        }
    }
}