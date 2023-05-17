namespace WhisperNetAPI.Models
{
    public class MessageDto
    {
        public string message { get; set; } = string.Empty;
        public bool isSeen { get; set; } = false;
    }
}
