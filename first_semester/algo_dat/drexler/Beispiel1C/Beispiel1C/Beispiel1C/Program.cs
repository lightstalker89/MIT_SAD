using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beispiel1C
{
    class Program
    {
        static void Main(string[] args)
        {
            long a = Fakultaet.BerechneStandardRekusion(1000000);
            Console.WriteLine(a.ToString());
            long b = Fakultaet.BerechneTailRekursion(1000000, 1);
            Console.WriteLine(b.ToString());     
            long c = Fakultaet.BerechneOhneRekursion(1000000);
            Console.WriteLine(c.ToString());
            Console.ReadKey();
        }
    }
}
