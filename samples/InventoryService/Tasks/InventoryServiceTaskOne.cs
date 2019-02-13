using System;
using ServiceOrchestrator.Core;
using InventoryService.Events;

namespace InventoryService.Tasks
{
    public class InventoryServiceTaskOne : ITask
    {
        private readonly IServiceCoordinator serviceCoordinator;

        public InventoryServiceTaskOne(IServiceCoordinator serviceCoordinator)
        {
            this.serviceCoordinator = serviceCoordinator;
        }

        public void InventoryServiceMethodOne(ServiceParams taskParams)
        {
            Console.WriteLine($"[{DateTime.Now.ToString()}] {nameof(InventoryService)}: {nameof(InventoryServiceMethodOne)}: Received message from Mars: {taskParams.Message}, count {taskParams.Count}");

            taskParams.Count++;

            serviceCoordinator.Raise<InventoryServiceEventOne>(taskParams).Wait();
        }
    }
}
