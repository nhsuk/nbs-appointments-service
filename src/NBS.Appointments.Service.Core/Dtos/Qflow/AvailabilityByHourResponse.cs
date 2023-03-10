namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class AvailabilityByHourResponse
    {
        public int SiteId { get; set; }
        public string VaccineType { get; set; }
        public List<QflowAvailabilityHourResponse> Availability { get; set; }
    }
}
