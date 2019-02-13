using System;
using ServiceOrchestrator.Core;
using AccountService.Events;

namespace AccountService.Tasks
{
    public class AccountServiceTaskOne : ITask
    {
        private readonly IServiceCoordinator serviceCoordinator;

        public AccountServiceTaskOne(IServiceCoordinator serviceCoordinator)
        {
            this.serviceCoordinator = serviceCoordinator;
        }

        public void AccountServiceMethodOne(ServiceParams taskParams)
        {
            Console.WriteLine($"[{DateTime.Now.ToString()}] {nameof(AccountService)}: {nameof(AccountServiceMethodOne)}: Received message from Mars: {taskParams.Message}, count {taskParams.Count}");

            taskParams.Count++;

            serviceCoordinator.Raise<AccountServiceEventOne>(taskParams).Wait();
        }
    }
}
