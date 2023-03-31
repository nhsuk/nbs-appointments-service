using System;
using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Models
{
    public class SiteAvailabilityRequest
    {
        [JsonPropertyName("site")]
        public string Site { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("service")]
        public string Service { get; set; }
    }    
}
