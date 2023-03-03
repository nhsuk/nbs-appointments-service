using NBS.Appointments.Service.Dtos.Qflow;
using NBS.Appointments.Service.Core.Interfaces.Services;
using Microsoft.AspNetCore.WebUtilities;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using Newtonsoft.Json;

namespace NBS.Appointments.Service.Core.Services
{
    public class QflowService : IQflowService
    {
        private const string QflowUrl = "http://localhost:4010";

        private readonly IHttpClientFactory _httpClientFactory;

        public QflowService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<SiteAvailabilityResponse[]> GetSiteAvailability(IEnumerable<string> siteIds, DateTime startDate, DateTime endDate,
            string dose, string vaccine, string externalReference)
        {
            var query = new Dictionary<string, string>
            {
                { "Dose", dose.ToString() },
                { "Vaccine", vaccine },
                { "SiteId", string.Join(",", siteIds)},
                { "StartDate", $"{startDate:yyyy-MM-dd}" },
                { "Days", endDate.DaysBetween(startDate).ToString() },
                { "ExternalReference", externalReference ?? "" }
            };
            var endpointUrl = QueryHelpers.AddQueryString($"{QflowUrl}/svcCustomAppointment.svc/rest/", query);
            
            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(endpointUrl);

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
            return JsonConvert.DeserializeObject<SiteAvailabilityResponse[]>(responseBody);
        }

        public async Task<SiteSlotAvailabilityByHourResponse> GetSiteSlotAvailabilityAsync(int siteId, DateTime date, string appointmentType)
        {
            var query = new Dictionary<string, string>
            {
                { "Date", $"{date:yyyy-MM-dd}" },
                { "SiteId", siteId.ToString() },
                { "VaccineType", appointmentType },
                { "ExternalReference", "NotSet" }
            };
            var endpointUrl = QueryHelpers.AddQueryString($"{QflowUrl}/GetSiteDoseAvailability", query);

            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(endpointUrl);

            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SiteSlotAvailabilityByHourResponse>(responseBody);
        }
    }    
}
