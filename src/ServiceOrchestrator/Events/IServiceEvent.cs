using System;
using System.Collections.Generic;

namespace ServiceOrchestrator.Events
{
    public interface IServiceEvent
    {
        Guid CorrelationId { get; set; }

        string Name { get; set; }

        IList<object> Params { get; set; }
    }
}
