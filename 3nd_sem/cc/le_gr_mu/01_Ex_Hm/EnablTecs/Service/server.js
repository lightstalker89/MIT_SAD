var express = require('express');
var vmManager = require('manager/vmManager');
var fs = require('fs');

var app = express();

/** List all virtual machines **/
app.get('/machines', function (request, response) {
    response.write(vmManager.getAll());
});

/** Search for specific virtual machines by operatin system and software **/
app.get('/machine/:operatingsystem/:softwarename', function (request, response) {

});

/** Add a new virtual machine **/
app.put('/machine', function (request, response) {
    console.log(JSON.stringify(request.files));
});

/** Add a new appliance **/
app.put('/appliance', function (request, response) {
    console.log(JSON.stringify(request.files));
});

/** Start or stop a virtual machine **/
app.post('/machine/:id/:operation', function (request, response) {
    vmManager.start(request.params.Id);
});

/** Change the description of a virtual machine **/
app.post('/machine/:id/:description', function (request, response) {
    vmManager.updateDescription(request.params.Id, request.params.Description);
});

/** Add a rating with a comment to the virtual machine **/
app.post('/machine/:id/:rating/:comment', function (request, response) {
    storageManager.updateRating(request.params.Id, request.params.Rating, request.params.Comment);
});