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

### node
node.js SDK.

Public versions: https://github.com/Microsoft/ApplicationInsights-node.js/releases

### ios/ osx

### javascript
JavaScript SDK.

Public versions: https://github.com/Microsoft/ApplicationInsights-js/releases

### android

### java
java SDK.

Public versions: https://github.com/Microsoft/ApplicationInsights-java/releases

### aspnetv5
ASP.NET core SDK

Public versions: https://github.com/Microsoft/ApplicationInsights-aspnetcore/releases

### sc
Snapshot Debugger (Microsoft.ApplicationInsights.SnapshotCollector)

Nuget: https://www.nuget.org/packages/Microsoft.ApplicationInsights.SnapshotCollector

### hockeysdk

