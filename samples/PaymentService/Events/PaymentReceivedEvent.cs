using System;
using System.Collections.Generic;
using ServiceOrchestrator.Events;

namespace PaymentService.Events
{
    class PaymentReceivedEvent : IServiceEvent
    {
        public PaymentReceivedEvent()
        {
            Name = "PaymentReceivedEvent";
        }

        public Guid CorrelationId { get; set; }

        public string Name { get; set; }

        public IList<object> Params { get; set; }
    }
}
