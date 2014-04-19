// *******************************************************
// * <copyright file="Mapping.cs" company="FGrill">
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
    /// Class representing the source folders
    /// </summary>
    public class Mapping
    {
        /// <summary>
        /// Gets or sets the source folders
        /// </summary>
        [XmlElement("SourceFolder")]
        public SourceFolder SourceFolders { get; set; }
    }
}
