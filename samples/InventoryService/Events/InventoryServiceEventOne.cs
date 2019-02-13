using System;
using System.Collections.Generic;
using ServiceOrchestrator.Events;

namespace InventoryService.Events
{
    public class InventoryServiceEventOne : IServiceEvent
    {
        public InventoryServiceEventOne()
        {
            Name = "PaymentServiceMethodTwo";
        }

        public Guid CorrelationId { get; set; }

        public string Name { get; set; }

        public IList<object> Params { get; set; }
    }
}
