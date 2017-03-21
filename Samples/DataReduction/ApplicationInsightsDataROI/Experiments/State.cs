using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ApplicationInsightsDataROI
{
    class _State
    {
        public System.Timers.Timer _timer;

        public bool IsTerminated { get; set; }

        public ProcessedItems Collected = new ProcessedItems();

        public ProcessedItems Sent = new ProcessedItems();

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (this.IsTerminated)
            {
                this._timer.Stop();
                return;
            }

            const int oneGb = 1000 * 1000 * 1000;
            double gbs = 1.0 * this.Collected.size / oneGb;

            if (gbs < 0.1)
            {
                Console.WriteLine($"Collected \tsize: {this.Collected.size} bytes \tin {this.Collected.count} items");
            }
            else
            {
                Console.WriteLine($"Collected \tsize: {gbs} Gb \tin {this.Collected.count} items");
            }

            gbs = 1.0 * this.Sent.size / oneGb;
            if (gbs < 0.1)
            {
                Console.WriteLine($"Sent    \tsize: {this.Sent.size} bytes \tin {this.Sent.count} items");
            }
            else
            {
                Console.WriteLine($"Sent    \tsize: {gbs} Gb \tin {this.Sent.count} items");
            }

            if (this.Sent.size == 0)
            {
                Console.WriteLine("Ratio: N/A");
            }
            else
            {
                Console.WriteLine($"Ratio: {this.Collected.size / this.Sent.size}");
            }
            Console.WriteLine();

            if (this.Sent.size > 100 * 1000)
            {
                this.IsTerminated = true;
            }
        }

        internal void Initialize()
        {
            this.IsTerminated = false;

            // Create a timer with a two second interval.
            this._timer = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            // Hook up the Elapsed event for the timer. 
            this._timer.Elapsed += this.OnTimedEvent;
            this._timer.AutoReset = true;
            this._timer.Enabled = true;
        }
    }
}
