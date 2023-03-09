using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Dtos.Qflow;

namespace NBS.Appointments.Service.Core.Interfaces.Services
{
    public interface IQflowService
    {
        public Task<SiteAvailabilityResponse[]> GetSiteAvailability(IEnumerable<string> siteIds, DateTime startDate, DateTime endDate, string dose, string vaccine, string externalReference);

        public Task<AvailabilityByHourResponse> GetSiteSlotAvailability(int siteId, DateTime date, int dose, string vaccineType, string externalReference);

        public Task<ReserveSlotResponse> ReserveSlot(int calendarId, int startTime, int endTime, int lockDuration);
    }
}
