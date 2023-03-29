using Newtonsoft.Json.Linq;

namespace NBS.Appointments.Service.Api.Tests.MockApi
{
    public class CallDetails
    {
        private readonly string _path;
        private readonly JObject? _body;
        private readonly Dictionary<string, string> _headers;

        public CallDetails(string path, JObject? body, Dictionary<string, string> headers)
        {
            _path = path;
            _body = body;
            _headers = headers;
        }

        public string Path => _path;

        public JObject? Body => _body;

        public IDictionary<string, string> Headers => _headers;
    }
}
