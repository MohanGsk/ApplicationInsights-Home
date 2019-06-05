# Status Monitor v2 API: Start-ApplicationInsightsMonitoringTrace (v0.3.0-alpha)

This article describes a cmdlet that's a member of the [Az.ApplicationMonitor PowerShell module](https://www.powershellgallery.com/packages/Az.ApplicationMonitor/).

> [!IMPORTANT]
> Status Monitor v2 is currently in public preview.
> This preview version is provided without a service-level agreement, and we don't recommended it for production workloads. Some features might not be supported, and some might have constrained capabilities.
> For more information, see [Supplemental Terms of Use for Microsoft Azure Previews](https://azure.microsoft.com/support/legal/preview-supplemental-terms/).

## Description

Collects [ETW Events](https://docs.microsoft.com/windows/desktop/etw/event-tracing-portal) from the codeless attach runtime. 
This is an alternative to running [PerfView](https://github.com/microsoft/perfview).

This cmdlet will run until it reaches the timeout duration (default 5 minutes) or is stopped manually (`Ctrl + C`).
As long as this cmdlet is running it will collect events and print the current time every 5 seconds to indicate that it is still running.

Collected events will be printed to the console in real-time and saved to an ETL file. The output ETL file can be opened by [PerfView](https://github.com/microsoft/perfview) for further investigation.


> [!IMPORTANT] 
> This cmdlet requires a PowerShell session with Admin permissions.

## Examples

### How to collect events

Normally we would ask that you collect events to investigate why your application isn't being instrumented.

The codeless attach runtime will emit ETW events when IIS starts up and when your application starts up.

To collect these events:
1. In a cmd console with admin privileges, execute `iisreset /stop` To turn off IIS and all web apps.
2. Execute this cmdlet
3. In a cmd console with admin privileges, execute `iisreset /start` To start IIS.
4. Try to browse to your app.
5. After your app finishes loading, you can wait for this cmdlet to timeout or manually stop it (`Ctrl + C`).

## Parameters

### -MaxDurationInMinutes
**Optional.** Use this parameter to set how long this script should collect events. Default is 5 minutes.

### -LogDirectory
**Optional.** Use this switch to set the output directory of the ETL file. 
By default, this will be created in the PowerShell Modules directory. 
The full path will be displayed during script execution.

### -Verbose
**Common parameter.** Use this switch to output detailed logs.

## Output


#### Example output

```
Starting...
Log File: D:\RedfieldAttach\bin\Debug\logs\20190604_180259_ApplicationInsights_ETW_Trace.etl
6:02:59 PM
6:03:04 PM
6:03:09 PM
6:03:12 PM EVENT: Microsoft-Demos-MySource MyFirstEvent
6:03:12 PM EVENT: Microsoft-Demos-MySource MySecondEvent this is a test
6:03:12 PM EVENT: Microsoft-Demos-MySource MyThirdEvent 0e41430e-21ca-4d6b-ba8a-11be41b330db
6:03:14 PM
6:03:19 PM
Timeout Reached. Stopping...
```

## Next steps

TODO
