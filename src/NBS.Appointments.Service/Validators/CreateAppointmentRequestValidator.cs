using FluentValidation;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using NBS.Appointments.Service.Models;
using System;

namespace NBS.Appointments.Service.Validators
{
    public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
    {
        public CreateAppointmentRequestValidator()
        {
            RuleFor(x => x.Slot)
                .NotEmpty()
                .WithMessage("Slot identifier must be provided.")
                .Must(BeAValidSlotUrn)
                .WithMessage("Slot identifier is not valid.");

            RuleFor(x => x.NhsNumber)
                .NotEmpty()
                .WithMessage("Nhs number must be provided.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Participant name must be provided")
                .Must(BeAValidName)
                .WithMessage("Name not in the correct format.");

            RuleFor(x => x.ContactDetails)
                .NotEmpty()
                .WithMessage("Contact details object must be provided.");
        }

        private bool BeAValidSlotUrn(string slotDescriptor)
        {
            try
            {
                QflowAppointmentDescriptor.FromString(slotDescriptor);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private bool BeAValidName(string name)
        {
            try
            {
                QflowNameDescriptor.FromString(name);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
