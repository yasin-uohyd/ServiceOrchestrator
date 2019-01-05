using System;
using ServiceOrchestrator.Core;

namespace ServiceOne
{
    public class ServiceOneTaskOne : ITask
    {
        private readonly IServiceCoordinator serviceCoordinator;

        public ServiceOneTaskOne(IServiceCoordinator serviceCoordinator)
        {
            this.serviceCoordinator = serviceCoordinator;
        }

        public void ServiceOneMethodOne(ServiceParams taskParams)
        {
            Console.WriteLine($"[{DateTime.Now.ToString()}] MethodOne: Received message from Mars: {taskParams.Message}");

            (new ServiceCoordinator().Raise<CustomServiceOneEvent>(null)).Wait();
        }
    }
}
