1. Install Docker
 - For Linux: Follow the steps [here](https://docs.docker.com/engine/installation/linux/ubuntu/#install-using-the-repository) to install Docker
2. Pull mongo image from dockerhub:   
``docker pull mongo``

3. To start this mongo instance and map port 27017 to 27017:   
``docker run --name mistral-mongo -p 27017:27017 -d mongo --auth``

4. Create user   
``docker exec -it mistral-mongo mongo admin``    
...   
``db.createUser({user:'unit_test_user', pwd: 'run',roles: [ { role: "root", db: "admin" } ]})``

4. Run DB init verification:   
``python -m unittest discover dbinit/ "*_test.py"``   

5. Connect mongodb Docker container to unit test Docker container. More details [here](https://deis.com/blog/2016/connecting-docker-containers-1/)