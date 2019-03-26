# IISConfigurator.POC

IISConfigurator is the replacement for [Status Monitor](https://docs.microsoft.com/azure/azure-monitor/app/monitor-performance-live-website-now). This provides code-less instrumentation of .NET web applications hosted on-prem. Once your application is instrumented, your telemetry will be sent to the Azure Portal where you can [monitor](https://docs.microsoft.com/azure/azure-monitor/app/app-insights-overview) your application.


IISConfigurator is a PowerShell Module published to the [PowerShellGallery](https://www.powershellgallery.com/packages/Microsoft.ApplicationInsights.IISConfigurator.POC). We will release new features as they are available. Customers should expect a 2 week release cadence as we add additional features. We expect our first Stable release sometime in May 2019.

NOTE: This guide will be published to docs.microsoft.com when we release our first Stable version.

## PowerShell Gallery

- https://www.powershellgallery.com/packages/Microsoft.ApplicationInsights.IISConfigurator.POC

## Disclaimer
This is a prototype application. 
We do not recommend using this on your production environments.


## Table of Contents

### Instructions
- Review our [Quick Start Instructions](QuickStart.md) to get started now with concise code samples.
- Review our [Detailed Instructions](DetailedInstructions.md) for a deep dive on how to get started.

### PowerShell API Reference
- [Enable-ApplicationInsightsMonitoring](api_EnableMonitoring.md)
- [Disable-ApplicationInsightsMonitoring](api_DisableMonitoring.md)
- [Get-ApplicationInsightsMonitoringStatus](api_GetStatus.md)

### Troubleshooting
- [Troubleshooting](Troubleshooting.md)
- [Known Issues](Troubleshooting.md#known-issues)


## FAQ

- Does IISConfigurator support proxy installations?

  **Yes**. You have multiple options to download the IISConfigurator. If your computer has internet access, you can onboard to the PowerShell Gallery using `-Proxy` parameters. Alternatively, you can manually download this module and either install it on your machine or use the module directly. Each of these options are described in our [Detailed Instructions](DetailedInstructions.md).
