namespace BiOWheelsConfigManager
{
    using System;
    using System.IO;

    public class ConfigurationManager : IConfigurationManager
    {
        #region Private Fields
        private const string FileLocation = "";
        private readonly object objectLock = new Object();
        #endregion

        #region Properties
        public Configuration Configuration { get; internal set; }
        #endregion

        #region Events
        public delegate void ConfigurationLoadingFailedHandler(object sender, EventArgs data);
        public event ConfigurationLoadingFailedHandler ConfigurationLoadingFailed;

        protected void OnConfigurationLoadingFailed(object sender, EventArgs data)
        {
            if (ConfigurationLoadingFailed != null)
            {
                ConfigurationLoadingFailed(this, data);
            }
        }
        #endregion

        public ConfigurationManager() { }

        /// <summary>
        /// Loads the configuration
        /// </summary>
        public void Load()
        {
            this.Configuration = new Configuration();

            if (File.Exists(FileLocation))
            {

            }
            else
            {
                OnConfigurationLoadingFailed(this, new EventArgs());
            }
        }
    }
}
