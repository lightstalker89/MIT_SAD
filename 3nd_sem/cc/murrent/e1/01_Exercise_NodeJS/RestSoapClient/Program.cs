using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Xml;
using RestSharp;
using RestSoapClient.Models;

namespace RestSoapClient
{
    public class Program
    {
        private static bool rest = true;
        private static readonly JavaScriptSerializer JavaScriptSerializer = new JavaScriptSerializer();
        private const string EndPoint = "http://localhost:1338";
        private static RestClient restClient;
        private static readonly Dictionary<ConsoleKey, string> RequestOptions = new Dictionary<ConsoleKey, string>
        {
            {ConsoleKey.S, "S) SOAP"},
            {ConsoleKey.R, "R) REST"}
        };
        private static readonly Dictionary<ConsoleKey, string> Requests = new Dictionary<ConsoleKey, string>
        {
            {ConsoleKey.G, "G) Get Customers"},
            {ConsoleKey.H, "H) Get Orders"},
            {ConsoleKey.J, "J) Add Customer"},
            {ConsoleKey.K, "K) Add Order"},
            {ConsoleKey.D, "D) Delete Customer"},
            {ConsoleKey.E, "F) Delete Order"}
        };

        public static void Main(string[] args)
        {
            ChooseMethod();
        }

        private static void ChooseMethod()
        {
            Console.Clear();
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Please choose request method");
            Console.WriteLine("---------------------------------");
            WriteDictionaryOptionsToConsole(RequestOptions);
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

        private static void ChangeRequest()
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine("X) Change request method");
            Console.WriteLine();
        }

