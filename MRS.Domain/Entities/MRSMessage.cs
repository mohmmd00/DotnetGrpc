namespace MRS.Domain.Entities
{
    public class MRSMessage
    {
        public string? PrimaryId { get; set; } //must be random
        public string? Sender { get; set; } //must be random
        public string? MessageText { get; set; } //must be random

        public MRSMessage(string? primaryId, string? sender, string? messageText)
        {
            PrimaryId = primaryId;
            Sender = sender;
            MessageText = messageText;
        }
    }

}
