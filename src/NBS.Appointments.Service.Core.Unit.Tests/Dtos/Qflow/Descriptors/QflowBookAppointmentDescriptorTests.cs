using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using Xunit;

namespace NBS.Appointments.Service.Core.Unit.Tests.Dtos.Qflow.Descriptors
{
    public class QflowBookAppointmentDescriptorTests
    {
        [Theory]
        [InlineData("not:enough:segments")]
        [InlineData("enough:segments:does:not:start:with:qflow")]
        [InlineData("too:many:segments:for:the:book:appointment:urn")]
        [InlineData("qflow:invalid:123:321:2023-03-31:456:654")]
        [InlineData("qflow:123:invalid:321:2023-03-31:456:654")]
        [InlineData("qflow:123:321:invalid:2023-03-31:456:654")]
        [InlineData("qflow:123:321:456:invalid:456:654")]
        [InlineData("qflow:123:321:456:2023-03-31:invalid:654")]
        [InlineData("qflow:123:321:456:2023-03-31:456:invalid")]
        public void FromString_ThrowsFormatException_WhenUrnIsFormattedIncorrectly(string descriptor)
        {
            Assert.Throws<FormatException>(() => QflowBookAppointmentDescriptor.FromString(descriptor));
        }

        [Fact]
        public void FromString_ProducesBookAppointmentDescriptor_WhenUrnFormattedCorrectly()
        {
            var result = QflowBookAppointmentDescriptor.FromString("qflow:123:321:2:2023-03-31:600:456");

            result.AppointmentDateAndTime.Should().Be(new DateTime(2023, 03, 31, 10, 00, 00));
            result.AppointmentTypeId.Should().Be(2);
            result.CalendarId.Should().Be(321);
            result.ServiceId.Should().Be(123);
            result.SlotOrdinalNumber.Should().Be(456);
        }
    }
}
