using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using Xunit;

namespace NBS.Appointments.Service.Core.Unit.Tests.Dtos.Qflow.Descriptors
{
    public class QflowNameDescriptorTests
    {
        [Theory]
        [InlineData("too:many:segments")]
        [InlineData("oneSegment")]
        public void FromString_ThrowsFormatException_WhenDescriptorFormattedIncorrectly(string descriptor)
        {
            Assert.Throws<FormatException>(() => QflowNameDescriptor.FromString(descriptor));
        }

        [Fact]
        public void FromString_ReturnsExpectedNameDescriptor()
        {
            var result = QflowNameDescriptor.FromString("Bruce:Wayne");

            result.FirstName.Should().Be("Bruce");
            result.Surname.Should().Be("Wayne");
        }
    }
}
