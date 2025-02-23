namespace MRS.Domain.Entities
{
    public class MRSHealthMessage
    {
        public string? PrimaryId { get; set; }
        public string? CurrentTime { get; set; }

        public MRSHealthMessage(string primaryId)
        {
            PrimaryId = primaryId;
            CurrentTime = DateTime.Now.ToString();
        }
    }
}
