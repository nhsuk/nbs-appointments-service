using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Models
{
    public class LockSlotResponse
    {
        [JsonPropertyName("ref")]
        public string Ref { get; set; }

        public static LockSlotResponse FromQflowResponse(QFlowSlotDescriptor descriptor, ReserveSlotResponse responseData) => new LockSlotResponse
        {
            Ref = $"qflow:{descriptor.ServiceId}:{descriptor.CalendarId}:{descriptor.AppointmentTypeId}:{descriptor.Date:yyyy-MM-dd}:{descriptor.StartTime}:{responseData.SlotOrdinalNumber}"
        };

    }
}
