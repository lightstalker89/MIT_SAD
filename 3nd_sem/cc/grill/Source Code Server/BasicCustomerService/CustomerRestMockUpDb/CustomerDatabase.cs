namespace CustomerRestMockUpDb
{
    using System.Collections.Generic;
    using BasicCustomerRestServiceInterfaces.Models;

    public class CustomerDatabase
    {
        private static CustomerDatabase instance;

        private readonly List<Customer> customers = new List<Customer>()
        {
            new Customer() { CustomerName = "Daniel Kienböck",  CustomerOrders = new List<List<string>>() { new List<string> { "DVD", "Handy", "Laptop"}, new List<string> { "Buch", "Stofftier", "Jacke"}}},
            new Customer() { CustomerName = "Mario Murrent", CustomerOrders = new List<List<string>>() { new List<string> { "Gameboy", "Handy", "PC-Spiel"}}},
            new Customer() { CustomerName = "Bernhard Stöckl" },
            new Customer() { CustomerName = "Roland Lehner", CustomerOrders = new List<List<string>>() { new List<string> { "DVD", "Spiele-Konsole"}}},
        };

        private CustomerDatabase()
        {
        }

        public static CustomerDatabase getInstance()
        {
            if (instance != null) return instance;
            instance = new CustomerDatabase();
            return instance;
        }

        public void AddCustomer(string customer)
        {
            this.customers.Add(new Customer { CustomerName = customer });
        }

        public bool DeleteCustomer(string name)
        {
            var customer = this.customers.Find(x => x.CustomerName == name);
            if (customer == null) return false;
            return this.customers.Remove(customer);
        }

        public bool DeleteOrder(string name, int orderIndex)
        {
            var customer = this.customers.Find(x => x.CustomerName == name);
            if (customer == null) return false;
            if (customer.CustomerOrders.Count - 1 < orderIndex) return false;
            customer.CustomerOrders.RemoveAt(orderIndex);
            return true;
        }

        public Customer GetCustomer(string name)
        {
            return this.customers.Find(x => x.CustomerName == name);
        }

        public List<Customer> GetCustomerList()
        {
            return this.customers;
        }
    }
}
