// *******************************************************
// * <copyright file="SourceMappingInfo.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
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
    /// </summary>
    public class SourceMappingInfo
    {
        /// <summary>
        /// </summary>
        [XmlAttribute]
        public bool Recursive { get; set; }

        /// <summary>
        /// </summary>
        public string SourceDirectory { get; set; }
    }
}