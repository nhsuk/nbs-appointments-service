namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class QFlowSiteDescriptor
    {
        public QFlowSiteDescriptor(int siteId)
        {
            SiteId = siteId;
        }

        // Validation for descriptor done before here in SlotAvailabilityRequestValidator
        public static QFlowSiteDescriptor FromString(string descriptor)
        {
            var parts = descriptor.Split(':');
            return new QFlowSiteDescriptor(int.Parse(parts[1]));
        }

        public int SiteId { get; set; }
    }
}
