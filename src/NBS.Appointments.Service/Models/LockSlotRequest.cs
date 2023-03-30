using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Models
{
    public class ReserveSlotRequest
    {
        [JsonPropertyName("slot")]
        public string Slot { get; set; }
        [JsonPropertyName("lockDuration")]
        public int LockDuration { get; set; }
    }
}
