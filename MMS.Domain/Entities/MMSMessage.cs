namespace MMS.Domain.Entities
{
    public class MMSMessage

    {
        public string? PrimaryId    { get; set; }
        public string? CurrentTime { get; set; }

        public MMSMessage(string? primaryId, string? currentTime)
        {
            PrimaryId = primaryId;
            CurrentTime = currentTime;
        }
    }
}
