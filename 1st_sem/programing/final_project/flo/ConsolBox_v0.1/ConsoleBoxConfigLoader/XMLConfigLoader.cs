using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace ConsoleBoxConfigLoader
{
    class XMLConfigLoader : IConfigLoader
    {
        private List<SrcFolder> sourceFolders = new List<SrcFolder>();
        public List<SrcFolder> SourceFolders
        {
            get { return sourceFolders; }
            set { sourceFolders = value; }
        }

        public T LoadConfig<T>(string path)
        {
            if (File.Exists(@path))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof (T));
                TextReader reader = new StreamReader(@path);
                object obj = deserializer.Deserialize(reader);
                T config = (T) obj;
                reader.Close();
                return config;
            }
            throw new ApplicationException("The type " + typeof(T).FullName + " is not registered in the container"); //Andere Lösung
            /*if (File.Exists(@path))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                TextReader reader = new StreamReader(@path);
                object obj = deserializer.Deserialize(reader);
                T config = (T)obj;
                reader.Close();
                return config;


                //Linq
                /*XDocument xdoc = XDocument.Load(path);
                var mapping = from map in xdoc.Descendants("Mapping")
                              from source in map.Elements("SourceFolders")
                              from destination in map.Elements("Destinations")
                              select new
                              {
                                  SourceFolders = source,
                                  DestinationFolders = destination
                              };

                foreach (var xElement in mapping)
                {
                    bool recursion;
                    List<string> sourceFolders = new List<string>();
                    List<string> destinationFolders = new List<string>();
                    List<string> folderExceptions = new List<string>();

                    foreach(var sources in xElement.SourceFolders.Elements())
                    {
                        foreach (var exceptions in ((XElement)sources).Descendants("ExceptionFolder"))
                        {
                            folderExceptions.Add(exceptions.Attribute("path").Value);
                        }
                        sourceFolders.Add(sources.Attribute("path").Value);
                        recursion = sources.Attribute("recursion").Value == "true" ? true : false;
                    }
                    foreach (var destinations in xElement.SourceFolders.Nodes())
                    {

                    }
                }
            }
            return new Object() as T;*/
        }
    }
}
