using System;
using System.Collections.Generic;
using ServiceOrchestrator.Events;

namespace ServiceOne
{
    public class CustomServiceOneEvent : IServiceEvent
    {
        public CustomServiceOneEvent()
        {
            Name = "ServiceTwoMethodOne";
        }

        public Guid CorrelationId { get; set; }

        public string Name { get; set; }

        public IList<object> Params { get; set; }
    }
}
