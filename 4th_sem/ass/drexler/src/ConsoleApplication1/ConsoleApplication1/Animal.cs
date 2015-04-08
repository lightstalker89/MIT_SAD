using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Extensibility;

namespace ConsoleApplication1
{
    [TestAspect]
    public class Animal
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public void GibLaut()
        {
            Console.WriteLine("Wau Wau!");
        }
    }
}
