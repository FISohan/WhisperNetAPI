using WhisperNetAPI.Models;

namespace WhisperNetAPI.Services
{
    public interface IMessageService
    {
      public Task<ResponseMessage<List<Message>>> GetAllMessage();
      public Task<ResponseMessage<string>> PostMessage(MessageDto message);
    }
}
