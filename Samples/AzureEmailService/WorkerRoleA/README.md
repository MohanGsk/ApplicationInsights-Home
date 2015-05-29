##ApplicationInsights: Worker Role A

**NOTE:** The purpose of this sample is to illustrate how an existing cloud service can be instrumented to report AI telemetry. 
Please see the [original code sample](https://code.msdn.microsoft.com/windowsapps/Windows-Azure-Multi-Tier-eadceb36) or [Azure documentation](http://azure.microsoft.com/en-us/services/cloud-services/) for information on cloud services in general.
The only changes to the original code, pertain to the lines of code/configuration related to Application Insights telemetry.

This sample worker role is instrumented with the [Application Insights for Web] (http://www.nuget.org/packages/Microsoft.ApplicationInsights.Web) nuget. In addition to collecting the following telemetry, we will also show how you can use the SDK to enable powerful diagnostics with correlated telemetry. 
With web roles traces, exceptions, dependency calls etc. are correlated out of the box to the specific request instances. For worker roles, you can do the same (enrich with custom logic!) with a custom TelemetryInitializer as we show in this sample.

**Important**: Please be sure to set the ApplicationInsights.config file to be copied always to the output directory. This is only required for worker roles.

* **Request**
  * A request is in essence a unit of *top level named* server side work that can be *timed* and independently *succeed/fail*
  * As such, Requests can be applied for worker roles, and provides a handy way to capture performance and success telemetry of the different operations a worker role may be doing.
  * In this particular worker role, the key operation is to scan an Azure table periodically for new messages that need to be processed. It then creates a sendEmail row for the other worker role for each recipient. It also archives any messages that have been processed
  * We have modeled this operation as the request. The following lines of code illustrate this:
    * We start a timer at the beginning of each iteration. This will help measure the time taken to complete each request. [loc](WorkerRoleA.cs#L34)
    * Create a named request instance, with the start time recorded. We are using a [helper class](Telemetry/RequestTelemetryHelper.cs) to do this for brevity. [loc](WorkerRoleA.cs#L35)
	* Use CallContext to set a correlation Id to be set for all telemetry items generated in the processing of this instance (more on this below). This helps find all traces, dependency calls, exceptions associated with a failed request for instance. [loc](WorkerRoleA.cs#L36)
	* Add a custom metric to record the number of messages processed in that instance. This can help understand how the processing scales with the number of messages for instance. [loc](WorkerRoleA.cs#L58)
	* If the processing completes successfully, we dispatch the request with the time taken, and the success flag. [loc](WorkerRoleA.cs#L96)
	* Else, we report a failed request instance by setting the success flag to false. [loc](WorkerRoleA.cs#L108)      
  * See [WorkerRoleB](../WorkerRoleB) for a different modeling of operations as requests

* **Dependency**
  * This worker role is set up to use the [Application Insights Agent](http://azure.microsoft.com/en-us/documentation/articles/app-insights-monitor-performance-live-website-now/) AKA "Status Monitor", that collects them
  * To use the AI Agent with your web/worker roles:
    * Add the [AppInsightsAgent](AppInsightsAgent) folder and the 2 files in it to your web/worker role projects. Be sure to set them up to be copied always into the output directory
	* Add the start up task to the CSDEF file as shown [here](../AzureEmailService/ServiceDefinition.csdef#L24)
	* For worker roles, add 3 environment variables as shown [here](../AzureEmailService/ServiceDefinition.csdef#L34)
  * In addition to collecting SYNC dependency calls, the [Application Insights Agent](http://azure.microsoft.com/en-us/documentation/articles/app-insights-monitor-performance-live-website-now/) has additional capabilities such as collection of SQL statements etc.
  * Here's a screenshot on the requests and dependency information for this worker role:
<img src="http://i.imgur.com/DIxrk0h.png" width="450">

* **Exception**
  * With System.Diagnostics traces, if you log out the exception, and have added the AI trace listener nuget - we will automatically collect and send those. [loc](WorkerRoleA.cs#L107)
  * In addition, if you would like to report any handled/unhandled exception, you can do that with a simple TrackException(ex) call 

* **Traces**
  * This worker role uses System.Diagnostics traces. They are automatically collected with the [TraceListener](http://www.nuget.org/packages/Microsoft.ApplicationInsights.TraceListener) nuget added
  * NLog, log4Net etc. also supported. See this [article](http://azure.microsoft.com/en-us/documentation/articles/app-insights-search-diagnostic-logs/) for more information on collecting traces from your worker roles

* **Custom Events & Metrics**
  * This worker role is instrumented to send a custom metric "SubscriberCount" as shown [here](WorkerRoleA.cs#L135).
  <img src="http://i.imgur.com/CQlUE37.png" width="450">

* **Performance Counters**
  * The following counters are collected by default:
    * \Process(??APP_WIN32_PROC??)\% Processor Time
	* \Memory\Available Bytes
	* \.NET CLR Exceptions(??APP_CLR_PROC??)\# of Exceps Thrown / sec
	* \Process(??APP_WIN32_PROC??)\Private Bytes
	* \Process(??APP_WIN32_PROC??)\IO Data Bytes/sec
	* \Processor(_Total)\% Processor Time
  * You can specify additional custom or other windows performance counters as shown [here](ApplicationInsights.config#L14)
  <img src="http://i.imgur.com/KSoNmfU.png" width="450">

* **Correlated Telemetry**
  * It is a rich diagnostic experience, when you can see what led to a failed or high latency request. With a custom telemetry initializer, you can set a common Operation.Id context attribute for all the telemetry to achieve this.
  * This will allow you to see whether the latency/failure issue was caused due to a dependency or your code, at a glance! Here's how:
    * Set the correlation Id into a CallContext as shown [here] (WorkerRoleA.cs#L36). In this case, we are using the Request ID as the correlation id
	* Add a custom TelemetryInitializer implementation, that will set the Operation.Id to the correlationId set above. Shown here: [ItemCorrelationTelemetryInitializer] (Telemetry/ItemCorrelationTelemetryInitializer.cs#L13)
	* Add the custom telemetry initializer. You could do that in the ApplicationInsights.config file, or in code as shown [here](WorkerRoleA.cs#L233)
	* That's it! The portal experience is already wired up to help you see all associated telemetry at a glace:
<img src="http://i.imgur.com/z0O5BIr.png" width="450">

**Environment support:** To collect AI telemetry from multiple environments (DEV/INT/Pre-Prod/PROD etc): 
* Set the Instrumentation key in the respective CSCFG files
* Configure it at role start up time, in the OnStart function as shown [here](WorkerRoleA.cs#L232)

###Important
* We encourage you to add the [Application Insights for Web] (http://www.nuget.org/packages/Microsoft.ApplicationInsights.Web) nuget as that adds modules that add server context like the Role information etc.
* If user/session telemetry is not applicable for your web/worker role, we recommend you remove the following telemetry modules and initializers from the ApplicationInsights.config file
  * [WebSessionTrackingTelemetryModule](ApplicationInsights.config#L34)
  * [WebUserTrackingTelemetryModule](ApplicationInsights.config#L35)
  * [WebSessionTelemetryInitializer](ApplicationInsights.config#L65)
  * [WebUserTelemetryInitializer](ApplicationInsights.config#L59)
* If your web/worker role has a mix of browser based clients & others, and you do have your web clients instrumented with the [JavaScript nuget](http://www.nuget.org/packages/Microsoft.ApplicationInsights.JavaScript):
  * Add SetCookie = false to the [WebSessionTrackingTelemetryModule](ApplicationInsights.config#L36) and [WebUserTrackingTelemetryModule](ApplicationInsights.config#L37) as mentioned [here](ApplicationInsights.config#L42)
