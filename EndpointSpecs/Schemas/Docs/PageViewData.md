
#AI.PageViewData
An instance of PageView represents a generic action on a page like a button click. It is also the base type for PageView.

1. **ver** : int

    Schema version
    
    Default value: 2
    
1. **name** : string

    Event name. Keep it low cardinality to allow proper grouping and useful metrics.
    
    **Question**: Why Custom Event name is shorter than Request name or dependency name?
    
    Max length: 512
    
1. **properties** : IDictionary[string, string]

    Collection of custom properties.
    
    Max key length: "150"
    
    Max value length: "8192"
    
    This field is optional.
    
1. **measurements** : IDictionary[string, double]

    Collection of custom measurements.
    
    Max key length: "150"
    
    This field is optional.
    
1. **url** : string

    Request URL with all query string parameters
    
    Max length: 2048
    
    This field is optional.
    
1. **duration** : string

    Request duration in format: DD.HH:MM:SS.MMMMMM. For a page view (PageViewData), this is the duration. For a page view with performance information (PageViewPerfData), this is the page load time. Must be less than 1000 days.
    
    This field is optional.
    
1. **id** : string

    Identifier of a page view instance. Used for correlation between page view and other telemetry items.
    
    Max length: 128

1. **referrerUri** : string

    Fully qualified page URI or URL of the referring page; if unknown, leave blank.
    
    Max length: 2048

    This field is optional.
    
