using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhisperNetAPI.Models;
using WhisperNetAPI.Services;

namespace WhisperNetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
  
        [HttpPost("Register")]
        public async Task<ActionResult<ResponseMessage<string>>>Register(UserDto user)
        {
            ResponseMessage<string>respone = await _userService.Register(user);
            if (respone.Success == false) return BadRequest(respone);
            return Ok(respone);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ResponseMessage<string>>> Login(UserDto user)
        {
            ResponseMessage<string>response = await _userService.Login(user);
            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("{username}")]
        public async Task<ActionResult<ResponseMessage<UserView>>>Get(string username)
        {
            ResponseMessage<UserView> response = await _userService.GetUser(username);
            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }
    }
}
