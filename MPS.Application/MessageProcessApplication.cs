using System.Text.RegularExpressions;
using MPS.Domian.Entities;
using MPS.Domian.Interfaces;

namespace MPS.Application
{
    public class MessageProcessApplication : IMessageProcessApplication
    {
        public MessageValidationResult ProcessMessageAsync(MessageInput message)
        {
            var regexMatches = new Dictionary<string, bool>
            {
                { "HasDigits", Regex.IsMatch(message.MessageText, @"\d") },
                { "HasUppercase", Regex.IsMatch(message.MessageText, @"[A-Z]") }
            };
            var messageLength = message.MessageText.Length;

            var result = new MessageValidationResult()
            {
                MessageId = message.PrimaryId,
                EngineType = "RegexEngine",
                MessageLength = messageLength,
                IsValid = true,
                AdditionalFields = regexMatches
            };

            return result;
        }
    }
}
