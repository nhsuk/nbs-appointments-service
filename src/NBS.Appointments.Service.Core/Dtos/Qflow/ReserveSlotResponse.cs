namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class ReserveSlotResponse
    {
        public ReserveSlotResponse(int slotOrdinalNumber, string? errorMsg)
        {
            SlotOrdinalNumber = slotOrdinalNumber;
            ErrorMsg = errorMsg;
        }

        public int SlotOrdinalNumber { get; set; }
        public string? ErrorMsg { get; set; }
    }

    // Temp class to investigate qflow issue, will be removed
    public class ErrorResponse
    {
        public int ErrorNumber { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
    }
}
