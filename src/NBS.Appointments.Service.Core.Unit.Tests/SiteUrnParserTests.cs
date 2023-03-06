using FluentAssertions;
using Xunit;

namespace NBS.Appointments.Service.Core.Unit.Tests
{
    public class SiteUrnParserTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("not-a-urn")]
        [InlineData("too:many:segments")]
        public void Parse_ThrowsFormatException_WhenUrnIsBadlyFormatted(string sample)
        {
            Assert.Throws<FormatException>(() => SiteUrnParser.Parse(sample));
        }

        [Fact]
        public void Parse_ProducesUrnData_WhenUrnIsFormattedCorrectly()
        {
            var urn = SiteUrnParser.Parse("test:urn");
            urn.Scheme.Should().Be("test");
            urn.Identifier.Should().Be("urn");
        }
    }
}