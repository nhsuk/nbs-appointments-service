using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class BookAppointmentPayload : BasePayload
    {
        [JsonPropertyName("serviceId")]
        public long ServiceId { get; set; }
        [JsonPropertyName("dateAndTime")]
        public string DateAndTime { get; set; }
        [JsonPropertyName("customerId")]
        public long CustomerId { get; set; }
        [JsonPropertyName("appointmentTypeId")]
        public long AppointmentTypeId { get; set; }
        [JsonPropertyName("calendarId")]
        public int CalendarId { get; set; }
        [JsonPropertyName("slotOrdinalNumber")]
        public int SlotOrdinalNumber { get; set; }
        [JsonPropertyName("customProperties")]
        public Dictionary<string, string>? CustomProperties { get; set; }

        /* properties with default values */
        [JsonPropertyName("parentCaseId")]
        public long ParentCaseId => 0;
        [JsonPropertyName("subject")]
        public string Subject => "";
        [JsonPropertyName("notes")]
        public string Notes => "";
        [JsonPropertyName("extRef")]
        public string ExtRef => "";
        [JsonPropertyName("preventAutoQueue")]
        public bool PreventAutoQueue => false;
        [JsonPropertyName("languageCode")]
        public string LanguageCode => "en";
        [JsonPropertyName("isWalkIn")]
        public bool IsWalkIn => false;
        [JsonPropertyName("forceSimultaneousAppointment")]
        public bool ForceSimultaneousAppointment => true;
        [JsonPropertyName("forceWastedDuration")]
        public bool ForceWastedDuration => true;
        [JsonPropertyName("autoFreeUp")]
        public bool AutoFreeUp => true;
        [JsonPropertyName("basedOnAppointmentRequestId")]
        public int BasedOnAppointmentRequestId => 0;
        [JsonPropertyName("duration")]
        public int Duration => 0;
        [JsonPropertyName("simulationOnly")]
        public bool SimulationOnly => false;
        [JsonPropertyName("forceNoDynamicVacancy")]
        public bool ForceNoDynamicVacancy => false;
        [JsonPropertyName("ExistingProcessId")]
        public int ExistingProcessId => 0;
        [JsonPropertyName("ActOptionId")]
        public int ActOptionId => 0;
    }
}
