
#AI.ContextTagKeys
1. **"ai.application.ver"** : string

    Application version. Information in the application context fields is always about the application that is sending the telemetry.
    
    Max length: 1024
    
    Default value: "ai.application.ver"
    
    This field is optional.
    
1. **"ai.device.id"** : string

    Unique client device id. Computer name in most cases.
    
    Max length: 1024
    
    Default value: "ai.device.id"
    
    This field is optional.
    
1. **"ai.device.locale"** : string

    Device locale using <language>-<REGION> pattern, following RFC 5646. Example 'en-US'.
    
    Max length: 64
    
    Default value: "ai.device.locale"
    
    This field is optional.
    
1. **"ai.device.model"** : string

    Model of the device end user of the application using. Used for client scenarios. If empty, derived from user agent.
    
    Max length: 256
    
    Default value: "ai.device.model"
    
    This field is optional.
    
1. **"ai.device.oemName"** : string

    Max length: 256
    
    Default value: "ai.device.oemName"
    
    This field is optional.
    
1. **"ai.device.osVersion"** : string

    Operation system name and version of the device end user of the application using. If empty derived from user agent. Example 'Windows 10 Pro 10.0.10586.0'
    
    Max length: 256
    
    Default value: "ai.device.osVersion"
    
    This field is optional.
    
1. **"ai.device.type"** : string

    Type of the device end user of the application is using. Used primarily to distinguish JavaScript telemetry from server side telemetry. Examples: 'PC', 'Phone', 'Browser'. 'PC' is a default value.
    
    Max length: 64
    
    Default value: "ai.device.type"
    
    This field is optional.
    
1. **"ai.location.ip"** : string

    The IPv4 IP address of the client device. IPv6 is not currently supported. Information in the location context fields is always about the end user. When telemetry is sent from a service, the location context is about the user that initiated the operation in the service.
    
    Max length: 45
    
    Default value: "ai.location.ip"
    
    This field is optional.
    
1. **"ai.operation.id"** : string

    A unique identifier for the operation instance. The operation.id is created by either a request or a page view. All other telemetry sets this to the value for the containing request or page view. Operation.id is used for finding all the telemetry items for a specific operation instance.
    
    Max length: 128
    
    Default value: "ai.operation.id"
    
    This field is optional.
    
1. **"ai.operation.name"** : string

    The name (group) of the operation. The operation.name is created by either a request or a page view. All other telemetry items set this to the value for the containing request or page view. Operation.name is used for finding all the telemetry items for a group of operations (i.e. 'GET Home/Index').
    
    Max length: 1024
    
    Default value: "ai.operation.name"
    
    This field is optional.
    
1. **"ai.operation.parentId"** : string

    The unique identifier of the telemetry item's immediate parent.
    
    Max length: 128
    
    Default value: "ai.operation.parentId"
    
    This field is optional.
    
1. **"ai.operation.syntheticSource"** : string

    Name of synthetic source. Some telemetry from the application may represent a synthetic traffic. It may be web crawler indexing the web site, site availability tests or traces from diagnostic libraries like Application Insights SDK itself.
    
    Max length: 1024
    
    Default value: "ai.operation.syntheticSource"
    
    This field is optional.
    
1. **"ai.operation.correlationVector"** : string

    The cV is a light weight vector clock which can be used to identify and order related events across clients and services.
    
    Max length: 64
    
    Default value: "ai.operation.correlationVector"
    
    This field is optional.
    
1. **"ai.session.id"** : string

    Session ID - the instance of the user's interaction with the app. Information in the session context fields is always about the end user. When telemetry is sent from a service, the session context is about the user that initiated the operation in the service.
    
    Max length: 64
    
    Default value: "ai.session.id"
    
    This field is optional.
    
1. **"ai.session.isFirst"** : string

    Boolean value indicating whether the session identified by ai.session.id is first for the user or not.
    
    **Question**: Should it be marked as JSType-bool for breeze?
    
    Max length: 5
    
    Default value: "ai.session.isFirst"
    
    This field is optional.
    
1. **"ai.user.accountId"** : string

    In multi-tenant applications account ID or name that user is acting with. Examples may be subscription ID for Azure portal or blog name blogging platform.
    
    Max length: 1024
    
    Default value: "ai.user.accountId"
    
    This field is optional.
    
1. **"ai.user.userAgent"** : string

    The browser's user agent string as reported by the browser. This property will be used to extract informaiton regarding customer browser, but will not be stored. Use custom properties to store the original user agent.
    
    Max length: 2048
    
    Default value: "ai.user.userAgent"
    
    This field is optional.
    
1. **"ai.user.id"** : string

    Anonymous user id. Represents the end user of the application. When telemetry is sent from a service, the user context is about the user than initiated the operation in the service.
    
    Max length: 128
    
    Default value: "ai.user.id"
    
    This field is optional.
    
1. **"ai.user.authUserId"** : string

    Authenticated user id. As opposite to ai.user.id represents the user with by the friendly name. Since it's PII information it is not collected by default by most SDKs.
    
    Max length: 1024
    
    Default value: "ai.user.authUserId"
    
    This field is optional.
    
1. **"ai.cloud.role"** : string

    Name of the role application is part of. Maps directly to the role name in azure.
    
    Max length: 256
    
    Default value: "ai.cloud.role"
    
    This field is optional.
    
1. **"ai.cloud.roleInstance"** : string

    Name of the instance where application is running. Computer name for on-premisis, instance name for Azure.
    
    Max length: 256
    
    Default value: "ai.cloud.roleInstance"
    
    This field is optional.
    
1. **"ai.internal.sdkVersion"** : string

    SDK version. See https://github.com/Microsoft/ApplicationInsights-Home/blob/master/SDK-AUTHORING.md#sdk-version-specification for information.
    
    Max length: 64
    
    Default value: "ai.internal.sdkVersion"
    
    This field is optional.
    
1. **"ai.internal.agentVersion"** : string

    Agent version. Used to indicate the version of StatusMonitor installed on the computer if it is used for data collection.
    
    Max length: 64
    
    Default value: "ai.internal.agentVersion"
    
    This field is optional.
    
