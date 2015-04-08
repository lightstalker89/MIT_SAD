using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostSharpExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Person a = new Person();
            Person b = new Person();

            for (int i = 0; i < 4; i++ )           
            {
                a.BuyDog();
                a.MyAnimal.GibLaut();
            }

            a.MyAnimal = null;
            b.BuyDog();
            b.MyAnimal.GibLaut();
            GC.Collect();

            Console.ReadLine();
        }
    }
}
