using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Beispiel1C
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInteger a = Fakultaet.BerechneStandardRekusion(800);
            Console.WriteLine(a.ToString());
            BigInteger b = Fakultaet.BerechneTailRekursion(800, 1);
            Console.WriteLine(b.ToString());     
            BigInteger c = Fakultaet.BerechneOhneRekursion(800);
            Console.WriteLine(c.ToString());
            Console.ReadKey();
        }
    }
}
