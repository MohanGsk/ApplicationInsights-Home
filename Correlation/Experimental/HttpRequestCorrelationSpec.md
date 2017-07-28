# Http Request Correlation Specification

In order to track a complex operation in a multi-tiered system, it is important to track the relationships between individual actions in this operation. For systems communicating over http protocol, each request needs to carry a context that can be used to build up this relationship.

If requests are sent via HTTP, it is necessary to pass this context along with the HTTP packet itself. In systems where a single vendor controls communication on both sides 
of the HTTP pipeline, it is relatively simple to define some vendor-specific scheme, which defines a request context and passes it in the HTTP headers. Indeed today there are 
a number of such schemes, but they do not interoperate with each other.

There are environments where multi-tiered applications uses components from different vendors. Some of those components may be out of the user control due to organizational silos. Some be part of a public cloud infrastructure and controled by cloud provider. Some may have pre-baked support for distributed traces that cannot be altered. It is useful to have uniform standard for the distributed context semantic and a format that would give each vendor-specific logging system the information it needs to 
trace the relationships among requests for the system as a whole. 

Most of the vendors defines two pieces of request context - unique identifier of the request and properties defining the overall complex operation (distributed trace). The [OpenTracing](http://opentracing.io/documentation/) Baggage 
concept [spec](https://github.com/opentracing/specification/blob/master/specification.md))
needs exactly this feature.   This mechanism can be used to propagate logging control 
information for more advanced logging features, and would likewise benefit from 
standardization.  

Ideally such a standard
1. Provides enough structure so that each vendor-specific logging system 
can get the information it needs from the parts of the system that are not
under the vendor's control (but conform to the standard)

2. Provides enough flexibility so vendors can easily modify their systems 
to conform to the standard and each vendor can innovate and provide advanced
features within its logging system.

This document describes such a standard, and the rational for the design 
choices (which should be based in the two principles above). 

# Http Header Fields

Http has the concept of a [Header Fields](https://tools.ietf.org/html/rfc7230#section-3.2)
for passing addition information with the request. This standard suggest defining  
the following new header fields.

* `Request-Id`: A string representing the unique identifier for this request.
* `Correlation-Context`:  A list of comma separated set of strings of the form KEY=VALUE.
These values are intended for control of logging systems and should be passed along
to any child requests.  

The exact syntax for the values of both of these fields is given in its own section below.

## The Request-Id Field

Section [The Expected Environment for Using Request-Id](#The Expected Environment for Using Request-Id) explains motivation of this format.

This protocol defines a strict `Request-Id` field format and a set of fall back rules that allows to experiment and move the protocol forward while keeping systems following the protocol with the required information.

`Request-Id` should follow the following format:

```
<trace-id>[-<span-base-id>[.<level>.<level>]]
```

Where:
- `<trace-id>` - identifies overall distributed trace. 22 base64 characters (* 6 = 132 bits ~= 8 bytes).
- `<span-base-id>` - Optional: defines the span that is base for the hierarchy of other spans. Not more than 11 base64 characters.
- `<level>` - Optional: sequence (or unique random) number of a call made by specific layer. Not more than 11 base64 characters.
- maximum length of the header value should be 1024 bytes.

There are three types of operations that can be made with the `Request-Id`:
- **Extend**: used to create a new unique request id.
    Request-Id = `3qdi2JDFioDFjDSF223f23-SdfD8DF908D.Ao.1`
    Extend(Request-Id) = `3qdi2JDFioDFjDSF223f23-SdfD8DF908D.Ao.1.+W` (`+W` represents two random base64 characters)
- **Increment**: used to mark the "next" attempt to call the dependant service
    Request-Id = `3qdi2JDFioDFjDSF223f23-SdfD8DF908D.3`
    Increment(Request-Id) = `3qdi2JDFioDFjDSF223f23-SdfD8DF908D.4`
- **Reset**: used to reset the long hierarchical string or as a replacement for either **Extend** or **Increment**.
    Request-Id = `3qdi2JDFioDFjDSF223f23-SdfD8DF908D`
    Reset(Request-Id) = `3qdi2JDFioDFjDSF223f23-MGY+gOT/kgZ`

This protocol expects every actor in a system to modify the `Request-Id` using one of the actions above. There are three scenarios how this protocol can be used.

### Scenario 1. Reset all the time

Resetting of request is the most straightforward operation. Reset can also benefit from existing request identifiers that server like nginx may already have.

```
Client sends: 3qdi2JDFioDFjDSF223f23
    A logs request: Reset(3qdi2JDFioDFjDSF223f23) => 3qdi2JDFioDFjDSF223f23-MGY+gOT/kgZ 
                    with the parent 3qdi2JDFioDFjDSF223f23

    A sends request to B: Reset(3qdi2JDFioDFjDSF223f23-MGY+gOT/kgZ) => 3qdi2JDFioDFjDSF223f23-5NHVoZG5NTm
        
        B logs request: Reset(3qdi2JDFioDFjDSF223f23-5NHVoZG5NTm) => 3qdi2JDFioDFjDSF223f23-MoeykjJSsoJ 
                        with the parent 3qdi2JDFioDFjDSF223f23-5NHVoZG5NTm

    A sends request to C: Reset(3qdi2JDFioDFjDSF223f23-MGY+gOT/kgZ) => 3qdi2JDFioDFjDSF223f23-F1YWhhcm5mM
        
        C logs request: Reset(3qdi2JDFioDFjDSF223f23-F1YWhhcm5mM) => 3qdi2JDFioDFjDSF223f23-OTh5NHVoZG5 
                        with the parent 3qdi2JDFioDFjDSF223f23-F1YWhhcm5mM
```

### Scenario 2. Extend + Increment all the time

Extend and Increment are very useful for very lossy telemetry systems. With only few requests 

```
Client sends request to A: 3qdi2JDFioDFjDSF223f23
    A logs request: Extend(3qdi2JDFioDFjDSF223f23) => 3qdi2JDFioDFjDSF223f23.Wf
                    with the parent 3qdi2JDFioDFjDSF223f23

    A sends request to B: Increment(3qdi2JDFioDFjDSF223f23.Wf) => 3qdi2JDFioDFjDSF223f23.Wf.0
        
        B logs request: Extend(3qdi2JDFioDFjDSF223f23.Wf.0) => 3qdi2JDFioDFjDSF223f23.Wf.0.mX
                        with the parent 3qdi2JDFioDFjDSF223f23.Wf.0

    A sends request to C: Increment(3qdi2JDFioDFjDSF223f23.Wf.0) => 3qdi2JDFioDFjDSF223f23.Wf.1
        
        C logs request: Extend(3qdi2JDFioDFjDSF223f23.Wf.1) => 3qdi2JDFioDFjDSF223f23.Wf.1.dk 
                        with the parent 3qdi2JDFioDFjDSF223f23.Wf.1
```

### Scenario 3. Increment on load balancer. Reset on tracer

Load balancers and proxies may be a complete black box for the tracing system. So it may be useful to preserve the correlation for the trace went thru it. Especially for the multiple re-tries scenarios. In the example below you can correlate the request sent from A `3qdi2JDFioDFjDSF223f23-5NHVoZG5NTm` with the request logged by B `3qdi2JDFioDFjDSF223f23-M2QgMWEgYjA` as it's parent is `3qdi2JDFioDFjDSF223f23-5NHVoZG5NTm.Wf.0`.

```
Client sends request to A: 3qdi2JDFioDFjDSF223f23
    A logs request: Reset(3qdi2JDFioDFjDSF223f23) => 3qdi2JDFioDFjDSF223f23-MGY+gOT/kgZ 
                    with the parent 3qdi2JDFioDFjDSF223f23

    A sends request to B: Reset(3qdi2JDFioDFjDSF223f23-MGY+gOT/kgZ) => 3qdi2JDFioDFjDSF223f23-5NHVoZG5NTm
        
        B logs request: Extend(3qdi2JDFioDFjDSF223f23-5NHVoZG5NTm) => 3qdi2JDFioDFjDSF223f23-5NHVoZG5NTm.Wf
                        with the parent 3qdi2JDFioDFjDSF223f23-5NHVoZG5NTm

        B sends request to C: Increment(3qdi2JDFioDFjDSF223f23-5NHVoZG5NTm.Wf) => 3qdi2JDFioDFjDSF223f23-5NHVoZG5NTm.Wf.0
        
                C logs request: Reset(3qdi2JDFioDFjDSF223f23-5NHVoZG5NTm.Wf.0) => 3qdi2JDFioDFjDSF223f23-M2QgMWEgYjA
                        with the parent 3qdi2JDFioDFjDSF223f23-5NHVoZG5NTm.Wf.0
```

### Fallback options

When the format of `Request-Id` does not match the expected format the following fallback options should be applied:

#### `<trace-id>` format mismatch

Any string with the allowed characters up to first `-` or to the very end of the string should be treated as a `<trace-id>`. Depending on vendors limitation protocol defines four behaviors in this priority order:

1. Use `<trace-id>` to log trace and propagate further even if do not match the expected format.
2. Use derived `<trace-id>` (hashed value) to log trace and propagate the **original** value further. 
3. Use derived `<trace-id>` (hashed value) to log trace and propagate the hashed value further.
4. Restart the trace with the fresh `Request-Id` that matches the format.

For hashing - use algorithm described in [Hashing for Fixed Sized ID Systems](#Hashing for Fixed Sized ID Systems) to hash.
When recording hashed value - consider storing an original string as an extra property.


#### `<span-base-id>.<level>.<level>` mismatch

Some vendors may experiment with the `span-id` format. Use longer `<span-base-id>` or use characters other than base64 and `'.'` to record it. Protocol requires to apply **Reset** function to the strings like this. 

## The Correlation-Context Field

The Correlation context is straightforward if the value does not end with whitespace 
or contain newlines or commas.   We will need a standard for escaping these characters if
we wish to support arbitrary values. 

The suggestion is to use \ as an escape character like what is used in java/c# including the \uXXXX
escape for arbitrary unicode characters.   The comma (and trailing space) must be escaped if it would
otherwise have a special meaning:

TODO: We could force JSON like conventions for strings (thus quoted values), but that seems strictly more complex.


# The Expected Environment for Using Request-Id

The fundamental use of a Request-Id is to link an activity that issues a HTTP
request with the code that runs (and produces logging messages) that services the
request.   It is useful to have a overview of how such systems typically work to frame
the design of the ID itself.  

The expectation is that the logging system has the concept like 
[OpenTracing's Span](https://github.com/opentracing/specification/blob/master/specification.md)
which represents some work being done that has a ID as well as parent-child relationships
with other Spans.   Thus there are 'top level' Spans that do not have a parent, and every
other Span remembers the ID of its parent as well as its own ID.   Somewhere in the logging
system a table will be maintained that allows a Span to be located given its ID.  Every 
logging message will also be marked with the Span that is currently active, and as a result
the logging system can group together all logging messages from any given Span (or any 
of its children).   

The requirements for the Request-ID for such an environment is pretty minimal.  It simply
needs to be unique to the Span.   The expectation is that such IDs will be generated by 
choosing a random number from a large space of possible numbers.   Number sizes from 64 bits
(8 bytes) to 128 bits (16 bytes) have been commonly used.   

The expectation is that when some 'top level' activity is running, it will generate a 
new ID and log the necessary bookkeeping information for that Span.  There may be additional
Spans generated as part of the processing but eventually an HTTP request is made.  As part
of sending the HTTP request, a new Span (with a unique ID) is created (and logged), and that 
Span's ID is inserted into the Request-Id field of the HTTP request's header.  When the request 
is received a new Span (with at new ID) is created  with a parent ID as specified by the 
Request-Id field (and logged) to represent the server side processing.   The whole sequence
can repeat in the case of nested requests.   

The result is that the logging system tags every message with the active Span, and every 
Span has an ID and its parent ID, which form a tree representing the causality of the 
execution of the multi-tiered application.  

## Simple Sampling with Request-Id

It is very likely that most systems will want to implement some form of Sampling to avoid
overloading the logging system.   On simple way of doing this is to simply not emit the 
Request-Id header field in the request.   This is both simple and efficient (no overhead
in when a particular request is not being logged).   This works well when logging control
is given to the top most activities in the system, but if this is not the case then you 
lose 'caller context' if you start tracing at some intermediate point.    Often systems 
wish to have this caller context available for rare logging (e.g. when errors happen), 
which means that you need to send a Request-Id with all request.   Nevertheless this 
mechanism is useful and is easily supported.

## Improving on Flat IDs

While the system described above works, it has several disadvantages 

1. It requires a table that maps from ID -> Span that is large O(#Spans).  
2. If ANY entries in the table are missing (e.g. because some components did not log properly), 
then some parent-child chain can be broken and cannot be recovered.   Effectively potentially large
chunks of the processing of a request might become 'orphaned' and not properly attributed to 
the top-level request that was responsible for it.  

These and other limitation can be avoided if IDs are given structure.   It is common to employ
a two-level structure where IDs have the form

    TOP_LEVEL_ID - SPAN_ID

Basically all children of a given top level Span have the same prefix.    With this ID structure
given any two Spans ID, they can be determined as belonging to the same top-level processing 
simply by comparing their IDs.   This has the following advantages

1. Is very fast
2. Does not need a global table 
3. Works in the presence of missing information on intermediate Spans.   

### Hierarchical IDs

The two level structure generalizes very naturally into a multi-level (hierarchical) ID.   THus an
ID would have the form

    TOP_LEVEL_ID - LEVEL_1_ID - LEVEL_2_ID - ... - LEVEL_N_ID

which creates ID for every Span by taking the parent's ID and adding a unique suffix.   With such 
a structure, it is possible to group things not only by the top level Span but also every intermediate 
level as well.   

### Encoding Additional Information into an ID

The previous section shows how the parent-child relationship can be encoded into the ID itself.  
In general ANY information that is CONSTANT over the lifetime of the Span can be encoded into the ID 
as well without destroying its utility as an ID (which just requires uniqueness).   For example the
start time, or start location could be part of the ID.   However doing this make the ID bigger and 
generally is not worthwhile unless the information is small and high value.   Candidates for such 
information high value, small size include

1. Control information (like a verbosity bit (or bits))
2. Format and interpretation bits (e.g. what version or semantic properties of the ID)

## Fixed (Bounded) Size IDs

Generally speaking fixed sized structures are easier to implement than variable sized structure.   
Thus there is non-trivial value if the IDs can be kept to a fixed maximum size.   Indeed many existing
logging systems mandate a particular size for the IDs (typically 64 or 128 bits).  

Note that systems that mandate fixed size IDs have a problem with hierarchical IDs (which have unbounded size).  
If a fixed-sized ID system receives a ID that is to large what can it do?   The solution is hashing.
Clearly a variable sized ID can be hashed to any particular fixed size without destroying its ability
to act as a unique ID.  However if some parts of the system hash (because they are fixed size), and other
do not, then in order to compare IDs it must be possible to look at both IDs, recognize that one is hashed
and then know how to perform the hash on the other before doing the comparison.   

# The Request-Id Field

The Request-ID is defined to be a variable length string of printable ASCII characters 
(from 0x20 through 0x7E inclusive).   This string uniquely identifies the request within 
the system as a whole, and is expected to be unique because some part of the ID was
chosen randomly from a large (64 bit or greater) number space.    Logging systems that
have only flat IDs will simply emit this ID as a string.   It is recommended that 
punctuation in the ASCII range 0x21-0x2A be reserved for use by this standard in the future.  

## Base64 encoding of binary blobs  

[Base64](https://en.wikipedia.org/wiki/Base64) is a standard way of encoding binary blob as 
a sequence of printable ASCII characters.   The characters used are confide to the alpha-numeric 
and two punctuation characters (+ and /).   Because there are 64 legal possible characters each 
one represents 6 bits of binary data.  

## Expected Output Format for Two Level IDs  

Systems that support two levels if hierarchy are expected to emit the two IDs (the top level ID 
and the Span ID separated by a '-'.    It is expected that these two IDs are will be encoded
using Base64.   For example a system having a 16 byte top level ID and a 8 byte Span ID could
emit an ID like the following
```
    3qdi2JDFioDFjDSF223f23-SdfD8DF908D
```
That is it would have 22 characters (* 6 = 132 bits ~= 8 bytes) for the top level ID and 11 characters
for the secondary ID (8 bytes)

It is recommended that the top level ID use 16 bytes of binary data for its top level ID space and
encode it using Base64.   This results in 22 characters for the top level ID.    

## Expected Output Format for a Multi-Level ID

Systems that support multiple levels of hierarchy will use a '.' to TERMINATE all but the top level
of the ID hierarchy.  Base64 is recommended for all the component IDs.   Like the two level system
the top level ID needs to be large so as to guarantee system-wide uniqueness.  Identifiers after the
top level only need to be unique within the context of the parent ID and thus can typically be very small 
An example might be:
```
    3qdi2JDFioDFjDSF223f23-A.3.B.q.3S.34.3.42.2.A.B.C.
```

Having the '.' be a terminator (rather than a separator) avoids ambiguous matches using simple
string comparison (for example 3qdi2JDFioDFjDSF223f23-A.3.B.q.3. should not be interpreted as 
a parent of 3qdi2JDFioDFjDSF223f23-A.3.B.q.3S. which it would be if the '.' just separated the 
levels).   

It is strongly recommended that top level ID be a 16 byte Base64 encoded ID.   This will give 
maximum compatibility with systems that use fixed size ID for the top level ID.   

### Overflow Syntax for Multi-Level IDs

Multi-level IDs can grow arbitrarily long.   As a practical matter, systems are likely to want to 
have a limit on the size of this ID, to keep performance reasonable in unusual cases like infinite
recursion.   To indicate this truncation end the node with a '#' character instead of a '.'  For example 
the ID  
```
    3qdi2JDFioDFjDSF223f23-A.3.3d43Ds#
```
indicates truncation because it does not end with a '.'  It might represent the sequence of requests 
```
  3qdi2JDFioDFjDSF223f23-A.           caused request
  3qdi2JDFioDFjDSF223f23-A.3.         which caused request
  3qdi2JDFioDFjDSF223f23-A.3.5.       which caused request
  3qdi2JDFioDFjDSF223f23-A.3.5.1.     which caused request
  ...
  3qdi2JDFioDFjDSF223f23-A.3.5.1.1.1.1.1.1.1.1.1.1.1.1.1.
```
But the system decided the IDs had grown too long and truncated it to 
```
    3qdi2JDFioDFjDSF223f23-A.3.3d43Ds#
```
Where the 3d43D is a value that insures uniqueness for the ID as a whole but no longer
represents the detailed parent-child relationship.   This allows the system to 'fall back gracefully' 
but still detect that truncation has happened.

Note that if the truncation happens as the last node, the trailing '#' be dropped.  Thus 
```
    3qdi2JDFioDFjDSF223f23-A.3.3d43Ds
```
is the same as
```
    3qdi2JDFioDFjDSF223f23-A.3.3d43Ds#
```
This makes the  the two-level syntax perfectly matches this truncation syntax.  Thus a two-level ID 
```
    3qdi2JDFioDFjDSF223f23-SdfD8DF908D
```
is interpreted as a multi-level ID  that it two levels, but because it does not end in a .
there is truncation at level 2 (which is exactly true).  

## Input parsing for Two (or more) Tier IDs 

In systems that support to or more levels, when reading in the ID it will search for a '-' Any characters
before the first '-' are considered the top level ID,  Anything after it is something that make the ID
unique within that top level scope.   In this later component the multi-level IDs can be parsed by looking
for '.' characters.  

## Hashing for Fixed Sized ID Systems

Systems that require the IDs to be a fixed size must hash IDs to fit into the required size (e.g. 16 
bytes or 8 bytes)  This must be one with the hash algorithm defined here which we call HASH_N (where
N is the number of bytes of the resulting binary blob).   HASH_N is describe precisely in the appendix
but it is basically a Base64 decoder modified to accept any input characters, and to circularly XOR
the output bytes until the input is consumed (hash).   This hash function has the following useful characteristics

1. It input can be anything string,
2. Its output is always N bytes.
3. When operating on Base64 encoded input that of size N it produces the same result as simply decoding (no information loss).
4. It is as efficient as Base64 decoding.   

This mechanism has the following useful ramifications:

1. IDs from two like minded systems work without any loss of information.  
2. Multi-tiered and two-tiered systems interoperate on the top-most their (since they both have a top tier ID)
3. A two-tiered fixed-size ID system can accept ANY Id, and in particular an ID from a multi-tiered ID system.
4. ID  have a useful comparison operator that work even in this mixed case.  Basically if the IDs don't match 
perfectly, you also test if they match if HASH_N is applied to both (you may need to do this twice, once for 
the N of the first operand, and once for the N of the other operand).  

## Practical Impacts of the standard

It is useful at this point to give some examples of what practical impacts would be on various logging systems.

### A two tiered ID system with a fixed 16 byte top ID and an fixed 8 byte secondary ID.  

For this type of logging system, when writing the ID to the HTTP header, all that is necessary is to conform
to the syntax specified about.   This means encoding the two IDs using Base64 and outputting them separated
by a '-'.  

When reading the Request-Id field, instead of using a Base64 decoder that would fail on illegal inputs they
would look for the '-' in the ID, and use HASH_16 for the part before the '-' and HASH_8 for the id after
the dash.   These routines will be just as fast as a normal Base64 decoder. 

### A multi-Tiered (variable sized) ID.   

A multi-level logging system should not have ID length limitations so it should accept the IDs without
modification.   The only thing such systems need to do is follow the syntax for the multi-level ID
(e.g. used '-' for the first level and '.' (termination) for all other levels).  



## Well Known Correlation-Context keys:

TODO COMPLETE: 

# Appendix

## Hash_N Algorithm

TODO Finish.  