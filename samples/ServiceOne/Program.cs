using System;
using System.Threading.Tasks;
using ServiceOrchestrator.Core;

namespace ServiceOne
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = new ServiceHostBuilder();

            await hostBuilder.StartAsync();

            Console.WriteLine("Client started...");
            Console.ReadLine();

            await hostBuilder.DisposeAsync();
        }
    }
}
