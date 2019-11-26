# Connection String Documentation [Draft]

## TODO: Description

- one setting for proxy configurations https://docs.microsoft.com/en-us/azure/azure-monitor/app/troubleshoot-faq#can-i-monitor-an-intranet-web-server
- Government clouds. https://docs.microsoft.com/en-us/azure/azure-government/documentation-government-services-monitoringandmanagement#net-with-applicationinsightsconfig


## How to set a connection string

Connection Strings are supported in the following SDK versions:\
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


### TODO: NEED example config for each SDK (Net, Java, Javascript, Node, Python).

### .Net Example

TelemetryConfiguration.ConnectionString: https://github.com/microsoft/ApplicationInsights-dotnet/blob/add45ceed35a817dc7202ec07d3df1672d1f610d/BASE/src/Microsoft.ApplicationInsights/Extensibility/TelemetryConfiguration.cs#L271-L274

### Java Example

TODO

### Javascript Example

TODO

### Node Example

TODO

### Python Example

TODO

## TODO: Schema

### Max Length

The connection has a maximum supported length of 4096 characters.


### Key-Value pairs

Connection string consists of a list of settings represented as key-value pairs separated by semicolon:
`key1=value1;key2=value2;key3=value3`

### Connection String Syntax

- `InstrumentationKey` (ex: 00000000-0000-0000-0000-000000000000)
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

#### TODO: VALID Prefixes

- Telemetry Ingestion: `dc`
- Live Metrics: `live`
- Profiler: `profiler`
- Snapshot: `snapshot`

**TODO: Include links to each individual product for reference**


## TODO: Examples

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
