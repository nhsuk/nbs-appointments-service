using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using Xunit;

namespace NBS.Appointments.Service.Core.Unit.Tests.Dtos.Qflow.Descriptors
{
    public class QflowSlotDescriptorTests
    {
        [Theory]
        [InlineData("not:enough:parts", "Slot descriptor is not formatted correctly.")]
        [InlineData("someFlow:1:2:3:2023-04-04:4:5", "String was not a qflow slot descriptor.")]
        [InlineData("qflow:n:2:3:2023-04-04:4:5", "ServiceId must be a number.")]
        [InlineData("qflow:1:n:3:2023-04-04:4:5", "CalendarId must be a number.")]
        [InlineData("qflow:1:2:n:2023-04-04:4:5", "Appointment type must be a number.")]
        [InlineData("qflow:1:2:3:n:4:5", "Date must be a valid date.")]
        [InlineData("qflow:1:2:3:2023-04-04:n:5", "Start time must be a number.")]
        [InlineData("qflow:1:2:3:2023-04-04:4:n", "End time must be a number.")]
        public void FromString_ThrowsFormatException_WhenDescriptorIsInvalid(string descriptor, string expectedErrorMsg)
        {
            var exception = Assert.Throws<FormatException>(() => QFlowSlotDescriptor.FromString(descriptor));
            exception.Message.Should().Be(expectedErrorMsg);
        }

        [Theory]
        [InlineData("qflow:1:2:3:2023-04-05:300:300", "Start time must be before end time.")]
        [InlineData("qflow:1:2:3:2023-04-05:300:295", "Start time must be before end time.")]
        public void FromString_ThrowsArgumentException_WhenStartTimeIsTheSameOrBeforeEndTime(string descriptor, string expectedErrorMsg)
        {
            var exception = Assert.Throws<ArgumentException>(() => QFlowSlotDescriptor.FromString(descriptor));
            exception.Message.Should().Be(expectedErrorMsg);
        }

        [Fact]
        public void FromString_ProducesSlotDescriptor_WhenFormattedCorrectly()
        {
            var testDate = DateTime.Today;
            var result = QFlowSlotDescriptor.FromString($"qflow:12:34:56:{testDate:yyyy-MM-dd}:78:90");

            result.ServiceId.Should().Be(12);
            result.CalendarId.Should().Be(34);
            result.AppointmentTypeId.Should().Be(56);
            result.Date.Should().Be(testDate);
            result.StartTime.Should().Be(78);
            result.EndTime.Should().Be(90);
        }
    }
}
