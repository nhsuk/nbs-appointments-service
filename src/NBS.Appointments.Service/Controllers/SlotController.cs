using Microsoft.AspNetCore.Mvc;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using NBS.Appointments.Service.Core.Interfaces.Services;
using NBS.Appointments.Service.Extensions;
using NBS.Appointments.Service.Models;
using NBS.Appointments.Service.Validators;
using System;
using System.Threading.Tasks;

namespace NBS.Appointments.Service.Controllers
{
    [Route("slot")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly IQflowService _qflowService;
        private readonly ReserveSlotRequestVadidator _lockSlotRequestVadidator;

        public SlotController(IQflowService qflowService, ReserveSlotRequestVadidator lockSlotRequestValidator)
        {
            _qflowService = qflowService
                ?? throw new ArgumentNullException(nameof(qflowService));

            _lockSlotRequestVadidator = lockSlotRequestValidator
                ?? throw new ArgumentNullException(nameof(lockSlotRequestValidator));
        }

        [HttpPost]
        [Route("reservation")]
        public async Task<IActionResult> ReserveSlot([FromBody] LockSlotRequest request)
        {
            var validationResult = _lockSlotRequestVadidator.Validate(request);

            if (!validationResult.IsValid)
            {
                var errorMsgs = validationResult.Errors.ToErrorMessages();
                return BadRequest(errorMsgs);
            }

            var slotDescriptor = QFlowSlotDescriptor.FromString(request.Slot);

            var response = await _qflowService.ReserveSlot(
                slotDescriptor.CalendarId,
                slotDescriptor.StartTime,
                slotDescriptor.EndTime,
                request.LockDuration);

            return Ok(response);
        }
    }
}
