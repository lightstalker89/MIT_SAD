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
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //DisplayNums(nums);
            //sw.Stop();

            //Timing tm = new Timing();
            //tm.StartTime();
            //DisplayNums(nums);
            //tm.StopTime();

            /*
            string input = string.Empty;
            while (input != "exit")
            {
                input = Console.ReadLine();
                int n = 0;
                Int32.TryParse(input, out n);
                int erwartetesErgebnis = 1;
                for (int i = 1; i <= n; i++)
                {
                    erwartetesErgebnis *= i;
                }
                Console.WriteLine("Erwartetes Ergebnis: {0}", erwartetesErgebnis.ToString());
                int result = Fakultaet.BerechneStandardRekusion(n);
                Console.WriteLine("Ergebnis: {0}", result.ToString());
            }
            */
        }
    }
}
