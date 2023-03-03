using Microsoft.AspNetCore.Mvc;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Core.Interfaces.Services;
using NBS.Appointments.Service.Extensions;
using NBS.Appointments.Service.Models;
using NBS.Appointments.Service.Validators;
using System;
using System.Threading.Tasks;

namespace NBS.Appointments.Service.Controllers
{
    [Route("slots")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly IQflowService _qflowService;
        private readonly SlotAvailabilityRequestValidator _slotAvailabilityRequestValidator;

        public SlotController(IQflowService qflowService, SlotAvailabilityRequestValidator slotAvailabilityRequestValidator)
        {
            _qflowService = qflowService
                ?? throw new ArgumentNullException(nameof(qflowService));

            _slotAvailabilityRequestValidator = slotAvailabilityRequestValidator
                ?? throw new ArgumentNullException(nameof(slotAvailabilityRequestValidator));
        }

        [HttpPost]
        [Route("available-slots")]
        public async Task<IActionResult> AvailableSlotsAsync([FromBody]SlotAvailabilityRequest request)
        {
            var validationResult = _slotAvailabilityRequestValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.ToErrorMessages();
                return BadRequest(errorMessages);
            }

            var siteDescriptor = QFlowSiteDescriptor.FromString(request.SiteIdentifier);

            var result = await _qflowService.GetSiteSlotAvailabilityAsync(
                siteDescriptor.SiteId,
                request.Date,
                request.AppointmentType);

            return Ok(result);
        }
    }
}
