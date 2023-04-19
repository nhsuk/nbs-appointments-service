namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public sealed class AppointmentResponse
    {
        public int UnitId { get; set; }
        public int CaseId { get; set; }
        public long CustomerId { get; set; }
        public int ProcessId { get; set; }
        public int AppointmentId { get; set; }
        public string UnitPath { get; set; }
        public string UnitName { get; set; }

        public string ServiceName { get; set; }
        public int ServiceId { get; set; }

        /// <summary>
        /// AppointmentDate format 2020-11-07T09:10:00
        /// </summary>
        public DateTime AppointmentDate { get; set; }
        public int AppointmentDuration { get; set; }
        public string AppointmentTypeName { get; set; }
        public int AppointmentTypeId { get; set; }
        public string UnitAddress { get; set; }
        public bool PreventAutoQueue { get; set; }
        public int CurrentEntityStatus { get; set; }
        public string AppointmentTypeExtRef { get; set; }
        public int Dose { get; set; }
        public int CancelationReasonId { get; set; }
        public string SelfReferalOccupation { get; set; }
    }
}
