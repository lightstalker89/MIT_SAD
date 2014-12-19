using System;
using System.Collections.Generic;

namespace RestSoapClient
{
    public static class ConsoleOutput
    {
        public static readonly Dictionary<ConsoleKey, string> RequestOptions = new Dictionary<ConsoleKey, string>
        {
            {ConsoleKey.S, "S) SOAP"},
            {ConsoleKey.R, "R) REST"}
        };

        public static readonly Dictionary<ConsoleKey, string> Requests = new Dictionary<ConsoleKey, string>
        {
            {ConsoleKey.G, "G) Get Customers"},
            {ConsoleKey.H, "H) Get Orders"},
            {ConsoleKey.J, "J) Add Customer"},
            {ConsoleKey.K, "K) Add Order"},
            {ConsoleKey.D, "D) Delete Customer"},
            {ConsoleKey.E, "F) Delete Order"}
        };

        public static void WriteDictionaryOptionsToConsole(Dictionary<ConsoleKey, string> dictionary)
        {
            foreach (var entry in dictionary)
            {
                Console.WriteLine(entry.Value);
            }
        }

        public static void AddOrder()
        {
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("---------  Add new order  ---------");
            Console.WriteLine("-----------------------------------");
        }

        public static void AddCustomer()
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine("-------  Add new customer  -------");
            Console.WriteLine("----------------------------------");
        }

        public static void GetOrders()
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("-------  Get orders for customer  -------");
            Console.WriteLine("-----------------------------------------");
        }

        public static void GetCustomers()
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine("--------  Get customers  --------");
            Console.WriteLine("---------------------------------");
        }

        public static void DeleteOrder()
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine("--------  Delete order  --------");
            Console.WriteLine("--------------------------------");
        }

        public static void DeleteCustomer()
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine("-------  Delete customer  -------");
            Console.WriteLine("---------------------------------");
        }

        public static void ChooseRequest()
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Please Choose Request");
            Console.WriteLine("---------------------------------");
        }

        public static void ChooseRequestMethod()
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Please choose request method");
            Console.WriteLine("---------------------------------");
        }

        public static void Customers()
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine("Customers");
            Console.WriteLine("-------------------------");
        }

        public static void Orders()
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine("Orders");
            Console.WriteLine("-------------------------");
        }

        public static void GoBack()
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine("X) Change request method");
        }
    }
}
