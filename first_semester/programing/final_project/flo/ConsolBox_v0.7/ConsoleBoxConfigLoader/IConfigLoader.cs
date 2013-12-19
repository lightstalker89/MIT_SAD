// *******************************************************
// * <copyright file="IConfigLoader.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxConfigLoader
{
    /// <summary>
    /// Interface representing methods of the <see cref="IConfigLoader"/>
    /// </summary>
    public interface IConfigLoader
    {
        /// <summary>
        /// Loads the config file from XmlConfigLoader class
        /// </summary>
        /// <param name="path">
        /// The path to the config
        /// </param>
        /// <typeparam name="T">
        /// Type of the instance
        /// </typeparam>
        /// <returns>
        /// The config file, generic type.
        /// </returns>
        T LoadConfig<T>(string path) where T : class;
    }
}
