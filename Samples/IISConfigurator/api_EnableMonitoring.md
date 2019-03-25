# IISConfigurator.POC API Reference (documentation in progress)

## Disclaimer
This is a prototype application. 
We do not recommend using this on your production environments.

# Enable-ApplicationInsightsMonitoring (v0.1.0-alpha)

**IMPORTANT**: This cmdlet must be run in a PowerShell Session with Administrator permissions and with Elevated Execution Policies. [Description](DetailedInstructions.md#run-powershell-as-administrator-with-elevated-execution-policies).

## Description

Enable code-less attach monitoring of IIS applications on a target machine.
This will modify the IIS applicationHost.config and set some registry keys.
This will also create an applicationinsights.ikey.config which defines what ikey is used by which application.
IIS will load the RedfieldModule at startup which will inject the Application Insights SDK into applications as those applications start up.

## Examples

## Parameters 

### -instrumentationKey
**Required.** Use this parameter to supply a single iKey for use by all applications on the target machine.

### -instrumentationKeyMap
**Required.** Use this parameter to supply multiple ikeys and a mapping of which apps to use which ikey.

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

## Inputs

## Outputs

## Notes



