using Microsoft.AspNetCore.Mvc;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using NBS.Appointments.Service.Core.Helpers;
using NBS.Appointments.Service.Core.Interfaces.Services;
using NBS.Appointments.Service.Extensions;
using NBS.Appointments.Service.Models;
using NBS.Appointments.Service.Validators;
using System;
using System.Linq;
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

            var createUpdateCustomerResult = await _qflowService.CreateOrUpdateCustomer(
                nameDescriptor.FirstName,
                nameDescriptor.Surname,
                customerDetails.NhsNumber,
                customerDetails.Dob,
                customerDetails.ContactDetails.Email,
                customerDetails.ContactDetails.PhoneNumber,
                customerDetails.ContactDetails.Landline,
                customerDetails.SelfReferralOccupation);

            if (!createUpdateCustomerResult.IsSuccessful)
            {
                return BadRequest("Failed to create or update customer record.");
            }

            var bookAppointmentResult = await _qflowService.BookAppointment(
                appointmentDescriptor.ServiceId,
                appointmentDescriptor.AppointmentDateAndTime,
                createUpdateCustomerResult.ResponseData.Id,
                appointmentDescriptor.AppointmentTypeId,
                appointmentDescriptor.SlotOrdinalNumber,
                appointmentDescriptor.CalendarId,
                _customPropertiesHelper.BuildCustomProperties(request.Properties));

            return bookAppointmentResult.IsSuccessful
                ? Ok(BookedAppointmentResponse.FromQflowResponse(createUpdateCustomerResult.ResponseData.Id, bookAppointmentResult.ResponseData))
                : StatusCode(410, "Slot no longer exists or reservation has expired.");
        }

        [HttpPost]
        [Route("cancel")]
        public async Task<IActionResult> Cancel([FromBody] CancelAppointmentRequest request)
        {
            var validator = _requestValidatorFactory.GetValidator<CancelAppointmentRequest>();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.ToErrorMessages());
            }

            var cancelAppointmentDescriptor = QflowCancelAppointmentDescriptor.FromString(request.Appointment);

            var appointments = await _qflowService.GetAllCustomerAppointments(cancelAppointmentDescriptor.QflowCustomerId);
            var appointmentToCancel = appointments.FirstOrDefault(x => x.ProcessId == cancelAppointmentDescriptor.ProcessId);

            if (appointmentToCancel is null)
                return NotFound($"Cannot find appointment with processId: {cancelAppointmentDescriptor.ProcessId} for customer: {cancelAppointmentDescriptor.QflowCustomerId}.");

            var cancelationReasonDescriptor = QflowCancelationReasonDescriptor.FromString(request.Cancelation);

            var cancelationResult = await _qflowService.CancelAppointment(
                appointmentToCancel.ProcessId,
                cancelationReasonDescriptor.CancelationReasonId,
                cancelationReasonDescriptor.TreatmentPlanCancelationMethod);

            return cancelationResult.IsSuccessful
                ? Ok()
                : BadRequest("Failed to cancel appointment");
        }

        [HttpPost]
        [Route("reschedule")]
        public async Task<IActionResult> Reschedule([FromBody] RescheduleAppointmentRequest request)
        {
            var validator = _requestValidatorFactory.GetValidator<RescheduleAppointmentRequest>();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.ToErrorMessages());
            }

            var descriptor = QflowRescheduleAppointmentDescriptor.FromString(request.Appointment);

            var rescheduleResult = await _qflowService.RescheduleAppointment(
                descriptor.ServiceId,
                descriptor.DateAndTime,
                descriptor.AppointmentTypeId,
                descriptor.CancelationReasonId,
                descriptor.OriginalProcessId);

            return rescheduleResult.IsSuccessful
                ? Ok(RescheduledAppointmentResponse.FromQflowResponse(rescheduleResult.ResponseData))
                : BadRequest("Failed to reschedule apointment.");
        }
    }
}
