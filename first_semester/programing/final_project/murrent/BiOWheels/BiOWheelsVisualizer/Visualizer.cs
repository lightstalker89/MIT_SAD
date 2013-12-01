// *******************************************************
// * <copyright file="Visualizer.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsVisualizer
{
    using System;

    /// <summary>
    ///  Class representing the <see cref="Visualizer"/>
    /// </summary>
    public class Visualizer : IVisualizer
    {
        #region Methods

        /// <summary>
        /// </summary>
        internal Visualizer()
        {
            Console.Title = "BiOWheels - (c) 2013 Mario Murrent";
        }

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
            Console.WriteLine("**************");
            Console.WriteLine(text);
        }

        /// <inheritdoc/>
        public void WriteText(string text)
        {
            Console.Write(text);
        }

        /// <inheritdoc/>
        public void GetHelp()
        {
            Console.WriteLine("Options:");
            Console.WriteLine("\t -h \t\t Shows the help");
        }

        /// <inheritdoc/>
        public void SetConsoleWindowSize(int width, int height)
        {
            Console.WindowHeight = height;
            Console.WindowWidth = width;
        }

        /// <inheritdoc/>
        public void MaximizeConsoleWindow()
        {
            Console.WindowHeight = Console.LargestWindowHeight - 40;
            Console.WindowWidth = Console.LargestWindowWidth - 40;
        }

        #endregion
    }
}