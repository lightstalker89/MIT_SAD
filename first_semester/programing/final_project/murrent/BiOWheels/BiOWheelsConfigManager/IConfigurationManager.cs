// *******************************************************
// * <copyright file="IConfigurationManager.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
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
    public interface IConfigurationManager
    {
        /// <summary>
        /// Loads the configuration
        /// </summary>
        /// <param name="configFileName">
        /// The filename of the configuration file
        /// </param>
        /// <returns>
        /// Returns either the configuration object of type T or a LoaderException
        /// </returns>
        object Load<T>(string configFileName);

        /// <summary>
        /// Write the configuration to the given file
        /// </summary>
        /// <typeparam name="T">
        /// Type of the configuration
        /// </typeparam>
        /// <param name="configFilename">
        /// Name of the configurationfile
        /// </param>
        /// <param name="configurationObject">
        /// Object of the configuration
        /// </param>
        /// <returns>
        /// </returns>
        WriterStatus Write<T>(string configFilename, T configurationObject);
    }
}