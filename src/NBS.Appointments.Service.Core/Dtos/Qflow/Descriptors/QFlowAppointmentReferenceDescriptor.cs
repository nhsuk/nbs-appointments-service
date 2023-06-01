using NBS.Appointments.Service.Core.Helpers;

namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    [Descriptor("qflow:{CustomerId}:{ProcessId}:{CaseId}")]
    public class QFlowAppointmentReferenceDescriptor
    {
        public int CustomerId { get; set; }
        public long ProcessId { get; set; }
        public long CaseId { get; set; }
    }
}
