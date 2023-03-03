namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class SiteSlotAvailabilityByHourResponse
    {
        public int SiteId { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public IList<SlotHourAvailabilityResponse> AvailabilityByHour { get; set; }
    }
}
