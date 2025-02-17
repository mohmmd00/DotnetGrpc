namespace MRS.Domain.Entities
{
    public class Message
    {
        public Guid PrimaryId { get; set; } //must be random
        public string? Sender { get; set; } //must be random
        public string? MessageText { get; set; } //must be random

        public Message(Guid primaryId, string sender, string messageText)
        {
            PrimaryId = primaryId;
            Sender = sender;
            MessageText = messageText;
        }
    }

}
