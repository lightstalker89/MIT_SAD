using ServiceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI
{
    class ServiceREST : IService
    {
        private HttpClient client;

        public ServiceREST()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost/ServiceREST/Service1.svc/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<ServiceData.Customer> GetCustomers()
        {
            HttpResponseMessage response = client.GetAsync("customers").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<IEnumerable<Customer>>().Result.ToList();
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return new List<Customer>();
            }
        }

        public List<ServiceData.Order> GetOrders(string customerName)
        {
            HttpResponseMessage response = client.GetAsync("orders/" + customerName).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<IEnumerable<Order>>().Result.ToList();
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return new List<Order>();
            }
        }

        public bool AddOrder(ServiceData.Order order)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("order/add", order).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Boolean>().Result; 
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return false;
            }
        }

        public bool AddCustomer(ServiceData.Customer customer)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("customer/add", customer).Result; 
            if (response.IsSuccessStatusCode) 
            {
                return response.Content.ReadAsAsync<Boolean>().Result; 
            } 
            else 
            { 
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return false;
            }
        }

        public bool DeleteOrder(ServiceData.Order order)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("order/delete", order).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Boolean>().Result; 
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return false;
            }
        }

        public bool DeleteCustomer(ServiceData.Customer customer)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("customer/delete", customer).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Boolean>().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return false;
            }
        }
    }
}
