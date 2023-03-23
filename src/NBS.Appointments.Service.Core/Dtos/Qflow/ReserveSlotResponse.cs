namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class ReserveSlotResponse
    {
        public ReserveSlotResponse(int slotOrdinalNumber, string? responseErrors)
        {
            SlotOrdinalNumber = slotOrdinalNumber;
            ResponseErrors = responseErrors;
        }

        public int SlotOrdinalNumber { get; set; }
        public string? ResponseErrors { get; set; }
    }
}