        private static void ChooseSoapRequest()
        {
            string requestParameter = "";
            string response = "";
            Console.Clear();
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Please choose request method");
            Console.WriteLine("---------------------------------");
            WriteDictionaryOptionsToConsole(Requests);
            ChangeRequest();
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.D:
                    requestParameter = GetParameter("Delete customer: ");
                    response = CallWebService("deleteCustomer", "<customerName>" + requestParameter + "</customerName>");
                    XmlDocument deleteCustomerDocument = new XmlDocument();
                    deleteCustomerDocument.LoadXml(response);
                    break;

                case ConsoleKey.F:
                    requestParameter = GetParameter("Delete order for customer: ");
                    response = CallWebService("deleteOrder", "<customerName>" + requestParameter + "</customerName>");
                    XmlDocument deleteOrderDocument = new XmlDocument();
                    deleteOrderDocument.LoadXml(response);
                    break;

                case ConsoleKey.G:
                    response = CallWebService("getCustomers", "<all></all>");
                    XmlDocument getCustomerDocument = new XmlDocument();
                    getCustomerDocument.LoadXml(response);
                    break;

                case ConsoleKey.H:
                    requestParameter = GetParameter("Get orders for customer: ");
                    response = CallWebService("getOrders", "<customerName>" + requestParameter + "</customerName>");
                    XmlDocument getOrdersDocument = new XmlDocument();
                    getOrdersDocument.LoadXml(response);
                    break;

                case ConsoleKey.J:
                    requestParameter = GetParameter("New customer name: ");
                    response = CallWebService("addCustomer", "<customerName>" + requestParameter + "</customerName>");
                    XmlDocument addCustomerDocument = new XmlDocument();
                    addCustomerDocument.LoadXml(response);
                    break;

                case ConsoleKey.K:
                    requestParameter = GetParameter("Add order for customer: ");
                    response = CallWebService("addOrder", "<customerName>" + requestParameter + "</customerName>");
                    XmlDocument addOrDocument = new XmlDocument();
                    addOrDocument.LoadXml(response);
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

        private static void WriteDictionaryOptionsToConsole(Dictionary<ConsoleKey, string> dictionary)
        {
            foreach (var entry in dictionary)
            {
                Console.WriteLine(entry.Value);
            }
        }

        private static void ChooseRestRequest()
        {
            RestRequest restRequest = null;
            string requestParameter = String.Empty;
            Console.Clear();
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Please Choose Request");
            Console.WriteLine("---------------------------------");
            WriteDictionaryOptionsToConsole(Requests);
            ChangeRequest();
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.D:
                    requestParameter = GetParameter("Delete customer: ");
                    restRequest = new RestRequest("customer/delete/{name}", Method.DELETE);
                    restRequest.AddUrlSegment("name", requestParameter);
                    IRestResponse deleteCustomerResponse = restClient.Execute(restRequest);
                    SuccessResponse deleteCustomerSuccessResponse = JavaScriptSerializer.Deserialize<SuccessResponse>(deleteCustomerResponse.Content);
                    Console.WriteLine("Success: " + deleteCustomerSuccessResponse.Success);
                    if (!deleteCustomerSuccessResponse.Success)
                    {
                        Console.WriteLine("Error: " + deleteCustomerSuccessResponse.ErrorMessage);
                    }
                    break;

                case ConsoleKey.F:
                    requestParameter = GetParameter("Delete order for customer: ");
                    restRequest = new RestRequest("order/delete/{name}", Method.DELETE);
                    restRequest.AddUrlSegment("name", requestParameter);
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
                    Console.WriteLine("-------------------------");
                    Console.WriteLine("Customers");
                    Console.WriteLine("-------------------------");
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
                    requestParameter = GetParameter("Order for customer: ");
                    restRequest = new RestRequest("order/{name}", Method.GET);
                    restRequest.AddUrlSegment("name", requestParameter);
                    IRestResponse ordersResponse = restClient.Execute(restRequest);
                    List<string> ordersContent = JavaScriptSerializer.Deserialize<List<string>>(ordersResponse.Content);
                    Console.Clear();
                    Console.WriteLine("-------------------------");
                    Console.WriteLine("Orders");
                    Console.WriteLine("-------------------------");
                    Console.WriteLine(requestParameter);
                    foreach (string order in ordersContent)
                    {
                        Console.WriteLine("  |-" + order);
                    }
                    break;

                case ConsoleKey.J:
                    requestParameter = GetParameter("New customer name: ");
                    restRequest = new RestRequest("customer/add/{name}", Method.PUT);
                    restRequest.AddUrlSegment("name", requestParameter);
                    IRestResponse newCustomerResponse = restClient.Execute(restRequest);
                    SuccessResponse customerSuccessResponse = JavaScriptSerializer.Deserialize<SuccessResponse>(newCustomerResponse.Content);
                    Console.WriteLine("Success: " + customerSuccessResponse.Success);
                    if (!customerSuccessResponse.Success)
                    {
                        Console.WriteLine("Error: " + customerSuccessResponse.ErrorMessage);
                    }
                    break;

                case ConsoleKey.K:
                    requestParameter = GetParameter("New order for customer: ");
                    restRequest = new RestRequest("order/add/{name}", Method.PUT);
                    restRequest.AddUrlSegment("name", requestParameter);
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

        public static string CallWebService(string action, string parameter)
        {
            string responseString = "";
            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(action, parameter);
            HttpWebRequest webRequest = CreateWebRequest("http://localhost:1337/SOAPWebService", action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            webRequest.BeginGetResponse(delegate(IAsyncResult asynchronousResult)
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                responseString = streamRead.ReadToEnd();
                Console.WriteLine(responseString);
                streamResponse.Close();
                streamRead.Close();
                response.Close();
            }, webRequest);

            return responseString;
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.KeepAlive = false;
            webRequest.Timeout = 300000;
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(string action, string parameter)
        {
            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(@"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""><SOAP-ENV:Header></SOAP-ENV:Header><SOAP-ENV:Body><" + action + ">" + parameter + "</" + action + "></SOAP-ENV:Body></SOAP-ENV:Envelope>");
            return soapEnvelop;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
    }
}
