using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using PostSharp;
using PostSharp.Extensibility;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;


[assembly: ClassDiagram.AspectPersonCountInstances(AttributeTargetTypes= "ClassDiagram.Person")]

namespace ClassDiagram
{
    [Serializable]
    [AspectPersonCountInstances(AttributeExclude = true)]
    public class AspectPersonCountInstances : TypeLevelAspect
    {
        private int instanceCounter = 0;

        /// <summary>
        /// Handles privileged access to ressources for threads
        /// </summary>
        private static Mutex mutex = new Mutex();

		[OnMethodEntryAdvice, MulticastPointcut(MemberName="regex:.ctor|.cctor|Finalize")]
        public void OnEntry(MethodExecutionArgs args)
        {
           
            if(args.Method.IsConstructor)
            {
                ++this.instanceCounter;
            }
            else
            {
                if(args.Method.Name.ToLower() == "finalize")
                {
                    --this.instanceCounter;
                }
            }

			this.LogToXML(@"C:\Temp", "AspectLog.xml");
        }

        /// <summary>
        /// Create a new xml file if not exist and log the values. If the file already exist
        /// update the values
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="fileName"></param>
        private void LogToXML(string directory, string fileName)
        {
            try
            {
                // Log information to log file
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string filePath = Path.Combine(directory, fileName);

                if (!File.Exists(filePath))
                {
                    using (XmlWriter xWriter = XmlWriter.Create(filePath))
                    {
                        xWriter.WriteStartDocument();
                        xWriter.WriteStartElement("Objects");
                        xWriter.WriteStartElement("Person");
                        xWriter.WriteElementString("InstanceCounter", this.instanceCounter.ToString());
                        xWriter.WriteEndElement();
                        xWriter.WriteEndElement();
                        xWriter.WriteEndDocument();
                    }
                }
                else
                {
					mutex.WaitOne();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(@"C:\Temp\AspectLog.xml");
                    XmlNode root = doc.DocumentElement;
					XmlNode classNode = root.SelectSingleNode("descendant::Person");

					if(classNode == null)
					{
						XmlElement newObject = doc.CreateElement("Person");
						root.AppendChild(newObject);
						classNode = root.SelectSingleNode("descendant::Person");
					}

                    XmlNode myNode = classNode.SelectSingleNode("descendant::InstanceCounter");
                    if (myNode != null && myNode.HasChildNodes)
                    {
                        myNode.FirstChild.Value = this.instanceCounter.ToString();
                    }
                    else
                    {
                        XmlElement element = doc.CreateElement("InstanceCounter");
                        element.InnerXml = this.instanceCounter.ToString();
                        classNode.AppendChild(element);
                    }

                    doc.Save(filePath);
					mutex.ReleaseMutex();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}