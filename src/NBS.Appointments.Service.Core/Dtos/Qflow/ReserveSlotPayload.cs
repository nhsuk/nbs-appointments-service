using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class ReserveSlotRequestContent : BasePayload
    {
        [JsonPropertyName("calendarId")]
        public int CalendarId { get; set; }

        [JsonPropertyName("startTime")]
        public int StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public int EndTime { get; set; }

        [JsonPropertyName("lockDuration")]
        public int LockDuration { get; set; }
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
    }
}
