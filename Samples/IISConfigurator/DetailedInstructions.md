# IISConfigurator.POC Detailed Instructions

## Disclaimer
This is a prototype application. 
We do not recommend using this on your production environments.

Please review our [Troubleshooting](Troubleshooting.md) guide for an explanation of known issues.

To quickly get started without detailed instructions, please review our [Quick Start Guide](QuickStart.md).



## Run PowerShell as Administrator with Elevated Execution Policies

Throughout this guide we will refer to running **PowerShell as Administrator**. 
PowerShell will need Administrator level permissions to make changes to your computer.

Throughout this guide we will also refer to **Elevated Execution Policies**.
By default, running PowerShell scripts will be disabled.
We recommend allowing RemoteSigned scripts for the Current Scope only.

For more information review: [Set-ExecutionPolicy](
https://docs.microsoft.com/powershell/module/microsoft.powershell.security/set-executionpolicy?view=powershell-6
)

- Cmd: `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force`

**Example Errors:**

```
Install-Module : The 'Install-Module' command was found in the module 'PowerShellGet', but the module could not be
loaded. For more information, run 'Import-Module PowerShellGet'.
	
Import-Module : File C:\Program Files\WindowsPowerShell\Modules\PackageManagement\1.3.1\PackageManagement.psm1 cannot
be loaded because running scripts is disabled on this system. For more information, see about_Execution_Policies at
https:/go.microsoft.com/fwlink/?LinkID=135170.
```


## Prerequisites for PowerShell

To audit your current version PowerShell run cmd: `$PSVersionTable`
These instructions were written and tested on a Windows 10 machine with these versions:

```
Name                           Value
----                           -----
PSVersion                      5.1.17763.316
PSEdition                      Desktop
PSCompatibleVersions           {1.0, 2.0, 3.0, 4.0...}
BuildVersion                   10.0.17763.316
CLRVersion                     4.0.30319.42000
WSManStackVersion              3.0
PSRemotingProtocolVersion      2.3
SerializationVersion           1.1.0.1
```

## Prerequisites for PowerShell Gallery

