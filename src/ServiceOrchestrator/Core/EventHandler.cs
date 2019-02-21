using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ServiceOrchestrator
{
    public class EventHandler
    {
        public EventHandler()
        {
            this.EventName = "";
            this.Handlers = new List<Handler>();
        }

        public string EventName { get; set; }

        public List<Handler> Handlers { get; set; }
    }

    public class Handler
    {
        public Handler()
        {

        }

        public string TaskName { get; set; }

        public string MethodName { get; set; }

        public Dictionary<string, string> Params { get; set; }
    }
}
