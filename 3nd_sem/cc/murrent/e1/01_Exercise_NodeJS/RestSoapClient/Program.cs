using System;
using System.Collections.Generic;
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
            }
            else
            {
                ChooseMethod();
            }
            ChooseRequest();
        }

        private static void WriteDictionaryOptionsToConsole(Dictionary<ConsoleKey, string> dictionary)
        {
            foreach (var entry in dictionary)
            {
                Console.WriteLine(entry.Value);
            }
        }

        private static void ChooseRequest()
        {
            RestRequest restRequest = null;
            Console.Clear();
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Please Choose Request");
            Console.WriteLine("---------------------------------");
            WriteDictionaryOptionsToConsole(Requests);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.D:
                    break;

                case ConsoleKey.F:
                    break;

                case ConsoleKey.G:
                    restRequest = new RestRequest("customers", Method.GET);
                    break;

                case ConsoleKey.H:
                    break;
                case ConsoleKey.J:
                    break;

                case ConsoleKey.K:
                    break;
            }

            IRestResponse response = restClient.Execute(restRequest);
            dynamic content = javaScriptSerializer.Deserialize<dynamic>(response.Content);
            Console.WriteLine(content);
            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
        }
    }
}
