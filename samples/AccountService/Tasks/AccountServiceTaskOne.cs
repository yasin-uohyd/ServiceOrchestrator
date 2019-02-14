using System;
using ServiceOrchestrator.Core;
using AccountService.Events;

namespace AccountService.Tasks
{
    public class AccountServiceTask : ITask
    {
        private readonly IServiceCoordinator serviceCoordinator;

        public AccountServiceTask(IServiceCoordinator serviceCoordinator)
        {
            this.serviceCoordinator = serviceCoordinator;
        }

        public void UpdateCart(ServiceParams taskParams)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss.fff tt")}] {nameof(AccountService)}: {nameof(UpdateCart)}: Items are being added to Cart...");
            taskParams.Count++;
            serviceCoordinator.Raise<CartUpdatedEvent>(taskParams).Wait();
            Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss.fff tt")}] {nameof(AccountService)}: {nameof(UpdateCart)}: Items added to cart...");
        }
    }
}
