# Overview

This document describes the protocol for client requests and responses to the data collection endpoint.

The data collection endpoint address is `https://dc.services.visualstudio.com/v2/track` and communication with it is by HTTPS.

## Client Request

A client request is a standard HTTP POST to the endpoint.

The endpoint accepts gzipped data specified by the Content-Encoding header and it's use is recommended.

The endpoint accepts body payloads in JSON and [line-delimited JSON](https://en.wikipedia.org/wiki/JSON_Streaming#Line_delimited_JSON) specified by the Content-Type header.

| Header           | Supported values                |
|------------------|---------------------------------|
| Content-Type     | x-json-stream, application/json |
| Content-Encoding | gzip                            |

## Endpoint Response

A data collection endpoint response is a standard HTTP response with a JSON payload.

### Response Status Codes

Clients are expected to handle the status codes in the response properly to ensure complete delivery of their data. Specifically, the data collection endpoint propogates transisent errors back to the client and expects the client to retry and send the data again.

Two distinct status codes are returned. The first, being the HTTP Status Code on the HTTP response. The second, being an individual status code for each telemetry item that was not accepted in the request batch. This allows clients to handle the status of each telemetry item individually. See [Response Body](#Response Body) for detailed information on individual error status codes. Only items rejected will have an individual status code returned in the repsonse body. Items accepted can be assumed to have a status code of 200.

The status represented by both the HTTP Status Code and the Telemetry Item statusCode is the same.

Status codes that have a specific endpoint status distinct from the general HTTP status are listed below.

| Status Code                 | Endpoint Status    | Should retry? | Description                                                                                                                              |
|-----------------------------|--------------------|---------------|------------------------------------------------------------------------------------------------------------------------------------------|
| 200 - OK                    |      Accepted      |       No      | All of the telemetry items were accepted and processed.                                                                                  |
| 206 - Partial Content       | Partially accepted |     Maybe     | One or more of the telemetry items were accepted and processed.                                                                          |
| 400 - Bad Request           |    Not accepted    |       No      | Invalid data was provided and the payload was rejected.                                                                                  |
| 402 - Payment Required      |    Not accepted    |       No      | Quota was exceeded and the telemetry items were rejected.                                                                                |
| 429 - Too Many Requests     |    Not accepted    |      Yes      | Burst throttling limit was exceeded and the telemetry items were rejected. Client should retry according to the retry policy. |
| 500 - Internal Server Error |    Not accepted    |      Yes      | An error occurred and the telemetry items were not processed.                                                                            |
| 503 - Service Unavailable   |    Not accepted    |      Yes      | Service is currently unavailable. Client should retry according to the retry policy.                                          |


**Note:**

 * 429 & 503 should retry according to the [Retry-After policy](#Retry-After Policy).
 
 * If one or more items but not all were accepted, 206 (Partial Accept) is returned as the HTTP Status Code. Each individual item may have a specific error status code as defined by the response body definition below allowing the SDK to know the status of specific items.

 * If no items were accepted, the endpoint returns the status code of the first item in the payload. Each individual item may have a specific error code as defined by the response body definition below.


### Response Body

The HTTP response body contains a JSON object with the status of each telemetry item.

**Response JSON document definition**

| Field         | Description
|---------------|-------------------------------------
| itemsReceived | The number of items received 
| itemsAccepted | The number of items accepted
| errors        | An array of error detail objects

**Errors Detail object definition**

| Field         | Description
|---------------|-------------------------------------------
| index         | The index in the original payload of the item 
| statusCode    | The item specific [HTTP Response status code](#Response Status Codes)
| message       | The error message

An example of a response body is:

```
{
  "itemsReceived": 5,
  "itemsAccepted": 3,
  "errors": [
    {
      "index": 0,
      "statusCode": 400,
      "message": "103: Field 'time' on type 'Envelope' is older than the allowed min date. Expected: now - 172800000ms, Actual: now - 226881806ms"
    },
    {
      "index": 2,
      "statusCode": 500,
      "message": "Internal Server Error"
    }
  ]
}
```

This payload would have a HTTP Status Code of 206, as 3 items were accepted and 2 items were rejected.

### Retry-After Policy

Clients should expect errors in the response body and are expected to retry using one of the two methods. Using the Retry-After response header value or an exponential back-off policy.

#### Retry-After

The [Retry-After](http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.37) header value indicates when the client should retry. 

An example is:

```
  Retry-After: Fri, 22 Apr 2016 23:59:59 GMT
```

The client should not retry before the specified time.

#### Exponential Back-Off

If the Retry-After header is not available, such in the case of a 500 status code, the client should retry the request using the [Exponential back-off](https://en.wikipedia.org/wiki/Exponential_backoff) method.