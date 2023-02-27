using System;
using Newtonsoft.Json;

namespace NBS.Appointments.Service.Models
{
    public class Availability
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("AM")]
        public int Am { get; set; }
        [JsonProperty("PM")]
        public int Pm { get; set; }
    }
}
