using Microsoft.AspNetCore.Mvc;
using NBS.Appointments.Service.Infrastructure.Interfaces.Services;
using NBS.Appointments.Service.Models;

namespace NBS.Appointments.Service.Controllers
{
    [Route("availability")]
    public class AvailabilityController : Controller
    {
        private readonly IQflowService _qflowService;
        public AvailabilityController(IQflowService qflowService)
        {
            _qflowService = qflowService;
        }

        [HttpPost]
        [Route("query")]
        public IActionResult Query([FromBody]QueryRequest request)
        {
            var services = request.GetServices();
            var response = _qflowService.GetSiteAvailability(request.Sites, request.From, request.Until, services[0],
                services[1], services[2], services[3]);
            return new OkObjectResult(response);
        }
    }
}
