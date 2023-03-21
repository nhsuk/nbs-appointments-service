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
        private readonly RequestValidatorFactory _requestValidatorFactory;

        public SlotController(IQflowService qflowService, RequestValidatorFactory requestValidatorFactory)
        {
            _qflowService = qflowService
                ?? throw new ArgumentNullException(nameof(qflowService));

            _requestValidatorFactory = requestValidatorFactory
                ?? throw new ArgumentNullException(nameof(requestValidatorFactory));
        }

        [HttpPost]
        [Route("reserve")]
        public async Task<IActionResult> ReserveSlot([FromBody] ReserveSlotRequest request)
        {
            var validator = _requestValidatorFactory.GetValidator<ReserveSlotRequest>();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                var errorMsgs = validationResult.Errors.ToErrorMessages();
                return BadRequest(errorMsgs);
            }

            var slotDescriptor = QFlowSlotDescriptor.FromString(request.Slot);

            try
            {
                var response = await _qflowService.ReserveSlot(
                    slotDescriptor.CalendarId,
                    slotDescriptor.StartTime,
                    slotDescriptor.EndTime,
                    request.LockDuration);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
