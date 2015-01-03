var express = require('express');
var fs = require('fs');
var log4js = require('log4js');
var _ = require('underscore');
var qs = require('querystring');
var logger = log4js.getLogger();
var client = null;
var file = "appliance.json";

var virtualMachines = [{
    "Id": "1",
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
}, {
    "Id": "2",
    "ReferencedVirtualMachineId": "",
    "Name": "Windows Vista",
    "Description": "Windows Vista",
    "Type": "Appliance",
    "ApplicationType": "",
    "OperatingSystem": "Windows Vista",
    "OperatingSystemType": "Windows",
    "OperatingSystemVersion": "5.1",
    "Size": "400000",
    "RecommendedCPU": "2000",
    "RecommendedRAM": "2048",
    "SupportedVirtualizationPlatform": "",
    "Software": [
        "Virtual Box",
        "Microsoft Office",
        "Microsoft Visual Studio 2013"
    ],
    "SupportedProgramingLanguages": [
        "C#",
        "C++",
        "HTML"
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
        return { Success: false, ErrorMessage: "Cannot create virtual machine. A machine with the given id already exists.", Data: null};
    } 
        logger.info("Adding new virtual machine");
        return { Success: true, ErrorMessage: "", Data: virtualMachines };
};

var getMachines = function(operatingSystem, software) {
    var machines = [];
    _.each(virtualMachines, function(machine) {
        var match = false;
    });
    return machines;
};

var updateDescription = function (id, description) {
    var machine = getMachine(description.Id);
    if (machine) {
        machine.Description = description;
        return { Success: true, ErrorMessage: "" };
    } 
        logger.error("Could not find virutal machine for the given id");
        return { Success: false, ErrorMessage: "Could not find virutal machine for the given id", Data: null };
};

var updateRating = function(id, rating, comment) {
    var machine = getMachine(description.Id);
    if (machine) {
        machine.Ratings = [];
        machine.Ratings.push({ Rating: rating, Comment: comment });
        return { Success: true, ErrorMessage: "", Data: null };
    } 
        logger.error("Could not find virutal machine for the given id");
        return { Success: false, ErrorMessage: "Could not find virutal machine for the given id", Data: null };
};

var getMachine = function(id) {
    var machine = _.findWhere(virtualMachines, { Id: id });
    return machine;
};

var app = express();

/** Download a virtual machine **/
app.get('/download/:id', function (request, response) {
    if (request.params.id) {
        var machine = getMachine(request.params.id);
        if (machine) {
            var stream = fs.createWriteStream(file);
            stream.once('open', function (fd) {
                stream.write(JSON.stringify(machine));
                stream.end();
            });
            response.download(file);
        } else {
            logger.error("Could not find virtual machine or appliance for id");
            response.send({ Success: false, ErrorMessage: "Could not find virtual machine or appliance for id", Data: null});
        }
    } else {
        logger.error("No id supplied. Error");
        response.send({ Success: false, ErrorMessage: "No id supplied. Error", Data: null });
    }
});

/** List all virtual machines **/
app.get('/machines', function (request, response) {
    logger.info("Received 'List all Virtual Machines' request");
    response.send({ Success: true, ErrorMessage: "", Data: virtualMachines });
});

/** Search for specific virtual machines by operating system and software **/
app.get('/machine/:operatingsystem/:softwarename', function (request, response) {
    logger.info("Received 'List Virtual Machines by operating syste and software' request");
});

/** Add a new virtual machine **/
app.post('/machine', function (request, response) {
    logger.info("Received 'Add new Virtual Machine' request");
    if (request.method == 'POST') {
        var body = '';
        request.on('data', function (data) {
            body += data;

            if (body.length > 1e6)
                request.connection.destroy();
        });
        request.on('end', function () {
            try {
                var jsonObject = JSON.parse(body);
                //virtualMachines.push(jsonObject);
                var vmResponse = add(jsonObject);
                response.send(response);
            } catch (e) {
                response.send({ Success: false, ErrorMessage: "Could not add virutal machine. Please try it again.", Data: null });
            }    
        });
    }
    response.send({ Success: false, ErrorMessage: "Could not add virutal machine. Please try it again."});
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