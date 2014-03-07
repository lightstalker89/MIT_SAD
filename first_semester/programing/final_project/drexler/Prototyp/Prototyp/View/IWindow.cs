//-----------------------------------------------------------------------
// <copyright file="IWindow.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Prototyp.Log;

    /// <summary>
    /// IWindow interface
    /// </summary>
    public interface IWindow
    {
        /// <summary>
        /// Gets or sets the height of the window
        /// </summary>
        int WindowHeight 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the height of the window
        /// </summary>
        int WindowWidth 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets a value indicating whether the value should be active or not
        /// </summary>
        bool Active 
        { 
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the title of the window
        /// </summary>
        string Title 
        { 
            get;
            set;
        }

        /// <summary>
        /// Draw a window
        /// </summary>
        /// <param name="title">Title of the window</param>
        void DrawWindow(string title);
    }
}
