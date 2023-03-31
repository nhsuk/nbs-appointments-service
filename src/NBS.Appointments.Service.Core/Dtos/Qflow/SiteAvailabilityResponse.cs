namespace NBS.Appointments.Service.Dtos.Qflow
{
    public class SiteAvailabilityResponse
    {
        public int SiteId { get; set; }

        public string SiteName { get; set; }

        public string SiteAddress { get; set; }

        public string VaccineType { get; set; }

        public AvailabilityResponse[] Availability { get; set; }
    }
}
