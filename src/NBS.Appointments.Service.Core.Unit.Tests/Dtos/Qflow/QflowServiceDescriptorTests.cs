using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using Xunit;

namespace NBS.Appointments.Service.Core.Unit.Tests.Dtos.Qflow
{
    public class QflowServiceDescriptorTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("not-a-urn")]
        [InlineData("qflow:too:short")]
        [InlineData("qflow:but:too:many:segments")]

        public void FromString_ThrowsFormatException_WhenUrnIsBadlyFormated(string sample)
        {
            Assert.Throws<FormatException>(() => QflowServiceDescriptor.FromString(sample));
        }

        [Fact]
        public void FromString_ThrowsFormatException_WhenUrnSchemeIsIncorrect()
        {
            Assert.Throws<FormatException>(() => QflowServiceDescriptor.FromString("notqflow:0:12345:ref"));
        }

        [Fact]
        public void FromString_ProducesSiteDescriptor_WhenFormattedCorrectly()
        {
            var siteDescriptor = QflowServiceDescriptor.FromString("qflow:0:12345:ref");
            siteDescriptor.Dose.Should().Be("0");
            siteDescriptor.Vaccine.Should().Be("12345");
            siteDescriptor.Reference.Should().Be("ref");
        }

    }
}