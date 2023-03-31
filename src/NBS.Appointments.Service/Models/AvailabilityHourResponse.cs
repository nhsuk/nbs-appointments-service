using NBS.Appointments.Service.Core.Dtos.Qflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Models
{
    public class AvailabilityHourResponse
    {
        [JsonPropertyName("siteId")]
        public string SiteId { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("availabilityByHour")]
        public List<AvailabilityHour> AvailabilityByHour { get; set; }

        public class AvailabilityHour
        {
            public AvailabilityHour(string hour, int count)
            {
                Hour = hour;
                Count = count;
            }

            [JsonPropertyName("hour")]
            public string Hour { get; set; }
            [JsonPropertyName("count")]
            public int Count { get; set; }
        }

        public static AvailabilityHourResponse FromQflowResponse(string site, string service, DateTime date, IEnumerable<SiteSlotAvailabilityResponse> slots) => new AvailabilityHourResponse
            {
                Date = date,
                SiteId = site,
                Type = service,
                AvailabilityByHour = slots
                    .GroupBy(x => x.Time.Hours)
                    .Select(x => new AvailabilityHour(x.Key.ToString(), x.Count()))
                    .ToList()
            };            
        
    }
}
