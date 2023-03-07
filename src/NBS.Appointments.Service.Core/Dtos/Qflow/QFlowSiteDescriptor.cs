namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class QFlowSiteDescriptor
    {
        public QFlowSiteDescriptor(int siteId)
        {
            SiteId = siteId;
        }

        public static QFlowSiteDescriptor FromString(string descriptor)
        {
            if (string.IsNullOrWhiteSpace(descriptor))
                throw new ArgumentException("Site descriptor must be provided.");

            var parts = descriptor.Split(':');

            var siteId = parts.FirstOrDefault(x => x == "qflow")
                ?? throw new FormatException("String was not a qflow site descriptor");

            if (!int.TryParse(parts[1], out _))
                throw new FormatException("SiteId must be a number");

            return new QFlowSiteDescriptor(int.Parse(parts[1]));
        }

        public int SiteId { get; set; }
    }
}
