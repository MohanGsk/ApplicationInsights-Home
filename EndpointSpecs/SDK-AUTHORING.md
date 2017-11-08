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

At this point the v2-schema definition is not available however we are working on releasing a formal definition for SDK authors to reference.

## Validating schema

An SDK author can validate data is conforming to authoring requirements by sending their payload to `https://dc.services.visualstudio.com/v2/validate`. Data sent to this endpoint will not be permenantly recorded.

## SDK Versions

See [SDK Versions](SDK-VERSIONS.md) for the information on how to report custom SDK version.