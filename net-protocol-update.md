# Http Correlation protocol update proposal

## Trace-Context modifications

Http correlation protocol with the multiple levels of semantic. 

Issues with the proposed `Trace-Context` format:
- Limited to the fixed length identifier 
- Limited to specific semantic of identifier
- Lengthy string

Goal - a single set of headers with the defined semantic. Special syntax or limitations may be applied inside a single system. However as far as system followed the semantic - it can be understood in inter-operability scenarios.

Take example of `User-Agent` string. Systems may have understanding of user agent on multiple levels. Some just trace the whole string, others detect mobile browsers to redirect to mobile site and some checks for the special features present.

Current format:

```
Trace-Context: 00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01 
base16(<Version>) = 00 
base16(<TraceId>) = 4bf92f3577b34da6a3ce929d0e0e4736 
base16(<SpanId>) = 00f067aa0ba902b7 
base16(<TraceOptions>) = 01  // sampled 
```

Specific changes in `Trace-Context` format:
1. Expect ANY (with the reasonable limitations on characters set) string as a request identifier. Treat it as a `SpanId`.
2. Treat `Version` prefix as `Format` prefix. Define magic numbers for well-known formats as an extension to the spec. One can be for Zipkin&Census, one for Microsoft's hierarchical ID.
3. In both formats define the `trace-id` as `base64` of 16 bytes number without the padding symbols. 
4. For unknown format number - expect `trace-id` to be ANY string all the way to the next `-`.
5. If `Format` prefix exists and trace was parsed - expect ANY string after the second `-`. 
6. Do not define flags inside the identifier header. Use separate name-value collection header for this purpose.

As an extra addition - change name to be easy to understand by people who not working on tracing every day. Proposed names - `Request-Id` and `Correlation-Context`.

There is a .NET http protocol defined [here](https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/HttpCorrelationProtocol.md). Main differences with the proposal is that it uses magic character `|` for format specification, not a `Format` prefix. Also `TraceId` even though present - not necessary have a fixed length and it's not a part of specification. Also .NET protocol uses other characters (not `-`) as delimiters to avoid "collisions" with GUIDs. 

Those modifications are insignificant for the proposed `Trace-Context` protocol and .NET protocol. In Microsoft we will be working we can plan to update the format built in to .NET to absorb those modifications.

Please note - any other providers can fit their identifiers in this protocol. 
- Heroku allows random numbers in ID. This is supported
- Amazon has longer span-ID and include timestamp into it. They also propagate name-value collection. It can be easily adopted
- Looking at docs publicly available for [Dynatrace](https://community.dynatrace.com/community/display/docdt63/Integration+with+Web+Load+Testing+and+Monitoring+Tools) and [AppDynamics](https://community.appdynamics.com/t5/tkb/articleprintpage/tkb-id/knowledge-base/article-id/81) - proposed correlation protocol can fit their existing payload.


## .NET format modifications

.NET libraries recently adopted the new [Http Correlation protocol](https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/HttpCorrelationProtocol.md) for distributed tracing. Protocol is designed to be feature-rich, but allows to fallback into very simple semantic.

Application Insights is working on implementing this protocol on other languages.


Interop is important. Scenarios:

- Web tests provider sends requests to monitored applications. For failed web tests it's easy to investigate having the correlation identifiers
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

Changes:
1. `Request-Id` is a unique identifier of every request. Remove provision on force making it unique
2. trace id and format version
3.  

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