var express = require('express');
var fs = require('fs');
var log4js = require('log4js');
var logger = log4js.getLogger();
var client = null;

var virtualMachines = [{
    "Id": "",
    "ReferencedVirtualMachineId": "",
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
        "Virtual Box",
        "Microsoft Office"
    ],
    "SupportedProgramingLanguages": [
        "C#",
        "C++"
    ]
}];

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
        logger.inf("Adding new virtual machine");
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

/** Download a virtual machine **/
app.get('/download/:id', function(request, response) {
    var file = "appliance.json";
    response.download(file);
});

/** List all virtual machines **/
app.get('/machines', function (request, response) {
    logger.info("Received 'List all Virtual Machines' request");
    response.send(virtualMachines);
});

/** Search for specific virtual machines by operating system and software **/
app.get('/machine/:operatingsystem/:softwarename', function (request, response) {
    logger.info("Received 'List Virtual Machines by operating syste and software' request");
});

/** Add a new virtual machine **/
app.put('/machine', function (request, response) {
    logger.info("Received 'Add new Virtual Machine' request");
    console.log(JSON.stringify(request.files));
});

/** Add a new appliance **/
app.put('/appliance', function (request, response) {
    logger.inf("Received 'Add new Virtual Appliance' request");
    console.log(JSON.stringify(request.files));
});

/** Start or stop a virtual machine **/
app.post('/machine/:id/:operation', function (request, response) {
    logger.inf("Received 'Operation for Virtual Machine' request");
    start(request.params.id);
});

/** Change the description of a virtual machine **/
app.post('/machine/:id/:description', function (request, response) {
    logger.info("Received 'Update Description for Virtual Machiner' request");
    updateDescription(request.params.id, request.params.description);
});

/** Add a rating with a comment to the virtual machine **/
app.post('/machine/:id/:rating/:comment', function (request, response) {
    logger.inf("Received 'Update Rating for Virtual Machine' request");
    updateRating(request.params.id, request.params.rating, request.params.comment);
});

var port = 1337;
var server = app.listen(port, function () {
    var host = server.address().address;
    var port = server.address().port;
    logger.info('Express server listening on listening at http://%s:%s', host, port);
});