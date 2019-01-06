using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceOrchestrator.Core;

namespace ServiceOne
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = new ServiceHostBuilder();

            hostBuilder.Build((hostBuilderConext, serviceCollection) =>
            {
                serviceCollection.AddScoped<ITask, ServiceOneTaskOne>();
            });


            await hostBuilder.StartAsync();

            //Just to Start the Flow
            var serviceCoordinator = hostBuilder.host.Services.GetRequiredService<IServiceCoordinator>();
            await serviceCoordinator.Raise<CustomServiceOneEvent>(null);

            await hostBuilder.WaitForShutdownAsync();
        }
    }
}
