//-----------------------------------------------------------------------
// <copyright file="ModelReader.cs" company="MD Development">
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
    using Microsoft.VisualStudio.ArchitectureTools.Extensibility;
    using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
    using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
    using Microsoft.VisualStudio.Uml.Classes;

    public class ModelReader
    {

        public void ReadClassDiagram(string projectPath, string digramName)
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
                    if (diagramFile.ToLower() == diagramFile.ToLower())
                    {
                        IDiagram diagram = projectReader.LoadDiagram(diagramFile);
                        foreach (IShape<IElement> shape in diagram.GetChildShapes<IElement>())
                        {
                            if (shape.Element is IClass)
                            {
                                IClass classShape = ((IClass)shape.Element);
                                Console.WriteLine(classShape.Name);
                            }
                        }
                    }
                }
            }
        }
    }
}
