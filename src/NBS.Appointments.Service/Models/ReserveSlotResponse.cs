using NBS.Appointments.Service.Core.Dtos.Qflow;
using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Models
{
    public class LockSlotResponse
    {
        [JsonPropertyName("ref")]
        public string Ref { get; set; }

        public static LockSlotResponse FromQflowResponse(int serviceId, int calendarId, int appointmentTypeId, int startTime, ReserveSlotResponse responseData) => new LockSlotResponse
        {
            Ref = $"qflow:{serviceId}:{calendarId}:{appointmentTypeId}:{startTime}:{responseData.SlotOrdinalNumber}"
        };

    }
}
