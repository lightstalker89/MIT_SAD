using System;

namespace ServiceModels
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateOfOrder { get; set; }
        public string Description { get; set; }
        public Product Products { get; set; }
    }
}
