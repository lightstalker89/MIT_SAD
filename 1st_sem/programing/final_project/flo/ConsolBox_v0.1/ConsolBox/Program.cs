using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleBoxConfigLoader;
using ConsoleBoxFileWatcher;
using ConsoleBoxLogger;

namespace ConsolBox
{
    class Program
    {
        private static IDIContainer container = new DIContainer();
        public static IDIContainer Container
        {
            get { return container; }
            set { container = value; }
        }

        private static ConfigFile config;
        public static ConfigFile Config
        {
            get { return config; }
            set { config = value; }
        }
        
        static void Main(string[] args)
        {
            LoadConfig(args);
            StartUpApplication();

            IFileWatcher fileWatcher = Container.GetService<IFileWatcher>();
            fileWatcher.write();

            Console.ReadKey();
        }

        private static void LoadConfig(string[] args)
        {
            Container.Map<IConfigLoader, ConfigLoader>();
            Config = Container.GetService<IConfigLoader>().LoadConfig<ConfigFile>(args.Length > 0 ? args[0] : "ConfigFile.xml");
        }

        private static void StartUpApplication()
        {
            Container.Map<ILogger, Logger>(new Logger { FileSize = 2000 });
            Container.Map<IFileWatcher, FileWatcher>();
        }
    }
}
