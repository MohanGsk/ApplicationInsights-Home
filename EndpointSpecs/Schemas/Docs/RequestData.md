
#AI.RequestData
Instance of Request represents completion of an external request to the application to do work, and contains a summary of that request execution and the results.

1. **ver** : int

    Schema version
    
    Default value: 2
    
1. **id** : string

    Identifier of a request call instance. Used for correlation between request and other telemetry items.
    
    Max length: 128
    
1. **duration** : string

    Request duration in TimeSpan 'G' (general long) format: d:hh:mm:ss.fffffff.
    
1. **responseCode** : string

    Result of a request execution. Http status code for http requests.
    
    Max length: 1024
    
1. **success** : bool

    Indication of successfull or unsuccessfull call.
    
1. **source** : string

    Source of the request. Examples are instrumentation key of the caller, ip address of the caller.
    
    Max length: 1024
    
    This field is optional.
    
1. **name** : string

    Name of the request. Represents code path taken to process request. Low cardinality value to allow better grouping of requests. For http requests represents http method and url path template like 'GET /values/{id}'.
    
    Max length: 1024
    
    This field is optional.
    
1. **url** : string

    Request url with all query string parameters
    
    Max length: 2048
    
    This field is optional.
    
1. **properties** : IDictionary[string, string]

    Collection of custom properties.
    
    Max key length: "150"
    
    Max value length: "8192"
    
    This field is optional.
    
1. **measurements** : IDictionary[string, double]

    Collection of custom measurements.
    
    Max key length: "150"
    
    This field is optional.
    
