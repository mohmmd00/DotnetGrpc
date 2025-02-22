namespace MPS.Domian.Entities
{
    public class MPSMessage
    {
        public string? PrimaryId { get; set; }
        public string? Sender { get; set; }
        public string? MessageText { get; set; }

        public MPSMessage(string primaryId, string sender, string messageText)
        {
            PrimaryId = primaryId;
            Sender = sender;
            MessageText = messageText;
        }
    }
}
