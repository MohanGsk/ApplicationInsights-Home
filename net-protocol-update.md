# Http Correlation protocol update proposal

.NET libraries recently adopted the new [Http Correlation protocol](https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/HttpCorrelationProtocol.md) for distributed tracing. Protocol is designed to be feature-rich, but allows to fallback into very simple semantic.

Application Insights is working on implementing this protocol on other languages.


Interop is important. Scenarios:
- Web tests provider sends requests to monitored applicaitons. For failed web tests it's easy to investigate having the correlation identifiers
- Built-in correlation protocols of cloud-hosted functions, lambdas, cloud load balancers and proxies support is easier having the standard
- Frameworks instrumentation is easier and more efficient without the need to expose an interop layer
- Simple log file writers for web servers can write log that can be correlated woth 


Every request has unique identifier:
    - obtaining of request ID should be as easy as read the header 
    - system without any knowledge about distributed tracing should be able to attach it to the request logs
    - any text can be accepted as request id

Distributed tracing providers may easily extract the trace id:
    - trace id long enough to provide uniquiness
    - random enough to be used for sampling
    - human "parseable" from the headers
    - efficiently parseable so simple proxies can propagate further

Allows to propagate the name-value pairs:
    - simple proxies just carry it over
    - propagation is optional


Proposal:

```
Request-Id: <format version>-base64(<trace-id>)-<span-id>
Correlation-Context: key1=value1, key2=value2
```

`Request-Id` is a unique identifier of every request. 
`Correlation-Context` is a name-value collection that needs to be propagated from incoming requests to the outgoing dependant services calls.

If incoming `Request-Id` has a `trace-id` prefix - outgoing calls should preserve the prefix.



[Http Correlation protocol](https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/HttpCorrelationProtocol.md)

```
Request-Id:("any-unique-string"|" | <root-id>.<local-id1>.<local-id2>.")
<root-id> = base64 or "-" string
<local-id1> = random suffix + "_" + monotonically incremented value
Correlation-Context: key1=value1, key2=value2
```

[Census protocol proposal](https://github.com/TraceContext/tracecontext-spec/pull/1/)

``` 
Trace-Context: 00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01 
base16(<Version>) = 00 
base16(<TraceId>) = 4bf92f3577b34da6a3ce929d0e0e4736 
base16(<SpanId>) = 00f067aa0ba902b7 
base16(<TraceOptions>) = 01  // sampled 
``` 

[B3](https://github.com/openzipkin/b3-propagation)

```
X-B3-TraceId: 463ac35c9f6413ad48485a3953bb6124
X-B3-SpanId: a2fb4a1d1a96d312
X-B3-ParentSpanId: 0020000000000001
X-B3-Sampled: 1
X-B3-Flags: 1
```

[AWS](http://docs.aws.amazon.com/elasticloadbalancing/latest/application/load-balancer-request-tracing.html)

```
X-Amzn-Trace-Id: Self=1-67891234-12456789abcdef012345678;Root=1-67891233-abcdef012345678912345678;CalledFrom=app
```

[Heroku](https://devcenter.heroku.com/articles/http-request-id)

```
X-Request-ID: 30f14c6c1fc85cba12bfd093aa8f90e3
```


[Microsoft WCF](http://download.microsoft.com/download/9/5/E/95EF66AF-9026-4BB0-A41D-A4F81802D92C/[MS-THCH].pdf) 

```
E2EActivity: GWABtfYCDEu4hxOZR7sWGQ== 
```

More links:
https://community.dynatrace.com/community/display/docdt63/Integration+with+Web+Load+Testing+and+Monitoring+Tools

x-dynaTrace: VU=1;PC=.1;ID=4;NA=SearchPage

https://community.appdynamics.com/t5/tkb/articleprintpage/tkb-id/knowledge-base/article-id/81