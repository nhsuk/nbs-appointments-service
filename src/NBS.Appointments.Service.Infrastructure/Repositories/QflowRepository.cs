using Microsoft.AspNetCore.WebUtilities;
using NBS.Appointments.Service.Dtos.Qflow;
using NBS.Appointments.Service.Infrastructure.ApiClient;
using NBS.Appointments.Service.Infrastructure.Entities.Api;
using NBS.Appointments.Service.Infrastructure.Interfaces.Repositories;

namespace NBS.Appointments.Service.Infrastructure.Repositories
{
    public class QflowRepository : ApiClientBase, IQflowRepository
    {

        public async Task<ApiResult<List<SiteAvailabilityResponse>>> GetAvailabilityAsync(int dose, string vaccine, IEnumerable<int> unitId, DateTime startDate, int duration, string externalReference)
        {
            var query = new Dictionary<string, string>
            {
                { "Dose", dose.ToString() },
                { "Vaccine", vaccine },
                { "SiteId", string.Join(",", unitId.Select(id => id.ToString())) },
                { "StartDate", $"{startDate:yyyy-MM-dd}" },
                { "Days", duration.ToString() },
                { "ExternalReference", externalReference ?? "" }
            };
            var endpointUrl = QueryHelpers.AddQueryString("qflow/availability", query);

            return await GetAsync<List<SiteAvailabilityResponse>>(endpointUrl);
        }
    }
}
