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

    me.getMachines = function(operatingSystem, software) {

    };

    me.updateDescription = function(id, description) {

    };

    me.updateRating = function(id, rating, comment) {

    };

    me.start = function(id) {

    };

    me.stop = function(id) {

    };
};
module.exports = VmManager;