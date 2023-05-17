namespace WhisperNetAPI.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public bool isSeen { get; set; } = false;
    }
}
