namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class SiteSlotsResponse
    {
        public int SiteId { get; set; }
        public string VaccineType { get; set; }
        public List<SiteSlotAvailabilityResponse> Availability { get; set; }
    }
}
