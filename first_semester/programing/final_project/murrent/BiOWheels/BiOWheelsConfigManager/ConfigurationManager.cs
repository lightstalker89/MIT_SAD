namespace BiOWheelsConfigManager
{
    using System;
    using System.IO;

    public class ConfigurationManager : IConfigurationManager
    {
        #region Private Fields
        private const string FileLocation = "";
        #endregion

        #region Properties
        public Configuration Configuration { get; internal set; }
        #endregion

        #region Events
        public delegate void ConfigurationLoadingFailedHandler(object sender, EventArgs data);
        public event ConfigurationLoadingFailedHandler ConfigurationLoadingFailed;

        protected virtual void OnConfigurationLoadingFailed(object sender, EventArgs data)
        {
            if (ConfigurationLoadingFailed != null)
            {
                ConfigurationLoadingFailed(this, data);
            }
        }
        #endregion

        public ConfigurationManager()
        {
            this.Load();
        }

        /// <summary>
        /// Loads the configuration
        /// </summary>
        protected void Load()
        {
            this.Configuration = new Configuration();

            if (File.Exists(FileLocation))
            {

            }
            else
            {
                this.OnConfigurationLoadingFailed(this, new EventArgs());
            }
        }
    }
}
