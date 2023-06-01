using FluentValidation;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using NBS.Appointments.Service.Models;

namespace NBS.Appointments.Service.Validators
{
    public class CancelAppointmentRequestValidator : AbstractValidator<CancelAppointmentRequest>
    {
        public CancelAppointmentRequestValidator()
        {
            RuleFor(x => x.Appointment)
                .NotEmpty()
                .WithMessage("Appointment URN must be provided.")
                .MustBeValidDescriptor<CancelAppointmentRequest, QFlowAppointmentReferenceDescriptor>()
                .WithMessage("Appointment descriptor is invalid.");

            RuleFor(x => x.Cancelation)
                .NotEmpty()
                .WithMessage("Cancellation URN mus be provided.")
                .MustBeValidDescriptor<CancelAppointmentRequest, QflowCancelationReasonDescriptor>()
                .WithMessage("Cancelation descriptor is invalid.");
        }               
    }
}
