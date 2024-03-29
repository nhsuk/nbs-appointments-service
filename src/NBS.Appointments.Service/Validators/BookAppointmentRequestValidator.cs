﻿using FluentValidation;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using NBS.Appointments.Service.Core.Helpers;
using NBS.Appointments.Service.Models;
using System;

namespace NBS.Appointments.Service.Validators
{
    public class BookAppointmentRequestValidator : AbstractValidator<BookAppointmentRequest>
    {
        public BookAppointmentRequestValidator()
        {
            RuleFor(x => x.Slot)
                .NotEmpty()
                .WithMessage("Slot identifier must be provided.")
                .MustBeValidDescriptor<BookAppointmentRequest, QflowBookAppointmentDescriptor>()
                .WithMessage("Slot identifier is not valid.");

            RuleFor(x => x.CustomerDetails)
                .NotEmpty()
                .WithMessage("Customer details object must be provided.");

            RuleFor(x => x.CustomerDetails.NhsNumber)
                .NotEmpty()
                .WithMessage("Nhs number must be provided.");

            RuleFor(x => x.CustomerDetails.Name)
                .NotEmpty()
                .WithMessage("Customer name must be provided")
                .MustBeValidDescriptor<BookAppointmentRequest, QflowNameDescriptor>()
                .WithMessage("Name not in the correct format.");

            RuleFor(x => x.CustomerDetails.Dob)
                .NotEmpty()
                .WithMessage("Customer date of birth must be provided.");

            RuleFor(x => x.CustomerDetails.ContactDetails)
                .NotEmpty()
                .WithMessage("Contact details object must be provided.");
        }                
    }
}
