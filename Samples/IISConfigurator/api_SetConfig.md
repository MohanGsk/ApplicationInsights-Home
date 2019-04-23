# IISConfigurator.POC API Reference

## Disclaimer
This is a prototype application. 
We do not recommend using this on your production environments.

# Set-ApplicationInsightsMonitoringConfig (v0.2.0-alpha)

**IMPORTANT**: This cmdlet must be run in a PowerShell Session with Administrator permissions.

## Description

Set the config file for IISConfigurator without doing a full reinstalling. 
You will need to restart IIS for your changes to take effect.

## Examples

### Example with single instrumentation key
In this example, all applications on the current machine will be assigned a single instrumentation key.

```powershell
PS C:\> Set-ApplicationInsightsMonitoringConfig -InstrumentationKey xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
```

### Example with instrumentation key map
In this example, 
- `MachineFilter` will match the current machine using the `'.*'` wildcard.
- `AppFilter='WebAppExclude'` provides a `null` InstrumentationKey. This app will not be instrumented.
- `AppFilter='WebAppOne'` will assign this specific app a unique instrumentation key.
- `AppFilter='WebAppTwo'` will also assign this specific app a unique instrumentation key.
- Lastly, `AppFilter` also uses the `'.*'` wildcard to match all other web apps not matched by the earlier rules and assigns a default instrumentation key.
- Spaces added for readability only.

```powershell
PS C:\> Set-ApplicationInsightsMonitoringConfig -InstrumentationKeyMap 
	@(@{MachineFilter='.*';AppFilter='WebAppExclude'},
	  @{MachineFilter='.*';AppFilter='WebAppOne';InstrumentationKey='xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx1'},
	  @{MachineFilter='.*';AppFilter='WebAppTwo';InstrumentationKey='xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx2'},
	  @{MachineFilter='.*';AppFilter='.*';InstrumentationKey='xxxxxxxx-xxxx-xxxx-xxxx-xxxxxdefault'})

```


## Parameters 


### -InstrumentationKey
**Required.** Use this parameter to supply a single iKey for use by all applications on the target machine.

### -InstrumentationKeyMap
**Required.** Use this parameter to supply multiple ikeys and a mapping of which apps to use which ikey. Using this, you can create a single installation script for several machines by setting the MachineFilter. 

**IMPORTANT:** Applications will match aganist rules in the order that they are provided. As such you should specify the most specific rules first and the most generic rules last.

#### Schema
`@(@{MachineFilter='.*';AppFilter='.*';InstrumentationKey='xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'})`

**Required**:
- MachineFilter is a required c# regex of the computer or vm name.
	- '.*' will match all
	- 'ComputerName' will match only computers with that exact name.
- AppFilter is a required c# regex of the computer or vm name.
	- '.*' will match all
	- 'ApplicationName' will match only IIS applications with that exact name.

**Optional**: 
- InstrumentationKey
	- InstrumentationKey is required to enable monitoring of the applications that match the above two filters.
	- Leave this null if you wish to define rules to exclude monitoring


### -Verbose
**Common Parameter.** Use this switch to output detailed logs.


## Output

No Output by default.

#### Example verbose output from setting the config file via -InstrumentationKey

```
VERBOSE: Operation: InstallWithIkey
VERBOSE: InstrumentationKeyMap parsed:
Filters:
0)InstrumentationKey: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx AppFilter: .* MachineFilter: .*
VERBOSE: set config file
VERBOSE: Config File Path:
C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\applicationInsights.ikey.config
```

#### Example verbose output from setting the config file via -InstrumentationKeyMap

```
VERBOSE: Operation: InstallWithIkeyMap
VERBOSE: InstrumentationKeyMap parsed:
Filters:
0)InstrumentationKey: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx AppFilter: .* MachineFilter: .*
1)InstrumentationKey: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx AppFilter: two MachineFilter: two
2)InstrumentationKey:  AppFilter: two MachineFilter: two
VERBOSE: set config file
VERBOSE: Config File Path:
C:\Program Files\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc\content\applicationInsights.ikey.config
```




