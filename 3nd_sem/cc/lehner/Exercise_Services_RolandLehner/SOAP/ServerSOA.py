#!/usr/bin/python

from pysimplesoap.server import SoapDispatcher, SOAPHandler
from BaseHTTPServer import HTTPServer

from Customer import Customer

customers = []

def getCustomers():
    "Return customers"
    return customers

def getOrders(customerName):
    "Return orders"
    for x in customers:
        if x.name == customerName:
            return x.orders

def addOrder(customerName, order):
    for customer in customers:
        if customer.name == customerName:
            customer.orders.append(order)
            return True
    return False

def addCustomer(customerName):
    customer = Customer(customerName)
    customers.append(customer)
    return True

def deleteOrder(customerName, identifier):
    for x in customers:
        if x.name == customerName:
            x.orders.remove(identifier)
            return True
    return False

def deleteCustomer(customerName):
    for x in customers:
        if x.name == customerName:
            customers.remove(x)
            return True
    return False

dispatcher = SoapDispatcher(
    'my_dispatcher',
    location = "http://localhost:8008/",
    action = 'http://localhost:8008/', # SOAPAction
    namespace = "http://example.com/sample.wsdl", prefix="ns0",
    trace = True,
    ns = True)

# register the user function
dispatcher.register_function('GetCustomers', getCustomers,
    returns={'GetCustomersResult': list},
    args={})

dispatcher.register_function('GetOrders', getOrders,
    returns={'GetOrdersResult': list},
    args={'customerName': str})

dispatcher.register_function('AddOrder', addOrder,
    returns={'AddOrderResult': bool},
    args={'customerName': str, 'order': str})

dispatcher.register_function('AddCustomer', addCustomer,
    returns={'AddCustomerResult': bool},
    args={'customerName': str})

dispatcher.register_function('DeleteOrder', deleteOrder,
    returns={'DeleteOrderResult': bool},
    args={'customerName': str ,'identifier': str})

dispatcher.register_function('DeleteCustomer', deleteCustomer,
    returns={'DeleteCustomerResult': bool},
    args={'customerName': str})

print "Starting server..."
httpd = HTTPServer(("", 8008), SOAPHandler)
httpd.dispatcher = dispatcher
httpd.serve_forever()