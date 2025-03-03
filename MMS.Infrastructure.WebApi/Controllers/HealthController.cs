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


        //[HttpPut]
        //public IActionResult Put(MMSPostResponse response)
        //{
        //    var newPostResponse = new MMSPostResponse 
        //    {
        //        IsEnabled = response.IsEnabled,
        //        ExpirationTime = DateTime.Now.ToString()
        //    };

        //    return Ok(newPostResponse);
        //}


        // POST: api/health
        [HttpPost]
        public IActionResult Post([FromBody] MMSMessage message)// bind !!! the incoming JSON payload from the request body to MMSmessage
        {
            if (message == null)
            {
                return BadRequest("Invalid health message data.");
            }

            var postResponse = new MMSPostResponse // static propertiers needs to be functional 
            {
                IsEnabled = true,
                ExpirationTime = DateTime.Now.AddMinutes(10).ToString()
            };

            //after grpc server post the message create a alog
            // Process the received health message ( log it or update system status) loged it by defualt :)
            _loggingService.HealthMessageReceivedByApiFromRouterToLog(message.PrimaryId, message.CurrentTime , message.ActiveClients);
           
            return Ok(postResponse);
        }
    }
}