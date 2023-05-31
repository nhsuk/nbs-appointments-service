using NBS.Appointments.Service.Core.Helpers;
using Xunit;
using FluentAssertions;

namespace NBS.Appointments.Service.Core.Unit.Tests.Helpers
{
    public class DescriptorConverterTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Parse_ThrowsArgumentException_WhenDescriptorStringIsNullOrWhitespace(string descriptor)
        {
            var exception = Assert.Throws<ArgumentException>(() => DescriptorConverter.Parse<TestDescriptor>(descriptor));
            exception.Message.Should().Be("Descriptor cannot be null or empty.");
        }

        [Fact]
        public void Parse_ThrowsInvalidOperationException_WhenTypeDoesNotHaveDescriptorAttribute()
        {
            var exception = Assert.Throws<InvalidOperationException>(() => DescriptorConverter.Parse<TestClassThatIsNotADescriptor>("prefix:test:12:2023-11-05"));
            exception.Message.Should().Be("Type was not decorated with a descriptor attribute.");
        }

        [Theory]
        [InlineData("prefix:test:123", 3)]
        [InlineData("prefix:test:123:2023-01-01:34", 5)]
        public void Parse_ThrowsFormatException_WhenDescriptorDoesNotHaveTheRightNumberOfElements(string descriptor, int parts)
        {
            var exception = Assert.Throws<FormatException>(() => DescriptorConverter.Parse<TestDescriptor>(descriptor));
            exception.Message.Should().Be($"Descriptor does not contain the right amount of elements. Expected 4 but found {parts}.");
        }

        [Fact]
        public void Parse_ThrowsFormatException_WhenStaticTextDoesNotMatch()
        {
            var data = "mismatch:test:123:2023-01-01";
            var exception = Assert.Throws<FormatException>(() => DescriptorConverter.Parse<TestDescriptor>(data));
            exception.Message.Should().Be("Expected prefix but found mismatch.");
        }

        [Fact]
        public void Parse_ThrowsInvalidOperationException_WhenUnderlyingClassDoesNotHaveProperty()
        {
            var exception = Assert.Throws<InvalidOperationException>(() => DescriptorConverter.Parse<BadTestDescriptor>("test"));
            exception.Message.Should().Be("Descriptor format expects property MissingProperty and it was not found.");
        }
        
        [Theory]
        [InlineData("prefix:test:nan:2023-05-23", "IntProp")]
        [InlineData("prefix:test:12:not-a-date", "DateTimeProp")]
        public void Parse_ThrowsFormatException_WhenTypeConversionFails(string descriptor, string propName)
        {            
            var exception = Assert.Throws<FormatException>(() => DescriptorConverter.Parse<TestDescriptor>(descriptor));
            exception.Message.Should().Be($"The element {propName} was not in the correct format.");
        }

        [Fact]
        public void Parse_ReturnsDescriptorObject_FromCorrectlyFormattedString()
        {
            var result = DescriptorConverter.Parse<TestDescriptor>("prefix:test:12:2023-11-05");
            result.StringProp.Should().Be("test");
            result.IntProp.Should().Be(12);
            result.DateTimeProp.Should().Be(new DateTime(2023, 11, 5));
        }
    }

    public class TestClassThatIsNotADescriptor {}


    [Descriptor("prefix:{StringProp}:{IntProp}:{DateTimeProp}")]
    public class TestDescriptor
    {
        public string StringProp {  get; set; }
        public int IntProp { get; set; }
        public DateTime DateTimeProp { get; set; }
    }

    [Descriptor("{MissingProperty}")]
    public class BadTestDescriptor
    {

    }
}
