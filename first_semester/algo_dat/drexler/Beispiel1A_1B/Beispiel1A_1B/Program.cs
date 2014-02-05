//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Beispiel1A_1B
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ListRandomNumbers;
    using System.Collections;

    class Program
    {
        static void Main(string[] args)
        {
            RandomNumbers numbers = new RandomNumbers();
            ArrayList randomNumbers = numbers.GetNonRepeatingRandomNumbers(10);

            Queue<int> myQueue = new Queue<int>(10);
            Stack<int> myStack = new Stack<int>(10);

            foreach (int i in randomNumbers)
            {
                myQueue.Enqueue(i);
                myStack.Push(i);
            }

            Console.WriteLine("Random Numbers:");
            Console.WriteLine(numbers.MyToString());
            Console.WriteLine("Random Numbers in Queue:");
            Console.WriteLine(myQueue.MyToString());
            Console.WriteLine("Random Numbers in Stack:");
            Console.WriteLine(myStack.MyToString());

            Console.ReadKey();
        }
    }
}
