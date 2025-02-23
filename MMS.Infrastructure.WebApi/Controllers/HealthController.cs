using Microsoft.AspNetCore.Mvc;
using MMS.Domain.Entities;

namespace MMS.Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        // POST: api/health
        [HttpPost]
        public IActionResult Post([FromBody] MMSMessage message)
        {
            if (message == null)
            {
                return BadRequest("Invalid health message data.");
            }

            // Process the received health message (e.g., log it or update system status)
            LogHealthMessage(message);

            return Ok(new { Status = "Success", Message = "Health message received." });
        }

        private void LogHealthMessage(MMSMessage message)
        {
            // Example: Logging the received health message
            Console.WriteLine($"Health Message Received - PrimaryId: {message.PrimaryId}, CurrentTime: {message.CurrentTime}");
            // You can also save this data to a database or perform other actions here
        }
    }
}