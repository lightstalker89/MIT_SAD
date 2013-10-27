// *******************************************************
// * <copyright file="IVisualizer.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
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
    }
}