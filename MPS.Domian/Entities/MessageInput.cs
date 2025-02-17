namespace MPS.Domian.Entities
{
    public class MessageInput
    {
        public Guid PrimaryId { get; set; }
        public string? Sender { get; set; }
        public string? MessageText { get; set; }

        public MessageInput(Guid primaryId, string sender, string messageText)
        {
            PrimaryId = primaryId;
            Sender = sender;
            MessageText = messageText;
        }
    }
}
