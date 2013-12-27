// *******************************************************
// * <copyright file="SourceFolder.cs" company="FGrill">
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

    /// <summary>
    /// Class representing the <see cref="SourceFolder"/>
    /// </summary>
    public class SourceFolder
    {
        /// <summary>
        /// Gets or sets the Path of the source folders
        /// </summary>
        [XmlAttribute("path")]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Recursion for file watcher
        /// in folder is on or off
        /// </summary>
        [XmlAttribute("recursion")]
        public bool Recursion { get; set; }

        /// <summary>
        /// Gets or sets the destination folders
        /// </summary>
        [XmlElement("DestinationFolder")]
        public List<string> DestinationFolder { get; set; }

        /// <summary>
        /// Gets or sets the exception folders
        /// </summary>
        [XmlElement("ExceptionFolder")]
        public List<string> ExceptionFolder { get; set; }
    }
}
