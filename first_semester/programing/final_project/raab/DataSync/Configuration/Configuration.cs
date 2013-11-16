using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataSync
{
    public class Configuration
    {
        public List<Source> SourceList { get; set; }
        private string XmlConfigPath { get; set; }
        private string XsdConfigPath { get; set; }

        public Configuration(string xmlConfigPath, string xsdConfigPath)
        {
            this.XsdConfigPath = xsdConfigPath;
            this.XmlConfigPath = xmlConfigPath;
            this.SourceList = GetSourceListXml();
        }

        public List<Source> GetSourceListXml()
        {

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.ValidationType = ValidationType.Schema;
            readerSettings.Schemas.Add(null, this.XsdConfigPath);
            readerSettings.ValidationEventHandler += ValidationCallback;

            XmlReader reader = XmlReader.Create(this.XmlConfigPath, readerSettings);

            List<Source> sourceList = new List<Source>();
            Source source = null;
            List<string> destinationPathList = null;
            List<string> excludedPathList = null;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "Sources":

                            sourceList = new List<Source>();

                            break;

                        case "Source":
                            
                            if (reader.HasAttributes)
                            {
                                string path = null;
                                bool includeSubDirectories = true;                               

                                while (reader.MoveToNextAttribute())
                                {


                                    if (reader.Name == "Path")
                                    {
                                        path = reader.Value;
                                    }
                                    else if (reader.Name == "IncludeSubdirectories")
                                    {
                                        bool.TryParse(reader.Value, out includeSubDirectories);
                                    }
                                }

                                source = new Source(
                                    (path != null) ? path : null // If path is given pass it as parameter, else pass null
                                    , null, null,
                                    includeSubDirectories
                                        );
                            }
                            sourceList.Add(source);
                            break;

                        case "Destinations":

                            destinationPathList = new List<string>();
                            source.DestinationPaths = destinationPathList;

                            break;

                        case "ExcludedSources":

                            excludedPathList = new List<string>();
                            source.ExcludedDestinationPaths = excludedPathList;

                            break;

                        case "Destination":

                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "Path")
                                    {
                                        source.DestinationPaths.Add(reader.Value);
                                    }
                                }
                            }

                            break;

                        case "ExcludedSource":

                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "Path")
                                    {
                                        source.ExcludedDestinationPaths.Add(reader.Value);
                                    }
                                }
                            }

                            break;
                        default:
                            break;
                    }

                }
            }

            reader.Close();

            return sourceList;
        }

        private void ValidationCallback(object sender, System.Xml.Schema.ValidationEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
