using System;
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
        private readonly IConfiguration configuration;
        private readonly ILogger logger;

        public OrchestrationService(IServiceProvider services, IConfiguration configuration, ILogger<OrchestrationService> logger)
        {
            Services = services;
            this.logger = logger;
            this.configuration = configuration;
        }

        public IServiceProvider Services { get; }

        public ILogger Logger => logger;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Orchestration Service is starting.");

            var serviceUtils = new Security(configuration["Azure:SignalR:ConnectionString"]);

            var url = GetHubUrl(serviceUtils.Endpoint, HubName);

            _connection = new HubConnectionBuilder()
                .WithUrl(url, option =>
                {
                    option.AccessTokenProvider = () =>
                    {
                        return Task.FromResult(serviceUtils.GenerateAccessToken(url, Guid.NewGuid().ToString()));
                    };
                }).Build();

            RegisterTasks();

            await _connection.StartAsync();
        }

        private void RegisterTasks()
        {
            logger.LogInformation("Orchestration Service is working.");

            _connection.On("EXEC",
                (string eventName, string clientId, ServiceParams message) =>
                {
                    using (var scope = Services.CreateScope())
                    {
                        var tasks = scope.ServiceProvider.GetServices<ITask>();
                        foreach (var task in tasks)
                        {
                            var test = scope.ServiceProvider.GetRequiredService<IServiceCoordinator>();
                            var methodInfo = task.GetType().GetMethod(eventName);
                            if (methodInfo != null)
                            {
                                methodInfo.Invoke(task, new[] { message });
                            }
                        }
                    }
                });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Orchestration Service is stopping.");

            await _connection.StopAsync(cancellationToken);
        }

        private string GetHubUrl(string endpoint, string hubName)
        {
            return $"{endpoint}/client/?hub={hubName}";
        }
    }
}