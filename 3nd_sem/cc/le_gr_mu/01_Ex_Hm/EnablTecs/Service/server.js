var express = require('express');
var fs = require('fs');
var log4js = require('log4js');
var _ = require('underscore');
var qs = require('querystring');
var sys = require('sys');
var exec = require('child_process').exec;
var Client = require('node-rest-client').Client;
var jstack = require('jstack-client');
var logger = log4js.getLogger();
var restClient = new Client();;
var file = "appliance.json";
var requestToken = null;

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
    ],
    "Rating": "5",
    "RatingDescription": "Usable for something",
    "Status": "Stopped"
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
    ],
    "Rating": "3",
    "RatingDescription": "Useable for anything",
    "Status": "Stopped"
}];

var start = function (id) {
    var machine = getMachine(id);
    if (machine) {
        machine.Status = "Started";
    }
};

var stop = function (id) {
    var machine = getMachine(id);
    if (machine) {
        machine.Status = "Stopped";
    }
};

var add = function(description) {
    var machine = getMachine(description.Id);
    if (machine) {
        logger.error("Cannot create virtual machine. A machine with the given id already exists.");
        return { Success: false, ErrorMessage: "Cannot create virtual machine. A machine with the given id already exists.", Data: null};
    } 
    logger.info("Adding new virtual machine");
    description.Rating = "";
    description.RatingDescription = "";
    description.Status = "Stopped";
    virtualMachines.push(description);
        return { Success: true, ErrorMessage: "", Data: virtualMachines };
};

var getMachines = function(operatingsystem, type) {
    var machines = [];
    _.each(virtualMachines, function(machine) {
        var match = false;
        if ((operatingsystem  && operatingsystem !== "all") && (type && type !== "all")) {
            if (machine.OperatingSystemType.indexOf(operatingsystem) > -1 && machine.Type.indexOf(type) > -1) {
                match = true;
            }
        } else if ((type && type.length > 0 && type !== "all") && (operatingsystem && operatingsystem === "all")) {
                if (machine.Type.indexOf(type) > -1) {
                    match = true;
                }
        } else if ((operatingsystem && operatingsystem !== "all") && (type && type === "all")) {
            if (machine.OperatingSystemType.indexOf(operatingsystem) > -1) {
                match = true;
            }
        } else if (type === "all" && operatingsystem === "all") {
            match = true;
        }
        if (match === true) {
            machines.push(machine);
        }
    });
    return machines;
};

var updateDescription = function (id, description) {
    var machine = getMachine(id);
    if (machine) {
        machine.Description = description;
        return { Success: true, ErrorMessage: "", Data: virtualMachines };
    } 
        logger.error("Could not find virutal machine for the given id");
        return { Success: false, ErrorMessage: "Could not find virutal machine for the given id", Data: null };
};

var updateRating = function(id, rating, comment) {
    var machine = getMachine(id);
    if (machine) {
        machine.Rating = rating;
        machine.RatingDescription = comment;
        return { Success: true, ErrorMessage: "", Data: virtualMachines };
    } 
        logger.error("Could not find virutal machine for the given id");
        return { Success: false, ErrorMessage: "Could not find virutal machine for the given id", Data: null };
};

var getMachine = function(id) {
    var machine = _.findWhere(virtualMachines, { Id: id });
    return machine;
};

var updateOperation = function (id, operation) {
    console.log(operation);
    if (operation === "Start") {
        console.log("Trying to start vm");
        var args = {
            data: {"os-start": null},
            headers: { "X-Auth-Token": requestToken }
        };

        restClient.post("http://172.20.10.6:5000//v2.0​/servers/​b5adde24-8d55-4959-b50d-540c294b2fa7​/action", args, function (data, response) {
            console.log(response);
        });
    } else if (operation === "Stop") {
        restClient.post("http://172.20.10.6:5000/v2.0/tokens", args, function (data, response) {
            console.log(response.token);
            requestToken = response.token;
        });
    }
};

var getToken = function() {
    var args = {
        "auth": {
            "tenantName": "admin",
            "passwordCredentials": {
                "username": "admin",
                "password": "supersecret"
            }
        }
    };

    restClient.post("http://172.20.10.6:5000/v2.0/tokens", args, function (data, response) {
        requestToken = response.token;
    });
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
app.get('/machine/:operatingsystem/:type', function (request, response) {
    logger.info("Received 'List Virtual Machines by operating syste and software' request");
    var machines = getMachines(request.params.operatingsystem, request.params.type);
    response.send({ Success: true, ErrorMessage: "", Data: machines });
});

/** Add a new virtual machine **/
app.post('/machine', function (request, response) {
    logger.info("Received 'Add new Virtual Machine' request");
    console.log(request.body);
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
                var parsedObject = JSON.parse(jsonObject);
                var vmResponse = add(parsedObject);
                response.send(vmResponse);
            } catch (e) {
                response.send({ Success: false, ErrorMessage: "Could not add virutal machine. Please try it again.", Data: null });
            }    
        });
    }else{
        response.send({ Success: false, ErrorMessage: "Could not add virutal machine. Please try it again.", Data: null});
    }
});

/** Start or stop a virtual machine **/
app.post('/machine/state/:id/:operation', function(request, response) {
    logger.info("Received 'Operation for Virtual Machine' request");
    getToken();
    updateOperation(request.params.id, request.params.operation);
    response.send({ Success: false, ErrorMessage: "", Data: null });
});

/** Change the description of a virtual machine **/
app.post('/machine/:id/:description', function (request, response) {
    logger.info("Received 'Update Description for Virtual Machiner' request");
    var desResponse = updateDescription(request.params.id, request.params.description);
    response.send(desResponse);
});

/** Add a rating with a comment to the virtual machine **/
app.post('/machine/:id/:rating/:comment', function (request, response) {
    logger.info("Received 'Update Rating for Virtual Machine' request");
    var ratResponse = updateRating(request.params.id, request.params.rating, request.params.comment);
    response.send(ratResponse);
});

var port = 1337;
var server = app.listen(port, function () {
    var host = server.address().address;
    var port = server.address().port;
    logger.info('Express server listening on listening at http://%s:%s', host, port);
});