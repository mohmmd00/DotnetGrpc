using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using Microsoft.Extensions.Logging;
using MRS.Domain.Entities;
using MRS.Domain.Interfaces;

namespace MRS.Application
{
    public class MessageRouterApplication : IMessageRouterApplication
    {
        private readonly System.Timers.Timer _timer;
        private readonly ILoggingService _loggingService;
        public MessageRouterApplication(ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _timer = new System.Timers.Timer(200);
            _timer.AutoReset = true;
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();

        }
        public void OnTimedEvent(object? source, ElapsedEventArgs e)
        {
            CreateMessage();
        }
        public void StopTimer()
        {
            _timer.Stop();
            _timer.Dispose();
        }
        public Message CreateMessage()
        {
            var message = new Message(
                primaryId: GenerateSystemGuid(),
                sender: GenerateRandomString(10), // 10 charachters long !!!
                messageText: GenerateRandomString(20));
            _loggingService.LogMessageCreation(message.PrimaryId, message.Sender, message.MessageText);
            return message;
        }
        private Guid GenerateSystemGuid()
        {
            string macAddress = GetMacAddress();
            using (SHA256 sha256 = SHA256.Create()) /* using Cryptography for creating a new crypted!!! for id using macaddress*/
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(macAddress)/*turned maccaddress to bytes then hash it then put it into buyr[] hash*/);

                /*SHA-256 generates a 32-byte hash so we need to put it into 16 bytes because guid only gets 16-bytes !!!! very important */
                byte[] guidBytes = new byte[16];
                Array.Copy(hash, guidBytes, 16);

                return new Guid(guidBytes);
            }
        }
        private string GetMacAddress()
        {
            return NetworkInterface.GetAllNetworkInterfaces()/*get all network adapeters*/
                .Where(n => n.OperationalStatus == OperationalStatus.Up)/*find all of network adapters that are up and running*/
                    .Select(n => n.GetPhysicalAddress().ToString())/*select that specific network adapter's physical address then turned into string(without '-')*/
                        .FirstOrDefault() ?? "000000000000";/*get the first mac address*/
        }
        private string GenerateRandomString(int length)
        {
            const string Allchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";/*all hte charecters */
            Random random = new();
            return new string/*covert array to string*/(Enumerable.Repeat(Allchars, length)/*stick some chars toghethter length! times*/
                   .Select(s => s[random.Next(s.Length)])/*Selects a random character from chars length times*/.ToArray()) /*stick legth chars toghether */;
        }
    }
}
