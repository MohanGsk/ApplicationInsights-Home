# Release Notes for Azure Web Apps extension for Application Insights.

The following contains the releases notes for Azure Web Apps Extension for runtime instrumentation with Application Insights.
This is only applicable for pre-installed extensions.

[Learn](https://docs.microsoft.com/azure/azure-monitor/app/azure-web-apps)
more about Azure Web App Extension for Application Insights

## FAQS

*How to find which version of extension I am currently on:*

Please navigate to: https://websitename.scm.azurewebsites.net/applicationinsights to find the exact version of the pre-installed
extension.

*What if I am using private extensions*?
Release notes for private extension can be found [here](https://www.nuget.org/packages/Microsoft.ApplicationInsights.Azurewebsites)

## Release Notes

### 2.8.14
- Update Asp.Net  Core SDK version from 2.3.0 to the latest (2.6.1) for Apps targetting .Net Core 2.1, 2.2. Apps targetting .Net Core 2.0 will continue to use 2.1.1 of the SDK.

### 2.8.12

- Support for Asp.Net Core 2.2 Apps.
- Fixed a bug in Asp.Net Core extension causing injection of SDK even when application is already instrumented with SDK. For 2.1 and 2.2 apps, mere presence of ApplicationInsights.dll in application folder will now cause extension to backoff.
For 2.0 apps, extension backs off only if ApplicationInsights is enabled with .UseApplicationInsights() call.

- Permanent fix for incomplete HTML Response for Asp.Net Core Apps. This fix is now extended to work for .Net Core 2.2 Apps.

- Added support to turn off Javascript injection for Asp.Net Core Apps. (APPINSIGHTS_JAVASCRIPT_ENABLED=false appsetting)
For asp.net core, the JavaScript Injection is "Opt-Out" mode i.e unless explicitly turned off, this will be enabled. (This is done to retain current behavior)

- Fix Asp.Net Core extension bug which caused injection even if ikey was not present.
- Fix a bug in SDK version prefix logic which caused incorrect SDK version in telemetry.

- Added SDK version prefix for Asp.Net Core apps to identify how telemetry was collected.
- Fixed SCM- ApplicationInsights page to correctly show the version of pre-installed extension version.

### 2.8.10
- Fix for incomplete HTML response for Asp.Net Core apps.
