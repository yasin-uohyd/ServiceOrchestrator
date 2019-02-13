using System;
using System.Collections.Generic;
using ServiceOrchestrator.Events;

namespace AccountService.Events
{
    public class AccountServiceEventOne : IServiceEvent
    {
        public AccountServiceEventOne()
        {
            Name = "PaymentServiceMethodOne";
        }

        public Guid CorrelationId { get; set; }

        public string Name { get; set; }

        public IList<object> Params { get; set; }
    }
}
