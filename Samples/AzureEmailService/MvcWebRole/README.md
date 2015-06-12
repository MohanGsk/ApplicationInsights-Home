##ApplicationInsights: Web role

This sample illustrates how an existing cloud service can be instrumented to report telemetry to [Visual Studio Application Insights](https://azure.microsoft.com/services/application-insights/). 
Please see the [original code sample](https://code.msdn.microsoft.com/windowsapps/Windows-Azure-Multi-Tier-eadceb36) or [Azure documentation](http://azure.microsoft.com/en-us/services/cloud-services/) for information on cloud services in general.
The only changes to the original code are lines related to Application Insights telemetry.

This sample web role is instrumented with the [Application Insights for Web] (http://www.nuget.org/packages/Microsoft.ApplicationInsights.Web) NuGet package. It reports the following telemetry:

## Reporting HTTP Requests

The [Application Insights for web NuGet package](http://www.nuget.org/packages/Microsoft.ApplicationInsights.Web) reports the response time and success or failure of each request.

#### Customizing the success criteria

An HTTP request is considered failed if the response has a statusCode >= 400, by default. If that does not work for you (for example if you don't consider 401 a failure), you can override the default behavior:

* Provide a custom implementation of the ITelemetryInitializer interface, like the example [here](Telemetry/MyTelemetryInitializer.cs
* Add this to the list of TelemetryInitializers in ApplicationInsights.config as shown [here](ApplicationInsights.config#L56). You can also [write code to add the initializer](http://go.microsoft.com/fwlink/?LinkID=401493#telemetry-initializers).
    
#### Request name

In the Application Insights portal, you can group Request telemetry by the "Request Name" attribute, so that you can get meaningful aggregations on the number of calls, response times, failures etc. 

The default naming scheme is the following:

* ASP.NET MVC: Request name is set to [VERB controller/action]( http://www.asp.net/mvc/overview/getting-started/introduction/adding-a-controller). For example, a request with URL http://myhost.com/api/movies will be reported as "GET api/movies".
* If the URL contains an MVC parameter segment such as “http://myhost.com/api/movies/5”, the request will be reported using the name of the parameter defined in the registered route maps. Using the default map, this would be “GET api/movies[id]”.
* If the routing table is empty or doesn’t have “controller”, HttpRequest.Path will be used as a request name. This property doesn’t include domain name and query string.
* NOTE: Request names are case-sensitive. 
    
If the default rules do not work for your application (for example, each request gets a unique name), you could provide a custom WebOperationNameTelemetryInitializer implementation to override the default behavior.

## Reporting Dependencies

Application Insights SDK can report calls that your app makes to external dependencies such as REST apis and SQL servers. This lets you see whether a particular dependency is causing slow responses or failures.

To track dependencies, you have to set up the web role with the [Application Insights Agent](http://azure.microsoft.com/documentation/articles/app-insights-monitor-performance-live-website-now/) also known as "Status Monitor".
 
To use the Application Insights Agent with your web roles:

* Add the [AppInsightsAgent](AppInsightsAgent) folder and the 2 files in it to your web/worker role projects. Be sure to set their build properties so that they are always copied into the output directory. These files install the agent.
* Add the start up task to the CSDEF file as shown [here](../AzureEmailService/ServiceDefinition.csdef#L18)
* NOTE: *Worker roles* require 3 environment variables as shown [here](../AzureEmailService/ServiceDefinition.csdef#L44). This is not required for web roles.

Here's an example of what you see at the Application Insights portal:

* Rich diagnostics with automatically correlated requests and dependencies:
  <img src="http://i.imgur.com/SMxacy4.png" width="450">

* Performance of the web role, with dependency information:
  <img src="http://i.imgur.com/6yOBtKu.png" width="450">

## Reporting Exceptions

This web role has MVC5 and Web API 2 controllers. The unhandled exceptions from the 2 are captured with the following:

* [AiHandleErrorAttribute](Telemetry/AiHandleErrorAttribute.cs) set up [here](App_Start/FilterConfig.cs#L12) for MVC5 controllers
* [AiWebApiExceptionLogger](Telemetry/AiWebApiExceptionLogger.cs) set up [here](App_Start/WebApiConfig.cs#L25) for Web API 2 controllers

See [this article](http://azure.microsoft.com/documentation/articles/app-insights-asp-net-exceptions/), for information on how you can collect unhandled exceptions from other application types.

## Traces

This web role already uses System.Diagnostics.Trace for diagnostic logging. We add the log stream into the Application Insights diagnostic stream so that we can correlate log events with requests, exceptions and other reports.

The [TraceListener](http://www.nuget.org/packages/Microsoft.ApplicationInsights.TraceListener) NuGet package automatically collects the log traces. (NLog and log4Net are also supported. See this [article](http://azure.microsoft.com/documentation/articles/app-insights-search-diagnostic-logs/).)

If you trace exceptions, the rich detail with exceptions will automatically be collected

## Performance Counters

The following counters are collected by default:

    * \Process(??APP_WIN32_PROC??)\% Processor Time
	* \Memory\Available Bytes
	* \.NET CLR Exceptions(??APP_CLR_PROC??)\# of Exceps Thrown / sec
	* \Process(??APP_WIN32_PROC??)\Private Bytes
	* \Process(??APP_WIN32_PROC??)\IO Data Bytes/sec
	* \Processor(_Total)\% Processor Time
	* \ASP.NET Applications(??APP_W3SVC_PROC??)\Requests/Sec	
	* \ASP.NET Applications(??APP_W3SVC_PROC??)\Request Execution Time
	* \ASP.NET Applications(??APP_W3SVC_PROC??)\Requests In Application Queue

You can specify additional custom or other windows performance counters as shown [here](ApplicationInsights.config#L22). [Learn more](https://azure.microsoft.com/documentation/articles/app-insights-configuration-with-applicationinsights-config/#performance-collector-module).

<img src="http://i.imgur.com/ogrk8Ub.png" width="450">

## Page Views

Collected automatically by adding the [JavaScript nuget](http://www.nuget.org/packages/Microsoft.ApplicationInsights.JavaScript) 

* You could also just add a JavaScript snippet to shared "master" file as shown [here](Views/Shared/_Layout.cshtml#L9)
* See [JavaScript SDK](https://azure.microsoft.com/documentation/articles/app-insights-web-track-usage/) for more information on custom usage telemetry you could collect
  <img src="http://i.imgur.com/L1INBSd.png" width="450">
  
## Environment support

To collect AI telemetry from multiple environments (DEV/INT/Pre-Prod/PROD etc): 

* Set the Instrumentation key in the respective CSCFG files
* Configure it at application start up time, in the global.asax.cs file for web roles as shown [here](Global.asax.cs#L27)
* The JavaScript can also read from the same as shown [here](Views/Shared/_Layout.cshtml#L9). 
  * Note that this has a slight performance overhead, but is helpful if you are looking to report both client and server side telemetry to the same instrumentation key

## Important

* If user/session telemetry is not applicable for your web/worker role, we recommend you remove the following telemetry modules and initializers from the ApplicationInsights.config file
  * [WebSessionTrackingTelemetryModule](ApplicationInsights.config#L9)
  * [WebUserTrackingTelemetryModule](ApplicationInsights.config#L10)
  * [WebSessionTelemetryInitializer](ApplicationInsights.config#L63)
  * [WebUserTelemetryInitializer](ApplicationInsights.config#L62)
* If your web/worker role has a mix of browser based clients & others, and you do have your web clients instrumented with the [JavaScript nuget](http://www.nuget.org/packages/Microsoft.ApplicationInsights.JavaScript):
  * Add SetCookie = false to the [WebSessionTrackingTelemetryModule](ApplicationInsights.config#L9) and [WebUserTrackingTelemetryModule](ApplicationInsights.config#L10) as mentioned [here](ApplicationInsights.config#L17)
