1. Install Docker
 - For Linux: Follow the steps [here](https://docs.docker.com/engine/installation/linux/ubuntu/#install-using-the-repository) to install Docker
2. Pull mongo image from dockerhub:   
``docker pull mongo``

3. To start this mongo instance and map port 27017 to 27017:   
``docker run --name mistral-mongo -p 27017:27017 -d mongo --auth``

4. Create user   
``docker exec -it mistral-mongo mongo admin``    
``db.createUser({user:'unit_test_user', pwd: 'run',roles: [{ role: "userAdminAnyDatabase", db: "admin" }, { role: "root", db: "admin" } ]})``

4. Run DB init test with local python:    
``python -m unittest discover dbinit/ "*_test.py"``   

5. To run tests from docker container use the following command. Note the `--link` option, linking python unit test container to mistral-mongo container created in the previous step:    
``
docker pull python && docker build -f ./test.dockerfile -t mistral-dbinit-test . &&  docker run -i -t --link mistral-mongo --name=mistral-dbinit-test mistral-dbinit-test python -m unittest discover -p *test.py &&  docker rm -f mistral-dbinit-test
``