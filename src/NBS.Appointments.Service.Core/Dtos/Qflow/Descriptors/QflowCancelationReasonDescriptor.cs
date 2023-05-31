using NBS.Appointments.Service.Core.Helpers;

namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    [Descriptor("qflow:{CancelationReasonId}:{TreatmentPlanCancelationMethod}")]
    public class QflowCancelationReasonDescriptor
    {
        public int CancelationReasonId { get; set; }
        public int TreatmentPlanCancelationMethod { get; set; }
    }
}
