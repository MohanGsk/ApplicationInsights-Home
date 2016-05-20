export default class AppInsights {
    static _createLazyMethod(name) {
        // Define a temporary method that queues-up a the real method call
        appInsights[name] = function() {
            // Capture the original arguments passed to the method
            var originalArguments = arguments;
            // If the queue is available, it means that the function wasn't yet replaced with actual function value
            if (appInsights.queue) {
                appInsights.queue.push(() => appInsights[name].apply(appInsights, originalArguments));
            }
            else {
                // otheriwse execute the function
                appInsights[name].apply(appInsights, originalArguments);
            }
        }

        AppInsights[name] = appInsights[name];
    };

    static initialize(aiConfig) {
        if (!window.appInsights) {
            window.appInsights = {
                config: aiConfig
            };

            var scriptElement = document.createElement("script");
            scriptElement.src = "http://az416426.vo.msecnd.net/scripts/a/ai.0.js";
            document.head.appendChild(scriptElement);

            // capture initial cookie
            appInsights.cookie = document.cookie;
            appInsights.queue = [];

            var method = ["trackEvent", "trackException", "trackMetric", "trackPageView", "trackTrace", "trackAjax", "setAuthenticatedUserContext", "clearAuthenticatedUserContext"];
            while (method.length) {
                AppInsights._createLazyMethod(method.pop());
            }

            // collect global errors
            if (!aiConfig.disableExceptionTracking) {
                AppInsights._createLazyMethod("_onerror");
                var originalOnError = window["_onerror"];
                window[method] = function(message, url, lineNumber, columnNumber, error) {
                    var handled = originalOnError && originalOnError(message, url, lineNumber, columnNumber, error);
                    if (handled !== true) {
                        appInsights["_onerror"](message, url, lineNumber, columnNumber, error);
                    }

                    return handled;
                };
            }
        }
    }
}