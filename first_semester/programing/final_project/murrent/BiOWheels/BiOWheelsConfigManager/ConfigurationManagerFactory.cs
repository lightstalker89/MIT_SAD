
namespace BiOWheelsConfigManager
{
    public class ConfigurationManagerFactory
    {
        public static IConfigurationManager CreateConfigurationManager()
        {
            return new ConfigurationManager();
        }
    }
}
