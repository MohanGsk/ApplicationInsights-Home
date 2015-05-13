##ApplicationInsights: Worker role

* Request
* Dependency
* Exception
* Traces
* Custom Events
* Custom Metrics
* Performance Counters

#Important
* We encourage you to add the web nuget as that 
* If user/session telemetry is not applicable for your worker role, we recommend you remove the following telemetry modules and initializers from the ApplicationInsights.config file
  * [WebUserTrackingTelemetryModule](/ApplicationInsights.config#L36)
  * WebSessionTrackingTelemetryModule
  * WebUserTelemetryInitializer
  * WebSessionTelemetryInitializer