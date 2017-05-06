Pull mongo image from dockerhub:   
``docker pull mongo``

To start this mongo instance:   
``$ docker run --name mistral -d mongo``

Run DB init verification:   
``python -m unittest discover dbinit/ "*_test.py"``   