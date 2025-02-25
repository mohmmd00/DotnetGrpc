using Framework_0.CustomLoggingService;
using Microsoft.AspNetCore.Mvc;
using MMS.Domain.Entities;

namespace MMS.Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ILoggingService _loggingService;

        public HealthController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        // POST: api/health
        [HttpPost]
        public IActionResult Post([FromBody] MMSMessage message)
        {
            if (message == null)
            {
                return BadRequest("Invalid health message data.");
            }

            // Process the received health message ( log it or update system status) loged it by defualt :)
            _loggingService.HealthMessageReceivedFromRouterByApi(message.PrimaryId, message.CurrentTime , message.ActiveClients);
           
            return Ok(new { Status = "Success", Message = "Health message received." });
        }
    }
}