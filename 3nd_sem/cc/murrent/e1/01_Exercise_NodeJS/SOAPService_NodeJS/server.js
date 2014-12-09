var soap = require('soap-server');
var fs = require('fs');

function SOAPWebService() {
}
SOAPWebService.prototype.getCustomers = function (all) {
    return "";
};
SOAPWebService.prototype.getOrders = function (customerName) {
    return "";
};
SOAPWebService.prototype.addOrder = function (orderName) {
    return "";
};
SOAPWebService.prototype.addCustomer = function (customerName) {
    return "";
};
SOAPWebService.prototype.deleteOrder = function (orderName) {
    return "";
};
SOAPWebService.prototype.delteCustomer = function (customerName) {
    return "";
};

var soapServer = new soap.SoapServer();
var soapService = soapServer.addService('SOAPWebService', new SOAPWebService());

soapServer.listen(1337, '127.0.0.1');
