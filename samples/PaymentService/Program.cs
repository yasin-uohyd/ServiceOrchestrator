using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ServiceOrchestrator.Core;
using PaymentService.Tasks;

namespace PaymentService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hostBuilder = new ServiceHostBuilder();

            hostBuilder.Build((hostBuilderConext, serviceCollection) =>
            {
                serviceCollection.AddScoped<ITask, PaymentServiceTask>();
            });

            await hostBuilder.StartAsyncAndWait();
        }
    }
}
