# Application Insights Heartbeat #

We have added a new feature called 'heartbeat' to the Application Insights SDK.
By adding this feature to the App Insights SDK we can enable more complete 
support scenarios for our customers while enhancing the customizations available 
for our customers.

All of the data associated with the health heartbeat would be stored in the
customer's data store, adding to their overall monitoring costs but providing
extra value with regards to maintenance and support. The new information that
the SDK will provide to the data stream is very low cost by default.

> See here for a description of the various [modules already in use within 
Application Insights], including the Heartbeat Provider

## Contents
1. [Specifications](#specifications)
2. [The Message Payload](#the-message-payload)
3. [Fields Included in Message Payloads](#fields-included-from-sdk-extensions)
4. [Interface to Extend the Heartbeat](#interface-to-extend-heartbeat)
5. [Configuration Via the ApplicationInsights.config File](#configuration-via-applicationinsights.config-file)


## Specifications

We have extended the functionality within the `DiagnosticsTelemetryModule` to
include the heartbeat. The module has a new set of classes that will:

- Send heartbeats at specified intervals (uses a default value for the interval
if not changed).
- Read a specific set of information on the running application that will be
added as properties to the heartbeat.
- Allows the user to configure the heartbeat to suite their needs, including:
  - disabling the heartbeat altogether
  - pruning out elements that the user wants excluded
  - adding elements to the heartbeat properties as they require
  - set a flag in the heartbeat message to denote an unhealthy state
- One metric will be sent to every iKey named in `ApplicationInsights.config`

## The Message Payload
The message that will deliver this heartbeat will be an Application Insights
`MetricTelemetry` type, where we will set the metric value to 0 to denote a 
healthy state, or increment this value by one for each field in the payload
that represents a potentially unhealthy status.

The data that will be included in the Heartbeat by default will be the following:
### Fields:
| Name                   | Type   | Example Value        | Description      |
|------------------------|--------|----------------------|------------------|
| osType                 | string | Windows"             | Operating system of service/application instrumented|
| baseSdkTargetFramework | string | .NET Framework 4.6.2 | The target framework the application was compiled against (to the best of our ability) |
| runtimeFramework       | string | 4.0.30319.36366      | Runtime framework version down to patch (as reported by the underlying .NET SDK) |
| processSessionId       | GUID   | e6f1eaea-486c-4f3b-b20e-8e107d531e43 | Unique GUID for each time the SDK is initialized. User can query this to discover when the app has been restarted. |
---

## Fields Included from SDK extensions
We have added some further extensions to the Heartbeat already, and the fields
that they add to the Heartbeat are described below.

### AppServicesHeartbeatTelemetryModule
This module will add some key environment variable values to the Heartbeat that
describe an Azure App Service that has been instrumented with the Application 
Insights SDK.

| Name            | Type   | Example Value        | Description      |
|-----------------|--------|----------------------|------------------|
| appSrv_SiteName | string | yourapp              | Name of the App Services site. (WEBSITE_SITE_NAME) |
| appSrv_wsStamp  | string | waws-prod-sn1-0      | Stamp that the app service application is running on. (WEBSITE_HOME_STAMPNAME) |
| appSrv_wsHost   | string | dek-app2infra-dev.azurewebsites.net | Web host name of host running the app service. (WEBSITE_HOSTNAME) |
| appSrv_wsOwner  | string | abcdef00-1234-5678-0000-001122334455+yourresourcegroup-SouthCentralUSwebspace | Combination of subscription ID, resource group name, and datacenter location. (WEBSITE_OWNER_NAME) |
| appSrv_SlotName | string | production | The slot that the app service application is running in, only available on Azure Functions. (WEBSITE_SLOT_NAME) |
---

### AzureComputeMetadataHeartbeatPropertyProvider
For applications that are running on Azure VMs, and that have access to the 
[Azure Instance Metadata Service] we capture values from that service and add
them to the heartbeat properties.

> Note: All descriptions of the fields below can be found on the Azure Instance
Metadata Service's [Instance metadata data categories section].

| Name             | Type   | Example Value        | 
|------------------|--------|----------------------|
| azInst_location  | string | westus               |
| azInst_name      | string | avset2               |
| azInst_offer     | string | UbuntuServer         |
| azInst_osType    | string | Linux                |
| azInst_placementGroupId | string | "" |
| azInst_platformFaultDomain | int | 1 |
| azInst_platformUpdateDomain | int | 1 |
| azInst_publisher | string | Canonical |
| azInst_resourceGroupName | string | yourresgrp |
| azInst_sku       | string | 16.04-LTS |
| azInst_subscriptionId | GUID  | abcdef00-1234-5678-0000-001122334455 |
| azInst_tags      | string | "" |
| azInst_version   | string | 16.04.201708030 |
| azInst_vmId      | string | 13f56399-bd52-4150-9748-7190aae1ff21 |
| azInst_vmSize    | string | Standard_D1 |
---

## Interface to extend Heartbeat

`IHeartbeatPropertyManager` holds all of the extensibility endpoints for the
feature, and can be obtained via the singleton `TelemetryModules.Instance` as 
follows:

````
var telemetryModules = TelemetryModules.Instance;

foreach (var module in telemetryModules.Modules)
{
    if (module is IHeartbeatPropertyManager hbeatManager)
    {
      // hbeatManager contains all extension points for Heartbeat feature...
    }
}
````
Note, the `IHeartbeatPropertyManager` will ensure no conflicting property names
are appended into the payload. It will also dissallow any of the default
heartbeat properties (defined in the base SDK only) from being modified by the
user.

## Configuration via ApplicationInsights.config File

Users can also manipulate the configuration of the Heartbeat feature via the
`ApplicationInsights.config` file. Using the exposed Properties in the
`IHeartbeatPropertyManager` the user can change the defaults by using the
following in the config file:

````
<TelemetryModules>
  ...
  <Add Type="Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsTelemetryModule, Microsoft.ApplicationInsights">
    <HeartbeatInterval>00:05:00</HeartbeatInterval> <!-- change the interval from the default to every 5 minutes -->
    <IsHeartbeatEnabled>true</IsHeartbeatEnabled> <!-- disable the heartbeat by setting this to false -->
    <ExcludedHeartbeatPropertyProviders /> <!-- disable entire property providers. Today this only includes a single provider called 'Base' -->
    <ExcludedHeartbeatProperties></ExcludedHeartbeatProperties> <!-- add default properties to exclude here -->
  </Add>
  ...
</TelemetryModules>
````


[modules already in use within Application Insights]: (https://docs.microsoft.com/en-us/azure/application-insights/app-insights-configuration-with-applicationinsights-config#application-insights-diagnostics-telemetry)
[Azure Instance Metadata Service]: (https://docs.microsoft.com/en-us/azure/virtual-machines/windows/instance-metadata-service) 
[Instance metadata data categories section]: (https://docs.microsoft.com/en-us/azure/virtual-machines/windows/instance-metadata-service#instance-metadata-data-categories)