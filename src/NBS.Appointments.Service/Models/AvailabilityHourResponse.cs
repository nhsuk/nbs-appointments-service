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

        public static AvailabilityHourResponse FromQflowResponse(SiteSlotsResponse qflowResponse, string vaccineType, DateTime date, DateTime currentDate)
        {
            var response = new AvailabilityHourResponse
            {
                Date = date,
                SiteId = qflowResponse.SiteId.ToString(),
                Type = vaccineType
            };

            var availableSlotTimes = qflowResponse.Availability
                .GroupBy(x => x.Time)
                .Select(x => x.Key)
                .ToList();

            var availabilityByHour = new List<AvailabilityHour>();

            if (!availableSlotTimes.Any())
            {
                response.AvailabilityByHour = availabilityByHour;
                return response;
            }

            var currentTime = currentDate.TimeOfDay;

            var isDateInTheFuture = DateTime.Compare(date, currentDate) > 0;

            foreach (var slotTime in availableSlotTimes)
            {
                if (!isDateInTheFuture && slotTime < currentTime)
                    continue;

                var count = qflowResponse.Availability
                    .Where(x => x.Time.Hours == slotTime.Hours)
                    .Count();

                availabilityByHour.Add(new AvailabilityHour(slotTime.Hours.ToString(), count));
            }

            response.AvailabilityByHour = availabilityByHour;
            return response;
        }
    }
}
