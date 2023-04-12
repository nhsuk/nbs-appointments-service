using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using Xunit;

namespace NBS.Appointments.Service.Core.Unit.Tests.Dtos.Qflow.Descriptors
{
    public class QflowCancelAppointmentDescriptorTests
    {
        [Theory]
        [InlineData("too:many:segments:for:descriptor", "Decscriptor is formatted incorrectly.")]
        [InlineData("someFlow:123:456", "String was not a qflow cancel appointment descriptor.")]
        [InlineData("qflow:n:123", "QflowCustomerId is in invalid format.")]
        [InlineData("qflow:123:n", "ProcessId must be a number.")]
        public void FromString_ThrowsFormatException_WhenDescriptorIsInvalid(string descriptor, string expectedErrorMsg)
        {
            var exception = Assert.Throws<FormatException>(() => QflowCancelAppointmentDescriptor.FromString(descriptor));
            exception.Message.Should().Be(expectedErrorMsg);
        }

        [Fact]
        public void FromString_ReturnsCancelAppointmentDescriptor_WhenUrnIsFormattedCorrectly()
        {
            var result = QflowCancelAppointmentDescriptor.FromString("qflow:123:456");

            result.QflowCustomerId.Should().Be(123);
            result.ProcessId.Should().Be(456);
        }
    }
}
