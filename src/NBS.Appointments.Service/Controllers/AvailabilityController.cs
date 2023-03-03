using Microsoft.AspNetCore.Mvc;
using NBS.Appointments.Service.Core.Interfaces.Services;
using NBS.Appointments.Service.Models;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Query([FromBody]QueryRequest request)
        {            
            request = new QueryRequest
            {
                Sites = new [] {"site:1"},
                From = System.DateTime.Today,
                Until= System.DateTime.Today.AddDays(10),
                Service = "covid:booster:snomed:none"
            };
            var serviceDescriptor = QflowCovidServiceDescriptor.FromString(request.Service);

            var response = await _qflowService.GetSiteAvailability(
                request.Sites, 
                request.From, 
                request.Until, 
                serviceDescriptor.Dose,
                serviceDescriptor.Vaccine,
                serviceDescriptor.Reference);
            return new OkObjectResult(response);
        }
    }
}
