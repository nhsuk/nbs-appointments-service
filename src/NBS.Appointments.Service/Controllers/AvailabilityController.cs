using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NBS.Appointments.Service.Core.Interfaces.Services;
using NBS.Appointments.Service.Models;
using NBS.Appointments.Service.Core;
using System.Collections.Generic;
using NBS.Appointments.Service.Validators;
using NBS.Appointments.Service.Extensions;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Core.Interfaces;
using NBS.Appointments.Service.Core.Helpers;
using Microsoft.Extensions.Logging;

namespace NBS.Appointments.Service.Controllers
{
    [ApiController]
    [Route("availability")]
    public class AvailabilityController : Controller
    {
        private readonly IQflowService _qflowService;
        private readonly RequestValidatorFactory _validatorFactory;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<AvailabilityController> _logger;

        public AvailabilityController(
            IQflowService qflowService,
            IDateTimeProvider dateTimeProvider,
            RequestValidatorFactory validatorFactory,
            ILogger<AvailabilityController> logger)
        {
            _qflowService = qflowService ?? throw new ArgumentNullException(nameof(qflowService));
            _validatorFactory = validatorFactory ?? throw new ArgumentNullException(nameof(validatorFactory));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("time")]
        public IActionResult Time() 
        {            
            return Ok(new
            {
                Utc = _dateTimeProvider.UtcNow.Hour,
                Local = _dateTimeProvider.LocalNow.Hour
            });
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
                _logger.LogWarning("Request for availability by days failed validation. Validation errors: {@Errors}. Request model: {@Request}",
                    errorMessages, request);
                return BadRequest(errorMessages);
            }

            var serviceDescriptor = DescriptorConverter.Parse<QflowServiceDescriptor>(request.Service);
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

            return new OkObjectResult(qflowResponse.Select(x => AvailabilityByDayResponse.FromQflowResponse(x, request.Service)));
        }

        [HttpPost]
        [Route("hours")]
        public Task<IActionResult> Hours([FromBody] SiteAvailabilityRequest request)
        {
            var localDateTime = _dateTimeProvider.LocalNow;
            return LookupAvailability(request, localDateTime, (slots) => Ok(AvailabilityHourResponse.FromQflowResponse(request.Site, request.Service, request.Date, slots)));
        }

        [HttpPost]
        [Route("slots")]
        public Task<IActionResult> Slots([FromBody] SiteAvailabilityRequest request)
        {
            var localDateTime = _dateTimeProvider.LocalNow;
            return LookupAvailability(request, localDateTime, (slots) => Ok(AvailabilitySlotResponse.FromQflowResponse(request.Site, request.Service, request.Date, slots)));
        }

        private async Task<IActionResult> LookupAvailability(SiteAvailabilityRequest request, DateTime notBefore, Func<IEnumerable<SiteSlotAvailabilityResponse>, IActionResult> responseConversion)
        {
            var validator = _validatorFactory.GetValidator<SiteAvailabilityRequest>();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.ToErrorMessages();
                _logger.LogWarning("Site availability request failed validation. Validation errors: {@Errors}. Request model: {@Request}",
                    errorMessages, request);
                return BadRequest(errorMessages);
            }

            var siteDescriptor = DescriptorConverter.Parse<QFlowSiteDescriptor>(request.Site);
            var serviceDescriptor = DescriptorConverter.Parse<QflowServiceDescriptor>(request.Service);

            var qflowResponse = await _qflowService.GetSiteSlotAvailability(
                siteDescriptor.SiteId,
                request.Date,
                serviceDescriptor.Dose,
                serviceDescriptor.Vaccine,
                serviceDescriptor.Reference);

            var date = request.Date.Date;
            var availableSlots = qflowResponse.Availability.Where(sl => date.Add(sl.Time) >= notBefore);

            return responseConversion(availableSlots);
        }
    }
}
