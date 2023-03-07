using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using Xunit;

namespace NBS.Appointments.Service.Core.Unit.Tests.Dtos.Qflow
{
    public class QflowSiteDescriptorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void FromString_ThrowsArgumentException_WhenUrnIsNullOrEmpty(string descriptor)
        {
            Assert.Throws<ArgumentException>(() => QFlowSiteDescriptor.FromString(descriptor));
        }

        [Theory]
        [InlineData("not-a-urn")]
        [InlineData("incorrect:site:descriptor")]
        [InlineData("qflow:siteId:not:integer")]
        public void FromString_ThrowFormatException_WhenUrnFormatIsIncorrect(string descriptor)
        {
            Assert.Throws<FormatException>(() => QFlowSiteDescriptor.FromString(descriptor));
        }

        [Fact]
        public void FromStringProducesSiteId_WhenUrnFormattedCorrectly()
        {
            var siteIdentifier = QFlowSiteDescriptor.FromString("qflow:123");
            siteIdentifier.SiteId.Should().Be(123);
        }
    }
}
