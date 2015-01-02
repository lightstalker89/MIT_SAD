var storageManager = require('manager/storageManager');

var VmManager = function () {
    var me = this;
    var client = null;

    var connect = function() {
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

    me.start = function(id) {

    };

    me.stop = function(id) {

    };
};
module.exports = VmManager;