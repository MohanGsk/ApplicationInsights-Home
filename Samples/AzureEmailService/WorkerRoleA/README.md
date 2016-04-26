##ApplicationInsights: Worker Role A

This sample illustrates how an existing cloud service can be instrumented to report telemetry to Application Insights so that you can monitor its performance and track usage. 
Please see the [original code sample](https://code.msdn.microsoft.com/windowsapps/Windows-Azure-Multi-Tier-eadceb36) or [Azure documentation](http://azure.microsoft.com/en-us/services/cloud-services/) for information on cloud services in general.
The only changes we've made to the original code are related to Application Insights telemetry.

We began by instrumenting this worker role with the [Application Insights for Web] (http://www.nuget.org/packages/Microsoft.ApplicationInsights.Web) NuGet package. That package collects a variety of telemetry. We also added some logic to collect additional diagnostic telemetry.

To monitor worker roles, there are some specific customizations that are advisable to get the best results.

**Important**: 
- Please be sure to set the ApplicationInsights.config file Build Action to Content.
- Please be sure to set the ApplicationInsights.config file to be copied always to the output directory. This is only required for worker roles.

## Report Requests

In web roles, the requests module collects data about HTTP requests. For worker roles like this one, we can adapt the logic to capture timing and success of the different operations that the worker role performs. A request is in essence a unit of *top level named* server side work that can be *timed* and independently *succeed/fail*

In this particular worker role, the key operation is to scan an Azure table periodically for new messages that have to be processed. It then creates a sendEmail row for each recipient, to be processed by the other worker role. It also archives any messages that have been processed. This is the operation that we model as a request.

The following lines of code illustrate this.

* At the beginning of each run iteration, we:
   * Start a timer, which measures the time taken to complete each request. [loc](WorkerRoleA.cs#L34)
   * Create a named request telemetry instance, with the start time recorded. We are using a [helper class](Telemetry/RequestTelemetryHelper.cs) to do this for brevity. [loc](WorkerRoleA.cs#L35)
   * Use CallContext to set a correlation Id to be set for all telemetry items generated in the processing of this instance (more on this below). This helps find all traces, dependency calls, exceptions associated with a failed request for instance. [loc](WorkerRoleA.cs#L36)
   * Add a custom metric to record the number of messages processed in that instance. This can help you understand how the processing scales with the number of messages per instance. [loc](WorkerRoleA.cs#L58)
* If the processing completes successfully, we dispatch the request telemetry with the time taken, and the success flag. [loc](WorkerRoleA.cs#L96)
* Else we report a failed request instance by setting the success flag to false. [loc](WorkerRoleA.cs#L108)      

See [WorkerRoleB](../WorkerRoleB) for a different modeling of operations as requests.

## Report Dependencies

Application Insights SDK can report calls that your app makes to external dependencies such as REST apis and SQL servers. This lets you see whether a particular dependency is causing slow responses or failures.

To track dependencies, you have to set up the web role with the [Application Insights Agent](http://azure.microsoft.com/documentation/articles/app-insights-monitor-performance-live-website-now/) also known as "Status Monitor".

To use the Application Insights Agent with your web/worker roles:

* Add the [AppInsightsAgent](AppInsightsAgent) folder and the two files in it to your web/worker role projects. Be sure to set their build properties so that they are always copied into the output directory. These files install the agent.
* Add the start up task to the CSDEF file as shown [here](../AzureEmailService/ServiceDefinition.csdef#L18)
* NOTE: *Worker roles* require 3 environment variables as shown [here](../AzureEmailService/ServiceDefinition.csdef#L44). This is not required for web roles.

Here's a screenshot on the requests and dependency information for this worker role:
<img src="http://i.imgur.com/a5R0PBk.png" width="450">

## Reporting Exceptions

There are two ways to track exceptions.

* TrackException(ex)
* If you have added the Application Insights trace listener NuGet package, you can use System.Diagnostics.Trace to log exceptions. [loc](WorkerRoleA.cs#L107)


## Traces

This worker role already uses System.Diagnostics.Trace  traces.diagnostic logging. We add the log stream into the Application Insights diagnostic stream so that we can correlate log events with requests, exceptions and other reports.

The [TraceListener](http://www.nuget.org/packages/Microsoft.ApplicationInsights.TraceListener) NuGet package automatically collects the log traces. (NLog and log4Net are also supported. See this [article](http://azure.microsoft.com/documentation/articles/app-insights-search-diagnostic-logs/).)


## Custom Events & Metrics

This worker role is instrumented to send a custom metric "SubscriberCount" as shown [here](WorkerRoleA.cs#L135).
  <img src="http://i.imgur.com/sJYomnb.png" width="450">

## Performance Counters

The following counters are collected by default:

    * \Process(??APP_WIN32_PROC??)\% Processor Time
	* \Memory\Available Bytes
	* \.NET CLR Exceptions(??APP_CLR_PROC??)\# of Exceps Thrown / sec
	* \Process(??APP_WIN32_PROC??)\Private Bytes
	* \Process(??APP_WIN32_PROC??)\IO Data Bytes/sec
	* \Processor(_Total)\% Processor Time

You can specify additional custom or other windows performance counters as shown [here](ApplicationInsights.config#L14)
  <img src="http://i.imgur.com/OLfMo2f.png" width="450">

## Correlated Telemetry

It is a rich diagnostic experience, when you can see what led to a failed or high latency request. With a custom telemetry initializer, you can set a common Operation.Id context attribute for all the telemetry to achieve this. This will allow you to see whether the latency/failure issue was caused due to a dependency or your code, at a glance! 

Here's how:

* Set the correlation Id into a CallContext as shown [here] (WorkerRoleA.cs#L36). In this case, we are using the Request ID as the correlation id
* Add a custom TelemetryInitializer implementation, that will set the Operation.Id to the correlationId set above. Shown here: [ItemCorrelationTelemetryInitializer] (Telemetry/ItemCorrelationTelemetryInitializer.cs#L13)
* Add the custom telemetry initializer. You could do that in the ApplicationInsights.config file, or in code as shown [here](WorkerRoleA.cs#L233)

That's it! The portal experience is already wired up to help you see all associated telemetry at a glace:
<img src="http://i.imgur.com/bHxuUhd.png" width="450">

## Environment support:

To collect Application Insights telemetry from multiple environments (DEV/INT/Pre-Prod/PROD etc): 

* Set the Instrumentation key in the respective CSCFG files
* Configure it at role start up time, in the OnStart function as shown [here](WorkerRoleA.cs#L232)

###Important

* We encourage you to add the [Application Insights for Web] (http://www.nuget.org/packages/Microsoft.ApplicationInsights.Web) nuget as that adds modules that add server context like the Role information etc.
