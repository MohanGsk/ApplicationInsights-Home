## DA Service Installation Instructions
1. Install Docker
    - For Linux: Follow the steps [here](https://docs.docker.com/engine/installation/linux/ubuntu/#install-using-the-repository) to install Docker
2. Pull node image from dockerhub:   
``docker pull node``

3. Build (or update) docker image 
``docker build -f da.dockerfile -t microsoft/sample-mistral:da_0.2 .``

4. Start docker container
``docker run --name microsoft/sample-mistral:da_0.2 -p 49160:8080 --link mistral-mongo -d mistral-da``

5. Test that it is working. `ok` should be returned   
``curl -i localhost:49160/checkdb``

6. Commit changes and push docker image to repo    
  ``docker push microsoft/sample-mistral:da_0.2``