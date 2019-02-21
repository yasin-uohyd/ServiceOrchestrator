using System;
using ServiceOrchestrator.Core;
using PaymentService.Events;
using System.Threading.Tasks;

namespace PaymentService.Tasks
{
    public class PaymentServiceTask : ITask
    {
        private readonly IServiceCoordinator serviceCoordinator;

        public PaymentServiceTask(IServiceCoordinator serviceCoordinator)
        {
            this.serviceCoordinator = serviceCoordinator;
        }

        public async Task MakePayment(ServiceParams taskParams)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss.fff tt")}] {nameof(PaymentService)}: {nameof(MakePayment)} payment in progress...");
            await Task.Delay(10000);
            taskParams.Count++;
            await serviceCoordinator.Raise<PaymentReceivedEvent>(taskParams);
            Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss.fff tt")}] {nameof(PaymentService)}: {nameof(MakePayment)} payment processed...");
        }
    }
}
