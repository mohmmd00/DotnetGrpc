namespace MPS.Domian.Entities
{
    public class MessageValidationResult
    {

        public Guid MessageId { get; set; } // ID of the validated message
        public string? EngineType { get; set; } // Type of validation engine used (e.g., RegexEngine)
        public int MessageLength { get; set; } // Length of the message text
        public bool IsValid { get; set; } // Validation status
        public Dictionary<string, bool>? AdditionalFields { get; set; } // Additional validation details


    }
}
