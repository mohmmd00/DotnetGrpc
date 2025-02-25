using System.Text.RegularExpressions;
using MPS.Domian.Entities;
using MPS.Domian.Interfaces;

namespace MPS.Application
{
    public class MessageProcessApplication : IMessageProcessApplication
    {
        public MPSProcessedMessage ProcessMessage(MPSMessage message)
        {

            // Perform regex validation and collect detailed results
            var regexDetails = new Dictionary<string, string>
            {
                { "HasDigits", Regex.IsMatch(message.MessageText, @"\d") ? "Valid" : "No digits found" },
                { "HasUppercase", Regex.IsMatch(message.MessageText, @"[A-Z]") ? "Valid" : "No uppercase letters found" },
                { "HasSpecialCharacter", !Regex.IsMatch(message.MessageText, @"[!@#$%^&*(),.?""{}|<>]") ? "Valid" : "No special characters found" },
                { "MinimumLength", message.MessageText.Length > 8 ? "Valid" : "Message is too short (minimum 8 characters required)" }
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