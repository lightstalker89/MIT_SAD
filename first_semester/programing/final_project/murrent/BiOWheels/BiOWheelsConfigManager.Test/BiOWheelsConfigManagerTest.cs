using NUnit.Framework;

namespace BiOWheelsConfigManager.Test
{
    [TestFixture]
    public class BiOWheelsConfigManagerTest
    {
        private IConfigurationManager configurationManager;

        [SetUp]
        public void Init()
        {
            configurationManager = new ConfigurationManager();
        }

        [TestCase]
        public void TestLoadWithWrongValue()
        {

        }

        [TestCase]
        public void TestWrite()
        {
            
        }

        [TestCase]
        public void TestLoad()
        {
            
        }
    }
}
