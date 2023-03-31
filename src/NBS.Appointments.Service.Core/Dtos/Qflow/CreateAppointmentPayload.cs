using Newtonsoft.Json;

namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class CreateAppointmentPayload : BasePayload
    {
        [JsonProperty("serviceId")]
        public long ServiceId { get; set; }
        [JsonProperty("dateAndTime")]
        public string DateAndTime { get; set; }
        [JsonProperty("customerId")]
        public long CustomerId { get; set; }
        [JsonProperty("appointmentTypeId")]
        public long AppointmentTypeId { get; set; }
        [JsonProperty("calendarId")]
        public int CalendarId { get; set; }
        [JsonProperty("slotOrdinalNumber")]
        public int SlotOrdinalNumber { get; set; }
        [JsonProperty("customProperties")]
        public Dictionary<string, string>? CustomProperties { get; set; }

        /* properties with default values */
        [JsonProperty("parentCaseId")]
        public long ParentCaseId => 0;
        [JsonProperty("subject")]
        public string Subject => "";
        [JsonProperty("notes")]
        public string Notes => "";
        [JsonProperty("extRef")]
        public string ExtRef => "";
        [JsonProperty("preventAutoQueue")]
        public bool PreventAutoQueue => false;
        [JsonProperty("languageCode")]
        public string LanguageCode => "en";
        [JsonProperty("isWalkIn")]
        public bool IsWalkIn => false;
        [JsonProperty("forceSimultaneousAppointment")]
        public bool ForceSimultaneousAppointment => true;
        [JsonProperty("forceWastedDuration")]
        public bool ForceWastedDuration => true;
        [JsonProperty("autoFreeUp")]
        public bool AutoFreeUp => true;
        [JsonProperty("basedOnAppointmentRequestId")]
        public int BasedOnAppointmentRequestId => 0;
        [JsonProperty("duration")]
        public int Duration => 0;
        [JsonProperty("simulationOnly")]
        public bool SimulationOnly => false;
        [JsonProperty("forceNoDynamicVacancy")]
        public bool ForceNoDynamicVacancy => false;
        [JsonProperty("ExistingProcessId")]
        public int ExistingProcessId => 0;
        [JsonProperty("ActOptionId")]
        public int ActOptionId => 0;
    }
}
