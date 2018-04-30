
## Question: 
Currently building numerous windows services which id like to monitor their running state, which I believe this would allow?
The ability to change the user-defined payload during runtime would provide me metrics on failure, i.e. what was the apps state before it doesn't call home again.

## Question: 
What's the advantage of doing this in the SDK versus letting apps do it themselves with, for example, a periodic timer calling _telemetryClient.TrackEvent("Heartbeat") ?
Is there going to be a special Azure Portal view for heart-beat events? What about support for live stream?
We already have "Availability" events. How does this compare?
How are you getting the values for the Azure-specific payload properties (location/offer/VMsize/fault-domain etc.)? Can they all be queried by running code in the user's app? Or is there some Azure fabric "magic" that can supply them?
I'm wondering whether this belongs in the base SDK at all. Is there anything in the proposal that requires access to base SDK internals? Perhaps it could be delivered as a separate Nuget package.

## Answer: 
@pharring and @matt-shepherd thanks for the comments. I'll try to answer your queries the best I can, but ultimately @SergeyKanzhelev or @Dmitry-Matveev will have the best ideas for this feature, so I'll let them chime in if they wish as well.

> What's the advantage of doing this in the SDK versus letting apps do it themselves... 

I believe the idea is to provide everyone with a heartbeat item that they can make use of however they like, *without having to implement anything on their own*. Of course, we need to keep it simple and extensible while allowing the user full control at the same time.

> Is there going to be a special Azure Portal view for heart-beat events? What about support for live stream?

I believe the idea is to support a feature on the portal eventually, but nothing specific has been discussed yet. @SergeyKanzhelev will have a better answer here I am sure. As for Live Metrics Stream, since the proposed payload is simply a MetricTelemetry item we can filter for this heartbeat specifically via the new filtering functionality (available in the LMS today).

> We already have "Availability" events. How does this compare?

Availability events, if I am understanding them correctly, currently are only used to measure availability via the WebTest functionality we expose in the portal. I see this as a more of a 'pull' availability function where the proposal here would be more of a 'push' one, and would be generic in terms of type of application. Also, when we were discussing the implementation of this feature we felt that overloading this type might confuse existing UX elements that are looking for this type today.

> How are you getting the values for the Azure-specific payload properties...

