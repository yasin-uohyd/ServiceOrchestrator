using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceOrchestrator.Core;

namespace ServiceOrchestrator
{
    internal class OrchestrationService : IHostedService
    {
        private HubConnection _connection;
        private const string HubName = "Communicator";
        private readonly ILogger _logger;

        public OrchestrationService(IServiceProvider services,
            ILogger<OrchestrationService> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service is starting.");

            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("service.json", false, true)
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

            RegisterTask();

            await _connection.StartAsync();
        }

        private void RegisterTask()
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service is working.");

            _connection.On("EXEC",
                (string eventName, string clientId, TaskParams message) =>
                {
                    using (var scope = Services.CreateScope())
                    {
                        var tasks = scope.ServiceProvider.GetServices<ITask>();
                        foreach (var task in tasks)
                        {
                            var methodInfo = task.GetType().GetMethod(eventName);
                            methodInfo.Invoke(task, new[] { message });
                        }
                    }
                });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            _connection.StopAsync(cancellationToken);

            return Task.CompletedTask;
        }

        private string GetClientUrl(string endpoint, string hubName)
        {
            return $"{endpoint}/client/?hub={hubName}";
        }
    }
}