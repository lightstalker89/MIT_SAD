// *******************************************************
// * <copyright file="MessageType.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsLogger
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Enumeration representing the <see cref="MessageType"/> values
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1606:ElementDocumentationMustHaveSummaryText", 
        Justification = "Reviewed.")]
    public enum MessageType
    {
        /// <summary> 
        /// </summary>
        INFO, 

        /// <summary>
        /// </summary>
        ERROR, 

        /// <summary>
        /// </summary>
        DEBUG, 

        /// <summary>
        /// </summary>
        WARNING
    }
}