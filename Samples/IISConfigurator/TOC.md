# IISConfigurator.POC

IISConfigurator is a PowerShell Module published to the [PowerShellGallery](https://www.powershellgallery.com/packages/Microsoft.ApplicationInsights.IISConfigurator.POC) and will be the replacement for Status Monitor.

We will release new features as they are available. Should expect a 2 week cadence on releases from now through May 2019.

This guide will be published to docs.microsoft.com when we release our first Stable version.



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

  **Yes**. You have multiple options to download the IISConfigurator. If your computer has internet access, you can onboard to the PowerShell Gallery using `-Proxy` parameters. Alternatively, you can manually download this module and either install it on your machine or use the module directly. We've described all options in our [Detailed Instructions](DetailedInstructions.md).
