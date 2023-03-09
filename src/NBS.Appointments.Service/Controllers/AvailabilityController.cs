using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NBS.Appointments.Service.Core.Interfaces.Services;
using NBS.Appointments.Service.Models;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Core;
using NBS.Appointments.Service.Extensions;
using NBS.Appointments.Service.Validators;

namespace NBS.Appointments.Service.Controllers
{
    [ApiController]
    [Route("availability")]
    public class AvailabilityController : Controller
    {
        private readonly IQflowService _qflowService;        
        private readonly RequestValidatorFactory _validatorFactory;

        public AvailabilityController(
            IQflowService qflowService,
            RequestValidatorFactory validatorFactory)
        {
            _qflowService = qflowService ?? throw new ArgumentNullException(nameof(qflowService));
            _validatorFactory = validatorFactory ?? throw new ArgumentNullException(nameof(validatorFactory));
        }

        [HttpPost]
        [Route("days")]
        public async Task<IActionResult> Days([FromBody] AvailabilityByDayRequest request)
        {
            var validator = _validatorFactory.GetValidator<AvailabilityByDayRequest>();
            
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.ToErrorMessages();
                return BadRequest(errorMessages);
            }

            QflowServiceDescriptor serviceDescriptor = QflowServiceDescriptor.FromString(request.Service);
            IEnumerable<SiteUrn> siteUrns = request.Sites.Select(s => SiteUrnParser.Parse(s)).ToList();

            if(siteUrns.Any(urn => urn.Scheme != "qflow"))
                return BadRequest(new {Message = "Only qflow sites are currently supported"});

            var qflowResponse = await _qflowService.GetSiteAvailability(
                siteUrns.Select(urn => urn.Identifier), 
                request.From, 
                request.Until, 
                serviceDescriptor.Dose,
                serviceDescriptor.Vaccine,
                serviceDescriptor.Reference);

            return new OkObjectResult(qflowResponse.Select(x => AvailabilityByDaysResponse.FromQflowResponse(x, request.Service)));
        }

        [HttpPost]
        [Route("hours")]
        public async Task<IActionResult> Hours([FromBody] AvailabilityByHourRequest request)
        {
            var validator = _validatorFactory.GetValidator<AvailabilityByHourRequest>();
            var validationResult = validator.Validate(request);

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
