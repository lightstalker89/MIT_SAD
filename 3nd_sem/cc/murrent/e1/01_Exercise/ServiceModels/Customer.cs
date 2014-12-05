using System;
using System.Collections.Generic;

namespace ServiceModels
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public List<Order> Orders { get; set; }
    }
}
