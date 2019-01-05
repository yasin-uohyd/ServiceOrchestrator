using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ServiceOrchestrator.Core;

namespace ServiceOrchestrator
{
    public class TaskHandler
    {
        private static readonly HttpClient Client = new HttpClient();

        private readonly string _serverName;

        private readonly Security _serviceUtils;

        private readonly string _hubName;

        private readonly string _endpoint;

        private PayloadMessage _defaultPayloadMessage;

        public TaskHandler(string connectionString)
        {
            _serverName = GenerateServerName();
            _serviceUtils = new Security(connectionString);
            _hubName = "Communicator";
            _endpoint = _serviceUtils.Endpoint;


        }

        public async Task Send(string name)
        {
            _defaultPayloadMessage = new PayloadMessage
            {
                Target = "EXEC",
                Arguments = new object[]
                {
                    name,
                    _serverName ,
                    new ServiceParams {Message="Hello from Mars"}
                }
            };

            await SendRequest("broadcast", _hubName);
        }

        public async Task SendRequest(string command, string hubName, string arg = null)
        {
            string url = null;
            switch (command)
            {
                case "user":
                    url = GetSendToUserUrl(hubName, arg);
                    break;
                case "users":
                    url = GetSendToUsersUrl(hubName, arg);
                    break;
                case "group":
                    url = GetSendToGroupUrl(hubName, arg);
                    break;
                case "groups":
                    url = GetSendToGroupsUrl(hubName, arg);
                    break;
                case "broadcast":
                    url = GetBroadcastUrl(hubName);
                    break;
                default:
                    Console.WriteLine($"Can't recognize command {command}");
                    break;
            }

            if (!string.IsNullOrEmpty(url))
            {
                var request = BuildRequest(url);

                var response = await Client.SendAsync(request);
                if (response.StatusCode != HttpStatusCode.Accepted)
                {
                    Console.WriteLine($"Sent error: {response.StatusCode}");
                }
            }
        }

        private Uri GetUrl(string baseUrl)
        {
            return new UriBuilder(baseUrl).Uri;
        }

        private string GetSendToUserUrl(string hubName, string userId)
        {
            return $"{GetBaseUrl(hubName)}/user/{userId}";
        }

        private string GetSendToUsersUrl(string hubName, string userList)
        {
            return $"{GetBaseUrl(hubName)}/users/{userList}";
        }

        private string GetSendToGroupUrl(string hubName, string group)
        {
            return $"{GetBaseUrl(hubName)}/group/{group}";
        }

        private string GetSendToGroupsUrl(string hubName, string groupList)
        {
            return $"{GetBaseUrl(hubName)}/groups/{groupList}";
        }

        private string GetBroadcastUrl(string hubName)
        {
            return $"{GetBaseUrl(hubName)}";
        }

        private string GetBaseUrl(string hubName)
        {
            return $"{_endpoint}/api/v1/hubs/{hubName.ToLower()}";
        }

        private string GenerateServerName()
        {
            return $"{Environment.MachineName}_{Guid.NewGuid():N}";
        }

        private HttpRequestMessage BuildRequest(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, GetUrl(url));

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _serviceUtils.GenerateAccessToken(url, _serverName));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(JsonConvert.SerializeObject(_defaultPayloadMessage), Encoding.UTF8, "application/json");

            return request;
        }
    }

    public class PayloadMessage
    {
        public string Target { get; set; }

        public object[] Arguments { get; set; }
    }
}
