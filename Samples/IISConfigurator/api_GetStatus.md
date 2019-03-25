# IISConfigurator.POC - API Reference

## Disclaimer
This is a prototype application. 
We do not recommend using this on your production environments.

# Get-ApplicationInsightsMonitoringStatus (v0.1.0-alpha)

(documentation in progress)


## Description

This cmdlet is provided for trobleshooting the PowerShell Module in use.
This will report version information and key files required for monitoring.
Additional parameters provide extra reports on the current status of monitoring.


## Examples


### Example: Basic information
```
PS C:\> Get-ApplicationInsightsMonitoringStatus


PowerShell Module version:
0.1.0-alpha

Application Insights SDK version:
2.9.0.0

Executing PowerShell Module Assembly:
Microsoft.ApplicationInsights.Redfield.Configurator.PowerShell, Version=2.8.13.5662, Culture=neutral, PublicKeyToken=31bf3856ad364e35

PowerShell Module Directory:
C:\Program Files\WindowsPowerShell\Modules\Microsoft.ApplicationInsights.IISConfigurator.POC\content\PowerShell

Runtime Paths:
ParentDirectory: C:\Program Files\WindowsPowerShell\Modules\Microsoft.ApplicationInsights.IISConfigurator.POC\content Exists: True
ConfigurationPath: C:\Program Files\WindowsPowerShell\Modules\Microsoft.ApplicationInsights.IISConfigurator.POC\content\applicationInsights.ikey.config Exists: False
ManagedHttpModuleHelperPath: C:\Program Files\WindowsPowerShell\Modules\Microsoft.ApplicationInsights.IISConfigurator.POC\content\Runtime\Microsoft.AppInsights.IIS.ManagedHttpModuleHelper.dll Exists: True
RedfieldIISModulePath: C:\Program Files\WindowsPowerShell\Modules\Microsoft.ApplicationInsights.IISConfigurator.POC\content\Runtime\Microsoft.ApplicationInsights.RedfieldIISModule.dll Exists: True
InstrumentationEngine86Path: C:\Program Files\WindowsPowerShell\Modules\Microsoft.ApplicationInsights.IISConfigurator.POC\content\Instrumentation32\MicrosoftInstrumentationEngine_x86.dll Exists: True
InstrumentationEngine64Path: C:\Program Files\WindowsPowerShell\Modules\Microsoft.ApplicationInsights.IISConfigurator.POC\content\Instrumentation64\MicrosoftInstrumentationEngine_x64.dll Exists: True
InstrumentationEngineExtensionHost86Path: C:\Program Files\WindowsPowerShell\Modules\Microsoft.ApplicationInsights.IISConfigurator.POC\content\Instrumentation32\Microsoft.ApplicationInsights.ExtensionsHost_x86.dll Exists: True
InstrumentationEngineExtensionHost64Path: C:\Program Files\WindowsPowerShell\Modules\Microsoft.ApplicationInsights.IISConfigurator.POC\content\Instrumentation64\Microsoft.ApplicationInsights.ExtensionsHost_x64.dll Exists: True
InstrumentationEngineExtensionConfig86Path: C:\Program Files\WindowsPowerShell\Modules\Microsoft.ApplicationInsights.IISConfigurator.POC\content\Instrumentation32\Microsoft.InstrumentationEngine.Extensions.config Exists: True
InstrumentationEngineExtensionConfig64Path: C:\Program Files\WindowsPowerShell\Modules\Microsoft.ApplicationInsights.IISConfigurator.POC\content\Instrumentation64\Microsoft.InstrumentationEngine.Extensions.config Exists: True
ApplicationInsightsSdkPath: C:\Program Files\WindowsPowerShell\Modules\Microsoft.ApplicationInsights.IISConfigurator.POC\content\Runtime\Microsoft.ApplicationInsights.dll Exists: True
```

### Example: Runtime Status
```
PS C:\> Get-ApplicationInsightsMonitoringStatus -InspectProcess

iisreset.exe /status
Status for IIS Admin Service ( IISADMIN ) : Running
Status for Windows Process Activation Service ( WAS ) : Running
Status for Net.Msmq Listener Adapter ( NetMsmqActivator ) : Running
Status for Net.Pipe Listener Adapter ( NetPipeActivator ) : Running
Status for Net.Tcp Listener Adapter ( NetTcpActivator ) : Running
Status for World Wide Web Publishing Service ( W3SVC ) : Running

handle64.exe -accepteula -p w3wp
BF0: File  (R-D)   C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\Runtime\Microsoft.AI.ServerTelemetryChannel.dll
C58: File  (R-D)   C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\Runtime\Microsoft.AI.AzureAppServices.dll
C68: File  (R-D)   C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\Runtime\Microsoft.AI.DependencyCollector.dll
C78: File  (R-D)   C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\Runtime\Microsoft.AI.WindowsServer.dll
C98: File  (R-D)   C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\Runtime\Microsoft.AI.Web.dll
CBC: File  (R-D)   C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\Runtime\Microsoft.AI.PerfCounterCollector.dll
DB0: File  (R-D)   C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\Runtime\Microsoft.AI.Agent.Intercept.dll
B98: File  (R-D)   C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\Runtime\Microsoft.ApplicationInsights.RedfieldIISModule.dll
BB4: File  (R-D)   C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\Runtime\Microsoft.ApplicationInsights.RedfieldIISModule.Contracts.dll
BCC: File  (R-D)   C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\Runtime\Microsoft.ApplicationInsights.Redfield.Lightup.dll
BE0: File  (R-D)   C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\Runtime\Microsoft.ApplicationInsights.dll

listdlls64.exe -accepteula w3wp
0x0000000019ac0000  0x127000  C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\Instrumentation64\MicrosoftInstrumentationEngine_x64.dll
0x00000000198b0000  0x4f000   C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\Instrumentation64\Microsoft.ApplicationInsights.ExtensionsHost_x64.dll
0x000000000c460000  0xb2000   C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\Instrumentation64\Microsoft.ApplicationInsights.Extensions.Base_x64.dll
0x000000000ad60000  0x108000  C:\Windows\TEMP\2.4.0.0.Microsoft.ApplicationInsights.Extensions.Intercept_x64.dll
```

## Parameters 

### (No Params)

By **default**, this cmdlet will report version numbers and paths of DLLs required for monitoring.

### -InspectProcess

**Optional**. This cmdlet will use external exes to report if IIS is running and also if required DLLs have been loaded into the IIS runtime.

## Inputs

## Outputs

## Notes
