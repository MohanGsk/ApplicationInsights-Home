
#AI.RemoteDependencyData
An instance of Remote Dependency represents an interaction of the monitored component with a remote component/service like SQL or an HTTP endpoint.

1. **ver** : int

    Schema version
    
    Default value: 2
    
1. **name** : string

    Name of the command initiated with this dependency call. Low cardinality value. Examples are stored procedure name and URL path template.
    
    Max length: 1024
    
1. **id** : string

    Identifier of a dependency call instance. Used for correlation with the request telemetry item corresponding to this dependency call.
    
    Max length: 128
    
    This field is optional.
    
1. **resultCode** : string

    Result code of a dependency call. Examples are SQL error code and HTTP status code.
    
    Max length: 1024
    
    This field is optional.
    
1. **duration** : string

    Request duration in TimeSpan 'G' (general long) format: d:hh:mm:ss.fffffff.
    
    This field is optional.
    
1. **success** : bool

    Indication of successfull or unsuccessfull call.
    
    Default value: true
    
    This field is optional.
    
1. **data** : string

    Command initiated by this dependency call. Examples are SQL statement and HTTP URL's with all query parameters.
    
    Max length: 8192
    
    This field is optional.
    
1. **type** : string

    Dependency type name. Very low cardinality value for logical grouping of dependencies and interpretation of other fields like commandName and resultCode. Examples are SQL, Azure table, and HTTP.
    
    Max length: 1024
    
    This field is optional.
    
1. **target** : string

    Target site of a dependency call. Examples are server name, host address.
    
    Max length: 1024
    
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
    
