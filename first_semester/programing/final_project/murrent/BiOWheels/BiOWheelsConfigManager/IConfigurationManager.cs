namespace BiOWheelsConfigManager
{
    public interface IConfigurationManager
    {
        Configuration Configuration { get; }

        event ConfigurationManager.ConfigurationLoadingFailedHandler ConfigurationLoadingFailed;

        void Load();
    }
}
