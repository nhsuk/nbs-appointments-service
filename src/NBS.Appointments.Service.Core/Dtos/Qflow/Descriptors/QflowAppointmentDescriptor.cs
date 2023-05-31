using NBS.Appointments.Service.Core.Helpers;

namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    [Descriptor("qflow:{ServiceId}:{CalendarId}:{AppointmentTypeId}:{AppointmentDate}:{AppointmentTime}:{SlotOrdinalNumber}")]
    public class QflowBookAppointmentDescriptor
    {
        public int ServiceId { get; set; }
        public int CalendarId { get; set; }
        public int AppointmentTypeId { get; set; }

        public DateOnly AppointmentDate { get; set; }

        public int AppointmentTime { get; set; }
        public DateTime AppointmentDateAndTime => AppointmentDate.ToDateTime(TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(AppointmentTime)));
        public int SlotOrdinalNumber { get; set; }
    }
}
