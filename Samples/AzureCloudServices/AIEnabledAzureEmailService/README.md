##ApplicationInsights: Azure Cloud Service

**NOTE:** The purpose of this sample is to illustrate how an existing cloud service can be instrumented to report AI telemetry. 
Please see the [original code sample](https://code.msdn.microsoft.com/windowsapps/Windows-Azure-Multi-Tier-eadceb36) or [Azure documentation](http://azure.microsoft.com/en-us/services/cloud-services/) for information on cloud services in general.

This is a sample Azure Cloud Service: [AzureEmailService](https://code.msdn.microsoft.com/windowsapps/Windows-Azure-Multi-Tier-eadceb36) that has a web role and 2 worker roles onboarded to Application Insights to send the following telemetry:

* Requests
  * The web role has a sample of how you can override the default behavior on the requests that are reported as failed
  * The worker roles show how you can instrument different operations in them, reported as requests
* Dependency
  * NOTE: ASYNC HTTP/SQL calls are automatically collected if you are running on .NET Framework versions 4.5.1 or higher
  * The web role has ASYNC HTTP calls, and therefore we get the dependencies out of the box
  * The worker roles show how you can set up the [Application Insights Agent](http://azure.microsoft.com/en-us/documentation/articles/app-insights-monitor-performance-live-website-now/) AKA "Status Monitor", that collects SYNC/ASYNC dependencies. 
  * In addition to collecting SYNC/ASYNC dependency calls, using the AI Agent also gets you additional information such as SQL statements
* Exception
  * The web role has MVC5 and Web API 2 controllers, and it is wired up for unhandled exceptions from both to be reported to AI
* Traces - automatically collected with a simple nuget add
* Custom Events & Metrics: The worker roles have samples of these
* Page Views for the web role
* Performance Counters

Visit our [homepage](http://azure.microsoft.com/en-us/services/application-insights/) for more information on Application Insights

  