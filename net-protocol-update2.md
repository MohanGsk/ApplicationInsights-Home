# Http Correlation protocol update proposal

In order to track a complex operation in a multi-tiered system tied together using HTTP requests, it is extremely valuable to track which component requests were generated as the result of a top-most operation. To do this tracking, each request needs to have an ID that uniquely identifies the request so that parent-child relationships among requests can be tracked. 

If requests are sent via HTTP, it is necessary to pass this ID along with the HTTP packet itself. In systems where a single vendor controls communication on both sides of the HTTP pipeline, it is relatively simple to define some vendor-specific scheme, which defines a request ID and passes it in the HTTP headers. Indeed today there are a number of such schemes, but they do not interoperate with each other. 

In an environment where users might create multi-tiered applications that use components from different vendors it is useful to have uniform standard for these IDs that would give each vendor-specific logging system the information it needs to trace the parent-child relationships among requests for the system as a whole. 

Ideally such a standard
1. Provides enough structure so that each vendor-specific logging system can get the information it needs from the parts of the system that are not under the vendor's control (but conform to the standard)
2. Provides enough flexibility so vendors can easily modify their systems to conform to the standard and each vendor can innovate and provide advanced features within its logging system.

This document attempts to describe such a standard, and the rational for the design choices (which should be based in the two principles above). 

## Useful Properties (and Structure) of the ID.

The main property of the request ID is simply its uniqueness. To keep the logging system simple, most systems simply choose random ID that was chosen randomly from a large enough number space (at least 8 bytes but the trend now is to use 16 bytes) that the number is unique with high probability. While such 'flat' IDs have been used, it is also possible to encode additional information into the ID. Typical things that can be encoded include

1. A top level 'trace-id'. Most logging systems distinguish between 'top level' requests that came from 'outside' the system (e.g. from a human interaction) from a request that was generated from other requests within the system. It is very convenient if all the IDs that were generated (recursively) from the same top level ID can be easily identified. This can be done by giving the ID structure, for example,  if the ID had the structure 
```
 TRACE_ID-SUBREQUEST_ID
```
then just by parsing the ID into its two pieces, logging logic can easily determine if a particular logging message is associated with a particular top-level request (trace). The standard should allow for this. 


2. Multi-level (hierarchical) IDs. The single level trace-id concept very naturally generalizes into a multi-level system. where the ID has the form
```
 TRACE_ID-LEVEL_1_ID-LEVEL_2_ID-LEVEL_3_ID ... -LEVEL_N_ID
```
Where the logging ID is a list (path) of IDs, where each sub-system creates is ID by appending something locally unique to the ID that caused the subsystem to be activated. The standard should allow for this.

3. Important attributes. A very useful observation is that any information that is known at the time the id is created and is CONSTANT over the lifetime of the request can simply be includes as part of the id. For example, the physical location, or the machine of origin, the parent ID, or user URL could be embedded into the ID without destroying the uniqueness property. However one of the more common attributes is that it useful to encode into the id is whether particular request is meant to be sampled. The idea is most requests are NOT logged, but based on some criteria the logging system decides to mark
one request as being 'sampled' and for that request logging is done. 

