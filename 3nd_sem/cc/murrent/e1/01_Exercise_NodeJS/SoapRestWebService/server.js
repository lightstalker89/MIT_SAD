var soap = require('soap-server');
var fs = require('fs');
var _ = require('underscore');
var uuid = require('node-uuid');
var application_root = __dirname;
var express = require('express');

var customers = [];
var orders = [];

var createCustomer = function (customerName) {
    var currentCustomer = _.findWhere(customers, { Name: customerName });
    if (currentCustomer) {
        return { Success: false, ErrorMessage: "Customer already exists" };
    } else {
        customers.push({ Name: customerName, Orders: [] });
        return { Success: true };
    }
};

var createOrder = function (customerName) {
    var currentCustomer = _.findWhere(customers, { Name: customerName });
    if (currentCustomer) {
        var orderId = uuid.v1();
        if (currentCustomer.Orders) {
            currentCustomer.Orders.push(orderId);
            return { Success: true };
        } else {
            return { Success: false, ErrorMessage: "Adding orders not available for this customer" };
        }
    } else {
        return { Success: false, ErrorMessage: "Could not add order, because customer could not be found" };
    }
};

var getOrders = function (customerName) {
    var currentCustomer = _.findWhere(customers, { Name: customerName });
    if (currentCustomer) {
        return currentCustomer.Orders;
    }
    return [];
};

var deleteOrder = function (customerName) {
    var currentCustomer = _.findWhere(customers, { Name: customerName });
    if (currentCustomer) {
        currentCustomer.Orders = [];
        return { Success: true };
    }
    return { Success: false, ErrorMessage: "Could not delete order, because customer could not be found" };
};

var deleteCustomer = function (customerName) {
    var currentCustomer = _.findWhere(customers, { Name: customerName });
    if (currentCustomer) {
        var index = _.indexOf(customers, currentCustomer);
        if (index !== -1) {
            customers.splice(index, 1);
            return { Success: true };
        } else {
            return { Success: false, ErrorMessage: "Could not delete customer, because customer could not be found" };
        }
    }
    return { Success: false };
};

var getFormattedCustomers = function () {
    var tmpObject = {};
    _.each(customers, function (customer) {
        tmpObject[customer.Name] = [];
        var i = 0;
        _.each(customer.Orders, function (order) {
            tmpObject[customer.Name]["order" + i] = order;
            i++;
        });
    });
    return tmpObject;
};

var getFormattedOrders = function (customerName) {
    var tmpObject = {};
    var currentCustomer = _.findWhere(customers, { Name: customerName });
    if (currentCustomer) {
        var i = 0;
        _.each(currentCustomer.Orders, function (order) {
            tmpObject["order" + i] = order;
            i++;
        });
    }
    return tmpObject;
};

//Create server
var app = express();
var htmlDir = app.use(express.static(__dirname + 'html'));

//Get all customers
app.get('/customers', function (request, response) {
    response.send(customers);
});
//Get an order by customer name
app.get('/order/:name', function (request, response) {
    response.send(getOrders(request.params.name));
});

//Update or add order
app.put('/customer/add/:name', function (request, response) {
    var success = createCustomer(request.params.name);
    response.send(success);
});

//Update or add order
app.put('/order/add/:name', function (request, response) {
    var success = createOrder(request.params.name);
    response.send(success);
});

//Delete an order by anme
app.delete('/order/delete/:name', function (request, response) {
    var success = deleteOrder(request.params.name);
    response.send(success);
});

//Delete a customer by name
app.delete('/customer/delete/:name', function (request, response) {
    var success = deleteCustomer(request.params.name);
    response.send(success);
});

//Start server
var port = 1338;
var server = app.listen(port, function () {
    var host = server.address().address;
    var port = server.address().port;
    console.log("REST service listening on port 1338");
});

function SOAPWebService() {
}
SOAPWebService.prototype.getCustomers = function (all) {
    return getFormattedCustomers();
};
SOAPWebService.prototype.getOrders = function (customerName) {
    return getFormattedOrders(customerName);
};
SOAPWebService.prototype.addOrder = function (customerName) {
    var success = createOrder(customerName);
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
SOAPWebService.prototype.deleteCustomer = function (customerName) {
    var success = deleteCustomer(customerName);
    return success;
};

var soapServer = new soap.SoapServer();
var soapService = soapServer.addService('SOAPWebService', new SOAPWebService());
console.log("SOAP Service listening on port 1337");
soapServer.listen(1337, '127.0.0.1');
