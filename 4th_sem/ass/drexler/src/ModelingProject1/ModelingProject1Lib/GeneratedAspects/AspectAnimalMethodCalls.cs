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


[assembly: ClassDiagram.AspectAnimalMethodCalls(AttributeTargetTypes= "ClassDiagram.Animal")]

namespace ClassDiagram
{
	[Serializable]
	[AspectAnimalMethodCalls(AttributeExclude = true)]
	public class AspectAnimalMethodCalls : TypeLevelAspect
	{
		private int methodCounter = 0;
		private Type type;

        /// <summary>
        /// Handles privileged access to ressources for threads
        /// </summary>
        private static Mutex mutex = new Mutex();

		[OnMethodEntryAdvice, MulticastPointcut(Targets = MulticastTargets.Method, MemberName="regex:^(?!.ctor|.cctor|Finalize).+")]
		public void OnEntry(MethodExecutionArgs args)
		{          
			if(!args.Method.IsConstructor)
			{
				++this.methodCounter;
				type = Type.Method;
			}

			this.LogToXML(@"C:\Temp", "AspectLog.xml", type.ToString(), args.Method.Name, args.Instance.ToString());
		}

        /// <summary>
        /// Create a new xml file if not exist and log the values. If the file already exist
        /// update the values
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="typeName"></param>
        private void LogToXML(string directory, string fileName, string type, string methodName, string typeName)
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
                        xWriter.WriteStartElement("AspectLogs");
                        xWriter.WriteStartElement("LogEntry");
                        xWriter.WriteAttributeString("TypeName", methodName);
                        xWriter.WriteAttributeString("ClassName", typeName);
                        xWriter.WriteAttributeString("Type", type);
                        xWriter.WriteAttributeString("Datetime", DateTime.Now.ToString("r"));
                        xWriter.WriteEndElement();
                        xWriter.WriteEndElement();
                        xWriter.WriteEndDocument();
                        xWriter.Flush();
                        xWriter.Close();
                    }
                }
                else
                {
                    mutex.WaitOne();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(filePath);
                    XmlNode rootNode = xmlDoc.SelectSingleNode("AspectLogs");
                    rootNode = (rootNode == null) ? xmlDoc.CreateElement("AspectLogs") : rootNode;

                    XmlElement newElement = xmlDoc.CreateElement("LogEntry");
                    newElement.SetAttribute("TypeName", methodName);
                    newElement.SetAttribute("ClassName", typeName);
                    newElement.SetAttribute("Type", type);
                    newElement.SetAttribute("Datetime", DateTime.Now.ToString("r"));
                    rootNode.AppendChild(newElement);
    
                    xmlDoc.Save(filePath);
                    mutex.ReleaseMutex();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

		/// <summary>
		/// Call type which is intercepted
		/// </summary>
		public enum Type
		{
			Constructor = 1,
			Destructor = 2,
			Method = 3
		}
	}
}