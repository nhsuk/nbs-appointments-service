using FluentValidation;
using NBS.Appointments.Service.Models;
using System;

namespace NBS.Appointments.Service.Validators
{
    public class SlotAvailabilityRequestValidator : AbstractValidator<SlotAvailabilityRequest>
    {
        public SlotAvailabilityRequestValidator()
        {
            RuleFor(x => x.SiteIdentifier)
                .NotEmpty()
                .WithMessage("Site identifier must be provided.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("A date must be provided.")
                .Must(BeAValidDate)
                .WithMessage("The date you provided must be a valid date.")
                .Must(BeDateInTheFuture)
                .WithMessage("The date must be in the future.");

            RuleFor(x => x.AppointmentType)
                .NotEmpty()
                .WithMessage("Appointment type must be provided.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default);
        }

        private bool BeDateInTheFuture(DateTime date)
        {
            return DateTime.Compare(date, DateTime.Today) > 0;
        }
    }
}
