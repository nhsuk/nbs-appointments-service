using Newtonsoft.Json.Linq;
using System.Text;

namespace NBS.Appointments.Service.Api.Tests.MockApi
{
    public class Client
    {
        private readonly string _mockApiHost;

        public Client(string mockApiHost)
        {
            _mockApiHost = mockApiHost;
        }

        public async Task SetupFromFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                throw new System.IO.FileNotFoundException(filePath);
            }

            await SetupFromJson(File.ReadAllText(filePath));
        }

        public async Task SetupFromJson(string routesDocument)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("MockApi-Action", "BulkSetup");
                var response = await client.PostAsync(_mockApiHost, new StringContent(routesDocument, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
        }

        public MockApiAction Setup(string method, string path)
        {
            return new MockApiAction(_mockApiHost, method, path, false);
        }

        public MockApiAction SetupOnce(string method, string path)
        {
            return new MockApiAction(_mockApiHost, method, path, true);
        }

        public async Task<IEnumerable<CallDetails>> Calls(string method, string path)
        {
            var uri = $"{_mockApiHost}/{path}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("MockApi-Action", "Validate");
                client.DefaultRequestHeaders.Add("MockApi-Method", method);

                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                var bodyAsString = await response.Content.ReadAsStringAsync();
                var bodyAsJson = JObject.Parse(bodyAsString);

                var requests = bodyAsJson.SelectToken("requests") as JArray;
                if (requests == null)
                    throw new InvalidOperationException("Could not find requests item in response");

                var results = new List<CallDetails>();

                foreach (var request in requests.Children())
                {
                    var requestBody = request["body"];
                    var requestPath = request["path"];
                    var headers = request["headers"];

                    if (request == null || requestBody == null || requestPath == null || headers == null)
                        throw new InvalidOperationException("Invalid request object found");

                    JObject? body = null;
                    var temp = requestBody.ToString();
                    if (!string.IsNullOrEmpty(temp))
                    {
                        body = JObject.Parse(temp);
                    }
                    results.Add(new CallDetails(requestPath.ToString(), body, headers.ToObject<Dictionary<string, string>>()));
                }

                return results;
            }
        }
    }
}
