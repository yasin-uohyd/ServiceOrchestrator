using System;
using ServiceOrchestrator.Core;
using InventoryService.Events;
using System.Threading.Tasks;

namespace InventoryService.Tasks
{
    public class InventoryServiceTask : ITask
    {
        private readonly IServiceCoordinator serviceCoordinator;

        public InventoryServiceTask(IServiceCoordinator serviceCoordinator)
        {
            this.serviceCoordinator = serviceCoordinator;
        }

        public async Task DeliverGoods(ServiceParams taskParams)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss.fff tt")}] {nameof(InventoryService)}: {nameof(DeliverGoods)}: Delivery in progress...");
            await Task.Delay(10000);
            taskParams.Count++;
            await serviceCoordinator.Raise<GoodsDeliveredEvent>(taskParams);
            Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss.fff tt")}] {nameof(InventoryService)}: {nameof(DeliverGoods)}: Delivery completed...");
        }
    }
}