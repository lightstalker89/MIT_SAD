using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBoxConfigLoader
{
    public interface IConfigLoader
    {
        List<SrcFolder> SourceFolders { get; set; }

        T LoadConfig<T>(string path);
    }
}
