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
[add-myget-feed]:  ./media/app-insights-app-map/add-myget-feed.png
[exceptions-and-logs]: ./media/app-insights-app-map/exceptions-and-logs.png
[open-app-map]: ./media/app-insights-app-map/open-app-map.png



# Application Insights Application Map Preview

This page constains an overview and instructions for trying out previews of the cross-tier Application Map and improved 
exception experiences. These features require installing beta SDKs and turning on feature flags on the Azure portal.

## Overview
We are previewing two improvements to the Application Map:
* You can see multiple server nodes from different AI resources on the same application map as well as calls between server nodes.
Previously you could only see one server node at a time on the map. 
* A new error pane that shows up when you click on server nodes, summarizing exceptions that occured for that node by request URL
and call stacks. Clicking on an exception takes you to a refreshed version of our exception details page.

![You can select a server node and get easy access to exception][error-pane-flow]

These features are supported for server nodes using the following Application Insights SDKs:
* .NET Framework
* Node.js

.NET Core is currently not supported, but is coming soon.

## Getting Started
To see cross-server calls on the application map, you need to:
* Have your Application Insights resources and instrumentation keys configured such that there is one Application Insights resource + one instrumentaiton key for each server node you want to show on the map
* Install the preview versions of the Application Insights SDKs that reports cross-server calls to Application Insights

### Configuring your resource group
In many cases, the resources you want to show will be contained in a single resource group. The app map will find server nodes
by looking for all Application Insights resources within the current resource group. It will also detect server nodes
by following any dependency calls tracked by Application Insights resources in the current resource group.

The following picture shows how the resource group resource group for the example TeamStandup app which has
one Node.js server (teamstandup-web) and one .NET API server (teamstandup-api):

![Configuring resource group for the app map][resource-group-setup]

Note how there is one Application Insights resource for each of the server nodes.

### Installing the .NET SDK
For .NET, you need to install a pre-release beta version of the Application Insights SDK from myget. Add the myget nuget package source
by going to Tools -> Options -> NuGet Package Manager, adding a new source with the following source url:
```
 https://www.myget.org/F/applicationinsights/
```

![Add the pre-release myget feed][add-myget-feed]

Check the "Include Preleases" checkbox, and search for "Microsoft.ApplicationInsights.Web". Make sure you select release
2.3.0-beta1-build631 or later.

![Installing the pre-release SDK][installing-beta-sdk]

NOTE: You will want to remove this myget feed from your NuGet package manager when you are done evaluating the preview,
as you may accidentally install development versions of Application Insights SDKs in the future.

The first time you install Application Insights using the IDE, you will be prompted to add code to your project to
send exceptions to Application Insights. This should automatically report exceptions that occur in MVC, but for Web API
and other project types see [reporting exceptions explicitly](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-asp-net-exceptions#reporting-exceptions-explicitly).

You will need to [configure instrumentation for your web app](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-monitor-performance-live-website-now)
to see calls between server nodes. For Azure App Service, you can simply open the Application Insights resource in the portal
and you will be prompted to install an extension.

### Setting up the Node.js SDK
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

[https://portal.azure.com/?appInsightsExtension_OverrideSettings=appMapMultiServerNode:true%2cappMapErrorPane:true](https://portal.azure.com/?appInsightsExtension_OverrideSettings=appMapMultiServerNode:true%2cappMapErrorPane:true)

This link sets feature flags in the portal that allow you to use the preview features. To get to the application map,
open up one of the Application Insights resources (e.g. teamstandup-web) and select the "App Map" tile.

For the best experience, maximize (full-screen) the map and pin it to your dashboard using the controls in the upper-right corner of the map.

![Opening the app map in the portal][open-app-map]

## Using the new map
Once enabled you should be able to see nodes from multiple servers on the same application map. The map is structured with:
* Client (browser) nodes on the left
* Server nodes in the middle
* Dependency calls made by the selected server node on the right

![Client nodes on the left, server nodes in the middle, dependency calls on the right][node-group-labels]

If the client call out to external services (e.g. browser downloading .js files from public CDNs), you can
see nodes for those external calls to the left of the client.

The health icon on each node reflects the status of each node, and will show red, yellow, or green depending on the percentage of
requests that failed in each node. 

By default a node that has more than 5% requests fail will show an orange triangle, and a node that has more than 20% of
incoming requests fail will show a red exclamation mark. You can change the thresholds on the map by clicking 
the "Options" button at the top of the map.

![Change thresholds by clicking the options tab][set-threshold]

Clicking on one of the server nodes will bring up the new error pane on the right-hand side. This pane shows a breakdown of the
exceptions observed on the server, grouped by the operation name (i.e. request URL), and the function that threw the exception 
(i.e. the top-most user code function found on the call stack).
![Errors for the API node][api-node-selected]


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

You can also reach out to us at [appmapfb@microsoft.com](mailto:appmapfb@microsoft.com) for any other questions or feedback
related to this preview release.