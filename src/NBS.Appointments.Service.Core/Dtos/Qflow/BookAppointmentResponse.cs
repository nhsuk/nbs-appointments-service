namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class BookAppointmentResponse
    {
        public long CaseId { get; set; }

        public long ProcessId { get; set; }

        public long AppointmentId { get; set; }

        public long CalendarId { get; set; }

        public long QNumber { get; set; }

        public string QCode { get; set; }

        public long CustomerTreatmentPlanId { get; set; }
    }
}
