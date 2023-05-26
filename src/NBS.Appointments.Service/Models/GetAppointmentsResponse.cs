using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBS.Appointments.Service.Models
{
    public class GetAppointmentsResponse
    {
        public string Ref { get; set; }
        public string Site { get; set; }
        public string Service { get; set; }
        public DateTime From { get; set; }
        public int Duration { get; set; }
        public Status Status { get; set; }
        public Attendee Attendee { get; set; }

        public static IList<GetAppointmentsResponse> FromQflowResponse(IList<AppointmentResponse> appointments, string nhsNumber, string customerName)
        {
            return appointments.Select(x => new GetAppointmentsResponse()
            {
                Ref = $"qflow:{x.CustomerId}:{x.ProcessId}",
                Site = $"qflow:{x.UnitId}",
                // TODO: The service urn for the availability endpoint contains the vaccine code but we don't have access to that here
                Service = $"qflow:{x.Dose}:{x.ServiceId}:{x.AppointmentTypeExtRef}",
                From = x.AppointmentDate,
                Duration = x.AppointmentDuration,
                Status = new Status
                {
                    Code = ((QflowAppointmentStatus)x.CurrentEntityStatus).ToString(),
                    Reason = ((AppointmentCancellationReason)x.CancelationReasonId).ToString()
                },
                Attendee = new Attendee
                {
                    NhsNumber = nhsNumber,
                    Name = customerName
                }
            })
            .ToList();
        }
    }

    public class Status
    {
        public string Code { get; set; }
        public string Reason { get; set; }
    }

    public class Attendee
    {
        public string NhsNumber { get; set; }
        public string Name { get; set; }
    }
}
