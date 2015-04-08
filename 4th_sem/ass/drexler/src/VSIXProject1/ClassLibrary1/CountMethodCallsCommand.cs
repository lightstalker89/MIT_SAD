using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using ClassLibrary1;
using EnvDTE;
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
using System.IO;

namespace VSIXProject1
{
    // Custom context menu command extension
    // See http://msdn.microsoft.com/en-us/library/ee329481(v=vs.120).aspx
    [Export(typeof(ICommandExtension))]
    [ClassDesignerExtension] // TODO: Add other diagram types if needed
    public class CountMethodCallsCommand : ICommandExtension
    {
        [Import]
        IDiagramContext context { get; set; }

        [Import]
        public SVsServiceProvider ServiceProvider { get; set; }

        [Import]
        public IVsSelectionContext sContext { get; set; }

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

                if(selClass != null)
                {
                    #region OldCode
                    //Microsoft.VisualStudio.Uml.Profiles.IProperty prop;

                    //if (stereoTypes != null && stereoTypes.Count() > 0)
                    //{
                    //    stereoType = stereoTypes.Where(m => m.Name == "class").Select(m => m).FirstOrDefault();
                    //    prop = stereoType.Properties
                    //        .Where(m => m.Name.ToLower() == "clrattributes")
                    //        .Select(m => m).FirstOrDefault();
                    //} 
                    #endregion

                    var com = comments.Where(m => m.Description == (selClass.GetId().ToString())).Select(m => m).FirstOrDefault();

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


                        #region OldCode
                        //var implementedAndInherited = Enumerable.Union<IType>(aspectClass.SuperClasses, aspectClass.InterfaceRealizations.Select(ir => ir.Contract));
                        //var types = implementedAndInherited.Where(type => type != null);

                        //foreach (IType type in types)
                        //{
                        //    IClassifier baseClassifier = type as IClassifier;
                        //    if (baseClassifier != null)
                        //    {
                        //        ITemplateBinding templateBinding = GetTemplateBinding(baseClassifier);
                        //        IClassifier bindingClassifier = GetBindingClassifier(templateBinding);
                        //        if (bindingClassifier != null)
                        //        {
                        //            baseClassifier = bindingClassifier;
                        //        }

                        //        foreach (IOperation operationInBase in GetOwnedOperations(baseClassifier))
                        //        {
                        //            //bool isInheritedMember = IsInheritedMember(operationInBase, operation, templateBinding);
                        //            //if (isInheritedMember)
                        //            //{
                        //            //    return true;
                        //            //}
                        //        }
                        //    }
                        //}

                        //aspectClass.TemplateBindings.ToList().Add();

                        //foreach (var item in stereoType.Properties)
                        //{
                        //    comment.Body += "\n";
                        //    comment.Body += string.Format("PropName:{0}, DefaultValue:{1}; \n", item.Name, item.DefaultValue);
                        //}
                        #endregion

                        diagram.Display(comment);
                        command.Text = string.Format("Deactivate Count Method Calls of class '{0}'", selClass.Name);
                    }
                    else
                    {
                        //com.Delete();
                        //command.Text = string.Format("Activate Count Method Calls of class '{0}'", selClass.Name);
                    }

                    var isCreated = GenerateAspectCodeFile(selClass);
                }
                else
                {
                    MessageBox.Show("No Shape selected!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Initialize the template with the Model Store.
                VdmGen generator = new VdmGen(context.CurrentDiagram.ModelStore);
                // Generate the text and write it.
                /*System.IO.File.WriteAllText(
                    System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"Generated.txt")
                   , generator.TransformText());*/
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
                command.Text = string.Format("Activate Count Method Calls of class '{0}'", selClass.Name);
                command.Visible = true;
                command.Enabled = true;
            }
        }

        public string Text
        {
            get { return "Count Method Calls of class {not selected}"; }
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
                string pathGeneratedAspects = Path.Combine(parentDir, "GeneratedAspects");

                if (!Directory.Exists(pathGeneratedAspects))
                {
                    Directory.CreateDirectory(pathGeneratedAspects);
                }

                // Get Namespace from anywhere
                string nameSpace = selClass.Namespace.Name;

                // Create a Session in which to pass parameters:
                sessionHost.Session = sessionHost.CreateSession();
                sessionHost.Session["AttributeTargetTypes"] = string.Concat(nameSpace, ".", selClass.Name);
                sessionHost.Session["AspectClassName"] = string.Concat("Aspect", selClass.Name, "MethodCalls");
                sessionHost.Session["ClassName"] = selClass.Name;

                string filePath = string.Concat(Environment.CurrentDirectory, @"\AspectCountMethodCalls.tt");
                // Process a text template:

                string result = t4.ProcessTemplate("AspectCountMethodCalls.tt", System.IO.File.ReadAllText(filePath));

                string aspectCodeFileName = string.Concat("Aspect", selClass.Name, "MethodCalls.cs");
                string aspectCodeFullName = Path.Combine(pathGeneratedAspects, aspectCodeFileName);
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
