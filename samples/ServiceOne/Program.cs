using System.Threading.Tasks;
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
            await new ServiceCoordinator().Raise<CustomServiceOneEvent>(null);

            await hostBuilder.WaitForShutdownAsync();
        }
    }
}
