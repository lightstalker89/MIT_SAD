namespace BiOWheelsConfigManager
{
    public interface IConfigurationManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configFileName"></param>
        /// <returns></returns>
        T Load<T>(string configFileName);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configFilename"></param>
        /// <param name="configurationObject"> </param>
        WriterStatus Write<T>(string configFilename, T configurationObject);
    }
}
