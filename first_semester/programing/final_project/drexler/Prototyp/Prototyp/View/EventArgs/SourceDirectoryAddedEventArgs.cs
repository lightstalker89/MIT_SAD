//-----------------------------------------------------------------------
// <copyright file="SourceDirectoryAddedEventArgs.cs" company="MD Development">
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
    /// Source directory added event args
    /// </summary>
    public class SourceDirectoryAddedEventArgs : System.EventArgs
    {
        private Directory sourceDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceDirectoryAddedEventArgs"/> class
        /// </summary>
        /// <param name="source">Source directory added for syncing</param>
        public SourceDirectoryAddedEventArgs(Directory source)
        {
            this.sourceDirectory = source;
        }

        /// <summary>
        /// Gets or sets the added source directory
        /// </summary>
        public Directory SourceDirectory
        {
            get { return this.sourceDirectory; }
            set { this.sourceDirectory = value; }
        }
    }
}
