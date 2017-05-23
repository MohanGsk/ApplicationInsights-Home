## DA Service Installation Instructions
1. Install Docker
    - For Linux: Follow the steps [here](https://docs.docker.com/engine/installation/linux/ubuntu/#install-using-the-repository) to install Docker
2. Pull node image from dockerhub:   
``docker pull node``

3. Build docker image 
``docker build -f ./da.dockerfile -t mistral-da .``

4. Start docker container
``docker run -name mistral-da -p 49160:8080 --link mistral-mongo -d mistral-da``

5. Test that it is working   
``curl -i localhost:49160/events``