using NBS.Appointments.Service.Dtos.Qflow;
using NBS.Appointments.Service.Core.Interfaces.Services;
using Microsoft.AspNetCore.WebUtilities;
using Polly;
using Microsoft.Extensions.Options;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using Newtonsoft.Json;

namespace NBS.Appointments.Service.Core.Services
{
    public class QflowService : IQflowService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IQflowSessionManager _sessionManager;
        private readonly QflowOptions _options;

        public QflowService(
            IOptions<QflowOptions> options,
            IHttpClientFactory httpClientFactory, 
            IQflowSessionManager sessionManager)
        {
            _options = options.Value;
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
        }

        public async Task<SiteAvailabilityResponse[]> GetSiteAvailability(IEnumerable<string> siteIds, DateTime startDate, DateTime endDate,
            string dose, string vaccine, string externalReference)
        {
            if(siteIds.Any() == false)
                throw new ArgumentException("At least one site must be provided", nameof(siteIds));

            if (String.IsNullOrEmpty(dose))
                throw new ArgumentException("A value must be provided", nameof(dose));

            if (String.IsNullOrEmpty(vaccine))
                throw new ArgumentException("A value must be provided", nameof(vaccine));

            int days = startDate.DaysBetween(endDate);
            if (days < 1)
                throw new ArgumentOutOfRangeException("The specified date range was not valid " + days);

            var query = new Dictionary<string, string>
            {
                { "Dose", dose },
                { "Vaccine", vaccine },
                { "SiteId", string.Join(",", siteIds)},
                { "StartDate", $"{startDate:yyyy-MM-dd}" },
                { "Days", days.ToString() },
                { "ExternalReference", externalReference ?? "" }
            };
            var endpointUrl = QueryHelpers.AddQueryString($"{_options.BaseUrl}/svcCustomAppointment.svc/rest/availability", query);

            var response = await Execute(query, endpointUrl);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SiteAvailabilityResponse[]>(responseBody);
        }

        public async Task<SiteSlotsResponse> GetSiteSlotAvailability(int siteId, DateTime date, string dose, string vaccineType, string externalReference)
        {
            if (string.IsNullOrWhiteSpace(vaccineType))
                throw new ArgumentException($"A value for {nameof(vaccineType)} must be provided.");

            var query = new Dictionary<string, string>
            {
                { "Date", $"{date:yyyy-MM-dd}" },
                { "SiteId", siteId.ToString() },
                { "Dose", dose },
                { "VaccineType", vaccineType },
                { "ExternalReference", externalReference }
            };
            var endpointUrl = QueryHelpers.AddQueryString($"{_options.BaseUrl}/svcCustomAppointment.svc/rest/GetSiteDoseAvailability", query);

            var response = await Execute(query, endpointUrl);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SiteSlotsResponse>(responseBody);
        }

        private async Task<HttpResponseMessage> Execute(Dictionary<string, string> query, string endpointUrl)
        {
            using var client = _httpClientFactory.CreateClient();
            var context = new Dictionary<string, object>
            {
                {"SessionId", _sessionManager.GetSessionId()}
            };

            var policy = GetRetryPolicy();
            return await policy.ExecuteAsync(async (context) =>
            {
                query["apiSessionId"] = context["SessionId"].ToString();
                return await client.GetAsync(endpointUrl);
            }, context);
        }

        private AsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return Policy
                .HandleResult<HttpResponseMessage>(rsp => rsp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                .RetryAsync(1, onRetry: (exception, retryCount, context) => {
                    Console.WriteLine("Session Invalid - retrying");
                    _sessionManager.Invalidate(context["SessionId"].ToString());
                    context["SessionId"] = _sessionManager.GetSessionId();
                });
        }
    }
}
