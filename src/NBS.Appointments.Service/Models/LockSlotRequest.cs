using Newtonsoft.Json;

namespace NBS.Appointments.Service.Models
{
    public class LockSlotRequest
    {
        [JsonProperty("slot")]
        public string Slot { get; set; }
        [JsonProperty("lockDuration")]
        public int LockDuration { get; set; }
    }
}
