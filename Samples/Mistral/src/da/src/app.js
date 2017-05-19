'use strict';

const express = require('express');

// Constants
const PORT = 8080;
const app = express();

app.get('/events', function (req, res) {
  res.send([])
})

app.get('/checkdb', function(req, res) {
  /* TODO: connect to database and return status check */
})

app.listen(PORT, function () {
  console.log('Example app listening on port ' + PORT.toString() + '!')
})