# Extending Heartbeat Properties

As of Application Insights .NET SDK 2.5.0 we added a feature called 
'Heartbeat'. The idea behind the feature is to send a 'pulse' to the customer's
data store at set intervals, containing information that can be used to help
diagnose issues.

One of the more compelling scenarios that this feature provides for customers
is the ability to add properties to the Heartbeat, along with some notion of
health status as well. 

In this sample, we show the user how to add properties to their heartbeat in
a .NET Core Console application.

> Note: This sample assumes you have the `dotnet cli` installed and available
in your PATH.


## Quick steps

1. Create your new application from the command prompt using `dotnet cli`:
  
````
  SHELL> dotnet new console --name NetCoreHeartbeatConsole
  SHELL> cd NetCoreHeartbeatConsole
  SHELL> dotnet add .\NetCoreHeartbeatConsole.csproj package Microsoft.ApplicationInsights --version 2.6.0-beta4
````

2. Build and run your application to ensure it works as expected (a simple 
Hello World app is what you should have).

````
  SHELL> dotnet build .\NetCoreHeartbeatConsole.csproj
  SHELL> dotnet .\bin\Debug\netcoreapp2.0\NetCoreHeartbeatConsole.dll
  Hello World!
````

3. 