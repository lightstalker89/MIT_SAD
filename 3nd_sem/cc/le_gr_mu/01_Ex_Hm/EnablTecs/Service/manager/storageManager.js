var storage = require('node-persist');
var uuid = require('node-uuid');
var _ = require('underscore');

var StorageManager = function () {
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

    var me = this;

    me.add = function (description) {

    };

    me.getAll = function () {
        return virtualMachines();
    };

    me.getMachines = function (operatingSystem, software) {

    };

    me.updateDescription = function (id, description) {

    };

    me.updateRating = function (id, rating, comment) {

    };
};
module.exports = StorageManager;