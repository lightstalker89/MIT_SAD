// *******************************************************
// * <copyright file="InitialCompareType.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher.Enums
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Enumeration representing the <see cref="InitialCompareType"/> values
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1606:ElementDocumentationMustHaveSummaryText", 
        Justification = "Reviewed.")]
    public enum InitialCompareType
    {
        /// <summary>
        /// </summary>
        DIRECTORY, 

        /// <summary>
        /// </summary>
        FILE
    }
}