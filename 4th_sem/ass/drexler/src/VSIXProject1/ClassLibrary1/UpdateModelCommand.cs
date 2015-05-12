//-----------------------------------------------------------------------
// <copyright file="UpdateModelCommand.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace ClassLibrary1
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
    using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
    using Microsoft.VisualStudio.Modeling.Diagrams;
    using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
    using Microsoft.VisualStudio.Uml.Classes;

    /// <summary>
    /// Custom context menu command extension
    /// See http://msdn.microsoft.com/en-us/library/ee329481(v=vs.120).aspx
    /// </summary>
    [Export(typeof(ICommandExtension))]
    [ClassDesignerExtension]
    public class UpdateModelCommand : ICommandExtension
    {
        /// <summary>
        /// Flag whether the background thread should update the GUI or not
        /// </summary>
        private static bool isAutomaticallyUpdateActivated = false;

        /// <summary>
        /// Flag to see if the log file has changed and the GUI has to be updated
        /// </summary>
        private static bool areUpdatesAvailable = false;

        /// <summary>
        /// Handles privileged access to ressources for threads
        /// </summary>
        private static Mutex mutex = new Mutex();

        /// <summary>
        /// Background thread which updates the GUI (class diagram)
        /// </summary>
        private Thread updateThread;

        /// <summary>
        /// Delegate for the UIThreadHolder of the class diagram
        /// </summary>
        /// <param name="xmlFile">Log file created by the aspect(s)</param>
        /// <param name="comments">Comment Shapes from the UML class diagram</param>
        public delegate void UpdateModelDelegate(XElement xmlFile, IEnumerable<IComment> comments);

        /// <summary>
        /// Gets or sets the diagram context
        /// </summary>
        [Import]
        public IDiagramContext context { get; set; }

        /// <summary>
        /// Gets or sets the value for the linked undo context
        /// </summary>
        [Import]
        public ILinkedUndoContext linkedUndoContext { get; set; }

        /// <summary>
        /// Gets the value of the menu command text
        /// </summary>
        public string Text
        {
            get { return "Activate Update Model"; }
        }

        /// <summary>
        /// Update all comment shapes with the logged informations for the annotated classes.
        /// </summary>
        /// <param name="logFile">Log file created by the aspect(s)</param>
        /// <param name="comments">Comment shapes from the UML class diagram</param>
        public void UpdateClassDiagram(XElement logFile, IEnumerable<IComment> comments)
        {
            foreach (var comment in comments)
            {
                int constructorCounts = 0;
                int destructorCounts = 0;
                int instanceCount = 0;
                Dictionary<string, int> methodCallCounts = new Dictionary<string, int>();

                // get the data from the log file for a specific class and annotation
                var data = this.QueryDataFromLogFile(logFile, "Constructor", comment.Description);
                var dataDestructor = this.QueryDataFromLogFile(logFile, "Destructor", comment.Description);
                var dataMethods = this.QueryDataFromLogFile(logFile, "Method", comment.Description);

                // count the log entries
                constructorCounts = data != null ? data.ToList().Count : 0;
                destructorCounts = data != null ? dataDestructor.ToList().Count : 0;
                instanceCount = constructorCounts - destructorCounts;
                methodCallCounts = this.GroupMethodCalls(dataMethods);

                // update comment shape within the model with the measured values
                comment.Body = comment.Description;
                comment.Body += string.Concat(Environment.NewLine, "InstanceCounter: ", instanceCount.ToString(), Environment.NewLine);
                comment.Body += string.Concat("Methods: ", Environment.NewLine);

                foreach (var item in methodCallCounts)
                {
                    comment.Body += string.Concat(item.Key, ": ", item.Value.ToString(), Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// Update UML ClassDiagram if a log file was created or changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            // Better to add jobs to a queue which will be processed from a thread?
            mutex.WaitOne();
            areUpdatesAvailable = true;
            mutex.ReleaseMutex();
        }

        /// <summary>
        /// Executes the update class diagram logic
        /// </summary>
        /// <param name="command">Current menu diagram</param>
        public void Execute(IMenuCommand command)
        {
            // Start a background thread to update the GUI (class diagram)
            try
            {
                if (this.updateThread == null)
                {
                    this.updateThread = new Thread(new ParameterizedThreadStart(this.DoWork));
                    this.updateThread.IsBackground = true;
                    this.updateThread.Name = "UpdateUMLClassDiagramThread";
                }

                if (!isAutomaticallyUpdateActivated)
                {
                    isAutomaticallyUpdateActivated = true;
                    MessageBox.Show("Update is activated!", "Info", MessageBoxButtons.OK);

                    if (!this.updateThread.IsAlive)
                    {
                        this.updateThread.Start();
                    }
                    else
                    {
                        this.updateThread.Resume();
                    }
                }
                else
                {
                    if (this.updateThread.IsAlive)
                    {
                        this.updateThread.Suspend();
                    }

                    isAutomaticallyUpdateActivated = false;
                    MessageBox.Show("Update is deactivated!", "Info", MessageBoxButtons.OK);
                }
            }
            catch (ThreadStartException ex)
            {
                this.DisplayErrorMessage(ex);
            }
            catch (ThreadAbortException ex)
            {
                this.DisplayErrorMessage(ex);
            }
            catch (ThreadInterruptedException ex)
            {
                this.DisplayErrorMessage(ex);
            }
            catch (ThreadStateException ex)
            {
                this.DisplayErrorMessage(ex);
            }
        }

        /// <summary>
        /// Handles the visibility of the MenuCommand item
        /// </summary>
        /// <param name="command">Current menu command</param>
        public void QueryStatus(IMenuCommand command)
        {
            command.Visible = command.Enabled = true;
            command.Text = isAutomaticallyUpdateActivated ? "Deactivate Update Model" : "Activate Update Model";
        }

        /// <summary>
        /// Query data for the given type and class name from the log file
        /// </summary>
        /// <param name="logFile">XML representation of the log file</param>
        /// <param name="type">Constructor, Destructor or Method</param>
        /// <param name="className">Class name of the object or method</param>
        /// <returns>All found entries for the given type and class name</returns>
        private IEnumerable<XElement> QueryDataFromLogFile(XElement logFile, string type, string className)
        {
            var data = from item in logFile.Descendants("LogEntry")
                       where (item.Attribute("ClassName").Value == className &&
                              item.Attribute("Type").Value == type)
                       select item;

            return data;
        }

        /// <summary>
        /// Group all method call log entries and count the method calls
        /// </summary>
        /// <param name="methodCallLogEntries">All log entries for method calls</param>
        /// <returns>All method names of a class with the count of method calls</returns>
        private Dictionary<string, int> GroupMethodCalls(IEnumerable<XElement> methodCallLogEntries)
        {
            Dictionary<string, int> methodCallCounts = new Dictionary<string, int>();

            if (methodCallLogEntries != null && methodCallLogEntries.Count() > 0)
            {
                foreach (var item in methodCallLogEntries)
                {
                    string methodName = item.Attribute("TypeName").Value;

                    if (!methodCallCounts.Keys.Contains(methodName))
                    {
                        methodCallCounts.Add(methodName, 1);
                    }
                    else
                    {
                        methodCallCounts[methodName] = ++methodCallCounts[methodName];
                    }
                }
            }

            return methodCallCounts;
        }

        /// <summary>
        /// Get all comment shapes from the model store
        /// </summary>
        /// <returns>All comment shapes from the model store</returns>
        private IEnumerable<IComment> ReadCommentsFromUMLClassDiagram()
        {
            try
            {
                IClassDiagram diagram = this.context.CurrentDiagram as IClassDiagram;
                IModelStore store = diagram.ModelStore;
                IPackage rootPackage = store.Root;
                IEnumerable<IComment> comments = store.AllInstances<IComment>();

                return comments;
            }
            catch (Exception ex)
            {
                this.DisplayErrorMessage(ex);
            }

            return null;
        }

        /// <summary>
        /// Read the log file
        /// </summary>
        /// <returns>XML representation of the log file</returns>
        private XElement ReadLogFile()
        {
            mutex.WaitOne();
            XElement doc = XElement.Load(@"C:\Temp\AspectLog.xml");
            mutex.ReleaseMutex();

            return doc;
        }

        /// <summary>
        /// Method to show error messages to the user
        /// </summary>
        /// <param name="ex">Exception message thrown</param>
        private void DisplayErrorMessage(Exception ex)
        {
            MessageBox.Show(ex.Message, "Ein Fehler ist aufgetreten!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Thread functionality to process
        /// </summary>
        /// <param name="obj">Passed object to the thread method</param>
        private void DoWork(object obj)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(@"C:\Temp", "AspectLog.xml");
            watcher.EnableRaisingEvents = isAutomaticallyUpdateActivated;
            watcher.Created += this.watcher_Changed;
            watcher.Changed += this.watcher_Changed;
            DiagramView uiThreadHolder = this.context.CurrentDiagram.GetObject<Diagram>().ActiveDiagramView;

            while (isAutomaticallyUpdateActivated)
            {
                if (areUpdatesAvailable)
                {
                    try
                    {
                        XElement logFile = this.ReadLogFile();
                        IEnumerable<IComment> comments = this.ReadCommentsFromUMLClassDiagram();
                        uiThreadHolder.Invoke(new UpdateModelDelegate(this.UpdateClassDiagram), logFile, comments);
                    }
                    catch(Exception ex)
                    {
                        this.DisplayErrorMessage(ex);
                    }
                    finally
                    {
                        mutex.WaitOne();
                        areUpdatesAvailable = false;
                        mutex.ReleaseMutex();
                    }
                }

                Thread.Sleep(5000);
            }
        }
    }
}
