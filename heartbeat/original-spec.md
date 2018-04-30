# Proposal: Health Heartbeat #

We would like to add a health 'heartbeat' to the Application Insights SDK. We believe that by adding this feature to the App Insights SDK we can enable more complete support scenarios for our customers.

The new information that the SDK will provide to the data stream will be very low cost, and have high value for supporting the use of Application Insights within a deployed service or application.  All of the data associated with the health heartbeat would be stored in the customer's data store, adding to their overall monitoring costs but providing extra value with regards to maintenance and support.

**Please comment on the proposal below, we are happy to hear feedback from the community!**

## Proposed new documentation regarding the Health Heartbeat: ##
*[Current documentation here](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-configuration-with-applicationinsights-config#application-insights-diagnostics-telemetry)*

>## Application Insights Diagnostics Telemetry ##
>
>The `DiagnosticsTelemetryModule` works at the Application Insights tooling level exclusively. The two things that it will emit will be (1) reporting errors in the Application Insights instrumentation code itself, and (2) report a 'health heartbeat' to aid in diagnostic efforts. 
>
>When reporting errors, for example, if the code cannot access performance counters or if an `ITelemetryInitializer` throws an exception. Trace telemetry tracked by this module appears in the Diagnostic Search. Sends diagnostic data to dc.services.vsallin.net.+
>
>The health heartbeat monitoring information is set to send a minimal amount of data to track general health information on your application as it would pertain to the Application Insights tooling itself. It will log information like platform versions, SDK versions, and other such data, and is open to extension should your application have specific health-heartbeat requirements. 
>
>The data is sent at very infrequent intervals and is by default sent once every *n (TBD: determine default n)* seconds. All data would be stored with your application telemetry and can be queried and used for support or other purposes later.
>
>- `Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsTelemetryModule`
>- `Microsoft.ApplicationInsights` NuGet package. If you only install this package, the `ApplicationInsights.config` file is not automatically created
>


## Proposed Enhancements ##

We will extend the functionality within the `DiagnosticsTelemetryModule` to include this health heartbeat functionality. The module have a new set of classes added that will:

- Perform heartbeat actions on a timed schedule
  
  - The user can configure the rate at which these heartbeat payloads are     sent in the `ApplicationInsights.config` file.

- Read a specific set of information on the running application that will be added as properties to the heartbeat message
  
  - This will include adding any extended information that the user has provided that they want appended to the message

- We will provide an interface & registration to the user of the system that will allow the addition of any further telemetry to the heartbeat message
  - This will include pruning out message elements that the user wants excluded
  - Configuration file elements will be added to allow the user to customize the payload of the health heartbeat to their liking.
			
- Set the status of the message to 0 if all elements are successfully read, and no obvious failure conditions are met
  - In the provided interface & registration for extending the health heartbeat for users, failure status can also be affected
		
- Send the message to the App Insights component

  - One metric will be sent to every iKey named in `ApplicationInsights.config`

  - Allow the user to completely remove the functionality as well, by removing it from the `ApplicationInsights.config` file.


## The Message & Payload ##

The message that will deliver this heartbeat will be an Application Insights metric type, where we will set the metric value to 0 for successful health heartbeat delivery, and increment this value by one for each field in the payload that represents a potentially unhealthy status.

The data that will be included in the health heartbeat by default will be the following...


### Fields: ###
| Name | Type | Example Value | Description |
|------|------|---------------|-------------|
|osType|string|"Windows"      | operating system of service/application instrumented|
|location|string|"EastUS"|string representing the location of the Azure region the VM is running in|
|name|string|"VM01"|name of the VM running this instance of your application|
|offer|string|"WIN10"|name of the VM image if it was deployed from the VM image gallery|
|platformFaultDomain|string|"0"|fault domain the VM image is running in|
|platformUpdateDomain|string|"0"|update domain the VM image is running|
|publisher|string|"Microsoft"|publisher of the VM image|
|sku|string|"10.0"|sku of the VM image|
|version|string|"7.3.2"|version of the VM image|
|vmId|string|"59a7f16f-a821-414b-abfe-da0fba8ab6b4"|id of the VM  instance application is running on|
|vmSize|string|"Standard_DS2"|VM size application is running on|
|targetFramework|string|".NET Framework 4.6.2"|The target framework the application was compiled against (to the best of our ability)|
|runtimeFramework|string|".NET Framework 4.6.2"|Runtime framework version down to patch|
|appinsightsSdkVer|string|"The current Application Insights SDK elements present, and their version numbers
---

### Potential Fields (TBD to include these or not): ###
| Name | Type | Example Value | Description |
|------|------|---------------|-------------|
|version|string|"1.0"|The version of the health heartbeat packet 'schema'|
|time|time|2017-03-22T12:34:59.12345Z|Timestamp of the heartbeat event in UTC|
|seqnum|int64|1004|index for a specific message enumerating the order of health heartbeat events sent|
|unhealthyItem|string[]|"userField01","userfield02"|name of any user fields that are reporting 'unhealthy' status|
|processId|int|7580|The process id of the application as reported by the underlying OS|
---


## Failure Status Evaluations ##

Failure status will be set based on the following criteria by default:
	
- A user extension is present that will affect the failure status


## Interfaces Available for Extension ##

`IHealthHeartbeatProperty` will be implemented by consumers of the Telemetry client and named in the `ApplicationInsights.config` file.

- Users can supply a set of name-value pairs that will be appended to the payload of the health heartbeat message
- Users can indicate that any of their properties represent unhealthy conditions have been met in the payload they are adding to the message

Note, the `DiagnosticsTelemetryModule` will ensure no conflicting property names are appended into the payload. Upon encountering such collisions, the name of the module adding the property will be prepended to the property name. If collisions are still present after performing this, an incremented number will be added as the suffix of the property name until there are no collisions.

The class that implements the `IHealthHeartbeatProperty` will undergo a registration in the `DiagnosticTelemetryModule` and will be added to the payload upon successful registration. The user can also register their class during runtime if they wish, but will miss out on any health heartbeat metrics sent prior to calling `RegisterHeartbeatPayload`.

Configuration File Elements Added

We will add a section to the `ApplicationInsights.config` file that will allow the users to do the following:
- Change the cadence at which the health heartbeat message is sent
- Remove specific payload fields that are sent in the health heartbeat message
- Name the custom extensions that the user has created to alter/add payload information with, and that must be loaded at runtime

**Updates:**

- Added processId to the list of potential fields in the heartbeat payload requested by @cijothomas 