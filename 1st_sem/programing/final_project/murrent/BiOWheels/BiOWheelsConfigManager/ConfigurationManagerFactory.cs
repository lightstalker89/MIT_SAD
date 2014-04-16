// *******************************************************
// * <copyright file="ConfigurationManagerFactory.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsConfigManager
{
    /// <summary>
    /// Class representing the <see cref="ConfigurationManagerFactory"/>
    /// </summary>
    public class ConfigurationManagerFactory
    {
        /// <summary>
        /// Creates the configuration manager.
        /// </summary>
        /// <returns>An instance of the <see cref="ConfigurationManager"/> class</returns>
        public static IConfigurationManager CreateConfigurationManager()
        {
            return new ConfigurationManager();
        }
    }
}