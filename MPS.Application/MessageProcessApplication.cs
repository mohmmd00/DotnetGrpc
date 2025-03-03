using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using MPS.Domian.Entities;
using MPS.Domian.Interfaces;

namespace MPS.Application
{
    public class MessageProcessApplication : IMessageProcessApplication
    {
        private string GetMacAddress()
        {
            return NetworkInterface.GetAllNetworkInterfaces() // Get all network adapters
                .Where(n => n.OperationalStatus == OperationalStatus.Up) // Filter for active interfaces
                .Select(n => n.GetPhysicalAddress().ToString()) // Extract the MAC address as a string
                .FirstOrDefault() ?? "000000000000"; // Default value if no MAC address is found
        }
        public Guid GenerateSystemGuid()
        {
            string macAddress = GetMacAddress(); // Get Mac Address


            /*the using part is for disposal 
            after creation of hashed id it will dispose and its good for security*/


            using (SHA256 sha256 = SHA256.Create()) // Use cryptography to create a hashed ID based on the MAC address
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(macAddress)); // Convert MAC address to bytes then hash it
                byte[] guidBytes = new byte[16]; // SHA-256 generates a 32-byte hash, so we take the first 16 bytes for a GUID
                Array.Copy(hash, guidBytes, 16);
                return new Guid(guidBytes);
            }
        }

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
            var isValid = regexDetails.Values.All(result => result == "Valid"); // set isvalid to ture if every single one of the regex filters were valid

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