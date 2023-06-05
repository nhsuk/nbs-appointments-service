using NBS.Appointments.Service.Core.Helpers;

namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    [Descriptor("qflow:{ServiceId}:{CalendarId}:{AppointmentTypeId}:{Date}:{StartTime}:{EndTime}")]
    public class QFlowSlotDescriptor
    {       
        public int ServiceId { get; set; }
        public int CalendarId { get; set; }
        public int AppointmentTypeId { get; set; }
        public DateTime Date { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
    }
}
