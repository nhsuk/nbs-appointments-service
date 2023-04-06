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
                .Must(BeAValidAppointmentDescriptor)
                .WithMessage("Appointment descriptor is invalid.");

            RuleFor(x => x.Cancellation)
                .NotEmpty()
                .WithMessage("Cancellation URN mus be provided.")
                .Must(BeAValidCancelationReasonDescriptor)
                .WithMessage("Cancelation descriptor is invalid.");
        }

        private bool BeAValidAppointmentDescriptor(string descrpitor)
        {
            try
            {
                QflowCancelAppointmentDescriptor.FromString(descrpitor);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool BeAValidCancelationReasonDescriptor(string descrpitor)
        {
            try
            {
                QflowCancelationrReasonDescriptor.FromString(descrpitor);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
