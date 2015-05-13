//-----------------------------------------------------------------------
// <copyright file="AverageAssociationCount.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
    using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
    using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
    using Microsoft.VisualStudio.Uml.Classes;
    using System.Windows.Forms;
    using System.IO;
    using Microsoft.VisualStudio.TextTemplating.VSHost;
    using Microsoft.VisualStudio.TextTemplating;
    using EnvDTE;

    // Custom context menu command extension
    // See http://msdn.microsoft.com/en-us/library/ee329481(v=vs.120).aspx
    [Export(typeof(ICommandExtension))]
    [ClassDesignerExtension]
    public class AverageAssociationCount : ICommandExtension
    {
        /// <summary>
        /// File path to save the generated aspects
        /// </summary>
        private static string generatedAspectFilePath = string.Empty;

        /// <summary>
        /// Keep in mind which UML Class are already annotated to count method calls
        /// </summary>
        private Dictionary<string, bool> isAverageAssociationCountActivated = new Dictionary<string, bool>();

        /// <summary>
        /// Gets or sets the diagram context
        /// </summary>
        [Import]
        IDiagramContext context { get; set; }

        /// <summary>
        /// Executes the average association count functionality
        /// </summary>
        /// <param name="command"></param>        
        public void Execute(IMenuCommand command)
        {
            try
            {
                IClassDiagram diagram = context.CurrentDiagram as IClassDiagram;
                IModelStore store = diagram.ModelStore;
                IPackage rootPackage = store.Root;
                IShape selShape = diagram.SelectedShapes.FirstOrDefault();
                IAssociation selAssociation = selShape != null ? (IAssociation)selShape.GetElement() : null;
                string fullNamespaceNameWithType = string.Empty;
                string associationName = string.Empty;

                if(!string.IsNullOrEmpty(selAssociation.Name))
                {
                    associationName = selAssociation.Name;
                }
                else
                {
                    associationName = string.Concat(
                        ((IClass)selAssociation.SourceElement).Name, 
                        ((IClass)selAssociation.TargetElement).Name);
                    selAssociation.Name = associationName;
                }
                
                fullNamespaceNameWithType = string.Concat(selAssociation.Namespace.Name, '.', associationName);
                IEnumerable<IComment> comments = store.AllInstances<IComment>();

                if (!isAverageAssociationCountActivated.Where(m => m.Key == associationName).Select(m => m.Value).SingleOrDefault())
                {
                    if (selAssociation != null)
                    {
                        var com = comments.Where(m => m.Description == fullNamespaceNameWithType).Select(m => m).FirstOrDefault();

                        // Check if Average Association Count already is activated
                        if (com == null)
                        {
                            IComment comment = rootPackage.CreateComment();
                            comment.Description = fullNamespaceNameWithType;
                            //comment.AnnotatedElements.Add();
                            comment.Body = associationName;
                            comment.Body += string.Format("{0}Average Association Count:{1}",
                                Environment.NewLine,
                                "0");

                            diagram.Display(comment);
                            command.Text = string.Format("Deactivate Average Association Count '{0}'", associationName);
                        }

                        // var isCreated = GenerateAspectCodeFile(selAssociation);
                        isAverageAssociationCountActivated[associationName] = true;
                    }
                    else
                    {
                        MessageBox.Show("No Shape selected!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    // Delete Aspect code file or notification?
                    //string aspectCodeFileName = string.Concat("Aspect", selAssociation.Name, "AssociationCount.cs");
                    //if (!string.IsNullOrEmpty(generatedAspectFilePath))
                    //{
                    //    string generatedAspectFileFullPath = Path.Combine(generatedAspectFilePath, aspectCodeFileName);
                    //    File.Delete(generatedAspectFileFullPath);
                    //    MessageBox.Show("Count Method Calls annotation removed!");
                    //}

                    isAverageAssociationCountActivated[associationName] = false;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Query the selected shape on the diagramm and show the command.
        /// </summary>
        /// <param name="command"></param>
        public void QueryStatus(IMenuCommand command)
        {
            command.Visible = command.Enabled = false;
            IShape selShape = context.CurrentDiagram.SelectedShapes.FirstOrDefault();
            IAssociation selAssociation = (selShape != null && (selShape.GetElement() is IAssociation)) ? (IAssociation)selShape.GetElement() : null;

            if (selAssociation != null)
            {
                // if dictionary doesn´t contain this annotation - add it
                if (!isAverageAssociationCountActivated.ContainsKey(selAssociation.Name))
                {
                    isAverageAssociationCountActivated.Add(selAssociation.Name, false);
                }

                command.Visible = true;
                command.Enabled = true;
                command.Text = !isAverageAssociationCountActivated[selAssociation.Name] ? string.Format("Activate Average Association Count for '{0}'", selAssociation.Name) : string.Format("Deactivate Average Association Count '{0}'", selAssociation.Name);
            }
        }

        public string Text
        {
            get { return "Activate Association Count"; }
        }

        /// <summary>
        /// Pass the C# Project file to which the aspect.cs should be generated
        /// </summary>
        /// <returns></returns>
        private string CSProjectFromOpenDialog()
        {
            var openFileDialog = new OpenFileDialog() { Filter = "C# Project (*.csproj)|*.csproj" };
            DialogResult dialogValid = openFileDialog.ShowDialog();
            return dialogValid != DialogResult.OK ? null : openFileDialog.FileName;
        }
    }
}
