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
    /// </summary>
    public class ConfigurationManagerFactory
    {
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static IConfigurationManager CreateConfigurationManager()
        {
            return new ConfigurationManager();
        }
    }
}