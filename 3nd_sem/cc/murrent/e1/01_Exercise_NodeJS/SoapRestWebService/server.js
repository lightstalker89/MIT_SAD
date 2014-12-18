var soap = require('soap-server');
var fs = require('fs');
var _ = require('underscore');
var express = require('express');

var customers = [];
var createCustomer = function (customerName) {
    console.log("Received request for creating a customer");
    var currentCustomer = _.findWhere(customers, { Name: customerName });
    if (currentCustomer) {
        return { Success: false, Error: "Error while adding a new customer. A customer with this name already exists." };
    } else {
        customers.push({ Name: customerName, Orders: [] });
        return { Success: true };
    }
};

var createOrder = function (customerName, orderName) {
    console.log("Received request for creating an order");
    var currentCustomer = _.findWhere(customers, { Name: customerName });
    if (currentCustomer) {
        var orderId = orderName;
        currentCustomer.Orders.push(orderId);
        return { Success: true };
    }
    return { Success: false, Error: "Error while creating an order. The customer could not be found." };
};

var getOrders = function (customerName) {
    console.log("Received request for getting orders for customer");
    var currentCustomer = _.findWhere(customers, { CustomerName: customerName });
    if (currentCustomer) {
        return currentCustomer.Orders;
    }
    return [];
};

var deleteOrder = function (orderName, customerName) {
    console.log("Received request for deleting an order");
    var currentCustomer = _.findWhere(customers, { Name: customerName });
    if (currentCustomer) {
        var customerOrderIndex = _.indexOf(currentCustomer.Orders, orderName);
        if (customerOrderIndex !== -1) {
            currentCustomer.Orders.splice(customerOrderIndex, 1);
            return { Success: true };
        }
    }
    return { Success: false, Error: "Error while deleting orders for the customer" };
};

var deleteCustomer = function (customerName) {
    console.log("Received request for deleting a customer");
    var currentCustomer = _.findWhere(customers, { Name: customerName });
    if (currentCustomer) {
        var index = _.indexOf(customers, currentCustomer);
        if (index !== -1) {
            customers.splice(index, 1);
            return { Success: true };
        }
    }
    return { Success: false, Error: "Error while deleting the customer" };
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
        tmpObject[currentCustomer.Name] = [];
        var i = 0;
        _.each(currentCustomer.Orders, function (order) {
            tmpObject[currentCustomer.Name]["order" + i] = order;
            i++;
        });
    }
    return tmpObject;
};

function SOAPWebService() {
}
SOAPWebService.prototype.getCustomers = function (all) {
    return getFormattedCustomers();
};
SOAPWebService.prototype.getOrders = function (customerName) {
    return getFormattedOrders(customerName);
};
SOAPWebService.prototype.addOrder = function (customerName, orderName) {
    var success = createOrder(customerName, orderName);
    return success;
};
SOAPWebService.prototype.addCustomer = function (customerName) {
    var success = createCustomer(customerName);
    return success;
};
SOAPWebService.prototype.deleteOrder = function (customerName, orderName) {
    var success = deleteOrder(orderName, customerName);
    return success;
};
SOAPWebService.prototype.deleteCustomer = function (customerName) {
    var success = deleteCustomer(customerName);
    return success;
};

var soapServer = new soap.SoapServer();
var soapService = soapServer.addService('SOAPWebService', new SOAPWebService());
console.log("SOAP service listening on port 1337");
soapServer.listen(1337, '127.0.0.1');


//Create server
var app = express();

//Get all customers
app.get('/customers', function (request, response) {
    response.send(customers);
});
//Get an order by customer name
app.get('/order/:customername', function (request, response) {
    response.send(getOrders(request.params.customername));
});

//Update or add order
app.put('/customer/add/:customername', function (request, response) {
    var success = createCustomer(request.params.customername);
    response.send(success);
});

//Update or add order
app.put('/order/add/:customername/:ordername', function (request, response) {
    var success = createOrder(request.params.customername, request.params.ordername);
    response.send(success);
});

//Delete an order by name
app.delete('/order/delete/:customername/:ordername', function (request, response) {
    var success = deleteOrder(request.params.ordername, request.params.customername);
    response.send(success);
});

//Delete a customer by name
app.delete('/customer/delete/:customername', function (request, response) {
    var success = deleteCustomer(request.params.customername);
    response.send(success);
});

//Start server
var port = 1338;
var server = app.listen(port, function () {
    console.log("REST service listening on port 1338");
});