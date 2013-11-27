// *******************************************************
// * <copyright file="IVisualizer.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsVisualizer
{
    /// <summary>
    /// Interface representing the must implement methods
    /// </summary>
    public interface IVisualizer
    {
        /// <summary>
        /// Get the menu for the console
        /// </summary>
        void GetMenu();

        /// <summary>
        /// Write an entry to the log file
        /// </summary>
        /// <param name="entry">
        /// Entry which should be written to the log file
        /// </param>
        void WriteLog(string entry);

        /// <summary>
        /// Write a line to the log file
        /// </summary>
        /// <param name="text">
        /// Line which should be written to the log file
        /// </param>
        void WriteLine(string text);

        /// <summary>
        /// Writes text to the log file
        /// </summary>
        /// <param name="text">
        /// Text which should be written to the log file
        /// </param>
        void WriteText(string text);

        /// <summary>
        /// Display help in console
        /// </summary>
        void GetHelp();

        /// <summary>
        /// Sets the size of the console window.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        void SetConsoleWindowSize(int width, int height);

        /// <summary>
        /// Maximizes the console window.
        /// </summary>
        void MaximizeConsoleWindow();
    }
}