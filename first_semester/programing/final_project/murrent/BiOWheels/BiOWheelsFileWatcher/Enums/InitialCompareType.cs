using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
