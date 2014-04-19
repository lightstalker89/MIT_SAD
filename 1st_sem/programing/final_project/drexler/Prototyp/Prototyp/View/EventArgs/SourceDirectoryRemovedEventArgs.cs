//-----------------------------------------------------------------------
// <copyright file="SourceDirectoryRemovedEventArgs.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp.View.EventArgs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Source directory removed event arg
    /// </summary>
    public class SourceDirectoryRemovedEventArgs : System.EventArgs
    {
        /// <summary>
        /// Source directory
        /// </summary>
        private Directory sourceDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceDirectoryRemovedEventArgs"/> class
        /// </summary>
        /// <param name="source">Removed source directory</param>
        public SourceDirectoryRemovedEventArgs(Directory source)
        {
            this.sourceDirectory = source;
        }

        /// <summary>
        /// Gets or sets a source directory
        /// </summary>
        public Directory SourceDirectory
        {
            get { return this.sourceDirectory; }
            set { this.sourceDirectory = value; }
        }
    }
}
