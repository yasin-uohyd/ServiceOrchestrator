using System;
using System.Collections.Generic;

namespace ServiceOrchestrator.Events
{
    public class EndServiceEvent : IServiceEvent
    {
        public EndServiceEvent()
        {
            Params = new List<object>();
        }

        public Guid CorrelationId { get; set; }
        public string Name { get; set; }
        public IList<object> Params { get; set; }
    }
}
