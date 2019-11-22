# Connection String Documentation [Draft]

## TODO: Description

- one place for proxy configurations https://docs.microsoft.com/en-us/azure/azure-monitor/app/troubleshoot-faq#can-i-monitor-an-intranet-web-server
- Government clouds. https://docs.microsoft.com/en-us/azure/azure-government/documentation-government-services-monitoringandmanagement#net-with-applicationinsightsconfig


## Environment variables

- Instrumentation Key: `APPINSIGHTS_INSTRUMENTATIONKEY`
- Connection String: `APPLICATIONINSIGHTS_CONNECTION_STRING`

## TODO: Schema

### Max Length

The connection has a maximum supported length of 4096 characters.


### Key-Value pairs

Connection string consists of a list of settings represented as key-value pairs separated by semicolon:
`key1=value1;key2=value2;key3=value3`

### Connection String Syntax

- `InstrumentationKey` (ex: 00000000-0000-0000-0000-000000000000)
- `Authorization` (ex: ikey)
- `EndpointSuffix` (ex: applicationinsights.azure.cn)
- Explicit Endpoints
   - `IngestionEndpoint` (ex: https://dc.applicationinsights.azure.cn)
   - `LiveEndpoint` (ex: https://live.applicationinsights.azure.cn)
   - `ProfilerEndpoint` (ex: https://profiler.applicationinsights.azure.cn)
   - `SnapshotEndpoint` (ex: https://snapshot.applicationinsights.azure.cn)


### Endpoint Schema

`<prefix>.<suffix>`

- Prefix: Defines a service. (ex: dc, live, profiler, snapshot)
- Suffix: Defines the common domain name. (ex: applicationinsights.azure.cn)


## TODO: Examples

### Connection string with endpoint suffix and explicit endpoint override 

This connection string uses endpoint suffix with one explicit endpoint override. It also specifies location which will be used to routing to regional endpoints (except for the explicit override) 
`InstrumentationKey=00000000-0000-0000-0000-000000000000;EndpointSuffix=ai.contoso.com;ProfilerEndpoint=https://custom.profiler.contoso.com:444/;`
In this example: 
- Authorization scheme defaults to “ikey” 
- Instrumentation Key: 00000000-0000-0000-0000-000000000000
- The regional service URIs are based on provided endpoint suffix and location, except for the override: 
   - Breeze: https://dc.ai.contoso.com  
   - Live metrics: https://live.ai.contoso.com   
   - Profiler: https://custom.profiler.contoso.com:444/ (this value is explictly overridden in the connection string)
   - Debugger: https://snapshot.ai.contoso.com   
