﻿using NBS.Appointments.Service.Core.Dtos;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Dtos.Qflow;

namespace NBS.Appointments.Service.Core.Interfaces.Services
{
    public interface IQflowService
    {
        public Task<SiteAvailabilityResponse[]> GetSiteAvailability(IEnumerable<string> siteIds, DateTime startDate, DateTime endDate, string dose, string vaccine, string externalReference);

        public Task<SiteSlotsResponse> GetSiteSlotAvailability(int siteId, DateTime date, string dose, string vaccineType, string externalReference);
        public Task<ReserveSlotResponse> ReserveSlot(int calendarId, int startTime, int endTime, int lockDuration);
        public Task<ApiResult<BookAppointmentResponse>> BookAppointment(int serviceId, DateTime dateAndTime, int customerId, int appointmentTypeId,
            int slotOrdinalNumber, int calendarId, Dictionary<string, string>? customProperties);
        public Task<CustomerDto> CreateOrUpdateCustomer(string firstName, string surname, string nhsNumber, string dob, string? email,
            string? phoneNumber, string? landline, string selfReferralOccupation);
    }
}
