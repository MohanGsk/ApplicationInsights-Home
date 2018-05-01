# Application Insights: Heartbeat usage in ASP NET Core applications
In this sample we describe how to set up and configure a ASP.NET Core application
such that it makes use of the Heartbeat feature.

## Quick Start Procedure

> This assumes you have the dotnet SDK installed, and the `dotnet` cli program
available on your PATH.

1. Create a folder for your application and enter it in the developer shell.
````
SHELL> mkdir ASPNetCoreSample
SHELL> cd ASPNetCoreSample
SHELL> dotnet new mvc
SHELL> dotnet add .\ASPNetCoreSample.csproj package Microsoft.ApplicationInsights.AspNetCore --version 2.3.0-beta1
````

> Note: The Heartbeat feature is enabled by default as of base SDK 2.5.0 and the
ability to configure the Heartbeat was added in 2.3.0-beta1.

2. Edit the `Program.cs` file located in your ASPNetCoreSample/ASPNetCoreSample
folder. Replace the following code:
````
  public static IWebHost BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
      .UseStartup<Startup>()
      .Build();
````
with this code:
````
  public static IWebHost BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
      .UseStartup<Startup>()
      .UseApplicationInsights()
      .Build();
````

3. Edit the `Startup.cs` file located there also,  and replace the following
code:
````
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddMvc();
    ...
````
with this code:
````
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddMvc();
    
    ApplicationInsightsServiceOptions aiOpts = 
      new ApplicationInsightsServiceOptions();
    aiOpts.EnableHeartbeat = true; // false to disable
    services.AddApplicationInsightsTelemetry(aiOpts);
    ...
````
(also, add `using Microsoft.ApplicationInsights.AspNetCore.Extensions;` to the
top of your file)

> Note: You can set the `EnableHeartbeat` to false here to disable the heartbeat.

4. Configure the Heartbeat feature in code by modifying the 
`IHeartbeatPropertyManager` directly. You can do this when you first obtain the
property manager via the `TelemetryModules.Instance` singleton.
````
  foreach (var md in TelemetryModules.Instance.Modules)
  {
    if (md is IHeartbeatPropertyManager heartbeatPropertyMan)
    {
      heartbeatPropertyMan.HeartbeatInterval = TimeSpan.FromMinutes(5.0);
      heartbeatPropertyMan.ExcludedHeartbeatProperties.Add("osType");
      ...
````


