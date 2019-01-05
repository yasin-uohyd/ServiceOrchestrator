using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceOrchestrator.Core
{
    public class ServiceHostBuilder
    {
        private IHost host;
        private readonly IHostBuilder hostBuilder;

        private const string HubName = "Communicator";

        public ServiceHostBuilder()
        {
            hostBuilder = new HostBuilder()
                            .ConfigureServices(
                                (hostContext, services) =>
                                {
                                    services.AddHostedService<OrchestrationService>();
                                    services.AddScoped<IServiceCoordinator, ServiceCoordinator>();
                                })
                            .UseConsoleLifetime();
        }

        public IHost Build(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            hostBuilder.ConfigureServices(configureDelegate);
            host = hostBuilder.Build();
            return host;
        }

        public async Task StartAsyncAndWait()
        {
            await host.StartAsync();
            await host.WaitForShutdownAsync();
        }

        public async Task StartAsync()
        {
            await host.StartAsync();
        }

        public async Task WaitForShutdownAsync()
        {
            await host.WaitForShutdownAsync();
        }
    }
}
