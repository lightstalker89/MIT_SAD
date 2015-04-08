using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Person
    {
        public Person()
        {
            this.Animal = new Animal();
        }

        public Animal Animal { get; set; }

        public void BuyAnimal()
        {
            this.Animal = new Animal();
            Console.WriteLine("Neues Animal gekauft!");
        }
    }
}
