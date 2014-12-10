using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Web.Script.Serialization;
using RestSharp;
using System.Web;

namespace RestSoapClient
{
    class Program
    {
        private static JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        private const string RestEndPoint = "http://localhost:1337";
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
        static void Main(string[] args)
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

            }
            else if (keyInfo.Key == ConsoleKey.R)
            {
                restClient = new RestClient(RestEndPoint);
                ChooseRestRequest();
            }
            else
            {
                ChooseMethod();
            }

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
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.D:
                    requestParameter = GetParameter("Customername: ");
                    restRequest = new RestRequest("customer/delete/{name}", Method.DELETE);
                    restRequest.AddParameter("name", requestParameter);
                    IRestResponse customerDeleteResponse = restClient.Execute(restRequest);
                    dynamic customerDeleteContent = javaScriptSerializer.Deserialize<dynamic>(customerDeleteResponse.Content);
                    Console.WriteLine("Success: " + customerDeleteContent);
                    break;

                case ConsoleKey.F:
                    requestParameter = GetParameter("Ordername: ");
                    restRequest = new RestRequest("order/delete/{name}", Method.DELETE);
                    restRequest.AddParameter("name", requestParameter);
                    IRestResponse orderDeleteResponse = restClient.Execute(restRequest);
                    dynamic orderDeleteContent = javaScriptSerializer.Deserialize<dynamic>(orderDeleteResponse.Content);
                    Console.WriteLine("Success: " + orderDeleteContent);
                    break;

                case ConsoleKey.G:
                    restRequest = new RestRequest("customers", Method.GET);
                    IRestResponse customersResponse = restClient.Execute(restRequest);
                    dynamic customersContent = javaScriptSerializer.Deserialize<dynamic>(customersResponse.Content);
                    Console.WriteLine(customersContent);
                    break;

                case ConsoleKey.H:
                    requestParameter = GetParameter("Ordername: ");
                    restRequest = new RestRequest("order", Method.POST);
                    restRequest.AddParameter("name", requestParameter);
                    IRestResponse ordersResponse = restClient.Execute(restRequest);
                    dynamic ordersContent = javaScriptSerializer.Deserialize<dynamic>(ordersResponse.Content);
                    Console.WriteLine(ordersContent);
                    break;

                case ConsoleKey.J:
                    requestParameter = GetParameter("New customer name: ");
                    restRequest = new RestRequest("customer/add/{name}", Method.PUT);
                    restRequest.AddParameter("name", requestParameter);
                    IRestResponse response = restClient.Execute(restRequest);
                    dynamic content = javaScriptSerializer.Deserialize<dynamic>(response.Content);
                    Console.WriteLine("Success: " + content);
                    break;

                case ConsoleKey.K:
                    requestParameter = GetParameter("New order name: ");
                    restRequest = new RestRequest("order/add/{name}", Method.PUT);
                    restRequest.AddParameter("name", requestParameter);
                    IRestResponse orderAddResponse = restClient.Execute(restRequest);
                    dynamic orderAddContent = javaScriptSerializer.Deserialize<dynamic>(orderAddResponse.Content);
                    Console.WriteLine("Success: " + orderAddContent);
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
            return parameter;
        }

        private static void StartFromBeginning()
        {
            Console.WriteLine("Start over? j/n. n will close the application");
            ConsoleKeyInfo info = Console.ReadKey(true);
            if (info.Key == ConsoleKey.J)
            {
                ChooseMethod();
            }
            else
            {
              Environment.Exit(0);
            }
        }
    }
}
