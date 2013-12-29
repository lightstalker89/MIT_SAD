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
        /// Initializes a new instance of the <see cref="Visualizer"/> class.
        /// </summary>
        internal Visualizer()
        {
            Console.Title = "BiOWheels - (c) 2013 Mario Murrent";
        }

        /// <inheritdoc/>
        public void DisplayLoadingBar(string loadingMessage, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            int width = Console.BufferWidth;

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.OutputEncoding = System.Text.Encoding.GetEncoding(1252);

            for (int i = 0; i < width; i++)
            {
                Console.SetCursorPosition(i, 0);

                if (i < loadingMessage.Length)
                {
                    Console.Write(loadingMessage[i]);
                }
                else
                {
                    Console.ForegroundColor = backgroundColor;
                    Console.Write((char)219);
                }
            }

            Console.ResetColor();
        }

        /// <inheritdoc/>
        public void DisplayProgressBar()
        {
        }

        /// <inheritdoc/>
        public string GetUserInput(string displayString)
        {
            int starCount = displayString.Length;
            string starString = string.Empty;

            for (int i = 0; i < starCount; i++)
            {
                starString += "*";
            }

            Console.WriteLine(starString);
            Console.Write(displayString);

            return Console.ReadLine();
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
        public void WriteChars(char charToWrite, int charCount)
        {
            string starString = string.Empty;

            for (int i = 0; i < charCount; i++)
            {
                starString += charToWrite;
            }

            Console.WriteLine(starString);
        }

        /// <inheritdoc/>
        public void GetHelp()
        {
            Console.WriteLine("Options:");
            Console.WriteLine("\t -h \t\t Shows the help");
            Console.WriteLine("\t -f \t\t Specifies the file name");
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