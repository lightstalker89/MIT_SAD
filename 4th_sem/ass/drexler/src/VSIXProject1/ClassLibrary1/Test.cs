using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Modeling.Diagrams.ExtensionEnablement;

namespace ClassLibrary1
{
    // Custom context menu command extension
    // See http://msdn.microsoft.com/en-us/library/ee329481(v=vs.120).aspx
    [Export(typeof(ICommandExtension))]
    [ClassDesignerExtension] // TODO: Add other diagram types if needed
    public class Test : ICommandExtension
    {
        [Import]
        IDiagramContext context { get; set; }

        [Import]
        public SVsServiceProvider ServiceProvider { get; set; }

        [Import]
        public IVsSelectionContext sContext { get; set; }
        

        public void Execute(IMenuCommand command)
        {
            // Get a service provider – how you do this depends on the context:
            DTE dte = (DTE)ServiceProvider.GetService(typeof(DTE));
            IServiceProvider serviceProvider = sContext.CurrentStore as IServiceProvider;
            //IServiceProvider serviceProvider = (IServiceProvider)ServiceProvider.GetService(typeof(DTE));

            // Get the text template service:
            ITextTemplating t4 = serviceProvider.GetService(typeof(STextTemplating)) as ITextTemplating;
            ITextTemplatingSessionHost sessionHost = t4 as ITextTemplatingSessionHost;

            // Create a Session in which to pass parameters:
            sessionHost.Session = sessionHost.CreateSession();
            sessionHost.Session["AttributeTargetTypes"] = "PostSharpExample.Animal";
            sessionHost.Session["MulticastPointcutTargets"] = "MulticastTargets.InstanceConstructor | MulticastTargets.Method";
            sessionHost.Session["AspectClassName"] = "HelloWorld";

            string filePath = string.Concat(Environment.CurrentDirectory, @"\AspectCountInstances.tt");
            // Process a text template:
            string result = t4.ProcessTemplate("AspectCountInstances.tt", System.IO.File.ReadAllText(filePath));            

        }

        public void QueryStatus(IMenuCommand command)
        {
            command.Visible = command.Enabled = false;
        }

        public string Text
        {
            get { return "Test"; }
        }
    }
}
