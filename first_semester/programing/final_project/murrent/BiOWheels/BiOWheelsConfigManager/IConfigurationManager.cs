// *******************************************************
// * <copyright file="IConfigurationManager.cs" company="MDMCoWorks">
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
    /// Interface representing all needed methods
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// Loads the configuration
        /// </summary>
        /// <typeparam name="T">
        /// Type of the configuration
        /// </typeparam>
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
        /// Name of the configuration file
        /// </param>
        /// <param name="configurationObject">
        /// Object of the configuration
        /// </param>
        /// <returns>
        /// The status of the writer
        /// </returns>
        WriterStatus Write<T>(string configFilename, T configurationObject);
    }
}