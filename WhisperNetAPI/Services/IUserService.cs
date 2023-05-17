using WhisperNetAPI.Models;

namespace WhisperNetAPI.Services
{
    public interface IUserService
    {
        public Task<ResponseMessage<string>> Register(UserDto user);
        public Task<ResponseMessage<string>>Login(UserDto user);
        public Task<ResponseMessage<UserView>> GetUser(string username);
    }
}
