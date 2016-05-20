## Importing AppInsights JS SDK as ES6 Module
This sample demonstrates how you can you can create a shim to include Application Insights JS SDK as a module in your ES6 browser application, for example react application. The shim dynamically loads JS SDK from CDN, so you will continue to get the benefits of continous improvements.

Here's an example of how you can import the module:

```
import AppInsights from "./components/AppInsights.js"
......
AppInsights.initialize({
    instrumentationKey:"9620fb22-8eb0-4575-80df-696352b08283"
    });
AppInsights.trackTrace("hello");
```

