using System;

namespace RestSoapClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ChooseMethod();
        }

        private static void ChooseMethod()
        {
            Console.WriteLine("Please choose request method");
            Console.WriteLine("S) SOAP");
            Console.WriteLine("R) REST");
            ConsoleKeyInfo keyInfo = Console.ReadKey(false);
            if (keyInfo.Key == ConsoleKey.S)
            {

            }
            else if (keyInfo.Key == ConsoleKey.R)
            {

            }
            else
            {
                ChooseMethod();
            }
        }

        private static void ChooseRequest()
        {
            Console.WriteLine("A)");
        }
    }
}
