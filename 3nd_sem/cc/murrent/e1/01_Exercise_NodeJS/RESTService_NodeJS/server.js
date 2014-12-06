var application_root = __dirname,
    express = require('express'); //Web framework

//Create server
var app = express();

// Configure server
app.use(function () {
    //parses request body and populates request.body
    app.use(express.bodyParser());
    
    //checks request.body for HTTP method overrides
    app.use(express.methodOverride());
    
    //perform route lookup based on url and HTTP method
    app.use(app.router);
    
    //Show all errors in development
    app.use(express.errorHandler({ dumpExceptions: true, showStack: true }));
});

//Router
//Get all customers
app.get('/api/customers', function (request, response) {
    var customers = [];
    
    response.send(customers);
});
//Get an order by name
app.get('/api/order/:name', function (request, response) {
    var orders = [];
    
    response.send(orders);
});

//Update or add order
app.put('/api/customer/:name', function (request, response) {
    response.send("Updated!");
});

//Update or add order
app.put('/api/order/:name', function (request, response) {
    response.send("Updated!");
});

//Delete an order by anme
app.delete('/api/order/:name', function (request, response) {
    response.send("Deleted");
});

//Delete a customer by name
app.delete('/api/customer/:name', function (request, response) {
    response.send("Deleted");
});

//Start server
var port = 6666;
app.listen(port, function () {
    console.log('Express server listening on port %d in %s mode', port, app.settings.env);
});