using Newtonsoft.Json;
using System;

namespace NBS.Appointments.Service.Models
{
    public class SlotAvailabilityRequest
    {
        [JsonProperty("siteIdentifier")]
        public string SiteIdentifier { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("appointmentType")]
        public string AppointmentType { get; set; }
    }
}
