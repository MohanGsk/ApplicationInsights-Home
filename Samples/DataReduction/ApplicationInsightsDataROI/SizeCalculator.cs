using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace ApplicationInsightsDataROI
{
    class SizeCalculatorTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor _next;
        private ProcessedItems ProcessedItems;


        public SizeCalculatorTelemetryProcessor(ITelemetryProcessor next, ProcessedItems items)
        {
            this._next = next;
            this.ProcessedItems = items;
        }

        public void Process(ITelemetry item)
        {
            try
            {
                item.Sanitize();

                byte[] content = JsonSerializer.Serialize(new List<ITelemetry>() { item }, false);
                int size = content.Length;
                string json = Encoding.Default.GetString(content);

                this.ProcessedItems.size += size;
                this.ProcessedItems.count += 1;
            }
            finally
            {
                this._next.Process(item);
            }
        }
    }
}
