//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace ConsoleClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommandHandler;

    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("The following commands are available in this program for the services:");
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("GetCustomers");
            Console.WriteLine("AddCustomer");
            Console.WriteLine("DeleteCustomer");
            Console.WriteLine("GetOrdersForCustomer ");
            Console.WriteLine("AddOrder");
            Console.WriteLine("DeleteOrder");
            Console.WriteLine("------------------------------------------------------------------------");

            CommandInterpreter commandInterpreter = new CommandInterpreter();

            while (ConsoleKey.Escape != Console.ReadKey(true).Key)
            {
                CommandInterpreter.Commands command = commandInterpreter.ReadCommand();

                if (command == CommandInterpreter.Commands.Invalid)
                {
                    Console.WriteLine("Invalid command! Try again!");
                    continue;
                }

                commandInterpreter.ExecuteCommand(command);
                Console.WriteLine("Press any key to repeat or [ESC] to finish!");
            }
        }
    }
}
