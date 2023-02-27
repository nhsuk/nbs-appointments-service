using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace NBS.Appointments.Service.Infrastructure.Builders
{
    public class HttpRequestBuilder
    {
        public HttpRequestMessage RequestMessage { get; set; }

        public HttpRequestBuilder(HttpMethod method, string requestUri)
        {
            RequestMessage = new HttpRequestMessage(method, requestUri);
        }

        public HttpRequestBuilder(HttpMethod method, string requestUri, AuthenticationHeaderValue value)
        {
            RequestMessage = new HttpRequestMessage(method, requestUri);
            RequestMessage.Headers.Authorization = value;
        }

        public HttpRequestBuilder AddHeader(string name, string value)
        {
            RequestMessage.Headers.Add(name, value);
            return this;
        }

        public HttpRequestBuilder AddAcceptEncoding(StringWithQualityHeaderValue value)
        {
            RequestMessage.Headers.AcceptEncoding.Add(value);
            return this;
        }

        public HttpRequestBuilder AddAccept(MediaTypeWithQualityHeaderValue value)
        {
            RequestMessage.Headers.Accept.Add(value);
            return this;
        }

        public HttpRequestBuilder AddContent(HttpContent content)
        {
            RequestMessage.Content = content;
            return this;
        }

        public HttpRequestBuilder AddJsonContent<T>(T content)
        {
            return AddContent(new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json"));
        }
    }
}
