﻿using FluentValidation;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using NBS.Appointments.Service.Models;
using System;

namespace NBS.Appointments.Service.Validators
{
    public class ReserveSlotRequestVadidator : AbstractValidator<ReserveSlotRequest>
    {
        public ReserveSlotRequestVadidator()
        {
            RuleFor(x => x.Slot)
                .NotEmpty()
                .WithMessage("Slot descriptor must be provided.")
                .Must(BeAValidDescriptor)
                .WithMessage("Slot descriptor is in invalid format.");

            RuleFor(x => x.LockDuration)
                .GreaterThan(0)
                .WithMessage("Lock duration must be greater than zero.");
        }

        private static bool BeAValidDescriptor(string slotDescriptor)
        {
            try
            {
                QFlowSlotDescriptor.FromString(slotDescriptor);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
