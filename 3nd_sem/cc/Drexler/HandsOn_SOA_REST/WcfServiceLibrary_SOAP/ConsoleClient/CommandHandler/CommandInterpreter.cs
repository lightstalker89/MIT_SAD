//-----------------------------------------------------------------------
// <copyright file="CommandInterpreter.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace ConsoleClient.CommandHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DataAccessService;
    using ServiceRef;

    public class CommandInterpreter
    {
        private IService serviceProvider;

        /// <summary>
        /// Try to match a given command to a predefined command
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Commands ReadCommand()
        {
            Console.WriteLine("Choose and enter a command to execute! (Case sensitive)");
            string input = Console.ReadLine();
            Console.WriteLine("Which service type do you want to use? (rest or soap)");
            string serviceType = Console.ReadLine();

            ServiceType svcType;
            Enum.TryParse(serviceType, out svcType);

            Commands command;

            if (string.IsNullOrEmpty(input) || !Enum.TryParse(input, out command))
            {
                return Commands.Invalid;
            }

            switch (svcType)
            {
                case ServiceType.rest: 
                    this.serviceProvider = new RESTService("http://localhost:9000/rest");
                    break;
                case ServiceType.soap: 
                    this.serviceProvider = new SOAPService();
                    break;
                default:
                    this.serviceProvider = new RESTService("http://localhost:9000/rest");
                    break;
            }

            return command;
        }

        /// <summary>
        /// Execute a service command depending on the service type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="command"></param>
        public void ExecuteCommand(Commands command)
        {
            switch (command)
            {
                case Commands.GetCustomers:
                    ServiceRef.Customer[] customers = this.serviceProvider.GetAllCustomers();

                    foreach (var cus in customers)
                    {
                        Console.WriteLine("Id:{0}, Forename:{1}, Lastname:{2}, CountOfOrders:{3}", cus.Id, cus.Forename, cus.Lastname, (cus.CustomerOrders != null ? cus.CustomerOrders.Count().ToString() : "null"));
                    }

                    break;
                case Commands.AddCustomer:
                    Customer c = new Customer();
                    long id;
                    Console.WriteLine("Enter an id of the new customer!");
                    long.TryParse(Console.ReadLine(), out id);
                    c.Id = id;
                    Console.WriteLine("Enter a forename of the new customer!");
                    c.Forename = Console.ReadLine();
                    Console.WriteLine("Enter a lastname of the new customer!");
                    c.Lastname = Console.ReadLine();

                    if(this.serviceProvider.AddCustomer(c))
                    {
                        Console.WriteLine("Customer Added!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add customer!");
                    }
                    
                    break;
                case Commands.DeleteCustomer:
                    long cId;
                    Console.WriteLine("Enter an Id of the customer you want to delete!");
                    long.TryParse(Console.ReadLine(), out cId);
                    
                    if(this.serviceProvider.DeleteCustomer(cId))
                    {
                        Console.WriteLine("Customer Deleted!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete customer! Id was {0}", cId);
                    }
                    
                    break;
                case Commands.AddOrder:
                    long cuId;
                    Order order = new Order();
                    Console.WriteLine("Enter an Id of the customer you want to add an order!");
                    long.TryParse(Console.ReadLine(), out cuId);
                    long oId;                
                    Console.WriteLine("Enter an Id for the new order!");
                    long.TryParse(Console.ReadLine(), out oId);
                    order.OrderId = oId;
                    Console.WriteLine("Enter a new order name!");
                    order.OrderName = Console.ReadLine();

                    if (this.serviceProvider.AddOrder(cuId, order))
                    {
                        Console.WriteLine("Order added!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add new order!");
                    }
                    break;
                case Commands.GetOrdersForCustomer:
                    long cusId;
                    Console.WriteLine("Enter an Id of the customer you want to get the orders!");
                    long.TryParse(Console.ReadLine(), out cusId);

                    Order[] orders = this.serviceProvider.GetOrdersForCustomer(cusId);

                    foreach (var o in orders)
                    {
                        Console.WriteLine("     OrderId:{0}, OrderName:{1}", o.OrderId, o.OrderName);
                    }

                    break;
                case Commands.DeleteOrder:
                    long custId;
                    Console.WriteLine("Enter an customer id for who you want to delete a order!");
                    long.TryParse(Console.ReadLine(), out custId);

                    long ordId;
                    Console.WriteLine("Enter an order id for the customer {0}, you want to delete a order!", custId);
                    long.TryParse(Console.ReadLine(), out ordId);

                    if (this.serviceProvider.DeleteOrder(custId, ordId))
                    {
                        Console.WriteLine("Order deleted!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete the order!");
                    }

                    break;
            }
        }

        public enum Commands
        {
            GetCustomers,
            AddCustomer,
            DeleteCustomer,
            GetOrdersForCustomer,
            AddOrder,
            DeleteOrder,
            Invalid
        }

        public enum ServiceType
        {
            soap,
            rest
        }
    }
}
