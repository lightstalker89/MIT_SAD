using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.Modeling.Diagrams;


namespace ClassLibrary1
{
    // Custom context menu command extension
    // See http://msdn.microsoft.com/en-us/library/ee329481(v=vs.120).aspx
    [Export(typeof(ICommandExtension))]
    [ClassDesignerExtension] // TODO: Add other diagram types if needed
    class UpdateModelCommand : ICommandExtension
    {
        private Thread updateThread;

        private static bool isAutomaticallyUpdateActivated = false;
        private static bool areUpdatesAvailable = false;
        private static Mutex mutex = new Mutex();

        public delegate void UpdateModelDelegate(XmlDocument xmlFile, IEnumerable<IComment> comments);

        //public ConcurrentQueue<T> UpdateQueue { get; set; }

        [Import]
        IDiagramContext context { get; set; }

        [Import]
        ILinkedUndoContext linkedUndoContext { get; set; }

        private void DisplayErrorMessage(Exception ex)
        {
            MessageBox.Show(ex.Message, "Ein Fehler ist aufgetreten!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DoWork(object obj)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(@"C:\Temp", "AspectLog.xml");
            watcher.EnableRaisingEvents = isAutomaticallyUpdateActivated;
            watcher.Created += watcher_Changed;
            watcher.Changed += watcher_Changed;
            DiagramView uiThreadHolder = context.CurrentDiagram.GetObject<Diagram>().ActiveDiagramView;

            while (isAutomaticallyUpdateActivated)
            {
                if(areUpdatesAvailable)
                {
                    XmlDocument logFile = this.ReadLogFile();
                    IEnumerable<IComment> comments = this.ReadCommentsFromUMLClassDiagram();
                    uiThreadHolder.Invoke(new UpdateModelDelegate(UpdateClassDiagram), logFile, comments);

                    mutex.WaitOne();
                    areUpdatesAvailable = false;
                    mutex.ReleaseMutex();
                }

                Thread.Sleep(5000);
            }
        }

        public static void UpdateClassDiagram(XmlDocument logFile, IEnumerable<IComment> comments)
        {
            foreach (var comment in comments)
            {
                XmlNode classNode = logFile.SelectSingleNode(string.Format("descendant::{0}", comment.Description));
                XmlNode instanceCounter = classNode.SelectSingleNode("descendant::InstanceCounter");
                XmlNode methodCounter = classNode.SelectSingleNode("descendant::MethodCallsCounter");
                comment.Body = comment.Description;
                if(instanceCounter != null)
                comment.Body += string.Concat(Environment.NewLine,"InstanceCounter: ", instanceCounter.InnerText, Environment.NewLine);

                if(methodCounter != null)
                comment.Body += string.Concat("MethodCounter: ", methodCounter.InnerText);
            }
        }

        private IEnumerable<IComment> ReadCommentsFromUMLClassDiagram()
        {
            try
            {
                IClassDiagram diagram = context.CurrentDiagram as IClassDiagram;
                IModelStore store = diagram.ModelStore;
                IPackage rootPackage = store.Root;
                IEnumerable<IComment> comments = store.AllInstances<IComment>();

                return comments;
            }
            catch(Exception ex)
            {
                this.DisplayErrorMessage(ex);
            }

            return null;
        }

        private XmlDocument ReadLogFile()
        {
            mutex.WaitOne();

            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Temp\AspectLog.xml");

            mutex.ReleaseMutex();

            return doc;
        }

        /// <summary>
        /// Update UML ClassDiagram if a log file was created or changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            mutex.WaitOne();
            areUpdatesAvailable = true;
            mutex.ReleaseMutex();
            // TODO Read Log File
            // TODO Get all IComments from ModelStore
            // TODO Write the right information to the right comments via UIThread
        }

        public void Execute(IMenuCommand command)
        {
            //DiagramView uiThreadHolder = context.CurrentDiagram.GetObject<Diagram>().ActiveDiagramView;
            //uiThreadHolder.Invoke(new UpdateModelDelegate(DoSomething), context);

            // Update the UML Class diagram with the informations from the Aspect
            try
            {
                if(updateThread == null)
                {
                    updateThread = new Thread(new ParameterizedThreadStart(DoWork));
                    updateThread.IsBackground = true;
                    updateThread.Name = "UpdateUMLClassDiagramThread";
                }

                if (!isAutomaticallyUpdateActivated)
                {
                    isAutomaticallyUpdateActivated = true;
                    MessageBox.Show("Update is activated!", "Info", MessageBoxButtons.OK);

                    if(!updateThread.IsAlive)
                    {
                        updateThread.Start();
                    }
                    else
                    {
                        updateThread.Resume();
                    }
                }
                else
                {
                    if (updateThread.IsAlive)
                    {
                        updateThread.Suspend();
                    }

                    isAutomaticallyUpdateActivated = false;
                    MessageBox.Show("Update is deactivated!", "Info", MessageBoxButtons.OK);
                }
            }
            catch(ThreadStartException ex)
            {
                this.DisplayErrorMessage(ex);
            }
            catch(ThreadAbortException ex)
            {
                this.DisplayErrorMessage(ex);
            }
            catch(ThreadInterruptedException ex)
            {
                this.DisplayErrorMessage(ex);
            }
            catch(ThreadStateException ex)
            {
                this.DisplayErrorMessage(ex);
            }
        }

        public void QueryStatus(IMenuCommand command)
        {
            command.Visible = command.Enabled = true;
            command.Text = isAutomaticallyUpdateActivated ? "Deactivate Update Model" : "Activate Update Model";
        }

        public string Text
        {
            get { return "Activate Update Model"; }
        }
    }
}
