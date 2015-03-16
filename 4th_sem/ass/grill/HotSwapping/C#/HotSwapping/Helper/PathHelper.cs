using System;
using System.IO;
using System.Reflection;

namespace HotSwapping.Helper
{
    public class PathHelper
    {
        public static string getAssemblyPath()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
