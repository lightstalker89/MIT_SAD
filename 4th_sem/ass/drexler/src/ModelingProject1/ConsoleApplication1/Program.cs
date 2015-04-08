using ClassDiagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;

            List<Person> pList = new List<Person>();
            List<Animal> aList = new List<Animal>();

            Console.WriteLine("Press 'A' to add a new animal");
            Console.WriteLine("Press 'P' to add a new person");
            Console.WriteLine("Press 'B' to buy a new animal");
            Console.WriteLine("Press 'U' to play with the animal");
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
                        pList.FirstOrDefault().BuyDog();
                        Console.WriteLine("First Person baught animal");
                        break;
                    case ConsoleKey.U:
                        pList.FirstOrDefault().PlayWithAnimal();
                        Console.WriteLine("First Person plays with animal");
                        break;
                    case ConsoleKey.K:
                        pList.FirstOrDefault().Animal = null;
                        GC.Collect();
                        Console.WriteLine("Animal of first person was killed");
                        break;
                }
            }
            while (cki.Key != ConsoleKey.Escape);

            //Console.WriteLine("Geben Sie bitte den Pfad für ein ModelingProject (ModelingProject1.modelproj) an");
            //string projectPath = Console.ReadLine();
            //Console.WriteLine("Geben Sie bitte den Dateinamen des Class Diagram an");
            //string diagramFileName = Console.ReadLine();

            //if (string.IsNullOrEmpty(projectPath))
            //{
            //    projectPath = @"C:\Users\Michael Drexler\Desktop\FHWN\MIT13_CodeSharing\4th_sem\ass\drexler\src\ModelingProject1\ModelingProject1\ModelingProject1.modelproj";
            //}

            //if (string.IsNullOrEmpty(diagramFileName))
            //{
            //    diagramFileName = @"UMLClassDiagram1.classdiagram";
            //}

            //try
            //{
            //    ModelReader mReader = new ModelReader();
            //    mReader.ReadClassDiagram(projectPath, diagramFileName);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            Console.ReadLine();
        }
    }
}
