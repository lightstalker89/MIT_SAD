#!/usr/bin/python
import requests
import json

serverAddr = "http://127.0.0.1:5000"

routeCustomers = "/orderService/api/v1.0/customers"
routeModifyCustomers = "/orderService/api/v1.0/customers/%s"
routeOrders = "/orderService/api/v1.0/customers/%s/orders"
routeDeleteOrder = "/orderService/api/v1.0/customers/%s/order/%s"
routeCreateOrder = "/orderService/api/v1.0/customers/%s/order"

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
    url = serverAddr + routeCustomers
    data = {'name': name}
    print("POST: " + url)
    print("************")
    print("------------")
    print(postRequest(url, data))
    print("------------")

def addOrder():
    customerName = raw_input("Enter customer name for order: ")
    answer = ""
    products = []
    while answer != "n":
    	product = raw_input("Enter Product you want to order: ")
    	products.append({'product': product})
    	answer = raw_input("Add another Product? y = yes, n = no ")
    data = {'products': products}
    url = serverAddr + routeCreateOrder % customerName
    print("POST: " + url)
    print("************")
    print("------------")
    print(postRequest(url, data))
    print("------------")

def deleteCustomer():
    customerName = raw_input("Enter Customer name: ")
    print("************")
    url = serverAddr + routeModifyCustomers % customerName
    print("DELETE: " + url)
    print("************")
    print("------------")
    print(deleteRequest(url))
    print("------------") 

def deleteOrder():
    customerName = raw_input("Enter customer name from whom you want to delete an order: ")
    order = raw_input("Enter Order ID you want to delete: ")
    print("************")
    url = serverAddr + routeDeleteOrder % (customerName, order)
    print("DELETE: " + url)
    print("************")
    print("------------")
    print(deleteRequest(url))
    print("------------") 

def showCustomers():
    print("************")
    url = serverAddr + routeCustomers
    print("GET: " + url)
    print("************")
    print("------------")
    print(getRequest(url))
    print("------------")

def showOrders():
    customerName = raw_input("Enter name from customer, to show his orders: ")
    print("************")
    url = serverAddr + routeOrders % customerName
    print("GET: " + url)
    print("************")
    print("------------")
    print(getRequest(url))
    print("------------")

def getRequest(url):
	result = requests.get(url)
	return result.text

def postRequest(url, data):
	headers = {"Content-Type": "application/json", "Accept": "*/*"}
	result = requests.post(url, data=json.dumps(data), headers=headers)
	return result.text

def deleteRequest(url):
	result = requests.delete(url)
	return result.text

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
