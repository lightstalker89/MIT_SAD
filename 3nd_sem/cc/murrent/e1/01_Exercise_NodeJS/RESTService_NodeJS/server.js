var application_root = __dirname;
var html_dir = './html/';
var express = require('express'); //Web framework
var _ = require('underscore');

var startPage = readIndexFile();

var customers = [];
var orders = [];
var createCustomer = function (customerName) {
    var currentCustomer = _.findWhere(customers, { name: customerName });
    if (currentCustomer) {
        return false;
    } else {
        customers.push({ name: customerName });
        return true;
    }
};

var createOrder = function (orderName, customerName) {
    var currentOrder = _.findWhere(orders, { name: orderName });
    if (currentOrder) {
        return false;
    } else {
        customers.push({ name: orderName, customer: customerName });
        return true;
    }
};

var getOrders = function (customerName) {
    var currentOrder = _.findWhere(orders, { customerName: customerName });
    return currentOrder || [];
};

var deleteOrder = function (orderName) {
    var currentOrder = _.findWhere(orders, { name: orderName });
    if (currentOrder) {
        var index = _.indexOf(order, currentOrder);
        if (index !== -1) {
            orders.splice(index, 1);
            return true;
        } else {
            return false;
        }
    }
    return false;
};

var deleteCustomer = function (customerName) {
    var currentCustomer = _.findWhere(customers, { name: customerName });
    if (currentCustomer) {
        var index = _.indexOf(customers, currentCustomer);
        if (index !== -1) {
            customers.splice(index, 1);
            return true;
        } else {
            return false;
        }
    }
    return false;
};

//Create server
var app = express();
var htmlDir = app.use(express.static('html'));
app.get('/', function (req, res) {
    res.sendfile(html_dir + 'index.html');
});

//Get all customers
app.get('/customers', function (request, response) {
    response.send(customers);
});
//Get an order by customer name
app.get('/order/:name', function (request, response) {
    response.send(getOrders(request.params.name));
});

//Update or add order
app.put('/customer/:name', function (request, response) {
    var success = createCustomer(request.params.name);
    response.send(success);
});

//Update or add order
app.put('/order/:name', function (request, response) {
    var success = createOrder(request.params.name);
    response.send(success);
});

//Delete an order by anme
app.delete('/order/:name', function (request, response) {
    var success = deleteOrder(request.params.name);
    response.send(success);
});

//Delete a customer by name
app.delete('/customer/:name', function (request, response) {
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