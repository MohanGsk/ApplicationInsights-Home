using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace ApplicationInsightsDataROI
{
    class PriceCalculatorTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor _next;
        private ProcessedItems _state;

        public PriceCalculatorTelemetryProcessor(ITelemetryProcessor next, ProcessedItems state)
        {
            this._next = next;
            this._state = state;
        }

        public void Process(ITelemetry item)
        {
            try
            {
                item.Sanitize();

                byte[] content = JsonSerializer.Serialize(new List<ITelemetry>() { item }, false);
                int size = content.Length;
                string json = Encoding.Default.GetString(content);

                this._state.size += size;
                this._state.count += 1;
            }
            finally
            {
                this._next.Process(item);
            }
        }
    }
}
