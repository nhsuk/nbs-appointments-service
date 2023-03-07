namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class AvailabilityByHourResponse
    {
        public int SiteId { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public IList<AvailabilityHourResponse> AvailabilityByHour { get; set; }
    }
}
