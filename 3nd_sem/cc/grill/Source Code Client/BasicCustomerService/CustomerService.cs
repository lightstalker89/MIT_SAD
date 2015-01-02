namespace BasicCustomerService
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using BasicCustomerServiceInterfaces;
    using BasicCustomerServiceInterfaces.Models;
    using CustomerMockUpDb;

    [ServiceBehavior(Namespace = "http://grill.com/webservices/", Name = "CustomerService")]
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

        public bool AddOrder(string customerName, List<string> order)
        {
            Console.WriteLine("- Add order to Customer {0}", customerName);
            var customer = this.db.GetCustomer(customerName);
            if (customer == null) return false;
            customer.CustomerOrders.Add(order);
            return true;
        }

        public bool AddCustomer(string customerName)
        {
            Console.WriteLine("- Add Customer {0} to service", customerName);
            this.db.AddCustomer(customerName);
            return true;
        }

        public bool DeleteCustomer(string customerName)
        {
            Console.WriteLine("- Delete Customer {0}", customerName);
            return this.db.DeleteCustomer(customerName);
        }

        public bool DeleteOrder(string customerName, int orderIndex)
        {
            Console.WriteLine("- Delete order from Customer {0}", customerName);
            return this.db.DeleteOrder(customerName, orderIndex);
        }
    }
}
