
# How to enable Application Insights profiler 

[Azure Application Insights](app-insights-overview.md) includes a profiling tool that shows you how much time is spent in each method in your live web application. It automatically highlights the 'hot path' that is using the most time. You can enroll in the preview trial of this tool. 


<a id="installation></a>
## Prerequisites

- The app you want to profile is an ASP.NET application running as an Azure Web App.
- Application Insights SDK 2.2 Beta or later is enabled on your web app.
- The Web App Service Plan must be Basic tier or above.

To enroll in the preview program for Application Insights Profiler, please send your Azure subscription id to serviceprofilerhelp@microsoft.com. You will receive a confirmation email.

## Set up the profiler through the linked Application Insights resource

1. After receiving the confirmation email, please verify you have created an Application Insights resource associated with the Web App you want to profile.
    * Open the Web App resource in [Azure portal](https://portal.azure.com).
    * Open "MONITORING -> Application Insights" to open Application Insight blade.
    * If there is no associated Application Insights resource, either create new or select existing one.
2. Open the Application Insights resource associated with the Web App. (From MONITORING -> Application Insights, scroll down and select VIEW MORE IN APPLICATION INSIGHTS.)
3. Select "INVESTIGATE -> Performance" to open Performance blade.
4. Click "Enable Application Insights Profiler" message popped up the top to open Enabling blade.
5. Click "Enable Profiling" button to set up the profiler on the Web App which including
    * Install Application Insights Profiler extension.
    * Add APPINSIGHTS_INSTRUMENTATIONKEY to App Settings.
    * Enable "Always On".
    * Set ".NET Framework version" to v4.6.
6. After the enabling completes, the profiler agent will run as a continuous web job in the Web App.

## Set up the profiler manually

1. After receiving the confirmation email, if you have already enabled Application Insights SDK 2.2 and above on the Web App, you can manually set up the profiler.
2. Open the Web App resource in [Azure portal](https://portal.azure.com).
3. Select "SETTINGS -> Application settings" to open Application settings blade.
4. Update the following settings
    * Set ".Net Framework version" to v4.6.
    * Set "Always On" to On.
    * Add app setting "APPINSIGHTS_INSTRUMENTATIONKEY" and set the value to the same instrumentation key used by the SDK.
4. Select "DEVELOPMENT TOOLS -> Extensions" to open Extensions blade.
5. Click "Add" button and choose "Application Insights Profiler".
6. After the extension is installed, the profiler agent will run as a continuous web job in the Web App.

## Stop the profiler

1. Open the Web App resource in [https://portal.azure.com](https://portal.azure.com)
2. Select "SETTINGS -> WebJobs" to open WebJobs blade.
3. Select "ApplicationInsightsProfiler" and click "Stop" button.
4. Later on, you can click "Start" button to re-enable the profiler.

## Delete the profiler

1. Open the Web App resource in [https://portal.azure.com](https://portal.azure.com)
2. Select "DEVELOPMENT TOOLS -> Extensions" to open Extensions blade.
3. Select "Application Insights Profiler" and click "Delete" button. After deletion, the profiler agent will also be removed.

## How to view data

To view data click on the performance tab in the overview blade or the first chart in the overview timeline.

![How to open the Performance blade][performance-blade]

When a row has an icon in the last column this means there are traces 
available for that operation name. Clicking on this icon will bring you to our trace explorer where you can view several different samples 
that we have captures. 

![Application Insights Performance blade Examples Column][performance-blade-examples]

In the explorer there are a few things that are available to you. On the left we give you a percentile breakdown with samples in the categories.
This gives you quick access to profile data in performance bucket. If you need more data or can't find what you need simply clicking on the 'Show all'
button at the top will reveal more data. 


![Application Insights Trace Explorer][trace-explorer]

In the toolbar we have some meta information with a precise timestamp you can use for tracing purposes. You also have the ability to download the .etl file
we collected this trace from. You can use tools such as PerfView to view this .etl file for more advanced investigations.

![Toolbar][trace-explorer-toolbar]

If our analysis is able to provide a performance tip we will show a notification at the top of the trace with what type of issue you might have. The common
case is "waiting" or "awaiting" where most of the time there is a network or I/O operation the framework was waiting for.

![Hint][trace-explorer-hint-tip]

In all traces we will always have a "Show hot path" button which will lead you to the biggest leaf node or at least something close. In some cases this node,
will be adjacent to where your performance bottle neck but not all times.

![Hot path][trace-explorer-hot-path]

This tool is still in its early stages and we are always looking for feedback on the UI. Feel free to send feedback to [serviceprofilerhelp@microsoft.com](mailto:serviceprofilerhelp@microsoft.com)

## How to read performance data
Microsoft service profiler uses a combination of sampling method and instrumentation to analyze the performance of your application. 
When detailed collection is in progress, service profiler samples the instruction pointer of each of the machine's CPUs every millisecond. 
Each sample captures the complete call stack of the thread currently executing, giving detailed and useful information about what that 
thread was doing at both high and low levels of abstraction. Service profiler also collects other events such as context switching events,
TPL events and threadpool events to track activity correlation and causality. 

The call stack shown in the timeline view is the result of the above sampling and instrumentation. Because each sample captures the complete
call stack of the thread, it will include code from .net framework as well as other frameworks you reference.

### <a id="jitnewobj"></a>Object Allocation (clr!JIT\_New or clr!JIT\_Newarr1)
clr!JIT\_New and clr!JIT\_Newarr1 are helper functions inside .net framework that allocates memory from managed heap. clr!JIT\_New is invoked
when an object is allocated. clr!JIT\_Newarr1 is invoked when an object array is allocated. These two functions are typically 
very fast and should take relatively small amount of time. If you see clr!JIT\_New or clr!JIT\_Newarr1 take a substantial amount of 
time in your timeline, it's an indication that the code may be allocating many objects and consuming significant amount of memory. 

### <a id="theprestub"></a>Loading Code (clr!ThePreStub)
clr!ThePreStub is a helper function inside .net framework that prepares the code to execute for the first time. This typically includes, 
but not limited to, JIT (Just In Time) compilation. For each C# method, clr!ThePreStub should be invoked at most once during the lifetime
of a process.

If you see clr!ThePreStub takes significant amount of time for a request, it indicates that request is the first one that executes 
that method, and the time for .net framework runtime to load that method is significant. You can consider a warm up process that executes
that portion of the code before your users access it, or consider NGen your assemblies. 

### <a id="lockcontention"></a>Lock Contention (clr!JITutil\_MonContention or clr!JITutil\_MonEnterWorker)
clr!JITutil\_MonContention or clr!JITutil\_MonEnterWorker indicate the current thread is waiting for a lock to be released. This typically
shows up when executing a c# lock statement, invoking Monitor.Enter method, or invoking a method with MethodImplOptions.Synchronized 
attribute. Lock contention typically happens when thread A acquires a lock, and thread B tries to acquire the same lock before thread 
A releases it. 

### <a id="ngencold"></a>Loading code ([COLD])
If the method name has the word "[COLD]" in it, such as mscorlib.ni![COLD]System.Reflection.CustomAttribute.IsDefined, it means the .net
framework runtime is executing code that is not optimized by <a href="https://msdn.microsoft.com/en-us/library/e7k32f4k.aspx">
profile-guided optimization</a> for the first time. For each method, it should show up at most once during the lifetime of the process. 

If loading code takes significant amount of time for a request, it indicates that request is the first one to execute the unoptimized 
portion of the method. You can consider a warm up process that executes that portion of the code before your users access it. 

### <a id="httpclientsend"></a>Send HTTP Request
Methods such as HttpClient.Send indicates the code is waiting for a HTTP request to complete.

### <a id="sqlcommand"></a>Database Operation
Method such as SqlCommand.Execute indicates the code is waiting for a database operation to complete.

### <a id="await"></a>Waiting (AWAIT\_TIME)
AWAIT\_TIME indicates the code is waiting for another task to complete. This typically happens with C# 'await' statement. When the code
does a C# 'await', the thread unwinds and returns control to the threadpool, and there is no thread that is blocked waiting for 
the 'await' to finish. However, logically the thread that did the await is 'blocked' waiting for the operation to complete. The
AWAIT\_TIME indicates the blocked time waiting for the task to complete.

### <a id="block"></a>Blocked Time
BLOCKED\_TIME indicates the code is waiting for another resource to be available, such as waiting for a synchronization object,
waiting for a thread to be available, or waiting for a request to finish. 

### <a id="cpu"></a>CPU Time
The CPU is busy executing the instructions.

### <a id="disk"></a>Disk Time
The application is performing disk operations.

### <a id="network"></a>Network Time
The application is performing network operations.

### <a id="when"></a>When column
This is a visualization of how the INCLUSIVE samples collected for a node vary over time. The total range of the request
is divided into 32 time buckets and the inclusive samples for that node are accumulated into those 32 buckets. Each bucket is then represented as 
a bar whose height represents a scaled value. For nodes marked CPU_TIME or BLOCKED_TIME, or where there is an obvious relationship of consuming a resource (cpu, disk, thread),
the bar represents consuming 1 of those resources for the period of time of that bucket. For these metrics you can get greater than 100% by consuming multiple 
resources (e.g. if on average you consume 2 CPUs over an interval than you will get 200%, which we currently do not visualize).


## <a id="troubleshooting"></a>Troubleshooting

### How can I know if Application Insights profiler is running after installation?

The profiler run as a continuous web job in Web App. You can open the Web App resource in https://portal.azure.com and check "ApplicationInsightsProfiler" status in WebJobs blade. If it's not running, you can click "Logs" button to find out more information.

### Why can't I find any stack examples even the profiler is running?

Here are a few things you can check.

1. Make sure your Web App Service Plan is Basic tier and above.
2. Make sure your Web App has Application Insights SDK 2.2 Beta and above enabled.
3. Make sure your Web App has the APPINSIGHTS_INSTRUMENTATIONKEY setting with the same instrumentation key used by AI SDK.
4. Make sure your Web App is running on .Net Framework 4.6.


After the profiler is started, there is a short warm-up period when the profiler actively collects several performance traces. After that, the profiler will collect 2-minutes performance trace once an hour. If you keep sending the traffic to the Web App, you will eventually get the stack examples. We plan to provide the configuration option in future release.

The profiler currently only supports ASP.NET application. We plan to support ASP.NET Core application soon.

### If I'm currently using Azure Service Profiler for my Web App, should I switch to Application Insights Profiler?  

If it is ASP.NET application, we strongly recommend you to switch to Application Insights Profiler which not only provides the same performance insight as Azure Service Profiler but also correlates with other Application Insights data. In fact, after you enable Application Insights Profiler on your Web App, the existing Azure Service Profiler agent will be disabled.

### <a id="double-counting"></a>Double counting of nodes

In some cases the total metric in the stack viewer is more than the wall clock time for the request. This is not necessarily a bug.  
This can happen any time two are more threads associated with a request are operating in parallel. In such cases each thread 
contributes thread time and it can add up to more than the total wall clock time. In many cases one thread may be simply 
waiting on the other to complete. The viewer tries to detect this and omit the uninteresting wait, but it can’t do a 
perfect job and errs on the side of showing too much rather than omitting what may be critical information.  

When you see this in your traces, you technically need to understand which threads are waiting on what so you can determine the ‘critical path’ for the request. 
However a much simpler heuristic almost always works. Simply look at all paths. Typically one or more will quickly wait and are uninteresting. 
Since it is likely that these are simply waiting on the other threads, concentrate on the rest and ignore the time in the uninteresting threads.   
This simple, ‘obvious’ technique, almost always works.   

If you have any further questions, please send mail to [serviceprofilerhelp@microsoft.com](mailto:serviceprofilerhelp@microsoft.com)

## Next steps

* [Working with Application Insights in Visual Studio](https://docs.microsoft.com/azure/application-insights/app-insights-visual-studio)

[performance-blade]: ./media/app-insights-serviceprofiler/performance-blade.png
[performance-blade-examples]: ./media/app-insights-serviceprofiler/performance-blade-examples.png
[trace-explorer]: ./media/app-insights-serviceprofiler/trace-explorer.png
[trace-explorer-toolbar]: ./media/app-insights-serviceprofiler/trace-explorer-toolbar.png
[trace-explorer-hint-tip]: ./media/app-insights-serviceprofiler/trace-explorer-hint-tip.png
[trace-explorer-hot-path]: ./media/app-insights-serviceprofiler/trace-explorer-hot-path.png
