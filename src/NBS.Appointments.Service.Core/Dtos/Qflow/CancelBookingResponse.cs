namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class CancelBookingResponse
    {
        public bool CanceledByScript { get; set; }
        public int ProcessId { get; set; }
        public int UserId { get; set; }
        public int DelegateId { get; set; }
    }
}
