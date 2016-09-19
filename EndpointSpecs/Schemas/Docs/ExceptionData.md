
#AI.ExceptionData
An instance of Exception represents a handled or unhandled exception that occurred during execution of the monitored application.

1. **ver** : int

    Schema version
    
    Default value: 2
    
1. **exceptions** : IList[ExceptionDetails]

    Exception chain - list of inner exceptions.
    
    [ExceptionDetails] "Exception details of the exception in a chain."
    
    1. **id** : int
    
        In case exception is nested (outer exception contains inner one), the id and outerId properties are used to represent the nesting.
        
        This field is optional.
        
    1. **outerId** : int
    
        The value of outerId is a reference to an element in ExceptionDetails that represents the outer exception
        
        This field is optional.
        
    1. **typeName** : string
    
        Exception type name.
        
        Max length: 1024
        
    1. **message** : string
    
        Exception message.
        
        Max length: 32768
        
    1. **hasFullStack** : bool
    
        Indicates if full exception stack is provided in the exception. The stack may be trimmed, such as in the case of a StackOverflow exception.
        
        Default value: true
        
        This field is optional.
        
    1. **stack** : string
    
        Text describing the stack. Either stack or parsedStack should have a value.
        
        Max length: 32768
        
        This field is optional.
        
    1. **parsedStack** : IList[StackFrame]
    
        List of stack frames. Either stack or parsedStack should have a value.
        
        This field is optional.
        
        [StackFrame] "Stack frame information."
        
        1. **level** : int
        
            Level in the call stack. For the long stacks SDK may not report every function in a call stack.
            
        1. **method** : string
        
            Method name.
            
            Max length: 1024
            
        1. **assembly** : string
        
            Name of the assembly (dll, jar, etc.) containing this function.
            
            Max length: 1024
            
            This field is optional.
            
        1. **fileName** : string
        
            File name or URL of the method implementation.
            
            Max length: 1024
            
            This field is optional.
            
        1. **line** : int
        
            Line number of the code implementation.
            
            This field is optional.
            
        
    
1. **severityLevel** : test.SeverityLevel

    Severity level. Mostly used to indicate exception severity level when it is reported by logging library.
    
    This field is optional.
    
    [SeverityLevel] "Defines the level of severity for the event."
    
        - Verbose = 0
        - Information = 1
        - Warning = 2
        - Error = 3
        - Critical = 4
        
1. **problemId** : string

    Identifier of where the exception was thrown in code. Used for exceptions grouping. Typically a combination of exception type and a function from the call stack.
    
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
    
