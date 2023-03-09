using Newtonsoft.Json;
using System;

namespace NBS.Appointments.Service.Models
{
    public class AvailabilityByHourRequest
    {
        [JsonProperty("site")]
        public string Site { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("service")]
        public string Service { get; set; }
    }
}
