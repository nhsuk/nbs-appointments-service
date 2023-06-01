using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Models
{
    public class RescheduleAppointmentRequest
    {
        [JsonPropertyName("originalAppointment")]
        public string OriginalAppointment { get; set; }

        [JsonPropertyName("rescheduledSlot")]
        public string RescheduledSlot { get; set; }
    }
}
