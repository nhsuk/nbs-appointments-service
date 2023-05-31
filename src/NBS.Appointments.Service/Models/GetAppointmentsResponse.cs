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
        public int Site { get; set; }
        public int Service { get; set; }
        public DateTime From { get; set; }
        public int Duration { get; set; }
        public Status Status { get; set; }
        public Attendee Attendee { get; set; }

        public static IList<GetAppointmentsResponse> FromQflowResponse(IList<AppointmentResponse> appointments, string nhsNumber, string customerName)
        {
            return appointments.Select(x => new GetAppointmentsResponse()
            {
                Ref = $"qflow:{x.CustomerId}:{x.ProcessId}",
                Site = x.UnitId,
                Service = x.ServiceId,
                From = x.AppointmentDate,
                Duration = x.AppointmentDuration,
                Status = new Status
                {
                    Code = ((QflowAppointmentStatus)x.CurrentEntityStatus).ToString(),
                    Reason = GetCancellationReason(x.CancelationReasonId)
                },
                Attendee = new Attendee
                {
                    NhsNumber = nhsNumber,
                    Name = customerName
                }
            })
            .ToList();
        }

        private static string GetCancellationReason(int cancellationReasonId)
        {
            return Enum.IsDefined(typeof(AppointmentCancellationReason), cancellationReasonId)
                ? ((AppointmentCancellationReason)cancellationReasonId).ToString()
                : "none";
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
