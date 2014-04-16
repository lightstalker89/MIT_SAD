// *******************************************************
// * <copyright file="SourceMappingInfo.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheels.BiOWheelsConfiguration
{
    using System.Xml.Serialization;

    /// <summary>
    /// Class representing the information for a source mapping
    /// </summary>
    public class SourceMappingInfo
    {
        /// <summary>
        /// Gets or sets a value indicating whether a folder should be synced recursive or not
        /// </summary>
        [XmlAttribute]
        public bool Recursive { get; set; }

        /// <summary>
        /// Gets or sets the source directory
        /// </summary>
        public string SourceDirectory { get; set; }
    }
}