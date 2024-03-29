﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using NBS.Appointments.Service.Core.Helpers;
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
        private readonly ILogger<SlotController> _logger;

        public SlotController(IQflowService qflowService, RequestValidatorFactory requestValidatorFactory, ILogger<SlotController> logger)
        {
            _qflowService = qflowService
                ?? throw new ArgumentNullException(nameof(qflowService));

            _requestValidatorFactory = requestValidatorFactory
                ?? throw new ArgumentNullException(nameof(requestValidatorFactory));

            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
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
                _logger.LogWarning("Reserve slot request object failed validation. Errors: {@Errors}. Request object: {@Request}",
                    errorMsgs, request);
                return BadRequest(errorMsgs);
            }

            var slotDescriptor = DescriptorConverter.Parse<QFlowSlotDescriptor>(request.Slot);

            var response = await _qflowService.ReserveSlot(
                slotDescriptor.CalendarId,
                slotDescriptor.StartTime,
                slotDescriptor.EndTime,
                request.LockDuration);

            return response.IsSuccessful
                ? Ok(LockSlotResponse.FromQflowResponse(slotDescriptor, response.ResponseData))
                : StatusCode(410, "Slot no longer exists or reservation has expired.");
        }
    }
}
