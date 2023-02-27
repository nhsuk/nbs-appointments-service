﻿using NBS.Appointments.Service.Dtos.Qflow;
using NBS.Appointments.Service.Core.Interfaces.Services;
using Microsoft.AspNetCore.WebUtilities;
using Polly;
using Microsoft.Extensions.Options;

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
            var query = new Dictionary<string, string>
            {
                { "Dose", dose.ToString() },
                { "Vaccine", vaccine },
                { "SiteId", string.Join(",", siteIds)},
                { "StartDate", $"{startDate:yyyy-MM-dd}" },
                { "Days", endDate.DaysBetween(startDate).ToString() },
                { "ExternalReference", externalReference ?? "" }
            };

            using var client = _httpClientFactory.CreateClient();
            var context = new Dictionary<string, object>
            {
                {"SessionId", await _sessionManager.GetSessionId()}
            };

            var policy = GetRetryPolicy();
            var response = await policy.ExecuteAsync(async (context) => {
                query["apiSessionId"] = context["SessionId"].ToString();
                var endpointUrl = QueryHelpers.AddQueryString($"{_options.BaseUrl}/svcCustomAppointment.svc/rest/", query);
                return await client.GetAsync(endpointUrl);
            }, context);            

            var responseBody = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SiteAvailabilityResponse[]>(responseBody);
        }

        private AsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return Policy
                .HandleResult<HttpResponseMessage>(rsp => rsp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                .RetryAsync(3, onRetryAsync: async (exception, retryCount, context) => {
                    Console.WriteLine("Session Invalid - retrying");
                    _sessionManager.Invalidate(context["SessionId"].ToString());
                    context["SessionId"] = await _sessionManager.GetSessionId();
                });
        }
    }
}
