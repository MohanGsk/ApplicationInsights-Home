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
Cmdlet was canceled.
```


```
Starting...
Log File: C:\Program Files\WindowsPowerShell\Modules\Az.ApplicationMonitor\content\logs\20190604_184919_ApplicationInsights_ETW_Trace.etl
6:49:19 PM
6:49:24 PM
6:49:30 PM
6:49:34 PM
6:49:39 PM
6:49:42 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace Resolved variables to: MicrosoftAppInsights_ManagedHttpModulePath='C:\Program Files\WindowsPowerShell\Modules\Az.ApplicationMonitor\content\Runtime\Microsoft.ApplicationInsights.RedfieldIISModule.dll', MicrosoftAppInsights_ManagedHttpModuleType='Microsoft.ApplicationInsights.RedfieldIISModule.RedfieldIISModule'
6:49:42 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace Resolved variables to: MicrosoftDiagnosticServices_ManagedHttpModulePath2='', MicrosoftDiagnosticServices_ManagedHttpModuleType2=''
6:49:44 PM6:49:42 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace Environment variable 'MicrosoftDiagnosticServices_ManagedHttpModulePath2' or 'MicrosoftDiagnosticServices_ManagedHttpModuleType2' is null, skipping managed dll loading
6:49:42 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace MulticastHttpModule.constructor, success, 90 ms

6:49:42 PM EVENT: Microsoft-ApplicationInsights-RedfieldIISModule Error StatusManager failed to initialize using '%HOME%\LogFiles\ApplicationInsights\status'. System.UnauthorizedAccessException: Access to the path '%HOME%\LogFiles\ApplicationInsights\status' is denied.
   at System.IO.__Error.WinIOError(Int32 errorCode, String maybeFullPath)
   at System.IO.Directory.InternalCreateDirectory(String fullPath, String path, Object dirSecurityObj, Boolean checkHost)
   at System.IO.Directory.InternalCreateDirectoryHelper(String path, Boolean checkHost)
   at Microsoft.ApplicationInsights.Extension.StatusManager..ctor()
6:49:42 PM EVENT: Microsoft-ApplicationInsights-RedfieldIISModule Trace Current assembly 'Microsoft.ApplicationInsights.RedfieldIISModule, Version=2.8.15.20663, Culture=neutral, PublicKeyToken=31bf3856ad364e35' location 'C:\Program Files\WindowsPowerShell\Modules\Az.ApplicationMonitor\content\Runtime\Microsoft.ApplicationInsights.RedfieldIISModule.dll'
6:49:44 PM EVENT: Microsoft-ApplicationInsights-RedfieldIISModule Trace Matched filter '.*'~'STATUSMONITORTE', '.*'~'DemoWebApp'
6:49:44 PM EVENT: Microsoft-ApplicationInsights-RedfieldIISModule Trace Lightup assembly calculated path: 'C:\Program Files\WindowsPowerShell\Modules\Az.ApplicationMonitor\content\Runtime\Microsoft.ApplicationInsights.Redfield.Lightup.dll'
6:49:44 PM EVENT: Microsoft-ApplicationInsights-FrameworkLightup Trace Loaded applicationInsights.config from assembly's resource Microsoft.ApplicationInsights.Redfield.Lightup, Version=2.8.15.20662, Culture=neutral, PublicKeyToken=31bf3856ad364e35/Microsoft.ApplicationInsights.Redfield.Lightup.ApplicationInsights-recommended.config
6:49:48 PM EVENT: Microsoft-ApplicationInsights-FrameworkLightup Trace Successfully attached ApplicationInsights SDK
6:49:49 PM6:49:48 PM EVENT: Microsoft-ApplicationInsights-RedfieldIISModule Trace RedfieldIISModule.LoadLightupAssemblyAndGetLightupHttpModuleClass, success, 3954 ms
6:49:48 PM EVENT: Microsoft-ApplicationInsights-RedfieldIISModule Trace RedfieldIISModule.CreateAndInitializeApplicationInsightsHttpModules(lightupHttpModuleClass), success

