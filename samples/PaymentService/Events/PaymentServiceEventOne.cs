using System;
using System.Collections.Generic;
using ServiceOrchestrator.Events;

namespace PaymentService.Events
{
    class PaymentServiceEventOne : IServiceEvent
    {
        public PaymentServiceEventOne()
        {
            Name = "InventoryServiceMethodOne";
        }

        public Guid CorrelationId { get; set; }

        public string Name { get; set; }

        public IList<object> Params { get; set; }
    }
}
