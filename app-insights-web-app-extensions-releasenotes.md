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

Please uninstall private site extension since it is no longer supported.

## Release Notes

### 2.8.33
  - .net, .netcore, Java & NodeJS agents and the Windows Extension: support for Sovereign Clouds. Connections strings can be used to send data to sovereign clouds.

### 2.8.31
- asnetcore agent: Fixed issue related to updated Application Insigths SDK's one of the references (see known issues for 2.8.26). If `System.Diagnostics.DiagnosticSource.dll` of incorrect version is already loaded by runtime, the codeless now will not crash the application and will simply back-off. For customers who was affected by that issue it is advised to remove the `System.Diagnostics.DiagnosticSource.dll` from their bin folder OR use older version of the extension by setting "ApplicationInsightsAgent_EXTENSIONVERSION=2.8.24", otherwise - the application monitoring will not be enabled.

### 2.8.26
- aspnetcore agent: Fixed issue related to updated Application Insights SDK - the agent will not try to load `AiHostingStartup` if the ApplicationInsights.dll is already present in bin folder. This resolves issues related to reflection via Assembly\<AiHostingStartup\>.GetTypes().
- Known issues: exception `System.IO.FileLoadException: Could not load file or assembly 'System.Diagnostics.DiagnosticSource, Version=4.0.4.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'` could be thrown if another verison of `DiagnosticSource` dll is loaded. This could happen for example if `System.Diagnostics.DiagnosticSource.dll` is present in the publish folder. As mitigation, use previous version of extension by setting app settings in app services: ApplicationInsightsAgent_EXTENSIONVERSION=2.8.24. Application Insights team is investigating if this scenario can be supported with future versions of ApplicationInsights SDK and agent.

### 2.8.24
- repackaged version of 2.8.21

### 2.8.23
- Added aspnetcore 3.0 codeless monitoring support.
- Updated Asp.Net Core SDK to [2.8.0](https://github.com/microsoft/ApplicationInsights-aspnetcore/releases/tag/2.8.0) for runtimes 2.1, 2.2 and 3.0. Apps targetting .Net Core 2.0 will continue to use 2.1.1 of the SDK.
- Java and NodeJS codeless monitoring on App Services windows private preview bits.

### 2.8.21
- private preview for Java and nodeJS APM agents for AppSvcs Windows.

### 2.8.14
- Update Asp.Net Core SDK version from 2.3.0 to the latest (2.6.1) for Apps targetting .Net Core 2.1, 2.2. Apps targetting .Net Core 2.0 will continue to use 2.1.1 of the SDK.

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