The intent here is to get the Azure-specific ones (when they are available) from the [Azure Instance Metadata Service](https://docs.microsoft.com/en-us/azure/virtual-machines/windows/instance-metadata-service). 

> Can they all be queried by running code in the user's app...

Yes, as per the link above.

> I'm wondering whether this belongs in the base SDK at all...

I think the primary focus here is the 'you get this for free' and it comes alongside/within the DiagnosticsTelemetryModule that is already in the base. The idea was that heartbeat data is immediately emitted and by default turned on (while still being controllable by the user) to help in user support scenarios, following the same idea behind the DiagnosticsTelemetryModule.


## Question:
@d3r3kk thanks. Especially thanks for the links to the Azure Instance Metadata Service. I was unaware of it. Of course, that works only on Azure VMs and not on Azure App Service (I tried it) or on-prem machines.

Of course it's up to you what you put in the base SDK. My mental test for what goes there is "Does any reasonable implementation this require internal knowledge of types in the base SDK?" and, as I understand it, this proposal fails that test. Perhaps the fact that it uses the "vsallin.net" endpoint is sufficient grounds, though. I assume that endpoint is special - although I don't know what else it's used for.

I mentioned availability events because it seems like a heartbeat is a sort of availability signal - albeit originating from within the app itself, rather than triggered by an external health-check. I don't know how heavily they're used today, but perhaps they could serve an enhanced purpose.

Anyway, I don't mean to derail the proposal. These are just things to think about. Good luck with the proposal.

## Question:
Here are some thoughts:
1. I do not think we'll send anything to vsallin.net and the documentation is probably incorrect on this point. This used to be staging endpoint long time ago but diagnostic module should've been sending to dc.services.visualstudio.com or applicationinsights.io addresses.
2. We need to be careful with the outgoing calls to fetch Azure information via external services - these calls will be monitored by our own Dependency collection and we might need to exclude those (and also differentiate between customer calls to that service and ours to keep only customer's calls).
3. If we have a lot of Azure specific information retrieved in this way, we may want to check if other cloud providers expose similar functionality and leverage them / address point (2) for them as well.
4. If we add OS / Name and other platform info into this event, we have a slight duplication as such information might be already present in all other telemetry items we collect.
5. It might be better to go with the separate module if we plan to brand is as an extensible heartbeat the customer may want to use and extend instead of the internal heartbeat that our modules will need to use (we discussed it with @d3r3kk offline)

@pharring, the initial idea was to have a metric with custom dimensions for the internal SDK health tracking and heartbeat with the most common properties and add ability for any SDK module (like Quick Pulse) to add to this information and change state of this particular SDK instance from healthy to broken. The higher the value of the metric, the more issues SDK is having. This goes in hand with some internal monitoring tools being widely used in MSFT as far as I know. This slightly differentiates heartbeat approach from the availability telemetry.

With heartbeat, one can answer the questions like the following in our UI or analytics:
- Does SDK stop populating data if everything is calm?
- Are any of my SDKs misconfigured (does not solve the wrong iKey case :) )?

That should also help support engineers to access the customer environment and SDK states quickly.
There was one more aspect to it (people asked for it offline) which I'm not certain is easy to address - use this telemetry to analyze if some issue has been fixed with the rollout of new SDK or similar self-monitoring approaches. So, that's still under the discussion at this point. Hope this helps!

## Answers:
Thanks @Dmitry-Matveev and @pharring for your feedback and comments. I am still ramping up my knowledge of this SDK and any feedback I can get from folks who know it better serves to make me more efficient at learning the important bits, faster.

I am still putting the code into a branch on my own forked repo to gain a better understanding of the SDK itself as well as the extension and how to best implement. I'm leaning a bit towards an extension module at the moment as including it within the base SDK doesn't seem to lend itself well to configuration that I want exposed.

From @Dmitry-Matveev comments:
> 1. I do not think we'll send anything to vsallin.net...

I will put that as an item to resolve when I go about updating the documentation for this feature.

> 2. We need to be careful with the outgoing calls to fetch Azure information via external services...

Ah yes I understand. Once I get to the part where I am querying for heartbeat payload data I will pay very close attention to this issue, hopefully I can provide a solution to anyone extending the feature as well in this regard (ie. What if someone wants to ping some website or make a request to some other service as part of the payload? We might want to allow them a simple way to 'mask' those call too?).

> 3. If we have a lot of Azure specific information retrieved in this way, we may want to check if other(s)...

Agreed. Will keep that in mind.

> 4. If we add OS / Name and other platform info into this event, we have a slight duplication...

Interesting. I will look to see how we gather that information elsewhere then and try and keep things consistent, and perhaps start a conversation on the value of adding that info to this payload.

> 5. It might be better to go with the separate module...

As stated above, I'm still evaluating this and leaning somewhat towards the modularization idea. Will have a better idea in the next couple of days.

## Question:
@d3r3kk You mentioned that this will be done through the `DiagnosticsTelemetryModule`. Is that a module that the user has to add explicitly to their config, or does it automatically and always get added?

Is the usage pattern such that customers would generally not have this module, but if something goes wrong, they would be encouraged to update their config, add this module and get more health information than they had before?

Does this proposal intend to change how telemetry is collected today? Today, AI SDK usage is monitored through fields (like sdkVersion) in every telemetry item's TelemetryContext. Can we use data gathered by this proposal to understand usage patterns instead?

My motivation for asking these questions is that I am evaluating what is the right option to monitor usage of Telemetry Initializers provided by [Application Insights for Service Fabric Sdk](https://github.com/Microsoft/ApplicationInsights-ServiceFabric). I initially thought about, using the internal Sdk Version field but that field already has a value filled in by the TelemetryModule that generated the telemetry - I don't want to blindly replace it in my TelemetryInitializer.

##Question:

Health Heartbeat is a very good idea. We need this so much. 

Our application ( a .net exe) runs at a lot of organizations non-stop as a windows service. It uses App Insights. Our requirement is finding out if the application stops running at any location. So if on one of our customers' servers application is not running e.g. because of a faulty update, we want to find out as soon as possible so we can respond.

Will this be possible with this feature? 

## Answers:
Thanks for the great questions all! Let me see if I can answer them...

From @nizarq:

> Is the usage pattern such that customers would generally not have this module, but if something goes wrong, they would be encouraged to update their config, add this module and get more health information than they had before?

Actually, my intention is that the health heartbeat would be automatically added (it sends very little data) and if you wish, you can turn it off. The user can control the data that will be sent with each heartbeat in that they can turn fields off if they don't want to see them, or if they would like to reduce data to the smallest possible footprint and still get a heartbeat. Also, the user can opt to add custom fields to the heartbeat pulse as they wish.

> Does this proposal intend to change how telemetry is collected today?

No. This is simply an enhancement to what we have today.

> Can we use data gathered by this proposal to understand usage patterns instead?

This is not the intention for this feature, no. You can use Application Insights to [understand usage patterns in other ways ](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-usage-overview) and [with some other features we've provided in Azure.](https://docs.microsoft.com/en-us/azure/application-insights/usage-funnels)

From @etm-admin:

>  ...if on one of our customers' servers application is not running e.g. because of a faulty update, we want to find out as soon as possible so we can respond. _Will this be possible with this feature?_

This is one way you can make use of the Health Heartbeat feature, yes. You could set up alerts that detect when an app stops sending the heartbeat, and respond as necessary.

From @cijothomas:

> I guess you are planning to add other properties like os, vmsize etc. 

Yes, I am learning as I go and getting the feature ready to ship is my initial concern. Adding 'default' fields that the SDK would emit will come second. 

> Could you also collect process-id as well as part of this?

I think we might be able to accommodate this, it does seem useful and most likely this is the best place for something like this. When I get to implementing the various payload quantities I will investigate & incorporate this. (I've updated the spec to include it so we don't forget).


