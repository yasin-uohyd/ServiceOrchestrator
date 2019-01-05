using System;
using System.IO;
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
        private readonly Security security;
        private readonly string serverName;
        private readonly string hubName = "Communicator";
        private readonly PayloadMessage _defaultPayloadMessage;

        public ServiceCoordinator()
        {
            //var configuration = new ConfigurationBuilder()
            //        .SetBasePath(Directory.GetCurrentDirectory())
            //        .AddJsonFile("service.json", false, true)
            //        .Build();

            //security = new Security(configuration["Azure:SignalR:ConnectionString"]);
            //serverName = GenerateServerName();
        }

        public async Task Raise<T>(object[] data) where T : IServiceEvent, new()
        {
            await new TaskHandler("Endpoint=https://winterready.service.signalr.net;AccessKey=UE7ehbJLmslL/KBL+zUokZ+uJ5U8guNiR+UilZtE1go=;Version=1.0;").Send(new T().Name);
            //using (var httpClient = new HttpClient())
            //{
            //    var defaultPayloadMessage = new PayloadMessage
            //    {
            //        Target = "EXEC",
            //        Arguments = new object[]
            //        {
            //        new T().Name,
            //        serverName ,
            //        new ServiceParams {Message="Hello from Mars"}
            //        }
            //    };

            //    var response = await httpClient.SendAsync(BuildRequest(defaultPayloadMessage));
            //    response.EnsureSuccessStatusCode();
            //}
        }

        private HttpRequestMessage BuildRequest(PayloadMessage defaultPayloadMessage)
        {
            var url = $"{security.Endpoint}/api/v1/hubs/{hubName.ToLower()}";
            var request = new HttpRequestMessage(HttpMethod.Post, GetUrl(url));

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", security.GenerateAccessToken(url, serverName));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(JsonConvert.SerializeObject(_defaultPayloadMessage), Encoding.UTF8, "application/json");

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
