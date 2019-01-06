using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceOrchestrator.Core
{
    public class ServiceHostBuilder
    {
        public IHost host;
        private readonly IHostBuilder hostBuilder;

        private const string HubName = "Communicator";

        public ServiceHostBuilder()
        {
            hostBuilder = new HostBuilder()
                            .ConfigureServices(
                                (hostContext, services) =>
                                {
                                    services.AddHostedService<OrchestrationService>();
                                    services.AddSingleton<IServiceCoordinator, ServiceCoordinator>();
                                })
                                .ConfigureAppConfiguration(configure =>
                                {
                                    configure.SetBasePath(Directory.GetCurrentDirectory());
                                    configure.AddJsonFile("service.json", false, true);
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
