using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Marvin.StreamExtensions;
using Microsoft.Extensions.Logging;
using NBS.Appointments.Service.Infrastructure.Builders;
using NBS.Appointments.Service.Infrastructure.Entities.Api;
using Newtonsoft.Json;

namespace NBS.Appointments.Service.Infrastructure.ApiClient
{
    public abstract class ApiClientBase
    {
        private readonly ILogger _logger;
        public HttpClient Client { get; }

        protected readonly string SubscriptionKey;

        protected ApiClientBase()
        {

        }

        public async Task<ApiResult<TResponse>> GetAsync<TResponse>(string endpointUrl)
            where TResponse : class
        {
            var response = await SendRequest(HttpMethod.Get, endpointUrl, null);
            if (response?.Headers.Contains("x-source") ?? false)
                _logger.LogDebug("Source: {0}, endpoint: {1}", response.Headers.GetValues("x-source"), response.RequestMessage?.RequestUri?.PathAndQuery);
            return await GetApiResult<TResponse>(response);
        }

        public async Task<HttpResponseMessage> SendRequest(HttpMethod method, string endpointUrl, object requestBody)
        {
            var builder = new HttpRequestBuilder(method, endpointUrl)
                .AddAccept(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json))
                .AddAcceptEncoding(new StringWithQualityHeaderValue("gzip"))
                .AddHeader("Subscription-Key", SubscriptionKey);

            var requestBodyJson = "none";
            if (requestBody != null)
            {
                requestBodyJson = JsonConvert.SerializeObject(requestBody);
                builder.AddContent(new StringContent(requestBodyJson, Encoding.UTF8, MediaTypeNames.Application.Json));
            }

            try
            {
                var response = await Client.SendAsync(builder.RequestMessage, HttpCompletionOption.ResponseHeadersRead);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Request failed: {method}:{endpointUrl}. Request content: {requestBodyJson}");
                }
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error calling endpoint. {method}:{endpointUrl}. Content: {requestBodyJson}");
                throw;
            }
        }

        private async Task<ApiResult<TResponse>> GetApiResult<TResponse>(HttpResponseMessage response)
            where TResponse : class
        {
            try
            {
                await using var stream = await response.Content.ReadAsStreamAsync();
                return DeserializeJson<TResponse>(response, stream);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error deserialising json into type {typeof(TResponse)}");
                throw;
            }
        }

        public ApiResult<TResponse> DeserializeJson<TResponse>(HttpResponseMessage response, Stream stream) where TResponse : class
        {
            var result = stream.ReadAndDeserializeFromJson<TResponse>(new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return new ApiResult<TResponse>(response.StatusCode, result);
        }

        private void LogError(HttpResponseMessage response, object requestBody = null)
        {
            var errorLog = requestBody == null
                ? $"API error - Status Code: {response.StatusCode}, Url: {response.RequestMessage?.RequestUri?.PathAndQuery}"
                : $"API error - Status Code: {response.StatusCode}, Url: {response.RequestMessage?.RequestUri?.PathAndQuery}, Request: {JsonConvert.SerializeObject(requestBody)}";
            _logger.LogError(errorLog);
        }
    }
}
