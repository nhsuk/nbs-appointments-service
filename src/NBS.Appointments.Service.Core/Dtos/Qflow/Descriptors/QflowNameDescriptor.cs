using NBS.Appointments.Service.Core.Helpers;

namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    [Descriptor("{FirstName}:{Surname}")]
    public class QflowNameDescriptor
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
    }
}
