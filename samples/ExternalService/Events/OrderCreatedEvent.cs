using System;
using System.Collections.Generic;
using ServiceOrchestrator.Events;

namespace ExternalService.Events
{
    class OrderCreatedEvent : IServiceEvent
    {
        public OrderCreatedEvent()
        {
            Name = "OrderCreatedEvent";
        }

        public Guid CorrelationId { get; set; }

        public string Name { get; set; }

        public IList<object> Params { get; set; }
    }
}
