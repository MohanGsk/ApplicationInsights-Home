##ApplicationInsights: Web role

**NOTE:** The purpose of this sample is to illustrate how an existing cloud service can be instrumented to report AI telemetry. 
Please see the [original code sample](https://code.msdn.microsoft.com/windowsapps/Windows-Azure-Multi-Tier-eadceb36) or [Azure documentation](http://azure.microsoft.com/en-us/services/cloud-services/) for information on cloud services in general.


This sample web role is instrumented with the [Application Insights for Web] (http://www.nuget.org/packages/Microsoft.ApplicationInsights.Web) nuget and reports the following telemetry:

* **Request**
  * Collected out of the box with the [Application Insights for web nuget](http://www.nuget.org/packages/Microsoft.ApplicationInsights.Web)
  * A request is considered failed if the response has a statusCode >= 400. If that does not work for you (401s are not failures for instance), you can override the default behavior:
    * Provide a custom implementation of the ITelemetryInitializer interface - as shown [here](Telemetry/MyTelemetryInitializer.cs)
	* Add this to the list of TelemetryInitializers in ApplicationInsights.config as shown [here](ApplicationInsights.config#L69)
  * Requests are groupable by the "Request Name" attribute, which allows us to provide meaningful aggregations on the number of calls, response times, failures etc. The default naming scheme is the following:
    * ASP.NET MVC: Request name is set to “VERB controller/action”.
	* ASP.NET MVC Web API: Per above both requests “/api/movies/” and “/api/movies/5” will be result in “GET movies”. To support Web API better, request name includes the list of all names of routing parameters if “action” parameter wasn’t found. In this case, the requests will be reported as “GET movies” and “GET movies[id]”.
    * If routing table is empty or doesn’t have “controller” - HttpRequest.Path will be used as a request name. This property doesn’t include domain name and query string.
    * NOTE: Request names are case-sensitive. If the default rules do not work for your application (each request gets a unique name for instance) - you can fix that by providing a custom WebOperationNameTelemetryInitializer implementation to override default behavior.
* **Dependency**
  * NOTE: ASYNC HTTP/SQL calls are automatically collected if you are running on .NET Framework versions 4.5.1 or higher
  * This web role has ASYNC HTTP calls, and therefore we get the dependencies out of the box
  * If your web role has SYNC dependency calls, then you will need to use the [Application Insights Agent](http://azure.microsoft.com/en-us/documentation/articles/app-insights-monitor-performance-live-website-now/) AKA "Status Monitor", that collects them
  * In addition to collecting SYNC/ASYNC dependency calls, using the AI Agent also gets you additional information such as SQL statements
  * Please see either of the worker roles for insstructions on how to set up the AI Agent with your web/worker roles 
* **Exception**
  * This web role has MVC5 and Web API 2 controllers. The unhandled exceptions from the 2 are captured with the following:
    * [AiHandleErrorAttribute](Telemetry/AiHandleErrorAttribute.cs) set up [here](App_Start/FilterConfig.cs#L12) for MVC5 controllers
	* [AiWebApiExceptionLogger](Telemetry/AiWebApiExceptionLogger.cs) set up [here](App_Start/WebApiConfig.cs#L25) for Web API 2 controllers
	* See [this article](http://azure.microsoft.com/en-us/documentation/articles/app-insights-asp-net-exceptions/), for information on how you can collect unhandled exceptions from other application types 
* **Page Views**
  * Collected automatically by adding the [JavaScript nuget](http://www.nuget.org/packages/Microsoft.ApplicationInsights.JavaScript)
  * You could also just add a JavaScript snippet to shared "master" file as shown [here](Views/Shared/_Layout.cshtml#L9)
* **Traces**
  * This web role uses System.Diagnostics traces. They are automatically collected with the [TraceListener](http://www.nuget.org/packages/Microsoft.ApplicationInsights.TraceListener) nuget added
  * NLog, log4Net etc. also supported. See this [article](http://azure.microsoft.com/en-us/documentation/articles/app-insights-search-diagnostic-logs/) for more information on collecting traces from your worker roles
* **Performance Counters**
  * The following counters are collected by default:
    * \Process(??APP_WIN32_PROC??)\% Processor Time
	* \Memory\Available Bytes
	* \.NET CLR Exceptions(??APP_CLR_PROC??)\# of Exceps Thrown / sec
	* \Process(??APP_WIN32_PROC??)\Private Bytes
	* \Process(??APP_WIN32_PROC??)\IO Data Bytes/sec
	* \Processor(_Total)\% Processor Time
	* \ASP.NET Applications(??APP_W3SVC_PROC??)\Requests/Sec	
	* \ASP.NET Applications(??APP_W3SVC_PROC??)\Request Execution Time
	* \ASP.NET Applications(??APP_W3SVC_PROC??)\Requests In Application Queue
  * You can specify additional custom or other windows performance counters as shown [here](ApplicationInsights.config#L14)

**Environment support:** To collect AI telemetry from multiple environments (DEV/INT/Pre-Prod/PROD etc): 
* Set the Instrumentation key in the respective CSCFG files
* Configure it at application start up time, in the global.asax.cs file for web roles as shown [here](Global.asax.cs#L27)

#Important
* If user/session telemetry is not applicable for your web/worker role, we recommend you remove the following telemetry modules and initializers from the ApplicationInsights.config file
  * [WebSessionTrackingTelemetryModule](ApplicationInsights.config#L36)
  * [WebUserTrackingTelemetryModule](ApplicationInsights.config#L37)
  * [WebSessionTelemetryInitializer](ApplicationInsights.config#L67)
  * [WebUserTelemetryInitializer](ApplicationInsights.config#L61)
* If your web/worker role has a mix of browser based clients & others, and you do have your web clients instrumented with the [JavaScript nuget](http://www.nuget.org/packages/Microsoft.ApplicationInsights.JavaScript):
  * Add <SetCookie>false</SetCookie> to the [WebSessionTrackingTelemetryModule](ApplicationInsights.config#L36) and [WebUserTrackingTelemetryModule](ApplicationInsights.config#L37) as mentioned [here](ApplicationInsights.config#L44)
