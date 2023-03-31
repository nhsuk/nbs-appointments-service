using System;
using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Models
{
    public class AvailabilityByDayRequest
    {
        [JsonPropertyName("sites")]
        public string[] Sites { get; set; }
        [JsonPropertyName("from")] 
        public DateTime From { get; set; }
        [JsonPropertyName("until")] 
        public DateTime Until { get; set; }
        [JsonPropertyName("service")] 
        public string Service { get; set; }
    }
}
