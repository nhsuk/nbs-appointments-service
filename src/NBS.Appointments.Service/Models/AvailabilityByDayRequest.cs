using System;
using Newtonsoft.Json;

namespace NBS.Appointments.Service.Models
{
    public class AvailabilityByDayRequest
    {
        [JsonProperty("sites")]
        public string[] Sites { get; set; }
        [JsonProperty("from")] 
        public DateTime From { get; set; }
        [JsonProperty("until")] 
        public DateTime Until { get; set; }
        [JsonProperty("service")] 
        public string Service { get; set; }
    }
}
