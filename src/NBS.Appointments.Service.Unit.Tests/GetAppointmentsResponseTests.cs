using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Models;
using Xunit;

namespace NBS.Appointments.Service.Unit.Tests
{
    public class GetAppointmentsResponseTests
    {
        [Fact]
        public void FromQflowResponse_MapsDataCorrectly()
        {
            var inputModel = new List<AppointmentResponse>
            {
                new AppointmentResponse
                {
                    AppointmentDate = new DateTime(2023, 06, 05),
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
                }
            };
            var nhsNumber = "1234567890";
            var customerName = "Bruce Wayne";

            var actual = GetAppointmentsResponse.FromQflowResponse(inputModel, nhsNumber, customerName);

            var expected = new List<GetAppointmentsResponse>
            {
                new GetAppointmentsResponse
                {
                    Ref = "qflow:987654:15243:54321",
                    Site = 151,
                    Service = 123,
                    From = new DateTime(2023, 06, 05),
                    Duration = 5,
                    Status = new Status
                    {
                        Code = "Expected",
                        Reason = "none"
                    },
                    Attendee = new Attendee
                    {
                        NhsNumber = nhsNumber,
                        Name = customerName
                    }
                }
            };

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
