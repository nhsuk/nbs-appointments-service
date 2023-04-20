using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class RescheduleAppointmentPayload : BasePayload
    {
        [JsonPropertyName("serviceId")]
        public int ServiceId { get; set; }

        [JsonPropertyName("dateAndTime")]
        public DateTime DateAndTime { get; set; }

        [JsonPropertyName("appointmentTypeId")]
        public int AppointmentTypeId { get; set; }

        [JsonPropertyName("cancelationReasonId")]
        public int CancelationReasonId { get; set; }

        [JsonPropertyName("originalProcessId")]
        public long OriginalProcessId { get; set; }
    }
}
