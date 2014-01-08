// *******************************************************
// * <copyright file="ConfigFile.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/
namespace ConsoleBox.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using ConsoleBox.Settings;

    /// <summary>
    /// Class representing the configuration
    /// </summary>
    public class ConfigFile
    {
        /// <summary>
        /// Gets or sets the folder settings
        /// </summary>
        [XmlElement("Folders")]
        public Folders Folders { get; set; }

        /// <summary>
        /// Gets or sets the application settings
        /// </summary>
        [XmlElement("ApplicationSettings")]
        public ApplicationSettings Settings { get; set; }
    }
}
