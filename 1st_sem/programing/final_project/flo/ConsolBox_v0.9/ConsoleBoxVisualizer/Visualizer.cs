// *******************************************************
// * <copyright file="Visualizer.cs" company="FGrill">
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

    /// <summary>
    /// Interface representing the <see cref="Visualizer"/>
    /// </summary>
    public class Visualizer : IVisualizer
    {
        /// <inheritdoc/>
        public void SetConsoleWindowSize(int width, int height)
        {
            Console.WindowHeight = height;
            Console.WindowWidth = width;
        }

        /// <inheritdoc/>
        public void MaximizeConsoleWindow()
        {
            Console.WindowHeight = Console.LargestWindowHeight - 20;
            Console.WindowWidth = Console.LargestWindowWidth - 20;
        }

        /// <inheritdoc/>
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        /// <inheritdoc/>
        public void Write(string line)
        {
            Console.Write(line);
        }

        /// <inheritdoc/>
        public void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine("----------------HELP----------------");
            Console.WriteLine("press h for help");
            Console.WriteLine("press p for changing parallel synchronisation");
            Console.WriteLine("press c for changing block compare size");
            Console.WriteLine("press b for changing block size");
            Console.WriteLine("press l for changing log file size");
            Console.WriteLine("press x for closing application");
            Console.WriteLine();
        }

        /// <inheritdoc/>
        public string GetUserInput(string displayString)
        {
            Console.Write(displayString);

            return Console.ReadLine();
        }
    }
}
