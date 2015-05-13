##ApplicationInsights: Worker role

This sample worker role is instrumented with the [Application Insights for Web] (http://www.nuget.org/packages/Microsoft.ApplicationInsights.Web) nuget and reports the following telemetry:

* **Request**
  * A request is in essence a unit of *named* server side work that can be *timed* and independently *succeed/fail*
  * As such, Requests can be applied for worker roles, and provides a handy way to capture performance and success telemetry of the different operations a worker role may be doing.
  * For instance, one of the operations in this worker role is to check a queue for messages periodically. That operation is reported as a [CheckMessagesQueue](WorkerRoleB.cs#L73) request when that completes successfully.
  * When the operation fails with an exception (or any other condition for that matter), you can report a failed request as shown [here](WorkerRoleB.cs#L92)
* **Dependency**
  * NOTE: ASYNC HTTP/SQL calls are automatically collected if you are running on .NET Framework versions 4.5.1 or higher
  * This worker role has SYNC HTTP dependency calls, and is set up to use the [Application Insights Agent](http://azure.microsoft.com/en-us/documentation/articles/app-insights-monitor-performance-live-website-now/) AKA "Status Monitor", that collects them
  * To use the AI Agent with your web/worker roles:
    * Add the [AppInsightsAgent](AppInsightsAgent) folder and the 2 files in it to your web/worker role projects. Be sure to set them up to be copied always into the output directory
	* Add the start up task to the CSDEF file as shown [here](../AzureEmailService/ServiceDefinition.csdef#L60)
	* For worker roles, add 3 environment variables as shown [here](../AzureEmailService/ServiceDefinition.csdef#L70)
  * In addition to collecting SYNC dependency calls, the [Application Insights Agent](http://azure.microsoft.com/en-us/documentation/articles/app-insights-monitor-performance-live-website-now/) has additional capabilities such as collection of SQL statements etc.
* **Exception**
  * Add a TrackException(ex) call to report any exceptions you would like to collect, as shown [here](WorkerRoleB.cs#L93)
* **Traces**
  * This worker role uses System.Diagnostics traces. They are automatically collected with the [TraceListener](http://www.nuget.org/packages/Microsoft.ApplicationInsights.TraceListener) nuget added
  * NLog, log4Net etc. also supported. See this [article](http://azure.microsoft.com/en-us/documentation/articles/app-insights-search-diagnostic-logs/) for more information on collecting traces from your worker roles
* **Custom Events & Metrics**
  * This worker role is instrumented to send 2 custom events as shown [here](WorkerRoleB.cs#L122) & [here](WorkerRoleB.cs#L187).
* **Performance Counters**
  * The following counters are collected by default:
    * \Process(??APP_WIN32_PROC??)\% Processor Time
	* \Memory\Available Bytes
	* \.NET CLR Exceptions(??APP_CLR_PROC??)\# of Exceps Thrown / sec
	* \Process(??APP_WIN32_PROC??)\Private Bytes
	* \Process(??APP_WIN32_PROC??)\IO Data Bytes/sec
	* \Processor(_Total)\% Processor Time
  * You can specify additional custom or other windows performance counters as shown [here](ApplicationInsights.config#L14)

**Environment support:** To collect AI telemetry from multiple environments (DEV/INT/Pre-Prod/PROD etc): 
* Set the Instrumentation key in the respective CSCFG files
* Configure it at role start up time, in the OnStart function as shown [here](WorkerRoleB.cs#L333)


#Important
* We encourage you to add the [Application Insights for Web] (http://www.nuget.org/packages/Microsoft.ApplicationInsights.Web) nuget as that adds modules that add server context like the Role information etc.
* If user/session telemetry is not applicable for your web/worker role, we recommend you remove the following telemetry modules and initializers from the ApplicationInsights.config file
  * [WebSessionTrackingTelemetryModule](ApplicationInsights.config#L36)
  * [WebUserTrackingTelemetryModule](ApplicationInsights.config#L37)
  * [WebSessionTelemetryInitializer](ApplicationInsights.config#L67)
  * [WebUserTelemetryInitializer](ApplicationInsights.config#L61)
* If your web/worker role has a mix of browser based clients & others, and you do have your web clients instrumented with the [JavaScript nuget](http://www.nuget.org/packages/Microsoft.ApplicationInsights.JavaScript):
  * Add <SetCookie>false</SetCookie> to the [WebSessionTrackingTelemetryModule](ApplicationInsights.config#L36) and [WebUserTrackingTelemetryModule](ApplicationInsights.config#L37) as mentioned [here](ApplicationInsights.config#L44)
