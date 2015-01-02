var express = require('express');
var vmManager = require('manager/vmManager');
var fs = require('fs');

var app = express();
storage.initSync();

/** List all virtual machines **/
app.get('/machines', function (request, response) {
    response.write(vmManager.getAll());
});

/** Search for specific virtual machines by operatin system and software **/
app.get('/machine/:operatingsystem/:softwarename', function (request, response) {

});

/** Add a new virtual machine **/
app.put('/machine', function (request, response) {
    
});

/** Add a new appliance **/
app.put('/appliance', function (request, response) {

});

/** Start or stop a virtual machine **/
app.post('/machine/:id/:operation', function (request, response) {
    
});

/** Change the description of a virtual machine **/
app.post('/machine/:id/:description', function (request, response) {

});

/** Add a rating with a comment to the virtual machine **/
app.post('/machine/:id/:rating/:comment', function (request, response) {

});