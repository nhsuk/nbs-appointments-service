namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    public class QflowCancelationReasonDescriptor
    {
        public static QflowCancelationReasonDescriptor FromString(string descriptor)
        {
            var parts = descriptor.Split(':');

            if (parts.Length != 3)
                throw new FormatException("Descriptor is formatted incorrectly.");

            if (parts[0] != "qflow")
                throw new FormatException("String was not a qflow cancelation reason descriptor.");

            if (!int.TryParse(parts[1], out var cancelationReasonId))
                throw new FormatException("CancelationReasonId must be a number.");

            if (!int.TryParse(parts[2], out var treatmentPlanCancelationMethod))
                throw new FormatException("TreatmentPlanCancelationMethod must be a number.");

            return new QflowCancelationReasonDescriptor(cancelationReasonId, treatmentPlanCancelationMethod);
        }

        public QflowCancelationReasonDescriptor(int cancelationReasonId, int treatmentPlanCancelationMethod) 
        {
            CancelationReasonId = cancelationReasonId;
            TreatmentPlanCancelationMethod = treatmentPlanCancelationMethod;
        }

        public int CancelationReasonId { get; set; }
        public int TreatmentPlanCancelationMethod { get; set; }
    }
}