**NOTE**: Support for PowerShell Gallery is included on Windows 10, Windows Server 2016, and PowerShell 6.
For older versions, please review this document: [Installing PowerShellGet](https://docs.microsoft.com/powershell/gallery/installing-psget)


1. Run PowerShell as Administrator
2. Nuget Package Provider 
    - Description: This is required to interact with NuGet-based repositories such as PowerShellGallery
    - Reference: [Install-PackageProvider](https://docs.microsoft.com/powershell/module/packagemanagement/install-packageprovider?view=powershell-6)
    - Cmd: `Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force`
	
	Will receive this prompt if not setup:
		
		NuGet provider is required to continue
		PowerShellGet requires NuGet provider version '2.8.5.201' or newer to interact with NuGet-based repositories. The NuGet
		 provider must be available in 'C:\Program Files\PackageManagement\ProviderAssemblies' or
		'C:\Users\t\AppData\Local\PackageManagement\ProviderAssemblies'. You can also install the NuGet provider by running
		'Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force'. Do you want PowerShellGet to install and import
		 the NuGet provider now?
		[Y] Yes  [N] No  [S] Suspend  [?] Help (default is "Y"):
	
3. Trusted Repositories
    - Description: By default, PowerShellGallery is an untrusted repository.
    - Reference: [Set-PSRepository](https://docs.microsoft.com/powershell/module/powershellget/set-psrepository?view=powershell-6)
    - Cmd: `Set-PSRepository -Name "PSGallery" -InstallationPolicy Trusted -Force`

	Will receive this prompt if not setup:

		Untrusted repository
		You are installing the modules from an untrusted repository. If you trust this repository, change its
		InstallationPolicy value by running the Set-PSRepository cmdlet. Are you sure you want to install the modules from
		'PSGallery'?
		[Y] Yes  [A] Yes to All  [N] No  [L] No to All  [S] Suspend  [?] Help (default is "N"):

	- Can confirm this change and audit all PSRepositories by running the cmd: `Get-PSRepository`

4. PowerShellGet version 
    - Description: This is the tooling used to get other modules from PowerShell Gallery. v1.0.0.1 ships with Windows 10 and Windows Server. Min version required is v1.6.0. To audit which version is installed run cmd: `Get-Command`
    - Reference: [Installing PowerShellGet](https://docs.microsoft.com/powershell/gallery/installing-psget)
    - Cmd: `Install-Module -Name PowerShellGet -Force`

	Will receive this error if not using newest version of PowerShellGet:
	
		Install-Module : A parameter cannot be found that matches parameter name 'AllowPrerelease'.
		At line:1 char:20
		Install-Module abc -AllowPrerelease
						   ~~~~~~~~~~~~~~~~
			CategoryInfo          : InvalidArgument: (:) [Install-Module], ParameterBindingException
			FullyQualifiedErrorId : NamedParameterNotFound,Install-Module
	
5. Restart PowerShell. Any new Powershell sessions will have the latest PowerShellGet loaded. Unable to load new version in the current session.

## Download & Install IISConfigurator via PowerShell Gallery

1. Follow all prerequisites.
2. Run PowerShell as Administrator with Elevated Execution Policies
3. [Install-Module](https://docs.microsoft.com/powershell/module/powershellget/install-module?view=powershell-6)
	- Cmd: `Install-Module -Name Microsoft.ApplicationInsights.IISConfigurator.POC -AllowPrerelease -AcceptLicense`
	- Optional Parameters:
		- `-Proxy`
		- `-AcceptLicense` This will skip the "Accept License" prompt
		- `-Force` This will ignore the "Untrusted Repository" warning

## Download & Install IISConfigurator manually (offline option)

### Manually download the latest nupkg

1. Navigate to: https://www.powershellgallery.com/packages/Microsoft.ApplicationInsights.IISConfigurator.POC
2. Select the latest version from the version history
3. At the top under "Installation Options" select "Manual Download"

### Option 1 - Install into PowerShell Modules Directory
Install the manually downloaded PowerShell Module to a PowerShell directory so it can be discoverable by PowerShell sessions.
For more information see: [Installing a PowerShell Module](https://docs.microsoft.com/powershell/developer/module/installing-a-powershell-module)

Unzip nupkg as zip using Expand-Archive (v1.0.1.0)
The base version of Microsoft.PowerShell.Archive (v1.0.1.0) will not unzip .nupkg files. You must first rename the file with the ".zip" extension.

```
$pathToNupkg = "C:\microsoft.applicationinsights.iisconfigurator.poc.0.1.0-alpha2.nupkg"
$pathToZip = ([io.path]::ChangeExtension($pathToNupkg, "zip"))
$pathToNupkg | rename-item -newname $pathToZip
$pathInstalledModule = "$Env:ProgramFiles\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc"
Expand-Archive -LiteralPath $pathToZip -DestinationPath $pathInstalledModule
```

Unzip nupkg using Expand-Archive (v1.1.0.0).
**REQUIRES** Microsft.PowerShell.Archive min version: 1.1.0.0 https://www.powershellgallery.com/packages/Microsoft.PowerShell.Archive/1.1.0.0

```
$pathToNupkg = "C:\microsoft.applicationinsights.iisconfigurator.poc.0.1.0-alpha2.nupkg"
$pathInstalledModule = "$Env:ProgramFiles\WindowsPowerShell\Modules\microsoft.applicationinsights.iisconfigurator.poc"
Expand-Archive -LiteralPath $pathToNupkg -DestinationPath $pathInstalledModule
```

### Option 2 - Unzip and import manually

**IMPORTART**: Installation will install DLLs via relative paths. Store the contents of this package into your intended runtime directory and confirm that access permissions allow read but not write.

- Change the extension to ".zip" and extract contents of package into your intended installation directory.
- Find the file path to "microsoft.applicationinsights.iisconfigurator.poc.psd1"
- Run PowerShell as Administrator with Elevated Execution Policies 
- Load the module via cmd: `Import-Module microsoft.applicationinsights.iisconfigurator.poc.psd1`
	
	


## Enable Application Insights Monitoring 

Cmd: `Enable-ApplicationInsightsMonitoring`

Please review our [API Reference](api_EnableMonitoring.md) for a detailed description of how to use this cmdlet. 
