using System.Collections.Generic;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace NetCoreHeartbeatConsole
{

    class SenderChannel : ITelemetryChannel
    {
        private bool devMode = true;
        private Dictionary<string,List<ITelemetry>> sentItems = new Dictionary<string,List<ITelemetry>>();

        public bool? DeveloperMode { get => devMode; set => devMode = value.GetValueOrDefault(devMode); }

        public string EndpointAddress { get; set; }

        public SenderChannel()
        {
        }

        public void Dispose()
        {
            sentItems = null;
        }

        public void Flush()
        {
        }

        public void Send(ITelemetry item)
        {
            string key = "UnknownKey";
            if (item is AvailabilityTelemetry)
            {
                key = "AvailabilityTelemetry";
            }
            else if (item is DependencyTelemetry)
            {
                key = "DependencyTelemetry";
            }
            else if (item is EventTelemetry)
            {
                key = "EventTelemetry";
            }
            else if (item is ExceptionTelemetry)
            {
                key = "ExceptionTelemetry";
            }
            else if (item is MetricTelemetry)
            {
                key = "MetricTelemetry";
            }
            else if (item is PageViewTelemetry)
            {
                key = "PageViewTelemetry";
            }
            if (item is RequestTelemetry)
            {
                key = "RequestTelemetry";
            }
            else if (item is TraceTelemetry)
            {
                key = "TraceTelemetry";
            }

            if (!sentItems.ContainsKey(key))
            {
                sentItems.Add(key, new List<ITelemetry>());
            }
            sentItems[key].Add(item);
        }

        public IEnumerable<ITelemetry> GetTelemetryTypeSent(string telemetryTypeName, bool clearAllItems = true)
        {
            List<ITelemetry> sentOfType = null;

            if (sentItems.ContainsKey(telemetryTypeName))
            {
                sentOfType = new List<ITelemetry>();
                sentOfType.AddRange(sentItems[telemetryTypeName]);
            }

            if (clearAllItems)
            {
                sentItems.Clear();
            }

            return sentOfType;
        }

        public IEnumerable<ITelemetry> GetAllTelemetrySent(bool clearItems = true)
        {
            List<ITelemetry> list = new List<ITelemetry>();
            foreach (var kvp in sentItems)
            {
                list.AddRange(kvp.Value);
            }

            if (clearItems)
            {
                sentItems.Clear();
            }

            return list;
        }

    }
}
