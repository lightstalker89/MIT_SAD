var storage = require('node-persist');
var uuid = require('node-uuid');
var _ = require('underscore');

var StorageManager = function () {
    var virtualMachines = [];
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