using System;
using System.Collections.Generic;

namespace ServiceOrchestrator.Events
{
    public class BeginServiceEvent : IServiceEvent
    {
        public BeginServiceEvent()
        {
            Params = new List<object>();
        }

        public Guid CorrelationId { get; set; }

        public string Name { get; set; }

        public IList<object> Params { get; set; }
    }
}
