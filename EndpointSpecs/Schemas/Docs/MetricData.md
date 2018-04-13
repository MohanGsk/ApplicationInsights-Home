
#AI.MetricData
An instance of the Metric item is a list of measurements (single data points) and/or aggregations.

1. **ver** : int

    Schema version
    
    Default value: 2
    
1. **metrics** : IList[DataPoint]

    List of metrics. Only one metric in the list is currently supported by Application Insights storage. If multiple data points were sent only the first one will be used.
    
    [DataPoint] "Metric data single measurement."
    
    1. **ns** : string
    
        Namespace of the metric.
        
        This field is optional.
        
        Max length: 256
    
    1. **name** : string
    
        Name of the metric.
        
        Max length: 1024
        
    1. **kind** : test.DataPointType
    
        Metric type. Single measurement or the aggregated value.
        
        Default value: Measurement
        
        This field is optional.
        
        [DataPointType] "Type of the metric data measurement."
        
            - Measurement = 0
            - Aggregation = 1
            
    1. **value** : double
    
        Single value for measurement. Sum of individual measurements for the aggregation.
        
    1. **count** : int
    
        Metric weight of the aggregated metric. Should not be set for a measurement.
        
        This field is optional.
        
    1. **min** : double
    
        Minimum value of the aggregated metric. Should not be set for a measurement.
        
        This field is optional.
        
    1. **max** : double
    
        Maximum value of the aggregated metric. Should not be set for a measurement.
        
        This field is optional.
        
    1. **stdDev** : double
    
        Standard deviation of the aggregated metric. Should not be set for a measurement.
        
        This field is optional.
        
    
1. **properties** : IDictionary[string, string]

    Collection of custom properties.
    
    Max key length: "150"
    
    Max value length: "8192"
    
    This field is optional.
    
