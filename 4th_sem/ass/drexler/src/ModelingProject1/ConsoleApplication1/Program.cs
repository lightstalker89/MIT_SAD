//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ClassDiagram;

    /// <summary>
    /// Test applicaton
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;

            List<Person> pList = new List<Person>();
            List<Animal> aList = new List<Animal>();

            Console.WriteLine("Press 'A' to add a new animal");
            Console.WriteLine("Press 'P' to add a new person");
            Console.WriteLine("Press 'B' to buy a new dog");
            Console.WriteLine("Press 'U' to play with the animal");
            Console.WriteLine("Press 'L' to play with the dog");
            Console.WriteLine("Press 'K' to kill the animal");

            do
            {
                cki = Console.ReadKey(true);
                switch(cki.Key)
                {
                    case ConsoleKey.P:
                        Person a = new Person();
                        pList.Add(a);
                        Console.WriteLine("Person added");
                        break;
                    case ConsoleKey.A:
                        Animal animal = new Animal();
                        aList.Add(animal);
                        Console.WriteLine("Animal added");
                        break;
                    case ConsoleKey.B:
                        Person firstPerson = pList.FirstOrDefault();
                        if (firstPerson != null)
                        {
                            firstPerson.BuyDog();
                            Console.WriteLine("First Person baught a dog!");
                        }
                        else
                        {
                            Console.WriteLine("No person found!");
                        }
                        
                        break;
                    case ConsoleKey.L:
                        Person firstPerson2 = pList.FirstOrDefault();
                        if(firstPerson2 != null)
                        {
                            firstPerson2.PlayWithDog();
                        }
                        else
                        {
                            Console.WriteLine("No person found!");
                        }

                        break;
                    case ConsoleKey.U:
                        Person firstPerson1 = pList.FirstOrDefault();
                        if (firstPerson1 != null)
                        {
                            firstPerson1.PlayWithAnimal();
                            Console.WriteLine("First Person plays with animal");
                        }
                        else
                        {
                            Console.WriteLine("No person found!");
                        }
                        
                        break;
                    case ConsoleKey.K:
                        if(pList.FirstOrDefault() != null)
                        {
                            pList.FirstOrDefault().Animal = null;
                            GC.Collect();
                            Console.WriteLine("Animal of first person was killed");
                        }
                        else
                        {
                            Console.WriteLine("No animal to kill!");
                        }
                        
                        break;
                }
            }
            while (cki.Key != ConsoleKey.Escape);

            Console.ReadLine();
        }
    }
}
