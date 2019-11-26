# Connection String Documentation [Draft]

**The final version of this document will be published on Azure Docs: https://docs.microsoft.com/en-us/azure/azure-monitor/**

## TODO: Description

- one setting for proxy configurations https://docs.microsoft.com/en-us/azure/azure-monitor/app/troubleshoot-faq#can-i-monitor-an-intranet-web-server
- Government clouds. https://docs.microsoft.com/en-us/azure/azure-government/documentation-government-services-monitoringandmanagement#net-with-applicationinsightsconfig


## Schema

### Max Length

The connection has a maximum supported length of 4096 characters.


### Key-Value pairs

Connection string consists of a list of settings represented as key-value pairs separated by semicolon:
`key1=value1;key2=value2;key3=value3`

### Connection String Syntax

- `InstrumentationKey` (ex: 00000000-0000-0000-0000-000000000000)
   The connection string is a **required** field.
- `Authorization` (ex: ikey) (This setting is optional because today we only support ikey authorization.)
- `EndpointSuffix` (ex: applicationinsights.azure.cn)
   Setting the endpoint suffix will instruct the SDK which Azure cloud to connect to. The SDK will assemble the rest of the endpoint for individual services.
- Explicit Endpoints.
  Any service can be explicitly overridden in the connection string.
   - `IngestionEndpoint` (ex: https://dc.applicationinsights.azure.com)
   - `LiveEndpoint` (ex: https://live.applicationinsights.azure.com)
   - `ProfilerEndpoint` (ex: https://profiler.applicationinsights.azure.com)
   - `SnapshotEndpoint` (ex: https://snapshot.applicationinsights.azure.com)


### Endpoint Schema

`<prefix>.<suffix>`

- Prefix: Defines a service. (ex: dc, live, profiler, snapshot)
- Suffix: Defines the common domain name. (ex: applicationinsights.azure.cn)

#### TODO: Valid Suffixes

#### VALID Prefixes

**TODO: Include links to each individual product for reference**

- Telemetry Ingestion: `dc`
- Live Metrics: `live`
- Profiler: `profiler`
- Snapshot: `snapshot`



## Connection string examples

### Connection string with endpoint suffix and explicit endpoint override 

`InstrumentationKey=00000000-0000-0000-0000-000000000000;EndpointSuffix=ai.contoso.com;ProfilerEndpoint=https://custom.profiler.contoso.com:444/;`

In this example, this connection string uses the endpoint suffix with one explicit endpoint override.

- Authorization scheme defaults to “ikey” 
- Instrumentation Key: 00000000-0000-0000-0000-000000000000
- The regional service URIs are based on provided endpoint suffix and location, except for the override: 
   - Breeze: https://dc.ai.contoso.com
   - Live metrics: https://live.ai.contoso.com
   - Profiler: https://custom.profiler.contoso.com:444  (this value is explictly overridden in the connection string)
   - Debugger: https://snapshot.ai.contoso.com   


## How to set a connection string

Connection Strings are supported in the following SDK versions:
- .NET and .NET Core v2.12.0
- Java v2.5.1
- Javascript v2.3.0
- NodeJS v1.5.0
- Python v1.0.0

A connection string can be set by either in code, environment variable, CLI (Java only), or Configuration File.

We do not recommend setting both Connection String and Instrumentation key. In the event that a user does set both, whichever was set last will take precidence. 



### Environment variables

- Instrumentation Key: `APPINSIGHTS_INSTRUMENTATIONKEY`
- Connection String: `APPLICATIONINSIGHTS_CONNECTION_STRING`


### .Net SDK Example

TelemetryConfiguration.ConnectionString: https://github.com/microsoft/ApplicationInsights-dotnet/blob/add45ceed35a817dc7202ec07d3df1672d1f610d/BASE/src/Microsoft.ApplicationInsights/Extensibility/TelemetryConfiguration.cs#L271-L274

.Net Explicitly Set:
```
var configuration = new TelemetryConfiguration
{
    ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000;"
};
```

.Net Config File:

```
<ConnectionString>InstrumentationKey=00000000-0000-0000-0000-000000000000</ConnectionString>
```


NetCore config.json: 

```
{
  "ApplicationInsights": {
    "ConnectionString" : "InstrumentationKey=00000000-0000-0000-0000-000000000000;"
    }
  }
```


### Java SDK Example

**TODO**

### Javascript SDK Example

**TODO**

### Node SDK Example

**TODO**

### Python SDK Example

It is recommended to set the environment variable.

To explicitly set the connection string:

```
from opencensus.ext.azure.trace_exporter import AzureExporter
from opencensus.trace.samplers import ProbabilitySampler
from opencensus.trace.tracer import Tracer

tracer = Tracer(exporter=AzureExporter(connection_string='InstrumentationKey=00000000-0000-0000-0000-000000000000;'), sampler=ProbabilitySampler(1.0))
```
