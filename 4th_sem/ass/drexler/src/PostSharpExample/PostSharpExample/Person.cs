using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostSharpExample
{
    public class Person
    {
        public Animal MyAnimal { get; set; }

        public void BuyDog()
        {
            this.MyAnimal = new Animal();
        }
    }
}
