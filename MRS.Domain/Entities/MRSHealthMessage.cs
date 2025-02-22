namespace MRS.Domain.Entities
{
    public class MRSHealthMessage
    {
        string? PrimaryId { get; set; }
        DateTime CurrentTime { get; set; }

        public MRSHealthMessage(string primaryId)
        {
            PrimaryId = primaryId;
            CurrentTime = DateTime.Now;
        }
    }
}
