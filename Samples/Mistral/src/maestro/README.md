## Maestro Ingestion Service Installation Instructions
1. Install Docker
    - For Linux: Follow the steps [here](https://docs.docker.com/engine/installation/linux/ubuntu/#install-using-the-repository) to install Docker
2. Pull node image from dockerhub:   
``docker pull microsoft/dotnet:1.1-sdk``

3. Build docker image 
``docker build -f ./maestro.dockerfile -t mistral-maestro .``

4. Start docker container
``docker run --name mistral-maestro -p 49161:5000 -d mistral-maestro``

5. Test that it is working. `alive` should be returned   
``curl -i localhost:49161/api/ping``