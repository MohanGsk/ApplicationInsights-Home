
#AI.Envelope
System variables for a telemetry item.

1. **ver** : int

    Envelope version. For internal use only. By assigning this the default, it will not be serialized within the payload unless changed to a value other than #1.
    
    Default value: 1
    
    This field is optional.
    
1. **name** : string

    Type name of telemetry data item.
    
    Max length: 1024
    
1. **time** : string

    Event date time when telemetry item was created. This is the wall clock time on the client when the event was generated. There is no guarantee that the client's time is accurate. This field must be formatted in UTC ISO 8601 format, with a trailing 'Z' character, as described publicly on https://en.wikipedia.org/wiki/ISO_8601#UTC. Note: the number of decimal seconds digits provided are variable (and unspecified). Consumers should handle this, i.e. managed code consumers should not use format 'O' for parsing as it specifies a fixed length. Example: 2009-06-15T13:45:30.0000000Z.
    
    Max length: 64
    
    Time offset in the past from the current time: -48 hours
    
    Time offset in the future from the current time: +2 hours
    
1. **sampleRate** : double

    Sampling rate used in application. This telemetry item represents 1 / sampleRate actual telemetry items.
    
    Default value: 100.0
    
    This field is optional.
    
1. **seq** : string

    Sequence field used to track absolute order of uploaded events.
    
    Max length: 64
    
    This field is optional.
    
1. **iKey** : string

    The application's instrumentation key. The key is typically represented as a GUID, but there are cases when it is not a guid. No code should rely on iKey being a GUID. Instrumentation key is case insensitive.
    
    Max length: 40
    
    This field is optional.
    
1. **flags** : long

    A collection of values bit-packed to represent how the event was processed. Currently represents whether IP address needs to be stripped out from event (set 0x200000) or should be preserved.
    
    This field is optional.
    
1. **tags** : IDictionary[string, string]

    Key/value collection of context properties. See ContextTagKeys for information on available properties.
    
    This field is optional.
    
1. **data** : test.Base

    Telemetry data item.
    
    This field is optional.
    
