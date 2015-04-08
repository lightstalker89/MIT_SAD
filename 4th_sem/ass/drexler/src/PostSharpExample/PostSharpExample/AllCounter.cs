using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PostSharp;
using PostSharp.Extensibility;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using System.Diagnostics;
using System.Xml;

[assembly: PostSharpExample.AllCounter(AttributeTargetTypes= "PostSharpExample.Animal")]
//[assembly: MethodCallAspect(AttributeTargetMembers = "regex:^(?!.ctor|.cctor|Finalize).+")]
//[assembly: CtorCallAspect(AttributeTargetMembers = ".ctor")]
//[assembly: FinalizeCallAspect(AttributeTargetMembers = "Finalize")]

namespace PostSharpExample
{
    [Serializable]
    [AllCounter(AttributeExclude = true)]
    public class AllCounter : TypeLevelAspect
    {
        private int instanceCounter = 0;

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

            // Access/Create background thread to add a new filesystemwatcher for the log file 

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
                        xWriter.WriteStartElement("Animal");
                        xWriter.WriteElementString("InstanceCounter", this.instanceCounter.ToString());
                        xWriter.WriteEndElement();
                        xWriter.WriteEndElement();
                        xWriter.WriteEndDocument();
                    }
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(@"C:\Temp\AspectLog.xml");
                    XmlNode root = doc.DocumentElement;
                    XmlNode myNode = root.SelectSingleNode("descendant::InstanceCounter");
                    if (myNode != null && myNode.HasChildNodes)
                    {
                        myNode.FirstChild.Value = this.instanceCounter.ToString();
                    }
                    else
                    {
                        XmlNode animalNode = root.SelectSingleNode("descendant::Animal");
                        XmlElement element = doc.CreateElement("InstanceCounter");
                        element.InnerXml = this.instanceCounter.ToString();
                        //element.AppendChild();
                        animalNode.AppendChild(element);
                    }

                    doc.Save(@"C:\Temp\AspectLog.xml");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}