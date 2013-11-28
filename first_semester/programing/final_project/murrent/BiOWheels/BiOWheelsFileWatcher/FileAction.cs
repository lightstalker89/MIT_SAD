// *******************************************************
// * <copyright file="FileAction.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Enumeration representing the <see cref="FileAction"/> values
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1606:ElementDocumentationMustHaveSummaryText", 
        Justification = "Reviewed.")]
    public enum FileAction
    {
        /// <summary>
        /// </summary>
        DELETE, 

        /// <summary>
        /// </summary>
        COPY, 

        /// <summary>
        /// </summary>
        DIFF, 

        /// <summary>
        /// </summary>
        CREATE,

        /// <summary>
        /// </summary>
        RENAME
    }
}