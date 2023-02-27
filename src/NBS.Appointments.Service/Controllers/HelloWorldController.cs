using Microsoft.AspNetCore.Mvc;

namespace NBS.Appointments.Service.Controllers
{
    [ApiController]
    [Route("/")]
    public class HelloWorldController : Controller
    {
        [Route("hello-world")]
        public IActionResult Index()
        {
            return Ok("Hello World");
        }
    }
}
