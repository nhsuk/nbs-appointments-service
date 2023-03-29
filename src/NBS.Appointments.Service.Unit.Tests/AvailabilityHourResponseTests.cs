using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Models;
using Xunit;
using static NBS.Appointments.Service.Models.AvailabilityHourResponse;

namespace NBS.Appointments.Service.Unit.Tests
{
    public class AvailabilityHourResponseTests
    {
        [Fact]
        public void FromQflowResponse_MapsDataCorrectly()
        {
            var slots = new List<SiteSlotAvailabilityResponse>
            {
                new SiteSlotAvailabilityResponse
                {
                    AppointmentTypeId = 1,
                    CalendarId = 1,
                    Duration = 5,
                    ServiceId = 1,
                    Time = new TimeSpan(10, 0, 0)
                },
                new SiteSlotAvailabilityResponse
                {
                    AppointmentTypeId = 1,
                    CalendarId = 1,
                    Duration = 5,
                    ServiceId = 1,
                    Time = new TimeSpan(11, 0, 0)
                }
            };

            var currentTestDate = new DateTime(2023, 03, 15, 10, 00, 00);

            var expected = new AvailabilityHourResponse
            {
                SiteId = "123",
                Date = currentTestDate,
                Type = "39115611000001103",
                AvailabilityByHour = new List<AvailabilityHour>
                {
                    new AvailabilityHour("10", 1),
                    new AvailabilityHour("11", 1)
                }
            };

            var actual = FromQflowResponse("123", "39115611000001103", currentTestDate, slots);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
