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
        public async Task<IActionResult> Query([FromBody] QueryRequest request)
        {
            QflowServiceDescriptor serviceDescriptor;
            IEnumerable<SiteUrn> siteUrns;

            try
            {
                serviceDescriptor = QflowServiceDescriptor.FromString(request.Service);
                siteUrns = request.Sites.Select(s => SiteUrnParser.Parse(s)).ToList();
            }
            catch (FormatException)
            {
                return BadRequest();
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

            var apiResponse = qflowResponse.Select(item => new AvailabilityResponse
            {
                Site = $"qflow:{item.SiteId}",
                Service = request.Service,
                Availability = item.Availability.Select(av => new Availability
                {
                    Date = av.Date.ToString("yyyy-MM-dd"),
                    Am = av.Am,
                    Pm = av.Pm,
                }).ToArray()
            });

            return new OkObjectResult(apiResponse);
        }
    }
}
