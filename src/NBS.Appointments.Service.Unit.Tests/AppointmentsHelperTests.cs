using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Helpers;
using Xunit;

namespace NBS.Appointments.Service.Unit.Tests
{
    public class AppointmentsHelperTests
    {
        [Fact]
        public void FilterPastAppointments_ShouldReturnAllAppointments_WhenIncludePastAppointmentsIsTrue()
        {
            var appointments = new List<AppointmentResponse>
            {
                new AppointmentResponse
                {
                    AppointmentDate = DateTime.Now.AddDays(30),
                    AppointmentDuration = 5,
                    AppointmentId = 12345,
                    CaseId = 54321,
                    ProcessId = 15243,
                    ServiceId = 123,
                    UnitAddress = "21 Jump Street, 21JS 2IC",
                    CustomerId = 987654,
                    CurrentEntityStatus = 0,
                    CancelationReasonId = 0,
                    UnitId = 151
                },
                new AppointmentResponse
                {
                    AppointmentDate = DateTime.Now.AddDays(-30),
                    AppointmentDuration = 5,
                    AppointmentId = 654875,
                    CaseId = 123456,
                    ProcessId = 321654,
                    ServiceId = 123,
                    UnitAddress = "21 Jump Street, 21JS 2IC",
                    CustomerId = 987654,
                    CurrentEntityStatus = 0,
                    CancelationReasonId = 0,
                    UnitId = 151
                }
            };

            var filteredAppointments = AppointmentsHelper.FilterPastCustomerAppointments(appointments, true);

            filteredAppointments.Count.Should().Be(2);
        }

        [Fact]
        public void FilterPastAppointments_ShouldReturnRemovePastAppointments()
        {
            var appointments = new List<AppointmentResponse>
            {
                new AppointmentResponse
                {
                    AppointmentDate = DateTime.Now.AddDays(30),
                    AppointmentDuration = 5,
                    AppointmentId = 12345,
                    CaseId = 54321,
                    ProcessId = 15243,
                    ServiceId = 123,
                    UnitAddress = "21 Jump Street, 21JS 2IC",
                    CustomerId = 987654,
                    CurrentEntityStatus = 0,
                    CancelationReasonId = 0,
                    UnitId = 151
                },
                new AppointmentResponse
                {
                    AppointmentDate = DateTime.Now.AddDays(-30),
                    AppointmentDuration = 5,
                    AppointmentId = 654875,
                    CaseId = 123456,
                    ProcessId = 321654,
                    ServiceId = 123,
                    UnitAddress = "21 Jump Street, 21JS 2IC",
                    CustomerId = 987654,
                    CurrentEntityStatus = 0,
                    CancelationReasonId = 0,
                    UnitId = 151
                }
            };

            var filteredAppointments = AppointmentsHelper.FilterPastCustomerAppointments(appointments, false);

            filteredAppointments.Count.Should().Be(1);
        }
    }
}
