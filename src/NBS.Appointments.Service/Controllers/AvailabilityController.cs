using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NBS.Appointments.Service.Core.Interfaces.Services;
using NBS.Appointments.Service.Models;
using NBS.Appointments.Service.Core;
using System;
using System.Collections.Generic;
using NBS.Appointments.Service.Validators;
using NBS.Appointments.Service.Extensions;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;

namespace NBS.Appointments.Service.Controllers
{
    [ApiController]
    [Route("availability")]
    public class AvailabilityController : Controller
    {
        private readonly IQflowService _qflowService;
        private readonly AvailabilityByHourRequestValidator _availabilityByHourRequestValidator;

        public AvailabilityController(IQflowService qflowService, AvailabilityByHourRequestValidator availabilityByHourRequestValidator)
        {
            _qflowService = qflowService;

            _availabilityByHourRequestValidator = availabilityByHourRequestValidator
                ?? throw new ArgumentNullException(nameof(availabilityByHourRequestValidator));
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

        [HttpPost]
        [Route("hours")]
        public async Task<IActionResult> Hours([FromBody] AvailabilityByHourRequest request)
        {
            var validationResult = _availabilityByHourRequestValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.ToErrorMessages();
                return BadRequest(errorMessages);
            }

            var siteDescriptor = QFlowSiteDescriptor.FromString(request.Site);
            var serviceDescriptor = QflowServiceDescriptor.FromString(request.Service);

            var result = await _qflowService.GetSiteSlotAvailability(
                siteDescriptor.SiteId,
                request.Date,
                serviceDescriptor.Dose,
                serviceDescriptor.Vaccine,
                serviceDescriptor.Reference);

            return Ok(result);
        }
    }
}
