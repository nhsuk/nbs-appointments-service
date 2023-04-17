namespace NBS.Appointments.Service.Core
{
    public class QflowOptions
    {
        public string BaseUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
        public string CallCentreBookingFlagId { get; set; }
        public string AppBookingFlagId { get; set; }
        public string CallCentreEmailFlagId { get; set; }
    }
}