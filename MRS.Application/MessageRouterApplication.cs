using MRS.Domain.Entities;
using MRS.Domain.Interfaces;

namespace MRS.Application
{
    public class MessageRouterApplication : IMessageRouterApplication
    {

        public MRSMessage CreateMessage(string primaryId)
        {
            Random random = new();
            var randomId = primaryId;
            var randomText = GenerateRandomString(random.Next(6, 25));
            var randomSender = GenerateRandomString(random.Next(5, 15));

            var message = new MRSMessage(primaryId: randomId, sender: randomSender, messageText: randomText);

            return message;
        }
        public MRSHealthMessage CreateHealthMessage(string primaryId)
        {
            var newHealthMessage = new MRSHealthMessage(primaryId: primaryId);
            return newHealthMessage;
        }

        #region Creation of Random message

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

    }
}