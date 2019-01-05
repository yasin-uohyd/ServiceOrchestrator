using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;

namespace ServiceOrchestrator.Core
{
    public class ServiceHostBuilder
    {
        private readonly HubConnection _connection;

        private const string HubName = "Communicator";

        public ServiceHostBuilder()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("service.json")
                .Build();

            var serviceUtils = new Security(configuration["Azure:SignalR:ConnectionString"]);

            var url = GetClientUrl(serviceUtils.Endpoint, HubName);

            _connection = new HubConnectionBuilder()
                .WithUrl(url, option =>
                {
                    option.AccessTokenProvider = () =>
                    {
                        return Task.FromResult(serviceUtils.GenerateAccessToken(url, Guid.NewGuid().ToString()));
                    };
                }).Build();

            _connection.On("SendMessage",
                (string server, string message) =>
                {
                    Console.WriteLine($"[{DateTime.Now.ToString()}] Received message from server {server}: {message}");
                });
        }

        public async Task StartAsync()
        {
            await _connection.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _connection.DisposeAsync();
        }

        private string GetClientUrl(string endpoint, string hubName)
        {
            return $"{endpoint}/client/?hub={hubName}";
        }
    }
}
