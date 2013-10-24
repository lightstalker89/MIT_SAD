// *******************************************************
// * <copyright file="Visualizer.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System;

namespace BiOWheelsVisualizer
{
    /// <summary>
    /// </summary>
    public class Visualizer : IVisualizer
    {
        #region Methods
        /// <inheritdoc/>
        public void GetMenu()
        {
        }

        /// <inheritdoc/>
        public void WriteLog(string entry)
        {
        }

        /// <inheritdoc/>
        public void WriteLine(string text)
        {
            Console.WriteLine("********");
            Console.WriteLine(text);
        }

        /// <inheritdoc/>
        public void WriteText(string text)
        {
            Console.Write(text);
        }

        #endregion
    }
}