using NBS.Appointments.Service.Core.Dtos.Qflow;

namespace NBS.Appointments.Service.Models
{
    public class RescheduledAppointmentResponse
    {
        public string Ref { get; set; }

        public static RescheduledAppointmentResponse FromQflowResponse(RescheduleAppointmentResponse response)
        {
            return new RescheduledAppointmentResponse
            {
                Ref = $"qflow:{response.CustomerId}:{response.ProcessId}:{response.CaseId}"
            };
        }
    }
}
