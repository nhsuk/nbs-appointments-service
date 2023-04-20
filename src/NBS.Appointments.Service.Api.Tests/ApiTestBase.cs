namespace NBS.Appointments.Service.Api.Tests
{
    public abstract class ApiTestBase
    {
        private readonly Lazy<HttpClient> _httpClient;
        protected const string BaseUrl = "http://localhost:4000";
        public abstract string PathToTest { get; }

        public string Endpoint => $"{BaseUrl}/{PathToTest}";

        protected ApiTestBase() 
        { 
            _httpClient = new Lazy<HttpClient>(() => CreateClient());
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("nbs-api-key", "supersecret");
            return client;
        }

        protected HttpClient HttpClient => _httpClient.Value;
    }
}