One way of encoding such attributes is by adding structure to <Level N ID> component. If we used the '*' and '=' character to define the following syntax
```
 <Level N ID> -> <ID>*AttribName1=Value1*AttibName2=Value2 ...
```
Then sampling (and other) information can be encoded into the ID. In the case of sampling, there is likely to be a desire to have 'full sampling' (That is once set, ANY child request is also selected for sampling), but also having attribute that allows only '1 level sampling' (don't enable the children) or have the ability for children to explicitly turn the sampling off for itself and its children. All of these possibilities can be encoded by defining attributes which indicate
that desire. 

In the example above a fairly verbose (but very flexible) scheme of Name=Value was chosen, but frankly any unambiguous syntax can be used. For example, you could make one letter shortcuts (with a distinct prefix letter to indicate that they are shortcuts) to denote important common cases. For example using the '%' as a marker and the letter 'S' to denote sampling 
<ID>%S might be designated the canonical way of expressing <ID>*Sampling=1 to make it short and efficient to encode and parse this important case. 

The main point however is encoding attributes into the ID is useful, and the system should allow for it. It is a powerful extension mechanism. 

## Information that is not the request ID 

Having a multi-level ID with attributes allow you to encode a lot of information into IDs. However there are several important reasons to have another mechanism as well. 

1. The information changes over the lifetime of the request. If you were to encode this information into the ID it would break uniqueness as an ID. 
2. The ID gets attached to every logging message, so ideally it is short as possible. Encoding a lot of information into the ID makes it large and thus inefficient. Some systems have a fixed number of bits reserved for the ID (e.g. typically 16 bytes) and thus only a modest amount of information can be encoded into the ID. 
3. If the information is sensitive (contains personally identifiable information), there can be security reasons for not encoding into an ID that by design must be available widely.
4. If the information needs to be propagated differently (e.g. selectively, or only to a certain depth ...) to how an ID would be propagated (completely). 

The [OpenTracing](http://opentracing.io/documentation/) standard has the concept of baggage for this extra information that needs to be propagated along with the request. This information is typically used for advanced filtering sampling or control. Encoding all of this into the ID would very likely be inappropriate for the reasons above. 

The main point here however, is that there is information logging systems would like to pass along in the HTTP headers that likely will need to standardize that is IN ADDITION to the request ID. Thus an HTTP correlation protocol would ideally address this concern as well and this would be independent of the ID. 

## Concrete Design of the HTTP Request ID.

With the general requirements for the ID understood, we are in a position to design a protocol for encoding this ID into HTTP. 

Fundamentally, HTTP defines a mechanism for including string based key-value pairs in the HTTP header. Thus the 'obvious' design is to simply declare a new header tag called 'Request-Id' which declares this value
```
 Request-Id: <ID String>
```
The important point here is that from the HTTP perspective, an ID is a string (not a number), and in particular HTTP does not place (limiting) constraints on its size. Thus at the most abstract level, a Request-Id is simply a string that is designed to be unique (generated by a sufficiently large random number generator)

Thus HTTP has no problem with variable sized IDs, and allowing the ID to be of variable size is certainly useful if we wish to try to encode additional structure into the ID. However existing loggings system may require fixed sided ID (of a particular number of bits), so our design principles are in apparent conflict (different vendors might choose different designs) The recommendation here however is to devise a system where systems that have fixed size ID and systems with variable sized ID can interoperate. The basic idea is that IDs are variable sized, but the standard also defines a hashing operation that can collapse variable sized IDs to any size needed by a particular fixed size logging system. The details are covered in the section on 'Interoping with fixed ID systems'. 

Thus at the most abstract level, IDs are simply arbitrary strings, and are passed in the HTTP request as the value of the 'Request-Id' header key. 

### Design Principle: Nested refinement

It is tempting to add some sort of 'version number' to the ID for 'extensibility'. However experience shows that these version numbers often are not useful because fairly quickly there is too much code that depends on the existing semantics (and
only recognizes the current version number) and thus changing the version number becomes impossible as a practical matter. Indeed it is far more likely that if such a 'incompatible' change were to be made, it would be better to create a 
new Http header Key (e.g. Request-Id-V2) so that a request could simultaneously support the new and old formats. The main point here, is that ideally the standard allows enough flexibility so that a version number is not necessary. 

Because the ID is a string, we can get this flexibility by defining the ID in such a way that it can be parsed to find particular 'pieces'. Thus the original arbitrary string now has some structure (it has pieces). This process can be repeated 
as needed (and evolved over time), so allow all the flexibility needed to evolve the system. 

### Supporting Trace-IDs

To support Trace-ID we can use the '-' character as a separator and simply mandate that if the Id has the form
```
 TRACE_ID-SUB_REQUEST_ID
```
That is it has a '-' sign in it, then the first part can be interpreted as the trace ID and the rest can be interpreted as a sub request ID. The key property of the trace ID is that it is used for ALL sub-operations that came from that top-level request. Thus the key constraint the standard makes is that systems that DON'T follow this two-tiered convention must REFRAIN from using a '-' in their trace ID. This constraint is not a hardship. 

Notice that systems that don't support trace id still work (they just see the ID as a flat unique string), however systems that care can make use of the underlying structure (e.g. to group all logging from a particular top-level
request). 

### Reserved Characters

Note that a subtle restriction has crept into the specification. In particular component TRACE_ID can no longer be any string, because it can't have a '-' in it. Generally this not a hardship (ID's themselves are likely to only use the Base64 character
set, so it easy to avoid particular characters). We do have to be clear however on exactly what the rules are. In particular we will be reserving some characters for use by the standard (not allowed to be used in IDs), so that 
we can extend functionality in the future. 

Tentatively this list is 
 
 -!"#$%&'()*:;<->??@

Which with the exception of '-' fall in the ASCII range 0x21-0x2B and 0x3A-0x40 (TODO: we should probably avoid the special case for '-' and simply use ! as the top-most separator). 

Thus ID cannot use ANY of these characters when generating their IDs, but importantly must ACCEPT as valid ID characters if they do not understand the special semantics defined here. This insures that less aware systems 'fall back' well. 

### Supporting Multi-level IDs. 

The two level structure can easily be generalized to an arbitrary number of levels. 
```
 TRACE_ID-LEVEL_1_ID-LEVEL_2_ID-LEVEL_3_ID ... -LEVEL_N_ID
```
This syntax simply states that you can have more than one '-' in your ID and each will be one level of the multi-level ID. Systems that only understand one level of ID are free to treat all characters after the first '-' as a flat ID. Systems
the only understand flat IDs can simple treat the ID as a flat ID. Thus all systems can interoperate with the same standard. 

