var storageManager = require('manager/storageManager');
var _ = require('underscore');

var VmManager = function () {
    var virtualMachines = [];
    var me = this;

    me.add = function(description) {

    };

    me.getAll = function () {
        return virtualMachines();
    };

    me.getMachines = function(operationSystem, software) {

    };

    me.updateDescription = function(name, description) {

    };

    me.updateRating = function(rating, comment) {

    };

    me.start = function(name) {

    };

    me.stop = function(name) {

    };
};
module.exports = VmManager;