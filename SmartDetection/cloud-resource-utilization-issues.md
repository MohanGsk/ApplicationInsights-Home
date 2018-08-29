# Cloud resource utilization issues (preview)

### Intro

Application Insights automatically analyzes the CPU consumption of the cloud resources in your application and detects resources that exhibit significant trends and issues in CPU utilization. These detections enable you to avoid service disruptions, or to decrease your Azure resources and save costs (by adjusting the size or the number of these resources).

This feature requires no special setup, other than [configuring performance counters](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-performance-counters) for your app. It is active when your app generates enough CPU performance counters telemetry (% Processor Time).


### When would I get this type of smart detection notification?
A typical notification will occur when your Web/Worker Role instances exhibit a significant and sustained high or low trend in CPU utilization over a long period of time (a few hours).

### Does my app definitely have a problem?
No, a notification doesn't mean that your app definitely needs to be scaled up, down, in or out. Although such patterns of high/low CPU utilization usually indicate that resource consumption could be increased or decreased, this behavior could have a natural business justification, and can be ignored.

### How do I fix it?
The notifications include diagnostic information to support in the diagnostics process:
1. **Triage.** The notification shows you the detected CPU utilization (in %), and the time range in which the utilization had increased or decreased. This can help you assign a priority to the problem.
2. **Scope.** How many roles exhibit the unusual CPU utilization pattern? This information can be obtained from the notification.
3. **Diagnose.** The detection contains the percentage of CPU utilized, showing CPU utilization of each instance over time. You can also use the related items and reports linking to supporting information, to help you further diagnose the issue. Where relevant, we'll provide a suggested action to use [Azure Autoscale](https://azure.microsoft.com/en-us/features/autoscale/), a service that allows setting rules for automatic scaling of your Cloud Services.
