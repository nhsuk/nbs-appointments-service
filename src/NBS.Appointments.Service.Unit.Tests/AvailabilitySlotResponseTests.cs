using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Models;
using Xunit;

namespace NBS.Appointments.Service.Unit.Tests
{
    public class AvailabilitySlotResponseTests
    {
        [Fact]
        public void FromQflowResponse_MapsDataCorrectly()
        {
            var testDate = DateTime.Today;
            var testSlots =  new List<SiteSlotAvailabilityResponse>
            {
                new SiteSlotAvailabilityResponse
                {
                    CalendarId = 99,
                    Duration = 20,
                    Time = new TimeSpan(9, 0, 0)
                }                
            };
            var actual = AvailabilitySlotResponse.FromQflowResponse("test-site", "test-service", testDate, testSlots);

            actual.Date.Should().Be(testDate);
            actual.SiteId.Should().Be("test-site");
            actual.Service.Should().Be("test-service");
            actual.Slots.Should().HaveCount(1);

            var actualSlot = actual.Slots.First();
            actualSlot.Duration.Should().Be(20);
            actualSlot.From.Should().Be(testDate.AddHours(9));
            actualSlot.Reference.Should().Be("qflow:99:540:560");
        }
    }
}
