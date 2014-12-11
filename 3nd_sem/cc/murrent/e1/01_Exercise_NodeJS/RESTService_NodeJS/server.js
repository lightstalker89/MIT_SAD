var application_root = __dirname;
var html_dir = './html/';
var express = require('express'); //Web framework
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
        if (currentCustomer.Orders) {
            currentCustomer.Orders.push(orderId);
            return { Success: true };
        } else {
            return { Success: false };
        }
    } else {
        return { Success: false };
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
    return { Success: false };
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
    return { Success: false };
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
    console.log(request.params);
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
var port = 1337;
var server = app.listen(port, function () {
    var host = server.address().address;
    var port = server.address().port;
    console.log('Express server listening on listening at http://%s:%s', host, port);
});