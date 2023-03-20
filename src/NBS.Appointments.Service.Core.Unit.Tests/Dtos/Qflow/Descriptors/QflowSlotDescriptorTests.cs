using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using Xunit;

namespace NBS.Appointments.Service.Core.Unit.Tests.Dtos.Qflow.Descriptors
{
    public class QflowSlotDescriptorTests
    {
        [Theory]
        [InlineData("not:enough:parts")]
        [InlineData("someFlow:1:2:3")]
        [InlineData("qflow:n:2:3")]
        [InlineData("qflow:1:n:3")]
        [InlineData("qflow:1:2:n")]
        public void FromString_ThrowsFormatException_WhenDescriptorIsInvalid(string descriptor)
        {
            Assert.Throws<FormatException>(() => QFlowSlotDescriptor.FromString(descriptor));
        }

        [Theory]
        [InlineData("qflow:1:300:300")]
        [InlineData("qflow:1:300:295")]
        public void FromString_ThrowsArgumentException_WhenStartTimeIsTheSameOrBeforeEndTime(string descriptor)
        {
            Assert.Throws<ArgumentException>(() => QFlowSlotDescriptor.FromString(descriptor));
        }

        [Fact]
        public void FromString_ProducesSlotDescriptor_WhenFormattedCorrectly()
        {
            var result = QFlowSlotDescriptor.FromString("qflow:12:34:56");

            result.CalendarId.Should().Be(12);
            result.StartTime.Should().Be(34);
            result.EndTime.Should().Be(56);
        }
    }
}
