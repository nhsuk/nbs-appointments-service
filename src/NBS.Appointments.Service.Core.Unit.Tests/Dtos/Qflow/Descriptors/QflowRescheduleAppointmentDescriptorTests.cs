using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using Xunit;

namespace NBS.Appointments.Service.Core.Unit.Tests.Dtos.Qflow.Descriptors
{
    public class QflowRescheduleAppointmentDescriptorTests
    {
        [Theory]
        [InlineData("not:enough:segments", "Descriptor was not formatted correctly.")]
        [InlineData("pflow:incorrect:starting:segment:value:correct:length", "Descriptor was not a valid qflow appointment descriptor.")]
        [InlineData("qflow:n:12:34:2023-04-29:545:3", "ServiceId must be a number.")]
        [InlineData("qflow:12:n:34:2023-04-29:545:3", "OriginalProcessId must be a number.")]
        [InlineData("qflow:12:34:n:2023-04-29:545:3", "AppointmentTypeId must be a number.")]
        [InlineData("qflow:12:34:56:n:545:3", "Appointment date must be a valid date.")]
        [InlineData("qflow:12:34:56:2023-04-29:n:3", "Start time must be a number.")]
        [InlineData("qflow:12:34:56:2023-04-29:545:n", "CancelationReasonId must be a number.")]
        public void FromString_ShouldThrowFormatException_WhenDescripterIsInvalid(string descripter, string expectedErrorMsg)
        {
            var result = Assert.Throws<FormatException>(() => QflowRescheduleAppointmentDescriptor.FromString(descripter));
            result.Message.Should().Be(expectedErrorMsg);
        }

        [Fact]
        public void FromString_ReturnsDescriptor_WhenUrnFormattedCorrectly()
        {
            var result = QflowRescheduleAppointmentDescriptor.FromString("qflow:12:34:56:2023-04-29:545:3");

            result.ServiceId.Should().Be(12);
            result.OriginalProcessId.Should().Be(34);
            result.AppointmentTypeId.Should().Be(56);
            result.DateAndTime.Should().Be(new DateTime(2023, 04, 29, 09, 05, 00));
            result.CancelationReasonId.Should().Be(3);
        }
    }
}
