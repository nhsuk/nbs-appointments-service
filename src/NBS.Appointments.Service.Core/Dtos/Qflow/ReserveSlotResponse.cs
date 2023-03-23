namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class ReserveSlotResponse
    {
        public ReserveSlotResponse(int slotOrdinalNumber)
        {
            SlotOrdinalNumber = slotOrdinalNumber;
        }

        public int SlotOrdinalNumber { get; set; }
    }
}
