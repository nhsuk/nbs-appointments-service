using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using Xunit;

namespace NBS.Appointments.Service.Core.Unit.Tests.Dtos.Qflow.Descriptors
{
    public class QflowCancelationReasonDescriptorTests
    {
        [Theory]
        [InlineData("too:many:segments:for:descriptor", "Descriptor is formatted incorrectly.")]
        [InlineData("someFlow:123:456", "String was not a qflow cancelation reason descriptor.")]
        [InlineData("qflow:n:456", "CancelationReasonId must be a number.")]
        [InlineData("qflow:123:n", "TreatmentPlanCancelationMethod must be a number.")]
        public void FromString_ThrowsFormatException_WhenDescriptorFormattedIncorrectly(string descriptor, string expectedErrorMsg)
        {
            var exception = Assert.Throws<FormatException>(() => QflowCancelationReasonDescriptor.FromString(descriptor));
            exception.Message.Should().Be(expectedErrorMsg);
        }

        [Fact]
        public void FromString_ReturnsDescriptor_WhenUrnFormattedCorrectly()
        {
            var result = QflowCancelationReasonDescriptor.FromString("qflow:123:456");

            result.CancelationReasonId.Should().Be(123);
            result.TreatmentPlanCancelationMethod.Should().Be(456);
        }
    }
}
