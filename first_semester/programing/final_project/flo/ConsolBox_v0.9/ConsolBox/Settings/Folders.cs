// *******************************************************
// * <copyright file="Folders.cs" company="FGrill">
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
    using System.Xml.Serialization;

    /// <summary>
    /// Class representing the folder mappings
    /// </summary>
    public class Folders
    {
        /// <summary>
        /// Gets or sets the folder mappings
        /// </summary>
        [XmlElement("Mapping")]
        public List<Mapping> FolderMapping { get; set; }
    }
}
