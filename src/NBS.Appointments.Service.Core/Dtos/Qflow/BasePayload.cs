using Newtonsoft.Json;

namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class BasePayload
    {
        [JsonProperty("apiSessionId")]
        public string? ApiSessionId { get; set; }
    }
}
