// *******************************************************
// * <copyright file="ConfigLoader.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

using System;

namespace ConsoleBoxConfigLoader
{
    /// <summary>
    /// Class representing the <see cref="ConfigLoader"/>
    /// </summary>
    public class ConfigLoader : IConfigLoader
    {
        /// <summary>
        /// Dictionary containing <see cref="xmlConfigLogger"/> instances
        /// </summary>
        private readonly IConfigLoader xmlConfigLogger = new XmlConfigLoader();

        /// <inheritdoc/>
        public T LoadConfig<T>(string path) where T : class
        {
            try
            {
                return this.xmlConfigLogger.LoadConfig<T>(path);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
