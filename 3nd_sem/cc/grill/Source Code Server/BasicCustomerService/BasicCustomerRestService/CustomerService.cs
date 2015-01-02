using System.Linq;

namespace BasicCustomerRestService
{
    using System;
    using System.Collections.Generic;
    using BasicCustomerRestServiceInterfaces;
    using BasicCustomerRestServiceInterfaces.Models;
    using CustomerRestMockUpDb;

    public class CustomerService : ICustomerService
    {
        private readonly CustomerDatabase db;

        public CustomerService()
        {
            this.db = CustomerDatabase.getInstance();
        }

        public List<Customer> GetCustomers()
        {
            Console.WriteLine("- Send current customer list to client");
            return this.db.GetCustomerList();
        }

        public List<List<string>> GetOrders(string customerName)
        {
            Console.WriteLine("- Send order list from customer {0}", customerName);
            var customer = this.db.GetCustomer(customerName);
            if (customer == null) return new List<List<string>>();
            return this.db.GetCustomer(customerName).CustomerOrders;
        }

        // Workaround
        public bool AddOrder(string customerName, string order)
        {
            List<string> orderList = order.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            Console.WriteLine("- Add order to Customer {0}", customerName);
            var customer = this.db.GetCustomer(customerName);
            if (customer == null) return false;
            customer.CustomerOrders.Add(orderList);
            return true;
        }

        public bool AddCustomer(string customerName)
        {
            Console.WriteLine("- Add Customer {0} to service", customerName);
            this.db.AddCustomer(customerName);
            return true;
        }

        public bool DeleteOrder(string customerName, string orderIndex)
        {
            Console.WriteLine("- Delete order from Customer {0}", customerName);
            int index;
            if (int.TryParse(orderIndex, out index))
            {
                return this.db.DeleteOrder(customerName, index);
            }

            return false;
        }

        public bool DeleteCustomer(string customerName)
        {
            Console.WriteLine("- Delete Customer {0}", customerName);
            return this.db.DeleteCustomer(customerName);
        }
    }
}
