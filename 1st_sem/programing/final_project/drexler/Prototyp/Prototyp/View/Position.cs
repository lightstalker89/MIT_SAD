//-----------------------------------------------------------------------
// <copyright file="Position.cs" company="MD Development">
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

    /// <summary>
    /// A point/position on the console/window
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Position on x-ache
        /// </summary>
        private int x;

        /// <summary>
        /// Position on y-ache
        /// </summary>
        private int y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class
        /// </summary>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets or sets the private x variable
        /// </summary>
        public int X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        /// <summary>
        /// Gets or sets the private y variable
        /// </summary>
        public int Y
        {
            get { return this.y; }
            set { this.y = value; }
        }
    }
}
