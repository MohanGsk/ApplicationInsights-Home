## What is Mistral?

Mistral is a canonical app that showcases how Application Insights can be used on microservices built with popular open source technologies for Linux platform and deployed via Docker.

## Requirements
### P1s
* [P1] Monitor calls made to popular open source storage technology (examples: MongoDB, MySQL)
* [P1] Propagating context via asynchronous transactions spanning multiple services written on different technologies
* [P1] Showcasing Application Insights Java SDK
* [P1] Showcasing Application Insights Node.JS SDK

### P2s
* [P2] Showcasing queue monitoring, i.e. metrics around queue throughput, item in queue duration
* [P2] Showcasing Application Insights SDK for DJango
* [P2] Showcasing ApplicationInsights SDK for PHP


## Proposed architecture   
The diagram below demonstrates the proposed architecture. 
![architecture](architecture.JPG)

## Repo structure
* [/src/da](./src/da) - Data Access Service. Docker image: `mistral-da`
* [/src/mongo](./src/mongo) - Mongo storage. Docker image: `mongo`
* [/src/mongo/dbinit/test](./src/mongo/dbinit/test) - DB test. Docker image: `mistral-dbinit-test`
* _(Doesn't exist)_ [/src/maestro](./src/maestro) - Ingestion service. Docker image `mistral-maestro`
* _(Doesn't exist)_ [/src/pr](./src/pr) - Processing service. Docker image `mistral-pr`
* _(Doesn't exist)_ [/src/fe-java](./src/fe-java) - FE Java service. Docker image `mistral-fe-java`
* _(Doesn't exist)_ [/src/fe-python](./src/fe-python) - FE Python service. Docker image `mistral-fe-python`
* _(Doesn't exist)_ [/src/fe-php](./src/fe-php) - FE PHP service. Docker image `mistral-fe-php`

## Next steps
* Connect `da` to `mongo`. Expose a `/test` endpoint on `da` that returns database connection status
* Publish `mistral-da` and `mistral-dbinit-test` images to Microsoft image repository
* Create container cluster using Kubernetes and publish to Azure using Azure Container Service
    * Now we have official instance of `mistral` running in Azure
* `da` next steps
    * Enable raw data endpoint
    * Enable metric data endpoint
* Create Data generator and purger
* Create PHP FE
* Create Python FE
* Create View generator. Look into: [Selenium on Docker](https://github.com/SeleniumHQ/docker-selenium)
* Enable Application Insights for Docker on all Docker containers used for `mistral`




