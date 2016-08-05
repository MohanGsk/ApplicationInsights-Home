
#AI.MetricData
An instance of the Metric item is a list of measurements (single data points) and/or aggregations.

1. **ver** : int

    Schema version
    
    Default value: 2
    
1. **metrics** : IList[DataPoint]

    List of metrics.
    
    [DataPoint] "Metric data single measurement."
    
    1. **name** : string
    
        Name of the metric.
        
        Max length: 1024
        
    1. **kind** : test.DataPointType
    
        Metric type.
        
        Default value: Measurement
        
        This field is optional.
        
        [DataPointType] "Type of the metric data measurement."
        
            - Measurement = 0
            - Aggregation = 1
            
    1. **value** : double
    
        Metric calculated value.
        
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
    
