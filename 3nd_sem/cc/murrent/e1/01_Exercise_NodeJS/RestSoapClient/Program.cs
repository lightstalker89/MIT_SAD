using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Xml;
using RestSharp;
using RestSoapClient.Models;
using RestSoapClient.Services;

namespace RestSoapClient
{
    public class Program
    {
        private static bool rest = true;
        private static readonly JavaScriptSerializer JavaScriptSerializer = new JavaScriptSerializer();
        private const string EndPoint = "http://localhost:1338";
        private static RestClient restClient;

        public static void Main(string[] args)
        {
            ChooseMethod();
        }

        private static void ChooseMethod()
        {
            Console.Clear();
            ConsoleOutput.ChooseRequestMethod();
            ConsoleOutput.WriteDictionaryOptionsToConsole(ConsoleOutput.RequestOptions);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.S)
            {
                rest = false;
                ChooseSoapRequest();
            }
            else if (keyInfo.Key == ConsoleKey.R)
            {
                restClient = new RestClient(EndPoint);
                ChooseRestRequest();
            }
            else
            {
                rest = true;
                ChooseMethod();
            }

        }

        private static void ChooseSoapRequest()
        {
            string requestParameter = String.Empty;
            string orderName = String.Empty;
            string response = "";
            Console.Clear();
            ConsoleOutput.ChooseRequest();
            ConsoleOutput.WriteDictionaryOptionsToConsole(ConsoleOutput.Requests);
            ConsoleOutput.GoBack();
            Console.WriteLine();
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.D:
                    ConsoleOutput.DeleteCustomer();
                    requestParameter = GetParameter("Specify customer name: ");
                    response += SoapRequestService.CallWebService("deleteCustomer", "<customerName>" + requestParameter + "</customerName>");
                    XmlDocument deleteCustomerDocument = new XmlDocument();
                    deleteCustomerDocument.LoadXml(response);
                    GetSccessFromResponse(deleteCustomerDocument);
                    break;

                case ConsoleKey.F:
                    ConsoleOutput.DeleteOrder();
                    requestParameter = GetParameter("Specify customer name: ");
                    orderName = GetParameter("Specify order name: ");
                    response += SoapRequestService.CallWebService("deleteOrder", "<customerName>" + requestParameter + "</customerName><orderName>" + orderName + "</orderName>");
                    XmlDocument deleteOrderDocument = new XmlDocument();
                    deleteOrderDocument.LoadXml(response);
                    GetSccessFromResponse(deleteOrderDocument);
                    break;

                case ConsoleKey.G:
                    response += SoapRequestService.CallWebService("getCustomers", "<all></all>");
                    XmlDocument getCustomerDocument = new XmlDocument();
                    getCustomerDocument.LoadXml(response);
                    ConsoleOutput.Customers();
                    GetItemsFromResponse(getCustomerDocument);
                    break;

                case ConsoleKey.H:
                    ConsoleOutput.GetOrders();
                    requestParameter = GetParameter("Specify customer name: ");
                    response += SoapRequestService.CallWebService("getOrders", "<customerName>" + requestParameter + "</customerName>");
                    XmlDocument getOrdersDocument = new XmlDocument();
                    getOrdersDocument.LoadXml(response);
                    ConsoleOutput.Orders();
                    GetItemsFromResponse(getOrdersDocument);
                    break;

                case ConsoleKey.J:
                    ConsoleOutput.AddCustomer();
                    requestParameter = GetParameter("Specify customer name: ");
                    response += SoapRequestService.CallWebService("addCustomer", "<customerName>" + requestParameter + "</customerName>");
                    XmlDocument addCustomerDocument = new XmlDocument();
                    addCustomerDocument.LoadXml(response);
                    GetSccessFromResponse(addCustomerDocument);
                    break;

                case ConsoleKey.K:
                    ConsoleOutput.AddOrder();
                    requestParameter = GetParameter("Specify customer name: ");
                    orderName = GetParameter("Specify order name: ");
                    response += SoapRequestService.CallWebService("addOrder", "<customerName>" + requestParameter + "</customerName><orderName>" + orderName + "</orderName>");
                    XmlDocument addOrDocument = new XmlDocument();
                    addOrDocument.LoadXml(response);
                    break;

                case ConsoleKey.X:
                    ChooseMethod();
                    break;

                default:
                    ChooseSoapRequest();
                    break;
            }
            StartFromBeginning();
            ChooseRestRequest();
        }

        private static void GetSccessFromResponse(XmlDocument document)
        {
            XmlNodeList elemList = document.GetElementsByTagName("Success");
            if (elemList.Count == 1)
            {
                XmlNode node = elemList[0];
                Console.WriteLine("Success: " + node.FirstChild.Value);
                if (node.FirstChild.Value.ToLower() == "false")
                {
                    string error = GetErrorFromResponse(document);
                    Console.WriteLine("Error: " + error);
                }
            }
        }

        private static void GetItemsFromResponse(XmlDocument document)
        {
            XmlNodeList elemList = document.GetElementsByTagName("return");
            if (elemList.Count == 1)
            {
                XmlNode node = elemList[0];
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    Console.WriteLine(childNode.Name);
                    if (childNode.HasChildNodes)
                    {
                        foreach (XmlNode childChildNoe in childNode.ChildNodes)
                        {
                            Console.WriteLine("  |--" + childChildNoe.InnerText);
                        }
                    }
                }
            }
        }

        private static string GetErrorFromResponse(XmlDocument document)
        {
            XmlNodeList elemList = document.GetElementsByTagName("Error");
            if (elemList.Count == 1)
            {
                XmlNode node = elemList[0];
                return node.FirstChild.Value;
            }
            return "";
        }

        private static void ChooseRestRequest()
        {
            RestRequest restRequest = null;
            string requestParameter;
            string orderName;
            Console.Clear();
            ConsoleOutput.ChooseRequest();
            ConsoleOutput.WriteDictionaryOptionsToConsole(ConsoleOutput.Requests);
            ConsoleOutput.GoBack();
            Console.WriteLine();
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.D:
                    ConsoleOutput.DeleteCustomer();
                    requestParameter = GetParameter("Specify customer name: ");
                    restRequest = new RestRequest("customer/{customername}", Method.DELETE);
                    restRequest.AddUrlSegment("customername", requestParameter);
                    IRestResponse deleteCustomerResponse = restClient.Execute(restRequest);
                    SuccessResponse deleteCustomerSuccessResponse = JavaScriptSerializer.Deserialize<SuccessResponse>(deleteCustomerResponse.Content);
                    Console.WriteLine("Success: " + deleteCustomerSuccessResponse.Success);
                    if (!deleteCustomerSuccessResponse.Success)
                    {
                        Console.WriteLine("Error: " + deleteCustomerSuccessResponse.ErrorMessage);
                    }
                    break;

                case ConsoleKey.F:
                    ConsoleOutput.DeleteOrder();
                    requestParameter = GetParameter("Specify customer name: ");
                    orderName = GetParameter("Specify order name: ");
                    restRequest = new RestRequest("order/{customername}/{ordername}", Method.DELETE);
                    restRequest.AddUrlSegment("ordername", orderName);
                    restRequest.AddUrlSegment("customername", requestParameter);
                    IRestResponse deleteOrdeResponseResponse = restClient.Execute(restRequest);
                    SuccessResponse deleteOrderSuccessResponse = JavaScriptSerializer.Deserialize<SuccessResponse>(deleteOrdeResponseResponse.Content);
                    Console.WriteLine("Success: " + deleteOrderSuccessResponse.Success);
                    if (!deleteOrderSuccessResponse.Success)
                    {
                        Console.WriteLine("Error: " + deleteOrderSuccessResponse.ErrorMessage);
                    }
                    break;

                case ConsoleKey.G:
                    restRequest = new RestRequest("customers", Method.GET);
                    IRestResponse customersResponse = restClient.Execute(restRequest);
                    List<Customer> customersContent = JavaScriptSerializer.Deserialize<List<Customer>>(customersResponse.Content);
                    Console.Clear();
                    ConsoleOutput.Customers();
                    foreach (Customer customer in customersContent)
                    {
                        Console.WriteLine(customer.Name);
                        foreach (string order in customer.Orders)
                        {
                            Console.WriteLine("  |--" + order);
                        }
                    }
                    break;

                case ConsoleKey.H:
                    ConsoleOutput.GetOrders();
                    requestParameter = GetParameter("Specify customer name: ");
                    restRequest = new RestRequest("order/{customername}", Method.GET);
                    restRequest.AddUrlSegment("customername", requestParameter);
                    IRestResponse ordersResponse = restClient.Execute(restRequest);
                    List<string> ordersContent = JavaScriptSerializer.Deserialize<List<string>>(ordersResponse.Content);
                    Console.Clear();
                    ConsoleOutput.Orders();
                    Console.WriteLine(requestParameter);
                    foreach (string order in ordersContent)
                    {
                        Console.WriteLine("  |-" + order);
                    }
                    break;

                case ConsoleKey.J:
                    ConsoleOutput.AddCustomer();
                    requestParameter = GetParameter("Specify customer name: ");
                    restRequest = new RestRequest("customer/{customername}", Method.PUT);
                    restRequest.AddUrlSegment("customername", requestParameter);
                    IRestResponse newCustomerResponse = restClient.Execute(restRequest);
                    SuccessResponse customerSuccessResponse = JavaScriptSerializer.Deserialize<SuccessResponse>(newCustomerResponse.Content);
                    Console.WriteLine("Success: " + customerSuccessResponse.Success);
                    if (!customerSuccessResponse.Success)
                    {
                        Console.WriteLine("Error: " + customerSuccessResponse.ErrorMessage);
                    }
                    break;

                case ConsoleKey.K:
                    ConsoleOutput.AddOrder();
                    requestParameter = GetParameter("Specify customer name: ");
                    orderName = GetParameter("Specify order name: ");
                    restRequest = new RestRequest("order/{customername}/{ordername}", Method.PUT);
                    restRequest.AddUrlSegment("customername", requestParameter);
                    restRequest.AddUrlSegment("ordername", orderName);
                    IRestResponse newOrderResponse = restClient.Execute(restRequest);
                    SuccessResponse orderSuccessResponse = JavaScriptSerializer.Deserialize<SuccessResponse>(newOrderResponse.Content);
                    Console.WriteLine("Success: " + orderSuccessResponse.Success);
                    if (!orderSuccessResponse.Success)
                    {
                        Console.WriteLine("Error: " + orderSuccessResponse.ErrorMessage);
                    }
                    break;

                case ConsoleKey.X:
                    ChooseMethod();
                    break;

                default:
                    ChooseRestRequest();
                    break;
            }
            StartFromBeginning();
            ChooseRestRequest();
        }

        private static string GetParameter(string text)
        {
            Console.Write(text);
            string parameter = Console.ReadLine();
            if (parameter != String.Empty)
            {
                return parameter;
            }
            return "Emtpy";
        }

        private static void StartFromBeginning()
        {
            Console.WriteLine();
            Console.WriteLine("************************");
            Console.WriteLine("Start over? y/n. n will close the application");
            ConsoleKeyInfo info = Console.ReadKey(true);
            if (info.Key == ConsoleKey.Y)
            {
                if (rest)
                {
                    ChooseRestRequest();
                }
                else
                {
                    ChooseSoapRequest();
                }
            }
            else if (info.Key == ConsoleKey.N)
            {
                Environment.Exit(0);
            }
        }
    }
}
