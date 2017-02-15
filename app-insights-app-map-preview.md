[multiple-server-nodes]: ./media/app-insights-app-map/multiple-server-nodes.png
[new-exception-blade]: ./media/app-insights-app-map/new-exception-blade.png
[error-pane-flow]: ./media/app-insights-app-map/error-pane-flow.png
[api-node-selected]: ./media/app-insights-app-map/api-node-selected.png
[operation-logs]: ./media/app-insights-app-map/operation-logs.png
[pin-to-dashboard]: ./media/app-insights-app-map/pin-to-dashboard.png
[resource-group-setup]: ./media/app-insights-app-map/resource-group-setup.png
[set-threshold]: ./media/app-insights-app-map/set-threshold.png
[node-group-labels]: ./media/app-insights-app-map/multiple-server-nodes.png
[installing-beta-sdk]: ./media/app-insights-app-map/installing-beta-sdk.png
[exceptions-and-logs]: ./media/app-insights-app-map/exceptions-and-logs.png
[open-app-map]: ./media/app-insights-app-map/open-app-map.png



# Application Insights Application Map Preview

This page constains an overview and instructions for trying out previews of the cross-server Application Map and improved 
exception experiences. These features require installing beta SDKs and using a custom link to visit the Azure portal.

## Overview
We are previewing two improvements to the Application Map:
* You can see multiple server components from different AI resources on the same application map as well as calls between server components.
Previously you could only see one server components at a time on the map. 
* A new error pane that shows up when you click on server components, summarizing exceptions that occured for that components by request URL
and call stacks. Clicking on an exception takes you to a refreshed version of our exception details page.

![You can select a server component and get easy access to exception][error-pane-flow]

These features are supported for server components using the following Application Insights SDKs:
* .NET Framework
* Node.js

.NET Core is currently not supported, but is coming soon.

## Getting Started
To see cross-server calls on the application map, you need to:
* Create one Application Insights resource and instrumentation key for each server component you want to show on the map
* Install the latest versions of the Application Insights SDKs to reports cross-server calls to Application Insights, for .NET you will need the beta SDK
* Open the portal using [this link](https://portal.azure.com/?appInsightsExtension_OverrideSettings=appMapExperience:appMapLegacyErrorPaneMultiServer)

### Configuring Application Insights Resources
To see multiple server components on the application map, you will need to create one Application Insights resource for each server component and 
configure each server with the appropriate instrumentation key.

The following picture shows how the resource group resource group for the example TeamStandup app which has
one Node.js server (teamstandup-web) and one .NET API server (teamstandup-api):

![Configuring resource group for the app map][resource-group-setup]

When opening the Application Map from a given Application Insights resource, the map will show additional server components by:
* Looking for other Application Insights resources in the same resource group
* Following HTTP dependency calls to/from the server components in the map
* Looking for servers with user-defined tags

These options can be configured by clicking on the Filter icon at the top of the map.

### Installing the .NET Beta SDK
To see multiple server components on the map with .NET you will need to:
* Install the latest beta version of the Application Insights SDK from NuGet
* Configure instrumentation for your web app to see calls between server components

From the manage NuGet packages page in Visual Studio check the "Include Preleases" checkbox, and search for "Microsoft.ApplicationInsights.Web". Select release
2.3.0-beta1 or later and click install.

![Installing the pre-release SDK][installing-beta-sdk]

The first time you install Application Insights using the IDE, you will be prompted to add code to your project to
send exceptions to Application Insights. This should automatically report exceptions that occur in MVC, but for Web API
and other project types see [reporting exceptions explicitly](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-asp-net-exceptions#reporting-exceptions-explicitly).

To enable instrumentation for Azure App Services, you can simply open the Application Insights resource in the portal
and you will be prompted to install an extension. For other resource types see [configure instrumentation for your web app](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-monitor-performance-live-website-now)
for instructions. 

### Installing the Node.js SDK
For node.js, simply install the latest applicationinsights SDK from npm using:
```
npm install --save applicationinsights 
```

Simply add the following two lines of code to the top of your server.js file:
```
import appInsights = require("applicationinsights");
appInsights.setup("<instrumentation_key>").start();
```

You can learn more about configuring the Node.js SDK on the 
[applicationinsights npm page](https://www.npmjs.com/package/applicationinsights).

Once you have installed these SDKs, you should now be able to use the preview portal to see cross-server calls on your application map.

### Viewing in the Preview Portal
To use these features in the portal, visit the Azure portal using the following link:

[https://portal.azure.com/?appInsightsExtension_OverrideSettings=appMapExperience:appMapLegacyErrorPaneMultiServer](https://portal.azure.com/?appInsightsExtension_OverrideSettings=appMapExperience:appMapLegacyErrorPaneMultiServer)

This link sets feature flags in the portal that allow you to use the preview features. To get to the application map,
open up one of the Application Insights resources (e.g. teamstandup-web) and select the "App Map" tile.

![Opening the app map in the portal][open-app-map]

## Using the new map
Once enabled you should be able to see multiple server components on the same application map. The map is structured with:
* Client (browser) components on the left
* Server components in the middle
* Dependency calls made by the selected server component on the right

![Client components on the left, server components in the middle, dependency calls on the right][node-group-labels]

If the client call out to external services (e.g. browser downloading .js files from public CDNs), you can
see components for those external calls to the left of the client. Dependency calls statistics for server to server calls
are shown as an extra dependency box (indicated by the purple triangle) attached to the bottom of the target server component.

The health icon on each component reflects the status that component, and will show red, yellow, or green depending on the percentage of
requests that failed in each component. 

By default a component that has more than 5% requests fail will show an orange triangle, and a component that has more than 20% of
incoming requests fail will show a red exclamation mark. You can change the thresholds on the map by clicking 
the "Options" button at the top of the map.

![Change thresholds by clicking the options tab][set-threshold]

Clicking on one of the server components will bring up the new error pane on the right-hand side. This pane shows a breakdown of the
exceptions observed on the server, grouped by the operation name (i.e. request URL), and the function that threw the exception 
(i.e. the top-most user code function found on the call stack).

![Errors for the API component][api-node-selected]

Clicking on an item in the list takes you to the exception details page showing the most recent example of that failed 
exception. You can navigate through the examples with the left/right arrows shown next to the timestamp.

![See exception details and get quick access to logs][exceptions-and-logs]

From here you can take quick actions such as searching the web for the message, and looking at telemetry related to the
exception. You can search telemetry by operation, session, time range, and exception type. For example, if you click 
"this operation" next to "Search telemetry for:", you can see the telemetry events that lead up to that particular exception
being thrown.

## Feedback
Let us know what you think of these new features! You can provide suggestions or report issues by clicking the smiley-face 
[portal feedback button](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-get-dev-support) in the 
upper right hand corner of the Azure portal.
