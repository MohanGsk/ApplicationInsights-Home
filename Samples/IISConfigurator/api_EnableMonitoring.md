# IISConfigurator.POC - API Reference

## Disclaimer
This is a prototype application. 
We do not recommend using this on your production environments.

# Enable-ApplicationInsightsMonitoring (v0.1.0-alpha)

(documentation in progress)

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

Schema: `@(@{MachineFilter='.*';AppFilter='.*';InstrumentationKey='xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'})`

### -Verbose
**Common Parameter.** Use this switch to output detailed logs.

### -WhatIf 
**Common Parameter.** Use this switch to test and validate your input parameters without actually enabling monitoring.

## Inputs

## Outputs

## Notes
