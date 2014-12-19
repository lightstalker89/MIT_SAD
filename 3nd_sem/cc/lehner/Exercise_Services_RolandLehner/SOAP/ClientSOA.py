#!/usr/bin/python
import traceback
import sys
from pysimplesoap.client import SoapClient, SoapFault

# create a simple consumer
client = SoapClient(
    location = "http://localhost:8008/",
    action = 'http://localhost:8008/', # SOAPAction
    namespace = "http://example.com/sample.wsdl", 
    soap_ns='soap',
    trace = True,
    ns = False)

def showHelp():
    print("Commands:")
    print("************")
    print("ac: Add Customer")
    print("ao: Add Order")
    print("dc: Delete Customer")
    print("do: Delete Order")
    print("sc: Show Customers")
    print("so: Show Orders")
    print("h: Help")
    print("exit: Leave Program")
    print("************")

def addCustomer():
    name = raw_input("Enter Customer name: ")
    print("************")
    response = client.AddCustomer(customerName=name)
    result = str(response.AddCustomerResult)
    print("************")
    if result == 'true':
        print("Customer added successfully")
    else:
        print("Add Customer failed")

def addOrder():
    customerName = raw_input("Enter customer name for order: ")
    order = raw_input("Enter Product you want to order: ")
    print("************")
    response = client.AddOrder(customerName=customerName, order=order)
    result = str(response.AddOrderResult)
    print("************")
    if result == 'true':
        print("Order successfully added")
    else:
        print("Add Order failed")

def deleteCustomer():
    customerName = raw_input("Enter Customer name: ")
    print("************")
    response = client.DeleteCustomer(customerName=customerName)
    result = str(response.DeleteCustomerResult)
    print("************")
    if result == 'true':
        print("Customer successfully deleted")
    else:
        print("Delete Customer failed")   

def deleteOrder():
    customerName = raw_input("Enter customer name from whom you want to delete an order: ")
    order = raw_input("Enter Product you want to delete: ")
    print("************")
    response = client.DeleteOrder(customerName=customerName, identifier=order)
    result = str(response.DeleteOrderResult)    
    print("************")
    if result == 'true':
        print("Order successfully deleted")
    else:
        print("Delete Order failed")  

def showCustomers():
    print("************")
    response = client.GetCustomers()
    result = list(response.GetCustomersResult)
    print("************")
    print("------------")
    for x in result:
        print(x)
    print("------------")

def showOrders():
    customerName = raw_input("Enter name from customer, to show his orders: ")
    print("************")
    response = client.GetOrders(customerName=customerName)
    result = list(response.GetOrdersResult)
    print("************")
    print("------------")
    for x in result:
        print(x)
    print("------------")

input_var = ""

showHelp()

while input_var != "exit":
    input_var = raw_input("Enter command: ")

    if input_var == "h":
        showHelp()
    elif input_var == "ac":
        addCustomer()
    elif input_var == "ao":
        addOrder()
    elif input_var == "dc":
        deleteCustomer()
    elif input_var == "do":
        deleteOrder()
    elif input_var == "sc":
        showCustomers()
    elif input_var == "so":
        showOrders()
    elif input_var != "exit":
        print("Command not supported!")