using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using MRS.Domain.Entities;
using MRS.Domain.Interfaces;

namespace MRS.Application
{
    public class MessageRouterApplication : IMessageRouterApplication
    {
        private readonly IRouterLoggingService _loggingService;
        public MessageRouterApplication(IRouterLoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public MRSMessage CreateMessage()
        {
            Random random = new();
            var randomId = GenerateSystemGuid().ToString();
            var randomText = GenerateRandomString(random.Next(6 ,25));
            var randomSender = GenerateRandomString(random.Next(5 ,14));

            var message = new MRSMessage(primaryId: randomId, sender: randomSender, messageText: randomText);

            return message;
        }
        public MRSHealthMessage CreateHealthMessage()
        {
            var randomId = GenerateSystemGuid().ToString();
            var newHealthMessage = new MRSHealthMessage(primaryId: randomId);
            return newHealthMessage;
        }

        #region Creation of Random message
        private Guid GenerateSystemGuid()
        {
            string macAddress = GetMacAddress();
            using (SHA256 sha256 = SHA256.Create()) // Use cryptography to create a hashed ID based on the MAC address
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(macAddress)); // Convert MAC address to bytes and hash it
                byte[] guidBytes = new byte[16]; // SHA-256 generates a 32-byte hash, so we take the first 16 bytes for a GUID
                Array.Copy(hash, guidBytes, 16);
                return new Guid(guidBytes);
            }
        }
        private string GetMacAddress()
        {
            return NetworkInterface.GetAllNetworkInterfaces() // Get all network adapters
                .Where(n => n.OperationalStatus == OperationalStatus.Up) // Filter for active interfaces
                .Select(n => n.GetPhysicalAddress().ToString()) // Extract the MAC address as a string
                .FirstOrDefault() ?? "000000000000"; // Default value if no MAC address is found
        }
        private string GenerateRandomString(int length)
        {
            const string AllChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; // Character set for random strings
            Random random = new();
            return new string( // Convert array to string
                Enumerable.Repeat(AllChars, length) // Repeat the character set 'length' times
                .Select(s => s[random.Next(s.Length)]) // Select a random character from the set
                .ToArray()); // Combine the selected characters into an array
        }
        #endregion


        public void LogSendMessage(MRSMessage message)
        {
            _loggingService.MessageSentToLog(message?.PrimaryId, message?.Sender, message?.MessageText);
        }
        public void LogReceivedProcessedMessage(MRSProcessedMessage message)
        {
            _loggingService.MessageReceivedToLog(message.MessageId, message.EngineType, message.IsValid, message.MessageLength);
        }

    }
}