namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class QFlowSiteDescriptor
    {
        public QFlowSiteDescriptor(int siteId, string? odsCode)
        {
            OdsCode = odsCode;
            SiteId = siteId;
        }

        public static QFlowSiteDescriptor FromString(string descriptor)
        {
            var parts = descriptor.Split(':');

            int.TryParse(parts.FirstOrDefault(x => x == "siteid"), out var siteId);
            var odsCode = parts.FirstOrDefault(x => x == "ods");

            return new QFlowSiteDescriptor(siteId, odsCode);
        }

        public string? OdsCode { get; set; }
        public int SiteId { get; set; }
    }
}
