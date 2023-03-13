using FluentValidation;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Models;
using System;
using System.Linq;

namespace NBS.Appointments.Service.Validators
{
    public class AvailabilityByDayRequestValidator : AbstractValidator<AvailabilityByDayRequest>
    {
        public AvailabilityByDayRequestValidator()
        {
            RuleFor(x => x.Sites)
                .NotEmpty()
                .WithMessage("SiteId must be provided.")
                .Must(BeValidSiteIdentifier)
                .WithMessage("SiteId is not valid.");

            RuleFor(x => x.From)
                .NotEmpty()
                .WithMessage("From date must be provided.")
                .Must(BeAValidDate)
                .WithMessage("The date you provided must be a valid date.")
                .Must(BeDateInTheFuture)
                .WithMessage("The date must be the current date or a date in the future.");

            RuleFor(x => x.Until)
                .NotEmpty()
                .WithMessage("Until date must be provided.")
                .Must(BeAValidDate)
                .WithMessage("The date you provided must be a valid date.")
                .Must((req, until) => AfterAnotherDate(until, req.From))
                .WithMessage("The date must be the current date or a date in the future.");

            RuleFor(x => x.Service)
                .NotEmpty()
                .WithMessage("Service must be specified.")
                .Must(BeValidServiceDescriptor)
                .WithMessage("Service descriptor is not valid");
        }

        private bool AfterAnotherDate(DateTime latter, DateTime check)
        {
            return latter.Date > check.Date;
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default);
        }

        private bool BeDateInTheFuture(DateTime date)
        {
            return DateTime.Compare(date, DateTime.Today) >= 0;
        }

        private bool BeValidSiteIdentifier(string[] siteIdentifiers)
        {
            foreach(var siteIdentifier in siteIdentifiers)
            {
            var parts = siteIdentifier.Split(':');
            var siteId = parts.FirstOrDefault(x => x == "qflow");

            if (siteId is null)
                return false;

            if(int.TryParse(parts[1], out _) == false)
                return false;
            }
            return true;
        }

        private bool BeValidServiceDescriptor(string serviceDescriptor)
        {
            try
            {
                QflowServiceDescriptor.FromString(serviceDescriptor);
                return true;
            }
            catch(FormatException)
            {
                return false;
            }
        }
    }
}
