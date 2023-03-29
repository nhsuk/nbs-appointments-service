using NBS.Appointments.Service.Core.Dtos.Qflow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBS.Appointments.Service.Models
{
    public class AvailabilityHourResponse
    {
        [JsonProperty("siteId")]
        public string SiteId { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("availabilityByHour")]
        public List<AvailabilityHour> AvailabilityByHour { get; set; }

        public class AvailabilityHour
        {
            public AvailabilityHour(string hour, int count)
            {
                Hour = hour;
                Count = count;
            }

            [JsonProperty("hour")]
            public string Hour { get; set; }
            [JsonProperty("count")]
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
