using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveCalculation
{
    public class FactorialCalc
    {

        public long Calc(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException("No positive number!");
            }

            if (value == 0)
                return 1;
            long factor = 1;
            for (int i = value; i > 0; --i)
            {
                factor *= i;
            }
            return factor;
        }

        public long CalcRecursive(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException("No positive number!");
            }

            if (value == 0)
                return 1;
            return value * this.CalcRecursive(value - 1);
        }

        public long CalcTailRecursive(int temp, long value)
        {
            //TODO!!!!!
            if (temp < 0)
            {
                throw new ArgumentException("No positive number!");
            }
            Console.WriteLine("temp: {0}", temp);
            return temp < 2 ? value : this.CalcTailRecursive(temp - 1, value * temp);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            FactorialCalc factor = new FactorialCalc();
            long normal = factor.Calc(21);
            long recursive = factor.CalcRecursive(21);
            long tailRecursive = factor.CalcTailRecursive(21, 1);

            Console.WriteLine("results: \nNormal: {0}\nRecursive: {1}\nTail Recursive: {2}", normal, recursive, tailRecursive);

        }
    }
}
