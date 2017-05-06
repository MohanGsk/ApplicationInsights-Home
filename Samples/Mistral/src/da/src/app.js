var express = require('express')
var app = express()

app.get('/events', function (req, res) {
  res.send([])
})

app.listen(3000, function () {
  console.log('Example app listening on port 3000!')
})