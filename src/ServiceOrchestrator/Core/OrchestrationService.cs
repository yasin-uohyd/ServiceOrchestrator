using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceOrchestrator.Core;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

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
                        var handlers = GetHandlers(eventName);

                        var tasks = scope.ServiceProvider.GetServices<ITask>();

                        foreach (var handler in handlers)
                        {
                            var task = tasks.First(t => t.GetType().Name == handler.TaskName);
                            var methodInfo = task.GetType().GetMethod(handler.MethodName);
                            if (methodInfo != null)
                            {
                                methodInfo.Invoke(task, new[] { message });
                            }
                        }
                    }
                });
        }

        private IList<Handler> GetHandlers(string eventName)
        {
            var childern = configuration.GetSection("EventHandlers").GetChildren();
            var handlers = new List<Handler>();

            foreach (var child in childern)
            {
                if (eventName == child.GetValue<string>("EventName"))
                {
                    handlers.Add(child.GetSection("Handler").Get<Handler>());
                }

            }

            return handlers;
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