using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ServiceOrchestrator.Core;
using ServiceTwo.Tasks;

namespace ServiceTwo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hostBuilder = new ServiceHostBuilder();

            hostBuilder.Build((hostBuilderConext, serviceCollection) =>
            {
                serviceCollection.AddScoped<ITask, ServiceTwoTaskOne>();
            });

            await hostBuilder.StartAsyncAndWait();
        }
    }
}