### Supporting attributes defined in this standard

To support attributes defined in this standard, we use the % character followed by one of characters defined below (this list will grow over time) as a suffix in any ID

 %S - (Sample) An indication that this request should be logged if possible 
 %s - (SampleOff) An indication that this request not be sampled (even if its parent request is) 
 %H8 - (Hash8) An indication that this ID has been hash to fit in 8 bytes
 %H16 - (Hash16) An indication that this ID has been hash to fit in 16 bytes

Each ID in a multi-level ID might have 0 or more of these attributes as a suffix. Thus a valid ID is
```
 2346393SAF%S-67sdfdFD34%s%H8
```
Where the TraceID has the Sample Attribute and the second tier ID has a sampling both the SampleOff attribute as well as the Hash8 attribute. The use of these attributes is explained in later sections. 

## Interoping with fixed ID systems

Some logging systems have been built with the assumption that the IDs are of fixed size (e.g. fit in 16 bytes). Thus it is problematic for these systems to 'flow' an ID that is larger than this size and thus can't support variable sized IDs that the multi-level or attribute syntax might produce. 

The solution to this is hashing. The basic idea is that the larger IDs are hashed down to a size that can fit, and flow through the fixed sized system. When this hashing happens, the ID is also marked (using attributes) on exactly how the hashing was done. When logging analysis tools are comparing IDs the attributes would consulted, if one ID was hashed and the other was not, the tool would first make sure that the IDs were hashed in the same way before making the comparison. Thus the fundamental property of uniqueness and comparison can work in a heterogeneous environment. 

After hashing, the IDs that were generated from variable length strings will look like any other fixed size ID. This means that the fixed size logging system won't know if the ID is a 'native' fixed size or if it was hashed down to be fixed size. This issue can be avoided if the hash function is idempotent. If the hash function returns the same value if it already fits, then the fixed size logging system can always report that the ID has been hashed. If it was native fixed size id, the hash will return the same value and ID comparison will simply work. 

There is also an issue if you have a multi-tiered, two tiered, and flat system because there are multiple ways that the multi-tiered ID could be hash (it might to directly to flat, or it migth be first flatted to a two tier, and then hashed to be flat). One solution is to NEVER go directly to a flat ID from a multi-tiered ID. Instead first convert to a two tiered (using 16 bytes for the trace ID, and 8 bytes for the spanID), and then convert to the flat system. This is not a fuly general solution, but it is likely to cover all important cases. 

TODO : define the hash (it can be something trivial like xor bits in N byte quantities)

TODO : It can be useful to encode some important attributes (e.g. the Sampling) attribute into the fixed-sized ID. This requires that the fixed sized system allocate one of its bits to such attributes. We should encourage fixed-sized ID systems to do this for important attributes. 

## Extensibility

As defined so far the '-' and '%'characters have been given specific meaning, and we have reserved a number of others specifically so that this standard could be extended to allow more structure to be defined for IDs. Existing systems will effectively ignore the special meaning of these newly used characters, but will still treat them as part of the ID structure that it currently knows about. This provides for useful extensibility in the future. 

## Best practice for encoding IDs

There is a strong requirement that ID avoid the reserved characters noted above (- and 0x21-0x2B, 0x3A-0x40). 

While not a strict requirement, because the characters will be UTF8 encoded, it is best to avoid non-ASCII characters. 

The expectation is that traceIds will be 16-byte random numbers that are encoded as [Base64](https://en.wikipedia.org/wiki/Base64) strings. If systems encode TraceIds in this way, then common systems with fixed TraceID widths will be able to pass the ID through their system without needing to hash. 

It is recommended that ID numbers be turned into Base64 strings (instead of for example printing as Hex). However there is no strong requirement for this, it is just smaller. 

IDs for the second and subsequent tiers of a multi-tier ID need only be unique with the context of their parent ID. Thus these can be small numbers (since you only need unique numbers for the number of children a particular request has). To keep ID as short as possible, it is recommended that you keep these numbers small. 

## Passing Context Information (Beyond the ID)

As mentioned, there may be a desire to pass additional information that is not part of the ID through the HTTP header. This information is likely to be simply a collection of key-value pairs that will have some semantic meaning to the logging system. 

The suggestion is that we encode this in the most 'natural' way possible for HTTP headers. In particular HTTP already has the concept that a header key might have multiple values (which are comma-separated) and and there is a natural KEY=VALUE syntax for representing key value pairs. 

Thus the suggestion is to define a new header entry called Correlation-Context (the name is arbitrary but conveys that this is for logging correlation) and that its value is a comma separated set of key-value pairs. 

Correlation-Context: Key-value pairs

Note that by making this a separate HTTP header we decouple it from IDs. Thus this is effectively and independent design issue. Standardization of this header can proceed independently (in particular we would not NEED to do it now)
