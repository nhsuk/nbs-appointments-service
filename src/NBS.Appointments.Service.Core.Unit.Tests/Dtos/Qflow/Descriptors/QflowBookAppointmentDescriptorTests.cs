using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using Xunit;

namespace NBS.Appointments.Service.Core.Unit.Tests.Dtos.Qflow.Descriptors
{
    public class QflowBookAppointmentDescriptorTests
    {
        [Theory]
        [InlineData("not:enough:segments", "Descriptor is not formatted correctly.")]
        [InlineData("enough:segments:does:not:start:with:qflow", "String was not a qflow service descriptor.")]
        [InlineData("too:many:segments:for:the:book:appointment:urn", "Descriptor is not formatted correctly.")]
        [InlineData("qflow:invalid:123:321:2023-03-31:456:654", "ServiceId must be a number.")]
        [InlineData("qflow:123:invalid:321:2023-03-31:456:654", "CalendarId must be a number.")]
        [InlineData("qflow:123:321:invalid:2023-03-31:456:654", "AppointmentTypeId must be a number.")]
        [InlineData("qflow:123:321:456:invalid:456:654", "Appointment date must be a valid date.")]
        [InlineData("qflow:123:321:456:2023-03-31:invalid:654", "Appointment time must be a number.")]
        [InlineData("qflow:123:321:456:2023-03-31:456:invalid", "Slot ordinal number must be a number.")]
        public void FromString_ThrowsFormatException_WhenUrnIsFormattedIncorrectly(string descriptor, string expectedErrorMsg)
        {
            var exception = Assert.Throws<FormatException>(() => QflowBookAppointmentDescriptor.FromString(descriptor));
            exception.Message.Should().Be(expectedErrorMsg);
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
