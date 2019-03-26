# IISConfigurator.POC API Reference

## Disclaimer
This is a prototype application. 
We do not recommend using this on your production environments.

# Enable-ApplicationInsightsMonitoring (v0.1.0-alpha)

**IMPORTANT**: This cmdlet must be run in a PowerShell Session with Administrator permissions and with Elevated Execution Policies. See [here](DetailedInstructions.md#run-powershell-as-administrator-with-elevated-execution-policies) for more information.

## Description

Enable code-less attach monitoring of IIS applications on a target machine.
This will modify the IIS applicationHost.config and set some registry keys.
This will also create an applicationinsights.ikey.config which defines what ikey is used by which application.
IIS will load the RedfieldModule at startup which will inject the Application Insights SDK into applications as those applications start up.

As of v0.1.0-alpha, we don't have a setting to verify that enablement was successful. 
We recommend using [Live Metrics](https://docs.microsoft.com/azure/azure-monitor/app/live-stream) to quickly observe if your application is sending us telemetry.

## Examples

### Example with single instrumentation key
In this example, all applications on the current machine will be assigned a single instrumentation key.

```powershell
PS C:\> Enable-ApplicationInsightsMonitoring -InstrumentationKey xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
```

### Example with instrumentation key map
In this example `MachineFilter` will match the current machine using the `'.*'` wildcard.
`AppFilter` also uses the `'.*'` wildcard to match all web apps on the current machine and assign a default instrumentation key.
`AppFilter='WebAppOne'` will assign this specific app a unique instrumentation key.
`AppFilter='WebAppTwo'` will also assign this specific app a unique instrumentation key.

```powershell
PS C:\> Enable-ApplicationInsightsMonitoring 
	-InstrumentationKeyMap @(@{MachineFilter='.*';AppFilter='.*';InstrumentationKey='xxxxxxxx-xxxx-xxxx-xxxx-xxxxxdefault'},
				@{MachineFilter='.*';AppFilter='WebAppOne';InstrumentationKey='xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx1'},
				@{MachineFilter='.*';AppFilter='WebAppTwo';InstrumentationKey='xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx2'},
				@{MachineFilter='.*';AppFilter='WebAppExclude'})

```
Spaces added for readability only.

## Parameters 

### -instrumentationKey
**Required.** Use this parameter to supply a single iKey for use by all applications on the target machine.

### -instrumentationKeyMap
**Required.** Use this parameter to supply multiple ikeys and a mapping of which apps to use which ikey. Using this, you can create a single installation script for several machines by setting the MachineFilter.

#### Schema
`@(@{MachineFilter='.*';AppFilter='.*';InstrumentationKey='xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'})`

**Required**:
- MachineFilter is a required c# regex of the computer or vm name.
	- ".*" will match all
	- "ComputerName" will match only computers with that exact name.
- AppFilter is a required c# regex of the computer or vm name.
	- ".*" will match all
	- "ApplicationName" will match only IIS applications with that exact name.

**Optional**: 
- InstrumentationKey
	- InstrumentationKey is required to enable monitoring of the applications that match the above two filters.
	- Leave this null if you wish to define rules to exclude monitoring



### -Verbose
**Common Parameter.** Use this switch to output detailed logs.

### -WhatIf 
**Common Parameter.** Use this switch to test and validate your input parameters without actually enabling monitoring.


## Notes

### Instrumentation Key
To get started you must have an instrumentation key. Read [here](https://docs.microsoft.com/azure/azure-monitor/app/create-new-resource#copy-the-instrumentation-key) for more information.
