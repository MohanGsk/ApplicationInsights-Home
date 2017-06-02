'use strict';

const express = require('express');
const mongoClient = require('mongodb').MongoClient;
const app = express();

// Constants
const PORT = 8080;

app.get('/events', function (req, res) {
  res.send([])
})

app.get('/checkdb', function (req, res) {
  // Connection URL
  var mongoPort = process.env.MISTRAL_MONGO_PORT;
  if (mongoPort) {
    mongoPort = mongoPort.substr(6);
  } else {
    mongoPort = PORT;
  }
  var url = "mongodb://unit_test_user:run@" + mongoPort;
  mongoClient.connect(url, function (err, db) {
    if (err) {
      res.send(err.toString());
    }
    else {
      res.send("ok");
      return;
    }
    db.close();
  });
}
)

app.listen(PORT, function () {
  console.log('Example app listening on port ' + PORT.toString() + '!')
})