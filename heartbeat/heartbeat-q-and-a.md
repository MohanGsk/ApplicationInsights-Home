
### Question: 
What's the advantage of doing this in the SDK versus letting apps do it themselves with, for example, a periodic timer calling _telemetryClient.TrackEvent("Heartbeat")?

### Answer:
The idea is to provide everyone with a heartbeat item that they can make use
of however they like, *without having to implement anything on their own*. 
Of course, we need to keep it simple and extensible while allowing the user
full control at the same time.

---

### Question: 
Is there going to be a special Azure Portal view for heart-beat events? What
about support for live stream?

### Answer: 
There may be a UX-based feature that will leverage the heartbeat, watch our
announcements page for anything of that nature. As for Live Metrics Stream, 
since the proposed payload is simply a MetricTelemetry item we can filter for
this heartbeat specifically via the new filtering functionality (available in
the Live Metrics Stream today).

---

### Question:
We already have "Availability" events. How does this compare?

### Answer:
Availability events are only used to measure availability via the WebTest
functionality we expose in the portal. I see this as a more of a 'pull'
availability function where the proposal here would be more of a 'push' one,
and would be generic in terms of type of application. Also, when we were 
discussing the implementation of this feature we felt that overloading this
type might confuse existing UX elements that are looking for this type today.

---

### Question:
How are you getting the values for the Azure-specific payload properties 
(location/offer/VMsize/fault-domain etc.)? Can they all be queried by running
code in the user's app? Or is there some Azure fabric "magic" that can supply 
them?

### Answer:
We obtain the Azure-VM specific values (when they are available) from the
[Azure Instance Metadata Service].

---

### Question:
This feature is enabled via the `DiagnosticsTelemetryModule`. Is that a module
that the user has to add explicitly to their config, or does it automatically
and always get added?

### Answer:
The `DiagnosticsTelemetryModule` is added by default to all apps that are
instrumented with the .NET SDK. The reason we chose this route was to ensure we
could provide the maximum number of users with the benefits that Heartbeats
provide with the least setup/maintainence cost. If the user wishes to configure
or turn the Heartbeat feature off, then they would have to edit their config
file or code to do so.

---

### Question:
We need to discover if our application stops running at any location as quickly
as possible. Will this scenario become possible with this feature?

### Answer:
This is one way you can make use of the Health Heartbeat feature, yes. You
could set up alerts that detect when an app stops sending the heartbeat, and
respond as necessary.

---

### Question:
Does this proposal intend to change how telemetry is collected today?

### Answer:
No. This is simply an enhancement to what we have today.

---

[Azure Instance Metadata Service]: (https://docs.microsoft.com/en-us/azure/virtual-machines/windows/instance-metadata-service)