using System;
using ServiceOrchestrator.Core;
using PaymentService.Events;

namespace PaymentService.Tasks
{
    public class PaymentServiceTaskOne : ITask
    {
        private readonly IServiceCoordinator serviceCoordinator;

        public PaymentServiceTaskOne(IServiceCoordinator serviceCoordinator)
        {
            this.serviceCoordinator = serviceCoordinator;
        }

        public void PaymentServiceMethodOne(ServiceParams taskParams)
        {
            Console.WriteLine($"[{DateTime.Now.ToString()}] {nameof(PaymentService)}: {nameof(PaymentServiceMethodOne)}: Received message from Mars: {taskParams.Message}, count {taskParams.Count}");

            taskParams.Count++;

            serviceCoordinator.Raise<PaymentServiceEventOne>(taskParams).Wait();
        }
    }
}
