// *******************************************************
// * <copyright file="XmlConfigLoader.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxConfigLoader
{
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Class representing the <see cref="XmlConfigLoader"/>
    /// </summary>
    internal class XmlConfigLoader : IConfigLoader
    {
        /// <summary>
        /// Loads the config file from an xml file which
        /// is located at the specific path. 
        /// </summary>
        /// <param name="path">
        /// The path to the xml config file
        /// </param>
        /// <typeparam name="T">
        /// Type of the instance
        /// </typeparam>
        /// <returns>
        /// The config file, generic type.
        /// </returns>
        public T LoadConfig<T>(string path) where T : class
        {
            if (File.Exists(@path))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                TextReader reader = new StreamReader(@path);
                object obj = deserializer.Deserialize(reader);
                T config = (T)obj;
                reader.Close();
                return config;
            }

            return null;
        }
    }
}
