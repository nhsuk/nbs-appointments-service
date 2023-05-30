using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NBS.Appointments.Service.Controllers
{
    [Route("devtools")]
    [ApiController]
    public class DevToolsController : ControllerBase
    {
        [HttpGet]
        [Route("raise-exception")]
        public IActionResult RaiseException()
        {
            throw new Exception();
        }
    }
}
