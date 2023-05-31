using NBS.Appointments.Service.Core.Helpers;

namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    [Descriptor("qflow:{Dose}:{Vaccine}:{Reference}")]
    public class QflowServiceDescriptor
    {
        public string Dose { get; init; }
        public string Vaccine { get; init; }
        public string Reference { get; init; }
    }
}
