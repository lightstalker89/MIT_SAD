using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Person a = new Person();
            a.BuyAnimal();
            //a.Animal.GibLaut();

            Console.WriteLine("Geben Sie bitte den Pfad für ein ModelingProject (ModelingProject1.modelproj) an");
            string projectPath = Console.ReadLine();
            Console.WriteLine("Geben Sie bitte den Dateinamen des Class Diagram an");
            string diagramFileName = Console.ReadLine();

            if(string.IsNullOrEmpty(projectPath))
            {
                projectPath = @"C:\Users\Michael Drexler\Desktop\FHWN\MIT13_CodeSharing\4th_sem\ass\drexler\src\ModelingProject1\ModelingProject1\ModelingProject1.modelproj";
            }

            if(string.IsNullOrEmpty(diagramFileName))
            {
                diagramFileName = @"UMLClassDiagram1.classdiagram";
            }

            try
            {
                Test(projectPath, diagramFileName);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            


            Console.ReadKey();
        }

        public static void Test(string projectPath, string diagramFileName)
        {
            using (IModelingProjectReader projectReader =
                       ModelingProject.LoadReadOnly(projectPath))
            {
                IModelStore store = projectReader.Store;
                
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("----------- ALL ICLASS INSTANCES -------");

                foreach (IClass umlClass in store.AllInstances<IClass>())
                {
                    Console.WriteLine("ClassName: " + umlClass.Name);
                }

                Console.WriteLine("----------------------------------------");
                Console.WriteLine("-- ALL ICLASS INSTANCES FROM DIAGRAM ---");

                foreach (string diagramFile in projectReader.DiagramFileNames)
                {
                    if(diagramFile.ToLower() == diagramFile.ToLower())
                    {
                        IDiagram diagram = projectReader.LoadDiagram(diagramFile);

                        foreach (IShape<IElement> shape in diagram.GetChildShapes<IElement>())
                        {
                            if(shape.Element is IClass)
                            {
                                Console.WriteLine(((IClass)shape.Element).Name);
                            }
                        }
                    }
                }
            }
        }
    }
}
