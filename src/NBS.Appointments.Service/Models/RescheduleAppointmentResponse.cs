using NBS.Appointments.Service.Core.Dtos.Qflow;

namespace NBS.Appointments.Service.Models
{
    public class RescheduledAppointmentResponse
    {
        public string Ref { get; set; }

        public static RescheduledAppointmentResponse FromQflowResponse(long qflowCustomerId, RescheduleAppointmentResponse response)
        {
            return new RescheduledAppointmentResponse
            {
                Ref = $"qflow:{qflowCustomerId}:{response.ProcessId}:{response.CaseId}"
            };
        }
    }
}
