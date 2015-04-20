//-----------------------------------------------------------------------
// <copyright file="InstanceCounterCommand.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace VSIXProject1
{
    using EnvDTE;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ComponentModel.Composition;
    using System.Windows.Forms;
    using System.IO;
    using ClassLibrary1;
    using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
    using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
    using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
    using Microsoft.VisualStudio.Uml.AuxiliaryConstructs;
    using Microsoft.VisualStudio.Uml.Classes;
    using Microsoft.VisualStudio.Uml.Profiles;
    using Microsoft.VisualStudio.TextTemplating.VSHost;
    using Microsoft.VisualStudio.TextTemplating;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Modeling.Diagrams.ExtensionEnablement;

    /// <summary>
    /// Custom context menu command extension 
    /// See http://msdn.microsoft.com/en-us/library/ee329481(v=vs.120).aspx
    /// </summary>
    [Export(typeof(ICommandExtension))]
    [ClassDesignerExtension]
    public class InstanceCounterCommand : ICommandExtension
    {
        /// <summary>
        /// The directory for all the generated aspects
        /// </summary>
        private static string generatedAspectFilePath = string.Empty;

        /// <summary>
        /// Flag to see whether the instance counter annotation is activated or not
        /// </summary>
        private Dictionary<string, bool> isInstanceCounterActivated = new Dictionary<string, bool>();

        /// <summary>
        /// Gets or sets the value of the diagram context
        /// </summary>
        [Import]
        IDiagramContext context { get; set; }

        /// <summary>
        /// Gets or sets the value of the Visual Studio Service Provider
        /// </summary>
        [Import]
        public SVsServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Gets or sets the value of the Visual Studio selection context
        /// </summary>
        [Import]
        public IVsSelectionContext sContext { get; set; }

        /// <summary>
        /// Gets or sets the value of the menu command text
        /// </summary>
        public string Text
        {
            get { return "Count Instances of class {not selected}"; }
        }

        /// <summary>
        /// Executes the instance counter functionality
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
                IClass selClass = selShape != null ? (IClass)selShape.GetElement() : null;
                
                #region OldCode
                //diagram.ModelStore.ProfileManager.AllProfiles
                //IProfile profile = diagram.ModelStore.ProfileManager.GetProfileByName("CSharpProfile");
                //IEnumerable<IStereotype> stereoTypes = profile != null ? profile.Stereotypes : null;
                //IStereotype stereoType = null;
                #endregion

                IEnumerable<IComment> comments = store.AllInstances<IComment>();

                if (!isInstanceCounterActivated.Where(m => m.Key == selClass.Name).Select(m => m.Value).SingleOrDefault())
                {
                    if (selClass != null)
                    {
                        var com = comments.Where(m => m.Description == selClass.Name).Select(m => m).FirstOrDefault();

                        if (com == null)
                        {
                            // Check if CountInstances already is activated
                            IComment comment = rootPackage.CreateComment();
                            comment.Description = (selClass.Name);
                            comment.AnnotatedElements.Add(selClass);
                            comment.Body = ((IClass)selShape.GetElement()).Name;
                            comment.Body += string.Format("{0}Instances:{1}, {2}MethodCalls:{3}, {4}Average Associations:{5}",
                                Environment.NewLine,
                                "0",
                                Environment.NewLine,
                                "0",
                                Environment.NewLine,
                                "0");

                            diagram.Display(comment);                 
                        }

                        var isCreated = GenerateAspectCodeFile(selClass);
                        command.Text = string.Format("Deactivate Count Instances of class '{0}'", selClass.Name);
                        isInstanceCounterActivated[selClass.Name] = true;
                    }
                    else
                    {
                        MessageBox.Show("No Shape selected!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    // Delete Aspect code file or notification?
                    string aspectCodeFileName = string.Concat("Aspect", selClass.Name, "CountInstances.cs");
                    if (!string.IsNullOrEmpty(generatedAspectFilePath))
                    {
                        string generatedAspectFileFullPath = Path.Combine(generatedAspectFilePath, aspectCodeFileName);
                        if(File.Exists(generatedAspectFileFullPath))
                        {
                            File.Delete(generatedAspectFileFullPath);
                        }
                        
                        MessageBox.Show("Instance Counter annotation removed!");
                    }

                    command.Text = string.Format("Activate Count Instances of class '{0}'", selClass.Name);
                    isInstanceCounterActivated[selClass.Name] = false;
                }
            }
            catch(Exception ex)
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
            IClass selClass = (selShape != null && (selShape.GetElement() is IClass)) ? (IClass)selShape.GetElement() : null;

            if(selClass != null)
            {
                // if dictionary doesn´t contain this annotation - add it
                if (!isInstanceCounterActivated.ContainsKey(selClass.Name))
                {
                    isInstanceCounterActivated.Add(selClass.Name, false);
                }

                command.Text = !isInstanceCounterActivated[selClass.Name] ? string.Format("Activate Count Instances of class '{0}'", selClass.Name) : string.Format("Deactivate Count Instances of class '{0}'", selClass.Name);
                command.Visible = true;
                command.Enabled = true;
            }
        }

        /// <summary>
        /// Transforms the text template file to the given output file
        /// </summary>
        /// <param name="selClassName"></param>
        /// <returns></returns>
        private bool GenerateAspectCodeFile(IClass selClass)
        {
            // Get a service provider – how you do this depends on the context:
            DTE dte = (DTE)ServiceProvider.GetService(typeof(DTE));
            IServiceProvider serviceProvider = sContext.CurrentStore as IServiceProvider;
            //IServiceProvider serviceProvider = (IServiceProvider)ServiceProvider.GetService(typeof(DTE));

            // Get the text template service:
            ITextTemplating t4 = serviceProvider.GetService(typeof(STextTemplating)) as ITextTemplating;
            ITextTemplatingSessionHost sessionHost = t4 as ITextTemplatingSessionHost;

            try
            {
                // Get cs project path to which the new aspect file should be generated
                string csProjectFilePath = CSProjectFromOpenDialog();
                string parentDir = Directory.GetParent(csProjectFilePath).FullName;
                generatedAspectFilePath = Path.Combine(parentDir, "GeneratedAspects");

                if (!Directory.Exists(generatedAspectFilePath))
                {
                    Directory.CreateDirectory(generatedAspectFilePath);
                }

                // Get Namespace from anywhere
                string nameSpace = selClass.Namespace.Name;

                // Create a Session in which to pass parameters:
                sessionHost.Session = sessionHost.CreateSession();
                sessionHost.Session["AttributeTargetTypes"] = string.Concat(nameSpace, ".", selClass.Name);
                sessionHost.Session["AspectClassName"] = string.Concat("Aspect", selClass.Name, "CountInstances");
                sessionHost.Session["ClassName"] = selClass.Name;

                //string filePath = string.Concat(Environment.CurrentDirectory, @"\AspectCountInstances.tt");
                string filePath = string.Concat(@"C:\Temp", @"\AspectCountInstances.tt");
                // Process a text template:

                string result = t4.ProcessTemplate("AspectCountInstances.tt", System.IO.File.ReadAllText(filePath));

                string aspectCodeFileName = string.Concat("Aspect", selClass.Name, "CountInstances.cs");
                string aspectCodeFullName = Path.Combine(generatedAspectFilePath, aspectCodeFileName);
                using (StreamWriter sw = new StreamWriter(aspectCodeFullName, false))
                {
                    sw.Write(result);
                    sw.Flush();
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
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
