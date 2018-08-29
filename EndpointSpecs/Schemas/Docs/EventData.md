
#AI.EventData
Instances of Event represent structured event records that can be grouped and searched by their properties. Event data item also creates a metric of event count by name.

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
    
