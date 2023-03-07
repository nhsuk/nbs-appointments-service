using FluentValidation;
using NBS.Appointments.Service.Models;
using System;
using System.Linq;

namespace NBS.Appointments.Service.Validators
{
    public class AvailabilityByHourRequestValidator : AbstractValidator<AvailabilityByHourRequest>
    {
        public AvailabilityByHourRequestValidator()
        {
            RuleFor(x => x.Site)
                .NotEmpty()
                .WithMessage("SiteId must be provided.")
                .Must(BeValidSiteIdentifier)
                .WithMessage("SiteId is not valid.");

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

        private bool BeValidSiteIdentifier(string siteIdentifier)
        {
            var parts = siteIdentifier.Split(':');
            var siteId = parts.FirstOrDefault(x => x == "qflow");

            if (siteId is null)
                return false;

            return int.TryParse(parts[1], out _);
        }
    }
}
