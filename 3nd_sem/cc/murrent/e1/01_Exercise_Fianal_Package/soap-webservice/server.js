var soap = require('soap-server');
var fs = require('fs');
var _ = require('underscore');
var uuid = require('node-uuid');

var customers = [];
var orders = [];
var createCustomer = function (customerName) {
    var currentCustomer = _.findWhere(customers, { Name: customerName });
    if (currentCustomer) {
        return { Success: false };
    } else {
        customers.push({ Name: customerName, Orders: [] });
        return { Success: true };
    }
};

var createOrder = function (customerName) {
    var currentCustomer = _.findWhere(customers, { Name: customerName });
    if (currentCustomer) {
        var orderId = uuid.v1();
        currentCustomer.Order.push(orderId);
        return { Success: true };
    } else {
        return { Success: false };
    }
};

var getOrders = function (customerName) {
    var currentCustomer = _.findWhere(customers, { CustomerName: customerName });
    if (currentCustomer) {
        return currentCustomer.Orders;
    }
    return [];
};

var deleteOrder = function (orderName) {
    var currentOrder = _.findWhere(orders, { Name: orderName });
    if (currentOrder) {
        var index = _.indexOf(order, currentOrder);
        if (index !== -1) {
            orders.splice(index, 1);
            return { Success: true };
        } else {
            return { Success: false };
        }
    }
    return false;
};

var deleteCustomer = function (customerName) {
    var currentCustomer = _.findWhere(customers, { Name: customerName });
    if (currentCustomer) {
        var index = _.indexOf(customers, currentCustomer);
        if (index !== -1) {
            customers.splice(index, 1);
            return { Success: true };
        } else {
            return { Success: false };
        }
    }
    return false;
};

function SOAPWebService() {
}
SOAPWebService.prototype.getCustomers = function (all) {
    return customers;
};
SOAPWebService.prototype.getOrders = function (customerName) {
    return getOrders(customerName);
};
SOAPWebService.prototype.addOrder = function (orderName) {
    var success = createOrder(orderName);
    return success;
};
SOAPWebService.prototype.addCustomer = function (customerName) {
    var success = createCustomer(customerName);
    return success;
};
SOAPWebService.prototype.deleteOrder = function (customerName) {
    var success = deleteOrder(customerName);
    return success;
};
SOAPWebService.prototype.deleteCustomer = function(customerName) {
    var success = deleteCustomer(customerName);
    return success;
};

var soapServer = new soap.SoapServer();
var soapService = soapServer.addService('SOAPWebService', new SOAPWebService());

soapServer.listen(1337, '127.0.0.1');
