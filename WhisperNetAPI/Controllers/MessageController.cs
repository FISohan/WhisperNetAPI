using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhisperNetAPI.Models;
using WhisperNetAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WhisperNetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        // GET: api/<MessageController>
        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseMessage<string>>>Post(MessageDto message)
        {
            ResponseMessage<string> response = await _messageService.PostMessage(message);
            if (!response.Success)return BadRequest(response);
            return Ok(response);
        }
    }
}
