using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using Xunit;

namespace NBS.Appointments.Service.Core.Unit.Tests.Dtos.Qflow.Descriptors
{
    public class QflowCustomPropertyDescriptorTests
    {
        [Theory]
        [InlineData("qflow:1")]
        [InlineData("not:starting:qflow")]
        public void FromString_ThrowsFormatException_WhenDescriptorFormattedIncorrectly(string descriptor)
        {
            Assert.Throws<FormatException>(() => QflowCustomPropertiesDescriptor.FromString(descriptor));
        }

        [Fact]
        public void FromString_ProducesCustomPropertiesDescriptor_WhenUrnFormattedCorrectly_WithTwoProperties()
        {
            var result = QflowCustomPropertiesDescriptor.FromString("qflow:0:1");

            result.AppBooking.Should().Be("1");
            result.CallCentreBooking.Should().Be("0");
            result.CallCentreEmailAddress.Should().BeNull();
        }

        [Fact]
        public void FromString_ProducesCustomPropertiesDescriptor_WhenUrnFormattedCorrectly_WithThreeProperties()
        {
            var result = QflowCustomPropertiesDescriptor.FromString("qflow:1:0:callcentre@email.com");

            result.AppBooking.Should().Be("0");
            result.CallCentreBooking.Should().Be("1");
            result.CallCentreEmailAddress.Should().Be("callcentre@email.com");
        }
    }
}
