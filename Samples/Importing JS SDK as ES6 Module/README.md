## Importing AppInsights JS SDK as ES6 Module
This sample demonstrates how you can you can create a shim to include [Application Insights JS SDK](https://github.com/Microsoft/ApplicationInsights-JS) as a module in your ES6 browser application, for example, react application. The shim dynamically loads JS SDK from CDN, so you will continue to get the benefits of continous improvements.

Here's an example of how you can import the module:

* Add AppInsights.js to your application
* Import it when you would like to call a track* method
```
import AppInsights from "./components/AppInsights.js"
......
AppInsights.initialize({
    instrumentationKey:"9620fb22-8eb0-4575-80df-696352b08283"
    });
AppInsights.trackTrace("hello");
```

Please note that you can only call initialize once, subsequent initializations have no effect. The following methods are shimmed: ``trackEvent``, ``trackException``, ``trackMetric``, ``trackPageView``, ``trackTrace``, ``trackAjax``, ``setAuthenticatedUserContext``, ``clearAuthenticatedUserContext``
You can enhance this sample by shimming additional methods - please see [JS SDK API reference](https://github.com/Microsoft/ApplicationInsights-JS/blob/master/API-reference.md).

