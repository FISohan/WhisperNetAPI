namespace WhisperNetAPI.Models
{
    public class ResponseMessage<T>
    {
        public string Response { get; set; } = string.Empty;
        public bool Success { get; set; } = true;
        public T? Data { get; set; }
    }
}
