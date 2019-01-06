using System;
using ServiceOrchestrator.Core;
using ServiceTwo.Events;

namespace ServiceTwo.Tasks
{
    public class ServiceTwoTaskOne : ITask
    {
        private readonly IServiceCoordinator serviceCoordinator;

        public ServiceTwoTaskOne(IServiceCoordinator serviceCoordinator)
        {
            this.serviceCoordinator = serviceCoordinator;
        }

        public void ServiceTwoMethodOne(ServiceParams taskParams)
        {
            Console.WriteLine($"[{DateTime.Now.ToString()}] Service One: MethodOne: Received message from Mars: {taskParams.Message}, count {taskParams.Count}");

            taskParams.Count++;

            serviceCoordinator.Raise<CustomServiceTwoEvent>(taskParams).Wait();
        }
    }
}
