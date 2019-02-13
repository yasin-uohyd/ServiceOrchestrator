using System.Threading.Tasks;
using AccountService.Events;
using AccountService.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ServiceOrchestrator.Core;

namespace AccountService
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = new ServiceHostBuilder();

            hostBuilder.Build((hostBuilderConext, serviceCollection) =>
            {
                serviceCollection.AddScoped<ITask, AccountServiceTaskOne>();
            });


            await hostBuilder.StartAsync();

            //Just to Start the Flow
            var serviceCoordinator = hostBuilder.host.Services.GetRequiredService<IServiceCoordinator>();
            await serviceCoordinator.Raise<AccountServiceEventOne>(new ServiceParams { Message = "Startup", Count = 0 });

            await hostBuilder.WaitForShutdownAsync();
        }
    }
}
