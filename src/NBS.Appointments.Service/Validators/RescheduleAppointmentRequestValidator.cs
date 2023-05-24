using FluentValidation;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using NBS.Appointments.Service.Models;
using System;

namespace NBS.Appointments.Service.Validators
{
    public class RescheduleAppointmentRequestValidator : AbstractValidator<RescheduleAppointmentRequest>
    {
        public RescheduleAppointmentRequestValidator()
        {
            RuleFor(x => x.Appointment)
                .NotEmpty()
                .WithMessage("Appointment descriptor must be provided.")
                .Must(BeAValidAppointmentDescriptor)
                .WithMessage("Appointment descriptor is in invalid format.");
        }

        private bool BeAValidAppointmentDescriptor(string descriptor)
        {
            try
            {
                QflowRescheduleAppointmentDescriptor.FromString(descriptor);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
