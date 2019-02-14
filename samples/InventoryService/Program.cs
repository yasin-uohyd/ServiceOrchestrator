using System.Threading.Tasks;
using InventoryService.Events;
using InventoryService.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ServiceOrchestrator.Core;

namespace InventoryService
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = new ServiceHostBuilder();

            hostBuilder.Build((hostBuilderConext, serviceCollection) =>
            {
                serviceCollection.AddScoped<ITask, InventoryServiceTask>();
            });

            await hostBuilder.StartAsync();
        }
    }
}
