using Microsoft.AspNetCore.Mvc;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using NBS.Appointments.Service.Core.Helpers;
using NBS.Appointments.Service.Core.Interfaces.Services;
using NBS.Appointments.Service.Extensions;
using NBS.Appointments.Service.Models;
using NBS.Appointments.Service.Validators;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NBS.Appointments.Service.Controllers
{
    [Route("appointment")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IQflowService _qflowService;
        private readonly RequestValidatorFactory _requestValidatorFactory;
        private readonly ICustomPropertiesHelper _customPropertiesHelper;

        public AppointmentController(IQflowService qflowService, RequestValidatorFactory requestValidatorFactory, ICustomPropertiesHelper customPropertiesHelper)
        {
            _qflowService = qflowService
                ?? throw new ArgumentNullException(nameof(qflowService));

            _requestValidatorFactory = requestValidatorFactory
                ?? throw new ArgumentNullException(nameof(requestValidatorFactory));

            _customPropertiesHelper = customPropertiesHelper
                ?? throw new ArgumentNullException(nameof(customPropertiesHelper));
        }

        [HttpPost]
        [Route("book")]
        public async Task<IActionResult> Book([FromBody] BookAppointmentRequest request)
        {
            var validator = _requestValidatorFactory.GetValidator<BookAppointmentRequest>();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.ToErrorMessages());
            }

            var appointmentDescriptor = QflowBookAppointmentDescriptor.FromString(request.Slot);
            var nameDescriptor = QflowNameDescriptor.FromString(request.CustomerDetails.Name);

            var customerDetails = request.CustomerDetails;

            var qflowCustomer = await _qflowService.CreateOrUpdateCustomer(
                nameDescriptor.FirstName,
                nameDescriptor.Surname,
                customerDetails.NhsNumber,
                customerDetails.Dob,
                customerDetails.ContactDetails.Email,
                customerDetails.ContactDetails.PhoneNumber,
                customerDetails.ContactDetails.Landline,
                customerDetails.SelfReferralOccupation);

            var result = await _qflowService.BookAppointment(
                appointmentDescriptor.ServiceId,
                appointmentDescriptor.AppointmentDateAndTime,
                qflowCustomer.Id,
                appointmentDescriptor.AppointmentTypeId,
                appointmentDescriptor.SlotOrdinalNumber,
                appointmentDescriptor.CalendarId,
                _customPropertiesHelper.BuildCustomProperties(request.Properties));

            return result.StatusCode == HttpStatusCode.OK
                ? Ok(result.ResponseData)
                : StatusCode(410, "Slot no longer exists or reservation has expired.");
        }
    }
}
