
#AI.PageViewPerfData
An instance of PageViewPerf represents: a page view with no performance data, a page view with performance data, or just the performance data of an earlier page request.

1. **ver** : int

    Schema version
    
    Default value: 2
    
1. **name** : string

    Event name. Keep it low cardinality to allow proper grouping and useful metrics.
    
    **Question**: Why Custom Event name is shorter than Request name or dependency name?
    
    Max length: 512
    
1. **properties** : IDictionary[string, string]

    Collection of custom properties.
    
    Max key length: 150
    
    Max value length: 8192
    
    This field is optional.
    
1. **measurements** : IDictionary[string, double]

    Collection of custom measurements.
    
    Max key length: 150
    
    This field is optional.
    
1. **url** : string

    Request URL with all query string parameters
    
    Max length: 2048
    
    This field is optional.
    
1. **duration** : string

    Request duration in format: DD.HH:MM:SS.MMMMMM. For a page view (PageViewData), this is the duration. For a page view with performance information (PageViewPerfData), this is the page load time. Must be less than 1000 days.
    
    This field is optional.
    
1. **referrerUri** : string

    Fully qualified page URI or URL of the referring page; if unknown, leave blank
    
    Max length: 2048
    
1. **id** : string

    Identifier of a page view instance. Used for correlation between page view and other telemetry items.
    
    Max length: 128
    
1. **perfTotal** : string

    Performance total in TimeSpan 'G' (general long) format: d:hh:mm:ss.fffffff
    
    This field is optional.
    
1. **networkConnect** : string

    Network connection time in TimeSpan 'G' (general long) format: d:hh:mm:ss.fffffff
    
    This field is optional.
    
1. **sentRequest** : string

    Sent request time in TimeSpan 'G' (general long) format: d:hh:mm:ss.fffffff
    
    This field is optional.
    
1. **receivedResponse** : string

    Received response time in TimeSpan 'G' (general long) format: d:hh:mm:ss.fffffff
    
    This field is optional.
    
1. **domProcessing** : string

    DOM processing time in TimeSpan 'G' (general long) format: d:hh:mm:ss.fffffff
    
    This field is optional.
    
