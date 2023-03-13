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

        public static AvailabilityHourResponse FromQflowResponse(SiteSlotsResponse qflowResponse, string vaccineType, DateTime date)
        {
            var response = new AvailabilityHourResponse
            {
                Date = date,
                SiteId = qflowResponse.SiteId.ToString(),
                Type = vaccineType
            };

            var availableHours = qflowResponse.Availability
                .Select(x => x)
                .OrderBy(x => x.Time.Hours)
                .GroupBy(x => x.Time.Hours)
                .Select(x => x.Key)
                .ToList();

            var availabilityByHour = new List<AvailabilityHour>();

            if (!availableHours.Any())
            {
                response.AvailabilityByHour = availabilityByHour;
                return response;
            }

            foreach (var hour in availableHours)
            {
                var count = qflowResponse.Availability
                    .Where(x => x.Time.Hours == hour)
                    .Count();

                availabilityByHour.Add(new AvailabilityHour(hour.ToString(), count));
            }

            response.AvailabilityByHour = availabilityByHour;
            return response;
        }
    }
}
