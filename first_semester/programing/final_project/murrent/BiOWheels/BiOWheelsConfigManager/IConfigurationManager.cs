namespace BiOWheelsConfigManager
{
    public interface IConfigurationManager
    {
        /// <summary>
        /// Load the configuration from a given file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configFileName"></param>
        /// <returns></returns>
        T Load<T>(string configFileName);

        /// <summary>
        /// Write the configuration to the given file
        /// </summary>
        /// <typeparam name="T">Type of the configuration</typeparam>
        /// <param name="configFilename">Name of the configurationfile</param>
        /// <param name="configurationObject">Object of the configuration</param>
        WriterStatus Write<T>(string configFilename, T configurationObject);
    }
}
