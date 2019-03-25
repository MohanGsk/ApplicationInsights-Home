# IISConfigurator.POC Quick Start Instructions

## Disclaimer
This is a prototype application. 
We do not recommend using this on your production environments.

Please review our [Troubleshooting](Troubleshooting.md) guide for an explanation of known issues.

[Detailed instructions](DetailedInstructions.md) provide an explanation of these commands, instructions on how to customize, and how to troubleshoot.

## Quick Start
These are the quick start commands expected to work for most environments. These instructions depend on the PowerShell Gallery to distribute updates. These commands support the PowerShell `-Proxy` parameter.


### Install Prerequisites
Run PowerShell as Administrator
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force
Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force
Set-PSRepository -Name "PSGallery" -InstallationPolicy Trusted
Install-Module -Name PowerShellGet -Force
```	
Exit PowerShell

### Install IISConfigurator
Run PowerShell as Administrator
```powershell	
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force
Install-Module -Name Microsoft.ApplicationInsights.IISConfigurator.POC -AllowPrerelease -AcceptLicense
```	

### Enable Monitoring
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force
Enable-ApplicationInsightsMonitoring -InstrumentationKey xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
```
	
		
##  Quick Start (offline install)
### Manual Download
Manually download the latest version of the Module from: https://www.powershellgallery.com/packages/Microsoft.ApplicationInsights.IISConfigurator.POC 

### Unzip and Install IISConfigurator
```powershell
$pathToNupkg = "C:\Users\t\Desktop\Microsoft.ApplicationInsights.IISConfigurator.POC.0.1.0-alpha.nupkg"
$pathToZip = ([io.path]::ChangeExtension($pathToNupkg, "zip"))
$pathToNupkg | rename-item -newname $pathToZip
$pathInstalledModule = "$Env:ProgramFiles\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc"
Expand-Archive -LiteralPath $pathToZip -DestinationPath $pathInstalledModule
```
### Enable Monitoring
```powershell
Enable-ApplicationInsightsMonitoring -InstrumentationKey xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
```
