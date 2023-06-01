using NBS.Appointments.Service.Core.Helpers;

namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    [Descriptor("qflow:{SiteId}")]
    public class QFlowSiteDescriptor
    {
        public int SiteId { get; set; }
    }
}
