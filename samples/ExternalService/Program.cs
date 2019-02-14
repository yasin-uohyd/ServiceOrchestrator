using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ServiceOrchestrator.Core;
using ExternalService.Events;
using System;

namespace ExternalService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hostBuilder = new ServiceHostBuilder();

            hostBuilder.Build((hostBuilderConext, serviceCollection) =>
            {
            });

            await hostBuilder.StartAsync();

            //Just to Start the Flow
            var serviceCoordinator = hostBuilder.host.Services.GetRequiredService<IServiceCoordinator>();

            Console.WriteLine("External Service started...");
            for (int i = 0; i< 10; i++)
            {
                await serviceCoordinator.Raise<OrderCreatedEvent>(new ServiceParams { Message = "Startup", Count = 0 });
            }

            await hostBuilder.StartAsyncAndWait();
        }
    }
}
