using AutoMapper;
using WhisperNetAPI.Models;

namespace WhisperNetAPI
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserView>();
        }
    }
}
