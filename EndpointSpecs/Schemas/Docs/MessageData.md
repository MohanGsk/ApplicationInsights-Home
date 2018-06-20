
#AI.MessageData
Instances of Message represent printf-like trace statements that are text-searched. Log4Net, NLog and other text-based log file entries are translated into intances of this type. The message does not have measurements.

1. **ver** : int

    Schema version
    
    Default value: 2
    
1. **message** : string

    Trace message
    
    Max length: 32768
    
1. **severityLevel** : test.SeverityLevel

    Trace severity level.
    
    This field is optional.
    
    [SeverityLevel] "Defines the level of severity for the event."
    
        - Verbose = 0
        - Information = 1
        - Warning = 2
        - Error = 3
        - Critical = 4
        
1. **properties** : IDictionary[string, string]

    Collection of custom properties.
    
    Max key length: 150
    
    Max value length: 8192
    
    This field is optional.
    
1. **measurements** : IDictionary[string, double]

    Collection of custom measurements.
    
    Max key length: 150
    
    This field is optional.
    
