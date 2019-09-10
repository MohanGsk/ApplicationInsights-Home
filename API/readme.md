This directory contains various scripts that are used to complement our Application Insights user experience.

- CreateReleaseAnnotation.ps1: Create a release annotation using a PowerShell script. 

## Issue creating annotation (TLS Error)

In older versions of .NET the default behavior was to use TLS 1.0 oir SSL3 to establish a SSL connection--this can cause problems when creating annotations. In recent versions of .NET the behavior has changed to include TLS 1.1 and TLS 1.2 by default. If you are seeing errors that resemble the ones below you should add `[System.Net.ServicePointManager]::SecurityProtocol = 'Tls, Tls11, Tls12'` to the annotation script above the `Invoke-WebRequest` method. This will force .NET to use the specified protocols.

### Errors
```
Failed to create an annotation with Id: <someguid value>. Error Exception, Description: The underlying connection was closed: An unexpected error occurred on a receive..
At C:\Output\CreateReleaseAnnotation.ps1:145 char:3
+      throw $output
+      ~~~~~~~~~~~~~
    + CategoryInfo          : OperationStopped: (Failed to creat... on a receive..:String) [], RuntimeException
    + FullyQualifiedErrorId : Failed to create an annotation with Id: <someguid value>. Error Exception, Description: The underlying connection was closed: An unexpected error occurred on a receive.
```

or

```
The client and server cannot communicate, because they do not possess a common algorithm
```

## Contributor License
This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
