// *******************************************************
// * <copyright file="IVisualizer.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxVisualizer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface representing the <see cref="IVisualizer"/>
    /// </summary>
    public interface IVisualizer
    {
        /// <summary>
        /// Shows the help.
        /// </summary>
        void ShowHelp();

        /// <summary>
        /// Writes a new line without break.
        /// </summary>
        /// <param name="line">The line.</param>
        void Write(string line);

        /// <summary>
        /// Writes a new line with break.
        /// </summary>
        /// <param name="line">The line.</param>
        void WriteLine(string line);

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

        /// <summary>
        /// Gets the user input.
        /// </summary>
        /// <param name="displayString">The display string (Explanation for user).</param>
        /// <returns>The user input.</returns>
        string GetUserInput(string displayString);
    }
}
