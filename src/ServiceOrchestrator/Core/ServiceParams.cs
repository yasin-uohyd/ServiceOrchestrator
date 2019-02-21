using System;

namespace ServiceOrchestrator.Core
{
    public class ServiceParams
    {
        public int Count { get; set; }

        public string Message { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
