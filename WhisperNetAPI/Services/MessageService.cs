using Microsoft.EntityFrameworkCore;
using WhisperNetAPI.Data;
using WhisperNetAPI.Models;

namespace WhisperNetAPI.Services
{
    public class MessageService : IMessageService
    {
        private readonly DataContext _context;
        public MessageService(DataContext context)
        {
            _context = context;
        }

        public async Task<ResponseMessage<List<Message>>> GetAllMessage()
        {
            ResponseMessage<List<Message>>response = new ResponseMessage<List<Message>>();
            response.Data = await _context.Messages.ToListAsync();
            return response;
        }

        public async Task<ResponseMessage<string>> PostMessage(MessageDto? message,string userId)
        {
           ResponseMessage<string> response = new ResponseMessage<string>();
            if(message.message != null || message.message.Length <= 0) {
                response.Success = false;
                response.Response = "Message can't empty";
            }
            Message _message = new Message();
            _message.message = message.message;
            _message.UserId = userId;
            _context.Messages.Add(_message);
            await _context.SaveChangesAsync();
            return response;
        }

        public Task<ResponseMessage<string>> PostMessage(MessageDto message)
        {
            throw new NotImplementedException();
        }
    }
}
