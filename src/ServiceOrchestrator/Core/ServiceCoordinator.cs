using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceOrchestrator.Events;

namespace ServiceOrchestrator.Core
{
    public class ServiceCoordinator : IServiceCoordinator
    {
        private readonly HttpClient httpClient;

        private readonly Security security;
        private readonly string serverName;
        private readonly string hubName = "Communicator";

        public ServiceCoordinator(IConfiguration configuration)
        {
            security = new Security(configuration["Azure:SignalR:ConnectionString"]);
            serverName = GenerateServerName();
            httpClient = new HttpClient();
        }

        public async Task Raise<T>(object[] data) where T : IServiceEvent, new()
        {
            var defaultPayloadMessage = new PayloadMessage
            {
                Target = "EXEC",
                Arguments = new object[]
                {
                    new T().Name,
                    serverName ,
                    new ServiceParams {Message="Hello from Mars"}
                }
            };

            var response = await httpClient.SendAsync(BuildRequest(defaultPayloadMessage));

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                Console.WriteLine($"Sent error: {response.StatusCode}");
            }
        }

        private HttpRequestMessage BuildRequest(PayloadMessage defaultPayloadMessage)
        {
            var url = $"{security.Endpoint}/api/v1/hubs/{hubName.ToLower()}";
            var request = new HttpRequestMessage(HttpMethod.Post, GetUrl(url));

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", security.GenerateAccessToken(url, serverName));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(JsonConvert.SerializeObject(defaultPayloadMessage), Encoding.UTF8, "application/json");

            return request;
        }

        private string GenerateServerName()
        {
            return $"{Environment.MachineName}_{Guid.NewGuid():N}";
        }

        private Uri GetUrl(string baseUrl)
        {
            return new UriBuilder(baseUrl).Uri;
        }
    }
}
