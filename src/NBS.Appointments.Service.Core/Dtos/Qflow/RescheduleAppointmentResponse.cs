namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class RescheduleAppointmentResponse
    {
        public SetAppointmentData SetAppointmentData { get; set; }
        public RescheduleAppointmentData RescheduleAppointmentData { get; set; }
        public object ScriptResults { get; set; }

        public long CaseId { get; set; }

        public long ProcessId { get; set; }

        public long AppointmentId { get; set; }

        public long CalendarId { get; set; }

        public long QNumber { get; set; }

        public string QCode { get; set; }

        public long CustomerId { get; set; }
    }

    public class SetAppointmentData
    {
        public long ParentCaseId { get; set; }
        public long CustomerId { get; set; }
        public long SlotOrdinalNumber { get; set; }
        public long CalendarId { get; set; }
        public long BasedOnAppointmentRequestId { get; set; }
        public bool SimulationOnly { get; set; }
        public long TreatmentPlanId { get; set; }
        public long TreatmentPlanStepId { get; set; }
        public long CustomerTreatmentPlanId { get; set; }
        public long ExistingProcessId { get; set; }
        public long ActOptionId { get; set; }
    }

    public class RescheduleAppointmentData
    {
        public long OriginalProccessId { get; set; }
        public int CancellationReasonId { get; set; }
        public int TreatmentPlanCancellationMethod { get; set; }
        public string Comments { get; set; }
        public object CustomProperties { get; set; }
        public bool UseExistingProcess { get; set; }
    }
}
