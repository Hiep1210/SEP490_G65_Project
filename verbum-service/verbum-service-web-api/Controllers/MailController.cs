using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace verbum_service.Controllers
{
    [Route("api/mail")]
    [ApiController]
    public class MailController : ControllerBase
    {

        // POST api/<MailController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
