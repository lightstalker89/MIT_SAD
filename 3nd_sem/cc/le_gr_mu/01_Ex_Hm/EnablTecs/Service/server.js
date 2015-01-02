var express = require('express');
var fs = require('fs');
var log4js = require('log4js');
var logger = log4js.getLogger();

var virtualMachines = [{
    "Id": "",
    "Name": "Windows XP",
    "Description": "Windows XP Service Pack 3",
    "Type": "Appliance",
    "ApplicationType": "",
    "OperatingSystem": "Windows XP",
    "OperatingSystemType": "Windows",
    "OperatingSystemVersion": "6.1",
    "Size": "400000",
    "RecommendedCPU": "1000",
    "RecommendedRAM": "1024",
    "SupportedVirtualizationPlatform": "",
    "Software": [

    ],
    "SupportedProgramingLanguages": [

    ]
}];
var client = null;

var connect = function () {
    client = require('pkgcloud').storage.createClient({
        provider: 'openstack',
        username: 'your-user-name',
        password: 'your-password',
        tenantId: 'exampleProject',
        region: 'exampleRegion',
        authUrl: 'https://identity.example.com/v2.0/'
    });
    client.on('log::*', function (message, object) {
        if (object) {
            console.log(this.event.split('::')[1] + ' ' + message);
            console.dir(object);
        }
        else {
            console.log(this.event.split('::')[1] + ' ' + message);
        }
    });
};

var start = function (id) {

};

var stop = function (id) {

};

var add = function(description) {
    var machine = getMachine(description.Id);
    if (machine) {
        logger.error("Cannot create virtual machine. A machine with the given id already exists.");
    } else {

    }
};

var getMachines = function(operatingSystem, software) {

};

    var updateDescription = function (id, description) {

    };

var updateRating = function(id, rating, comment) {

};

var getMachine = function(id) {
    var machine = _.findWhere(virtualMachines, { Id: id });
    return machine;
};

var app = express();

/** List all virtual machines **/
app.get('/machines', function (request, response) {
    response.send(virtualMachines);
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
    start(request.params.Id);
});

/** Change the description of a virtual machine **/
app.post('/machine/:id/:description', function (request, response) {
    updateDescription(request.params.Id, request.params.Description);
});

/** Add a rating with a comment to the virtual machine **/
app.post('/machine/:id/:rating/:comment', function (request, response) {
    updateRating(request.params.Id, request.params.Rating, request.params.Comment);
});

var port = 1337;
var server = app.listen(port, function () {
    var host = server.address().address;
    var port = server.address().port;
    logger.info('Express server listening on listening at http://%s:%s', host, port);
});