6:49:48 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace ManagedHttpModuleHelper, multicastHttpModule.Init() success, 5866 ms
6:49:49 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace Resolved variables to: MicrosoftAppInsights_ManagedHttpModulePath='C:\Program Files\WindowsPowerShell\Modules\Az.ApplicationMonitor\content\Runtime\Microsoft.ApplicationInsights.RedfieldIISModule.dll', MicrosoftAppInsights_ManagedHttpModuleType='Microsoft.ApplicationInsights.RedfieldIISModule.RedfieldIISModule'
6:49:49 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace Resolved variables to: MicrosoftDiagnosticServices_ManagedHttpModulePath2='', MicrosoftDiagnosticServices_ManagedHttpModuleType2=''
6:49:49 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace Environment variable 'MicrosoftDiagnosticServices_ManagedHttpModulePath2' or 'MicrosoftDiagnosticServices_ManagedHttpModuleType2' is null, skipping managed dll loading
6:49:49 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace MulticastHttpModule.constructor, success, 0 ms
6:49:49 PM EVENT: Microsoft-ApplicationInsights-RedfieldIISModule Trace RedfieldIISModule.CreateAndInitializeApplicationInsightsHttpModules(lightupHttpModuleClass), success
6:49:49 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace ManagedHttpModuleHelper, multicastHttpModule.Init() success, 0 ms
6:49:54 PM
6:50:00 PM
6:50:01 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace Resolved variables to: MicrosoftAppInsights_ManagedHttpModulePath='C:\Program Files\WindowsPowerShell\Modules\Az.ApplicationMonitor\content\Runtime\Microsoft.ApplicationInsights.RedfieldIISModule.dll', MicrosoftAppInsights_ManagedHttpModuleType='Microsoft.ApplicationInsights.RedfieldIISModule.RedfieldIISModule'
6:50:01 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace Resolved variables to: MicrosoftDiagnosticServices_ManagedHttpModulePath2='', MicrosoftDiagnosticServices_ManagedHttpModuleType2=''
6:50:01 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace Environment variable 'MicrosoftDiagnosticServices_ManagedHttpModulePath2' or 'MicrosoftDiagnosticServices_ManagedHttpModuleType2' is null, skipping managed dll loading
6:50:01 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace MulticastHttpModule.constructor, success, 0 ms
6:50:01 PM EVENT: Microsoft-ApplicationInsights-RedfieldIISModule Trace RedfieldIISModule.CreateAndInitializeApplicationInsightsHttpModules(lightupHttpModuleClass), success
6:50:01 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace ManagedHttpModuleHelper, multicastHttpModule.Init() success, 0 ms
6:50:01 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace Resolved variables to: MicrosoftAppInsights_ManagedHttpModulePath='C:\Program Files\WindowsPowerShell\Modules\Az.ApplicationMonitor\content\Runtime\Microsoft.ApplicationInsights.RedfieldIISModule.dll', MicrosoftAppInsights_ManagedHttpModuleType='Microsoft.ApplicationInsights.RedfieldIISModule.RedfieldIISModule'
6:50:01 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace Resolved variables to: MicrosoftDiagnosticServices_ManagedHttpModulePath2='', MicrosoftDiagnosticServices_ManagedHttpModuleType2=''
6:50:01 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace Environment variable 'MicrosoftDiagnosticServices_ManagedHttpModulePath2' or 'MicrosoftDiagnosticServices_ManagedHttpModuleType2' is null, skipping managed dll loading
6:50:01 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace MulticastHttpModule.constructor, success, 0 ms
6:50:01 PM EVENT: Microsoft-ApplicationInsights-RedfieldIISModule Trace RedfieldIISModule.CreateAndInitializeApplicationInsightsHttpModules(lightupHttpModuleClass), success
6:50:01 PM EVENT: Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper Trace ManagedHttpModuleHelper, multicastHttpModule.Init() success, 0 ms
6:50:05 PM
6:50:09 PM
6:50:14 PM
6:50:19 PM
6:50:24 PM
Timeout Reached. Stopping...
```

## Next steps

- Review additional troubleshooting steps here: https://docs.microsoft.com/azure/azure-monitor/app/status-monitor-v2-troubleshoot

- Review our [API Reference](status-monitor-v2-overview.md#powershell-api-reference) to find a parameter you might have missed.

- If you need additional help you may contact us [here](https://github.com/Microsoft/ApplicationInsights-Home/issues).
