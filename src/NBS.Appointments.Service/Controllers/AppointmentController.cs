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
    [Route("appointment")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IQflowService _qflowService;
        private readonly RequestValidatorFactory _requestValidatorFactory;

        public AppointmentController(IQflowService qflowService, RequestValidatorFactory requestValidatorFactory)
        {
            _qflowService = qflowService
                ?? throw new ArgumentNullException(nameof(qflowService));

            _requestValidatorFactory = requestValidatorFactory
                ?? throw new ArgumentNullException(nameof(requestValidatorFactory));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentRequest request)
        {
            var validator = _requestValidatorFactory.GetValidator<CreateAppointmentRequest>();

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.ToErrorMessages());
            }

            var appointmentDescriptor = QflowAppointmentDescriptor.FromString(request.Slot);
            var nameDescriptor = QflowNameDescriptor.FromString(request.Name);

            return Ok();
        }
    }
}
