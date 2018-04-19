# SDK Version

SDK version is a field you can specify on every telemetry item. This field represent the specific SDK collected this particular item. This field is used for troubleshooting.

## SDK Version Specification

SDKs are required to include their name and version in the telemetry item using the `ai.internal.sdkVersion` tag conforming to the format below.

```
{
  "tags": {
    "ai.internal.sdkVersion:" "dotnet:2.0.0"
  }
}
```

### SDK Version Format

```
  [PREFIX_]SDKNAME:SEMVER
```  



| Section          | Required | Description                                                             | Example |
|------------------|----------|-------------------------------------------------------------------------|---------|
| Prefix           | No       | An optional single lowercase letter (a-z) followed by an underscore (_) | a_      |
| SDK Name         | Yes      | An alpha lowercase string (a-z)                                         | dotnet  |
| Semantic Version | Yes      | A [Semantic Versioning](http://semver.org/) compatible version string   | 2.0.0   |

SDK name and semver are delimited by a single colon (:).

### Examples

```
  r_dotnet:2.0.0-12345
    dotnet:2.0.0-beta.1
  | ------ ------------
  |    |        |
  |    |        +-------> Semantic Version Format
  |    |
  |    +----------------> SDK Name
  |
  +---------------------> Prefix (optional)
```

## SDK Names

Define your own SDK name and send PR to update the list below. Please do not re-use the same SDK name.

### dotnet
Base .NET SDK API was used to Track telemetry item, either manually, or from SDK that does not supply its own version.

Public versions: https://github.com/Microsoft/ApplicationInsights-dotnet/releases

### rddf
Remote dependency telemetry that was collected via Framework instrumentation (Event Source)

Public versions: https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases

### web
telemetry that was collected by AI Web SDK, mostly is found on requests

Public versions: https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases

### rddp
Remote dependency telemetry that was collected via Profiler instrumentation

Public versions: https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases

### rdddsd
Remote dependency telemetry that was collected via Diagnostic Source instrumentation for Desktop framework

Public versions: https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases

### rdddsc 
Remote dependency telemetry collected via Diagnostic Source for .NET Core.

Public versions: https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases

### pc
performance counters

Public versions: https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases

### wcf
WCF package from [labs](https://github.com/Microsoft/ApplicationInsights-sdk-labs) repository

### unobs
unobserved exceptions - part of web SDK

Public versions: https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases

### unhnd
unhandled exceptions – part of web SDK

Public versions: https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases

### azwapc 
azure web app performance counters

### sd
System diagnostics trace (Microsoft.ApplicationInsights.TraceListener)

Repo: https://github.com/Microsoft/ApplicationInsights-dotnet-logging

Nuget: https://www.nuget.org/packages/Microsoft.ApplicationInsights.TraceListener

### etw
ETW listener (Microsoft.ApplicationInsights.EtwCollector)

Repo: https://github.com/Microsoft/ApplicationInsights-dotnet-logging

Nuget: https://www.nuget.org/packages/Microsoft.ApplicationInsights.EtwCollector

### dsl
DiagnosticSource listener (Microsoft.ApplicationInsights.DiagnosticSourceListener)

Repo: https://github.com/Microsoft/ApplicationInsights-dotnet-logging

Nuget: https://www.nuget.org/packages/Microsoft.ApplicationInsights.DiagnosticSourceListener

### evl
EventSource listener (Microsoft.ApplicationInsights.EventSourceListener)

Repo: https://github.com/Microsoft/ApplicationInsights-dotnet-logging

Nuget: https://www.nuget.org/packages/Microsoft.ApplicationInsights.EventSourceSourceListener

### nlog
.NET logging adapter for nlog (Microsoft.ApplicationInsights.NLogTarget)

Repo: https://github.com/Microsoft/ApplicationInsights-dotnet-logging

Nuget: https://www.nuget.org/packages/Microsoft.ApplicationInsights.NLogTarget

### log4net
.NET logging adapter for log4net (Microsoft.ApplicationInsights.Log4NetAppender)

Repo: https://github.com/Microsoft/ApplicationInsights-dotnet-logging

Nuget: https://www.nuget.org/packages/Microsoft.ApplicationInsights.Log4NetAppender

### wad
Windows Azure Diagnostics reporting through AI

### m-agg
metric aggregation pipeline reported this metric

Public versions: https://github.com/Microsoft/ApplicationInsights-dotnet/releases

### m-agg2

metric aggregation pipeline reported this metric. Second implementation

Public versions: https://github.com/Microsoft/ApplicationInsights-dotnet/releases

### hb
Heartbeat telemetry sent in intervals reported this metric item

Public versions: https://github.com/Microsoft/ApplicationInsights-dotnet/releases

### node
node.js SDK.

Public versions: https://github.com/Microsoft/ApplicationInsights-node.js/releases

### ios/ osx

### javascript
JavaScript SDK.

Public versions: https://github.com/Microsoft/ApplicationInsights-js/releases

### java
java SDK.

Public versions: https://github.com/Microsoft/ApplicationInsights-java/releases

### aspnetv5
ASP.NET core SDK

Public versions: https://github.com/Microsoft/ApplicationInsights-aspnetcore/releases

### sc
Snapshot Debugger (Microsoft.ApplicationInsights.SnapshotCollector)

Nuget: https://www.nuget.org/packages/Microsoft.ApplicationInsights.SnapshotCollector

### ap
Application Insights Profiler: Getting call traces, diagnose application performance.

Repo: https://github.com/Microsoft/ApplicationInsights-Profiler-AspNetCore

Nuget: https://www.nuget.org/packages/Microsoft.ApplicationInsights.Profiler.AspNetCore

### azwapc

Performance counters collected via Azure App Services extensibility. See https://github.com/Microsoft/ApplicationInsights-dotnet-server/blob/eb884b81c568b1054f9b7168ea4b0ec61f9e3506/Src/PerformanceCollector/Perf.Shared/Implementation/PerformanceCounterUtility.cs#L27

### azurefunctions

Telemetry produced by Azure Functions Host instrumentation.
https://github.com/Azure/azure-functions-host/blob/1f243e9febc4d431af3f0341bc8af74975d51659/src/WebJobs.Script/Host/ScriptTelemetryClientFactory.cs#L28

### webjobs

Telemetry collected by Azure Web Jobs hosting. See https://github.com/Azure/azure-webjobs-sdk/blob/5d3952d010c0981477e8b09f60b62312f85d4e1f/src/Microsoft.Azure.WebJobs.Logging.ApplicationInsights/DefaultTelemetryClientFactory.cs#L54

### wad2ai

Telemetry collected by Application Insight's Azure Diagnostics sink. See [Send Cloud Service, Virtual Machine, or Service Fabric diagnostic data to Application Insights](https://docs.microsoft.com/azure/monitoring-and-diagnostics/azure-diagnostics-configure-application-insights).

### wcf

WCF Application Application Insights lab project. 

Repo: https://github.com/Microsoft/ApplicationInsights-SDK-Labs/tree/master/WCF

MyGet: > Install-Package "Microsoft.ApplicationInsights.Wcf" -Source "https://www.myget.org/F/applicationinsights-sdk-labs/" -Pre

Blog: https://azure.microsoft.com/en-us/blog/wcf-monitoring-with-application-insights/

### py3

Telemetry collected by python Application Insights SDK for py3 application. See https://github.com/Microsoft/ApplicationInsights-Python/blob/7ac535f451383d78d63bfc2b8aad518cdde598c7/applicationinsights/channel/TelemetryChannel.py#L9-L15

### py2

Telemetry collected by python Application Insights SDK for py2 application. See https://github.com/Microsoft/ApplicationInsights-Python/blob/7ac535f451383d78d63bfc2b8aad518cdde598c7/applicationinsights/channel/TelemetryChannel.py#L9-L15

### rb

Application Insights Ruby SDK https://github.com/Microsoft/ApplicationInsights-Ruby/blob/c78bb54c8b5c0f70218482219fb8447416cfe550/lib/application_insights/channel/telemetry_channel.rb#L89

### exstat

Experimental exceptions statistics feature. https://github.com/Microsoft/ApplicationInsights-dotnet-server/blob/eb884b81c568b1054f9b7168ea4b0ec61f9e3506/Src/WindowsServer/WindowsServer.Net45/FirstChanceExceptionStatisticsTelemetryModule.cs#L102

### rddsr

Azure Service Fabric service remoting call - Client side. See 
https://github.com/Microsoft/ApplicationInsights-ServiceFabric/blob/275166d8034f1b94881982073e304166fbaef6bd/src/ApplicationInsights.ServiceFabric.Native.Shared/DependencyTrackingModule/ServiceRemotingClientEventListener.cs#L41

### serviceremoting

Azure Service Fabric service remoting call - Server side. See https://github.com/Microsoft/ApplicationInsights-ServiceFabric/blob/275166d8034f1b94881982073e304166fbaef6bd/src/ApplicationInsights.ServiceFabric.Native.Shared/RequestTrackingModule/ServiceRemotingServerEventListener.cs#L29

### ai-k8s

Application Insights telemetry produced by Kubrnetes module. See https://github.com/Microsoft/ApplicationInsights-Kubernetes/blob/578f20e824e6248029554a1f8990b29c4a7c6d11/src/ApplicationInsights.Kubernetes/Utilities/SDKVersionUtils.cs#L34

### azurefunctionscoretools

Azure Functions Core Tools for local development expirience. See https://github.com/Azure/azure-functions-core-tools/blob/acb5fd3b8d8fd77420ec500861c995ade2cead69/src/Azure.Functions.Cli/Diagnostics/ConsoleTelemetryClientFactory.cs#L22

### rddfd

Telemetry was processed via framework and diagnosticsource paths. Deprecated in latest versions of SDK.

### logary

Telemetry produced by F# logging library Logary. See https://github.com/logary/logary/blob/f86bdf05c66ab0387598f0bb3040c0dafe1f92b8/src/targets/Logary.Targets.ApplicationInsights/Targets_AppInsights.fs#L72-L74

### one-line-ps

http://apmtips.com/blog/2017/03/27/oneliner-to-send-event-to-application-insights/


### angular

Unofficial Angular telemetry collection module for Application Insights.

https://www.npmjs.com/package/angular-applicationinsights

https://github.com/VladimirRybalko/angular-applicationinsights/blob/244a003a6df2df487d903c99f75fd497d698dede/src/ApplicationInsights.ts#L47


### owin

May point to unofficial OWIN telemetyr module:

https://github.com/MatthewRudolph/Airy-ApplicationInsights-Owin/blob/a555ddc810edb5b9e8d4866c41ba18ddf793bc1d/src/Dematt.Airy.ApplicationInsights.Owin/ExceptionTracking/MvcExceptionHandler.cs#L38




## Prefixes
Define the prefixes for the SDK.

| SDK Name | Prefix | Description                         |
|----------|:------:|-------------------------------------|
| ap       |   w_   | Telemetry from **Windows** Platform |
| ap       |   l_   | Telemetry from **Linux** Platform   |