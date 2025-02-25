namespace MMS.Domain.Entities
{
    public class MMSMessage

    {
        public string? PrimaryId    { get; set; }
        public string? CurrentTime { get; set; }
        public int ActiveClients { get; set; }

        public MMSMessage(string? primaryId, string? currentTime,int activeClients)
        {
            PrimaryId = primaryId;
            CurrentTime = currentTime;
            ActiveClients = activeClients;
        }
    }
}
