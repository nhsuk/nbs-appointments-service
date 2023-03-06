using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NBS.Appointments.Service.Core.Interfaces.Services;
using NBS.Appointments.Service.Models;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Core;
using System;
using System.Collections.Generic;

namespace NBS.Appointments.Service.Controllers
{
    [ApiController]
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
        public async Task<IActionResult> Query([FromBody] AvailabilityQueryRequest request)
        {
            QflowServiceDescriptor serviceDescriptor;
            IEnumerable<SiteUrn> siteUrns;

            try
            {
                serviceDescriptor = QflowServiceDescriptor.FromString(request.Service);
                siteUrns = request.Sites.Select(s => SiteUrnParser.Parse(s)).ToList();
            }
            catch (FormatException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }

            if(siteUrns.Any(urn => urn.Scheme != "qflow"))
                return BadRequest(new {Message = "Only qflow sites are currently supported"});

            var qflowResponse = await _qflowService.GetSiteAvailability(
                siteUrns.Select(urn => urn.Identifier), 
                request.From, 
                request.Until, 
                serviceDescriptor.Dose,
                serviceDescriptor.Vaccine,
                serviceDescriptor.Reference);

            return new OkObjectResult(qflowResponse.Select(x => AvailabilityQueryResponse.FromQflowResponse(x, request.Service)));
        }
    }
}
