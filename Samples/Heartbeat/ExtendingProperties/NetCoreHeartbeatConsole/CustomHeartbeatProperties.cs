using System;
using System.Text;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

namespace NetCoreHeartbeatConsole 
{

    public class CustomHeartbeatProperties
    {
        private IHeartbeatPropertyManager hbeatPropertyManager = null;

        public CustomHeartbeatProperties()
        {
        }

        // Gather system data that we want to add to the heartbeat
        public bool Initialize(TimeSpan hbeatInterval)
        {
            foreach (var md in TelemetryModules.Instance.Modules)
            {
                if (md is IHeartbeatPropertyManager heartbeatPropertyMan)
                {
                    this.hbeatPropertyManager = heartbeatPropertyMan;
                    this.hbeatPropertyManager.IsHeartbeatEnabled = false;
                    this.hbeatPropertyManager.HeartbeatInterval = hbeatInterval;
                    this.hbeatPropertyManager.IsHeartbeatEnabled = true;
                }
            }

            if (this.hbeatPropertyManager == null)
            {
                System.Console.WriteLine("Could not find the Heartbeat Property Manager. Heartbeat properties will not be added.");
                return false;
            }

            return true;
        }

        // Add heartbeat data to the heartbeat module
        public void AddCustomHeartbeatProperty()
        {
            this.hbeatPropertyManager.AddHeartbeatProperty("A_Custom_Property", "Custom property value.", true);
        }

        // Update heartbeat data that has already been set into the heartbeat
        public void UpdateCustomHeartbeatProperty()
        {
            this.hbeatPropertyManager.SetHeartbeatProperty("A_Custom_Property", "Updated custom property value.", true);
        }
    }
}