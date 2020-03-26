# Application Insights SDK Self-Diagnostics

## Introduction
We want to improve supportability across Application Insights SDKs by introducing a unified configuration.

This proposal is to introduce a means to enable temporary logging in production environments. Temporary logging instructions would supercede any logging configuration in the SDK, to enable DevOps to take control.

Permanent changes to logging would belong in the SDK's configuration (config file or code-based initialization), but may use this same schema.

### Challenges

- We cannot depend on Customers being able to run additional executables in production to collect product logs.
    - Azure for example requires additional signing to run executables.
- Customers do not want to re-deploy applications to change a configuration. (Safe Deploy requires a 3-5 day staged delpoyment.) They would like to enable/disable settings in their production environments.

## Proposal
- Provide a tool-less method for collecting logs.
- Define a configuration string that can be expanded in the future for additional scenarios.
- Environment Variables enable customers to configure the SDK without a redeploy.
  - Note that this requires restarting an application because our SDKs are not monitoring these settings for changes.
- [Stretch Goal] Turn on/off diagnostics without re-starting an application.
  - Note that this may require a larger engineering effort for some SDKs.

## Technical Specification

### Control Plane

Each product is free to define unique ways to set a configuration (aka "control plane"). But we are asking all SDKs to support Environment Variables for the sake of consistency.

#### Environment Variable
- `APPLICATIONINSIGHTS_SELF_DIAGNOSTICS`

### Configuration String

- The configuration string will consist of a list of key-value pairs separated by semicolon:
`key1=value1;key2=value2;key3=value3`
- Reserved characters
    - `=` equal sign
    - `;` semi colon
- The configuration string keywords will be case insensitive.
- The configuration string values will be case insensitive except where specified otherwise. (example: file path)
- Keys will not be order dependent and are expected to appear only once.
- The full length will not exceed 2048 characters.
- Every process and SDK that reads the configuration string must apply these settings equally.
- If the configuration string is invalid or cannot be parsed, the SDK will take no action.

### Schema

- `Destination`
    - Values: **File**, **ETW**, **Console**, **None**
    - Specifying a destination will "turn-on" temporary logging. This can override anything in the SDK's normal configuration. The destination will define where logs are sent and the value will define the sub keywords.
    - If this value is not found or is invalid, the SDK will take no action.
- [Optional] `Level`
    - Value: Verbose, Information, Warning, Error
    - Default Value: Verbose
    - Reference: https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.tracing.eventlevel?view=netframework-4.8

The SDK will revert back to the previous configuration if any of the following conditions is met:
- The Environment Variable `APPLICATIONINSIGHTS_SELF_DIAGNOSTICS` is deleted.
- The Environment Variable is set to `Destination=None`.
- The Environment Variable is illegal or cannot be parsed.
    
#### "Destination=File" schema
All SDKs must support File logging to provide a basic and consistent supportability experience.
The following values are optional. If any value cannot be parsed, the default value will be used instead.

- [Optional] `Directory`
    - Value: The full path to a directory that the SDK will have write access. IMPORTANT: this field must be case sensitive
    - Default Value: "%TEMP%"
    - If the provided value directory does not exist, the SDK needs to try to create it.
    - If the provided value is illegal for any reason, the SDK will fall back to the default value "%TEMP%".
- [Optional] `MaxSizeMB`
    - Value: Integer specifying max size in megabytes. A value of '0' will be interpreted as no limit.
    - Default: 0.
    - When a max size is exceeded, the SDK should close the file and start a new file.
    - Note that not all SDKs will be able to support this field due to library constraints. We recommend a size of 20MB which is the maximum attachment size for several email clients.


##### File Name
`ApplicationInsightsLog_{DateTime.UtcNow.ToInvariantString("yyyyMMdd_HHmmss")}_{process.ProcessName}_{process.Id}.log`

- The datetime represents the moment the the file was created.
- Process name is from the process hosting the SDK.
- Process id is from the process hosting the SDK.

This format is proposed because multiple SDKs may have access to the same environment variable. This format will help users identify the program or process writing to this file.
Both the process name and id are required because a single process may host many applications (for example: IIS's process is w3wp, but it can host multiple web applications on unique process ids).
When a file exceeds the max size, the file will be closed and a new file will be created with a new timestamp.

##### File Contents

Files need to include a metadata header at the top to identify where the file came from.
This header helps the customer identify where a file comes from, and how to turn them off.
This header also helps support teams get specific information about an SDK or application needed for troubleshooting.

The file header should include
- sdk name
- sdk version
- how the file was turned on (ex: Environment Variable or other)


##### Full Example
`Destination=File;Directory=C:/Temp;Level=verbose;MaxSizeMB=50`

##### Minimum Valid Example
`Destination=File`
This configuration simply instructs the SDK to write logs to file using all default values.

##### Commitments
- DotNet: Yes
- Java: Yes
- JavaScript: Not Applicable
- Node: ???
- Python: ???
- App Service Extensions: ???

#### "Destination=ETW" schema

**TO BE DEFINED**

The DotNet SDKs have ETW logging always on. Any changes to this would be a breaking change.
Java is specifically interested in supporting ETW.

#### "Destination=Console" schema

**TO BE DEFINED**
