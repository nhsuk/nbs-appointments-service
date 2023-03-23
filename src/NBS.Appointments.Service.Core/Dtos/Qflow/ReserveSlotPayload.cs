using Newtonsoft.Json;

namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class ReserveSlotRequestContent : BasePayload
    {
        [JsonProperty("calendarId")]
        public int CalendarId { get; set; }

        [JsonProperty("startTime")]
        public int StartTime { get; set; }

        [JsonProperty("endTime")]
        public int EndTime { get; set; }

        [JsonProperty("lockDuration")]
        public int LockDuration { get; set; }
        [JsonProperty("userId")]
        public int UserId { get; set; }
    }
}
