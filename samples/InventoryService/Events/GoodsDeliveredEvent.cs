using System;
using System.Collections.Generic;
using ServiceOrchestrator.Events;

namespace InventoryService.Events
{
    public class GoodsDeliveredEvent : IServiceEvent
    {
        public GoodsDeliveredEvent()
        {
            Name = "GoodsDeliveredEvent";
        }

        public Guid CorrelationId { get; set; }

        public string Name { get; set; }

        public IList<object> Params { get; set; }
    }
}
