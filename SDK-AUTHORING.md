# SDK Authoring

This document describes the content requirements for SDKs when sending telemetry to the data collection endpoint.

SDK authors should also read [Endpoint Protocol](ENDPOINT-PROTOCOL.md) to ensure they conform to the request-response protocol.

## Content Overview

As defined in the [Endpoint Protocol](ENDPOINT-PROTOCOL.md) specification, the body of a request should contain one or more telemetry items in JSON form.

## JSON Schema

The endpoint accepts JSON documents that conform to a specific schema currently referred to as `v2-schema`.

The schema has three main parts:

1. An outer envelope.
2. A property bag called tags.
3. A type-specific data object.

## Validating schema

An SDK author can validate data is conforming to authoring requirements by sending their payload to `https://dc.services.visualstudio.com/v2/validate`. Data sent to this endpoint will not be permenantly recorded.

## Deprecation of old SDK versions

Old SDK versions will be deprecated and rejected by the endpoint according to the following policy.

 1. SDK version does not meet the [SDK Version specification](#SDK Version specification)
 2. SDK version is semver pre-release version and a major version of 1+ has been released.
 3. SDK version is 2 majors versions older than the currently released major version.
    - E.g. If the current release version is 5.0.0, all versions lower than 3.0.0 will be rejected.

## SDK Version Specification

SDKs are required to include their name and version in the telemetry item using the `ai.internal.sdkVersion` tag conforming to the format below.

E.g.

```
{
  "tags": {
    "ai.internal.sdkVersion:" "dotnet:2.0.0"
  }
}

### SDK Version Format

```
  [PREFIX_]SDKNAME:SEMVER
```  

| Section          | Required | Description                                                             | Example |
|------------------|----------|-------------------------------------------------------------------------|---------|
| Prefix           | No       | An optional single lowercase letter (a-z) followed by an underscore (_) | a_      |
| SDK Name         | Yes      | An alpha lowercase string (a-z)                                         | dotnet  |
| Semantic Version | Yes      | A [Semantic Versioning](http://semver.org/) compatible version string   | 2.0.0   |

SDK name and semver are delmited by a single colon (:).

### Examples

```
  r_dotnet:2.0.0-12345
    dotnet:2.0.0-beta.1
  | ------ ------------
  |    |        |
  |    |        +-------> Semantic Version Format
  |    |
  |    +----------------> SDK Name
  |
  +---------------------> Prefix (optional)
```