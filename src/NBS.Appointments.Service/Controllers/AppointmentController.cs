using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(
            IQflowService qflowService,
            RequestValidatorFactory requestValidatorFactory,
            ICustomPropertiesHelper customPropertiesHelper,
            ILogger<AppointmentController> logger)
        {
            _qflowService = qflowService
                ?? throw new ArgumentNullException(nameof(qflowService));

            _requestValidatorFactory = requestValidatorFactory
                ?? throw new ArgumentNullException(nameof(requestValidatorFactory));

            _customPropertiesHelper = customPropertiesHelper
                ?? throw new ArgumentNullException(nameof(customPropertiesHelper));

            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("book")]
        public async Task<IActionResult> Book([FromBody] BookAppointmentRequest request)
        {
            var validator = _requestValidatorFactory.GetValidator<BookAppointmentRequest>();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.ToErrorMessages();
                _logger.LogWarning("Book appointment request failed validation. Errors: {@Errors}. Request model: {@Request}",
                    errorMessages, request);
                return BadRequest(errorMessages);
            }

            var appointmentDescriptor = DescriptorConverter.Parse<QflowBookAppointmentDescriptor>(request.Slot);
            var nameDescriptor = DescriptorConverter.Parse<QflowNameDescriptor>(request.CustomerDetails.Name);

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
                _logger.LogWarning("Failed to create or update customer record. Customer details: {@Customer}", customerDetails);
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
                var errorMessages = validationResult.Errors.ToErrorMessages();
                _logger.LogWarning("Cancel appointment request failed validation. Errors: {@Errors}. Request object: {@Request}",
                    errorMessages, request);
                return BadRequest(errorMessages);
            }

            var cancelAppointmentDescriptor = DescriptorConverter.Parse<QFlowAppointmentReferenceDescriptor>(request.Appointment);

            var appointments = await _qflowService.GetAllCustomerAppointments(cancelAppointmentDescriptor.CustomerId);
            var appointmentToCancel = appointments.SingleOrDefault(x => x.ProcessId == cancelAppointmentDescriptor.ProcessId);

            if (appointmentToCancel is null)
                return NotFound($"Cannot find appointment with processId: {cancelAppointmentDescriptor.ProcessId} for customer: {cancelAppointmentDescriptor.CustomerId}.");

            var cancelationReasonDescriptor = DescriptorConverter.Parse<QflowCancelationReasonDescriptor>(request.Cancelation);

            var cancelationResult = await _qflowService.CancelAppointment(
                appointmentToCancel.ProcessId,
                cancelationReasonDescriptor.CancelationReasonId,
                cancelationReasonDescriptor.TreatmentPlanCancelationMethod);

            return cancelationResult.IsSuccessful
                ? Ok()
                : BadRequest("Failed to cancel appointment");
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllCustomerAppointments(string nhsNumber, bool includePastAppointments)
        {
            if (string.IsNullOrEmpty(nhsNumber))
            {
                _logger.LogWarning("NHS Number not provided when trying to get all customer appointments.");
                return BadRequest("Customer NHS Number must be provided.");
            }

            var qflowCustomer = await _qflowService.GetCustomerByNhsNumber(nhsNumber);

            if (!qflowCustomer.IsSuccessful)
            {
                _logger.LogWarning("Could not find qflow customer with NHS Number: {@NhsNumber}", nhsNumber);
                return NotFound($"Could not find qflow customer with NhsNumber: {nhsNumber}.");
            }

            var appointments = await _qflowService.GetAllCustomerAppointments(qflowCustomer.ResponseData.Id);

            return includePastAppointments
                ? Ok(appointments)
                : Ok(appointments.Where(x => DateTime.Compare(x.AppointmentDate.ToUniversalTime(), DateTime.Today.ToUniversalTime()) >= 0));
        }

        [HttpPost]
        [Route("reschedule")]
        public async Task<IActionResult> Reschedule([FromBody] RescheduleAppointmentRequest request)
        {
            var validator = _requestValidatorFactory.GetValidator<RescheduleAppointmentRequest>();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.ToErrorMessages();
                _logger.LogWarning("Reschedule appointment request failed validation. Errors: {@Errors}. Request object: {@Request}",
                    errorMessages, request);
                return BadRequest(errorMessages);
            }

            var originalAppointmentDescriptor = DescriptorConverter.Parse<QFlowAppointmentReferenceDescriptor>(request.OriginalAppointment);
            var targetAppointment = DescriptorConverter.Parse<QFlowSlotDescriptor>(request.RescheduledSlot);
            
            var rescheduleResult = await _qflowService.RescheduleAppointment(
                targetAppointment.ServiceId,
                targetAppointment.Date.Date.Add(TimeSpan.FromMinutes(targetAppointment.StartTime)),
                targetAppointment.AppointmentTypeId,
                originalAppointmentDescriptor.ProcessId);

            return rescheduleResult.IsSuccessful
                ? Ok(RescheduledAppointmentResponse.FromQflowResponse(rescheduleResult.ResponseData))
                : BadRequest("Failed to reschedule apointment.");
        }
    }
}
