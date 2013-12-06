using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueStack
{
    class Program
    {
        private static bool loop = true;
        private static QueueClass queue;
        private static StackClass stack;

        static void Main(string[] args)
        {
            queue = new QueueClass();
            stack = new StackClass();

            Console.WriteLine("Welcome\n");

            while (loop)
            {
                Console.WriteLine("What do you want to use: -> Stack or Queue");
                Console.Write("Type s for Stack, q for Queue or e for Exit: ");
                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "q":
                        useQueue();
                        break;
                    case "s":
                        //useStack();
                        break;
                    case "e":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Illegal operation");
                        Console.Clear();
                        break;

                }
            }

            Console.ReadKey();


        }

        private static void useQueue()
        {
            Console.WriteLine("You are using queue!");
            Console.Write("Type f for Fill something in, t for take something out or r for Return: ");
            string input = Console.ReadLine();
            Console.WriteLine();

            switch (input)
            {
                case "f":
                    fillInQueue();
                    break;
                case "t":
                    takeOutQueue();
                    break;
                case "r":
                    break;
                default:
                    Console.WriteLine("Illegal operation returning back!");
                    Console.Clear();
                    break;

            }
        }

        private static void takeOutQueue()
        {
            Console.Write("How many items you want (max 10)?");
            int count = 0;
            string input;
            string[] elements = new string[10];
            do
            {
                input = Console.ReadLine();
                if (!int.TryParse(input, out count))
                {
                    Console.Write("Please only numbers");
                }
                else
                {
                    if (count <= 1)
                    {
                        elements = queue.TakeOutQueue(count);
                    }
                    else
                    {
                        Console.Write("Too big returning!");
                    }
                }
            } while (int.TryParse(input, out count));

        }

        private static void fillInQueue()
        {
            Console.Write("Type your 10 elements!");
            int count = 0;
            string[] elements = new string[10];
            do
            {
                elements[count] = Console.ReadLine();
                ++count;
                Console.WriteLine();
            } while (count <= 9);
            queue.FillInQueue(elements);
        }
    }
}
