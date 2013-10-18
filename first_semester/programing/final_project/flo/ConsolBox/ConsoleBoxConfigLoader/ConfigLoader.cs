using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBoxConfigLoader
{
    public class ConfigLoader : IConfigLoader
    {
        private readonly IConfigLoader  xmlConfigLogger = new XMLConfigLoader();

        public List<SrcFolder> SourceFolders
        {
            get { return xmlConfigLogger.SourceFolders; }
            set { xmlConfigLogger.SourceFolders = value; }
        }

        public T LoadConfig<T>(string path)
        {
            return this.xmlConfigLogger.LoadConfig<T>(path);
        }
    }
}
