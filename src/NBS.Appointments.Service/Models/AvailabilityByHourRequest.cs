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
        [JsonProperty("vaccineType")]
        public string VaccineType { get; set; }
        [JsonProperty("dose")]
        public int Dose { get; set; }
        [JsonProperty("externalReference")]
        public string ExternalReference { get; set; }
    }
}
