using FluentValidation;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using NBS.Appointments.Service.Models;

namespace NBS.Appointments.Service.Validators
{
    public class RescheduleAppointmentRequestValidator : AbstractValidator<RescheduleAppointmentRequest>
    {
        public RescheduleAppointmentRequestValidator()
        {
            RuleFor(x => x.OriginalAppointment)
                .NotEmpty()
                .WithMessage("Appointment descriptor must be provided.")                
                .MustBeValidDescriptor<RescheduleAppointmentRequest, QFlowAppointmentReferenceDescriptor>()
                .WithMessage("Appointment descriptor is in invalid format.");

            RuleFor(x => x.RescheduledSlot) 
                .NotEmpty()
                .WithMessage("Slot descriptor must be provided")
                .MustBeValidDescriptor<RescheduleAppointmentRequest, QFlowSlotDescriptor>()
                .WithMessage("Slot descriptor is in invalid format.");
        }
    }
}
