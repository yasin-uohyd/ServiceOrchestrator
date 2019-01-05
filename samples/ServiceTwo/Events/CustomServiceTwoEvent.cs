using System;
using System.Collections.Generic;
using ServiceOrchestrator.Events;

namespace ServiceTwo.Events
{
    class CustomServiceTwoEvent : IServiceEvent
    {
        public CustomServiceTwoEvent()
        {
            Name = "ServiceOneMethodOne";
        }

        public Guid CorrelationId { get; set; }

        public string Name { get; set; }

        public IList<object> Params { get; set; }
    }
}
