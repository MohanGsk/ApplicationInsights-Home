##ApplicationInsights: Azure Cloud Service

**NOTE:** The purpose of this sample is to illustrate how an existing cloud service can be instrumented to report AI telemetry. 
Please see the [original code sample](https://code.msdn.microsoft.com/windowsapps/Windows-Azure-Multi-Tier-eadceb36) for information on this application, or [Azure documentation](http://azure.microsoft.com/en-us/services/cloud-services/) for information on cloud services in general.
The only changes to the original code, pertain to the lines of code/configuration related to Application Insights telemetry.

This is a sample Azure Cloud Service: [AzureEmailService](https://code.msdn.microsoft.com/windowsapps/Windows-Azure-Multi-Tier-eadceb36) that has a web role and 2 worker roles onboarded to Application Insights.
In addition to collecting the following telemetry, we will also show how you can use the SDK to enable powerful diagnostics with correlated telemetry. With the web role this is available out of the box. We correlate the traces, exceptions, dependency calls to the specific request instance for you.
For worker roles, you can do the same (enrich with custom logic!) with a custom TelemetryInitializer as we show in the sample.

Click through to the [web role](MvcWebRole) or either worker role [A](WorkerRoleA) or [B](WorkerRoleB) for documentation on how each is configured to collect the following telemetry.

* Requests
  * The web role has a sample of how you can override the default behavior on the requests that are reported as failed
  * The worker roles show how you can instrument different operations in them, reported as requests
* Dependency
  * The web and worker roles are set up to use the [Application Insights Agent](http://azure.microsoft.com/en-us/documentation/articles/app-insights-monitor-performance-live-website-now/) AKA "Status Monitor".
  * In addition to collecting SYNC/ASYNC dependency calls, using the AI Agent also gets you additional information such as SQL statements
* Exception
  * The web role has MVC5 and Web API 2 controllers, and it is wired up for unhandled exceptions from both to be reported to AI. [This] (https://azure.microsoft.com/en-us/documentation/articles/app-insights-asp-net-exceptions/) has more information
  * If you are adding exceptions to the trace statements, they will be collected without additional line of code. The worker roles have samples of this.
* Traces - automatically collected with a simple nuget add. See [this](https://azure.microsoft.com/en-us/documentation/articles/app-insights-search-diagnostic-logs/) for more information on collecting traces from different frameworks
* Custom Events & Metrics: The worker roles have samples of these
* Page Views & Usage telemetry for the web role: [JavaScript SDK](https://azure.microsoft.com/en-us/documentation/articles/app-insights-web-track-usage/) has more information on this
* Performance Counters: See the section on performance counters [here] (https://azure.microsoft.com/en-us/documentation/articles/app-insights-web-monitor-performance/) for more information

Visit our [homepage](http://azure.microsoft.com/en-us/services/application-insights/) for more information on Application Insights

  