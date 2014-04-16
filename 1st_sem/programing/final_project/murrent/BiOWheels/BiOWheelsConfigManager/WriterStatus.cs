// *******************************************************
// * <copyright file="WriterStatus.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsConfigManager
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1606:ElementDocumentationMustHaveSummaryText", 
        Justification = "Reviewed.")]
    public enum WriterStatus
    {
        /// <summary>
        /// </summary>
        TYPEMISSMATCH, 

        /// <summary>
        /// </summary>
        SUCCESSFULL, 

        /// <summary>
        /// </summary>
        FAILED, 

        /// <summary>
        /// </summary>
        PATHISNULL, 

        /// <summary>
        /// </summary>
        ACCESSERROR, 

        /// <summary>
        /// </summary>
        PATHERROR
    }
}