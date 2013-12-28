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
        /// Gets the user input for the given words
        /// </summary>
        /// <param name="displayString">
        /// String to print to the console
        /// </param>
        /// <returns>
        /// The input from the user
        /// </returns>
        string GetUserInput(string displayString);

        /// <summary>
        /// Write an entry to the console
        /// </summary>
        /// <param name="entry">
        /// Entry which should be written to the console
        /// </param>
        void WriteLog(string entry);

        /// <summary>
        /// Write a line to the console
        /// </summary>
        /// <param name="text">
        /// Line which should be written to the console
        /// </param>
        void WriteLine(string text);

        /// <summary>
        /// Writes text to the console
        /// </summary>
        /// <param name="text">
        /// Text which should be written to the console
        /// </param>
        void WriteText(string text);

        /// <summary>
        /// Writes chars to the console
        /// </summary>
        /// <param name="charToWrite">
        /// The character to write.
        /// </param>
        /// <param name="charCount">
        /// The character count.
        /// </param>
        void WriteChars(char charToWrite, int charCount);

        /// <summary>
        /// Display help in console
        /// </summary>
        void GetHelp();

        /// <summary>
        /// Sets the size of the console window.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        void SetConsoleWindowSize(int width, int height);

        /// <summary>
        /// Maximizes the console window.
        /// </summary>
        void MaximizeConsoleWindow();
    }
}