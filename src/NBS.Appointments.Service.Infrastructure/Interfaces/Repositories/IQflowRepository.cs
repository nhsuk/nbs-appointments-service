using NBS.Appointments.Service.Dtos.Qflow;
using NBS.Appointments.Service.Infrastructure.Entities.Api;

namespace NBS.Appointments.Service.Infrastructure.Interfaces.Repositories
{
    public interface IQflowRepository
    {
        public Task<ApiResult<List<SiteAvailabilityResponse>>> GetAvailabilityAsync(int dose, string vaccine,
            IEnumerable<int> unitId, DateTime startDate, int duration, string externalReference);
    }
}
