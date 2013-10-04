// -----------------------------------------------------------------------
// <copyright file="ConsoleWindow.cs" company="Fachhochschule Wiener Neustadt">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>
// This program represents a simple application which draws a window into
// the console.
// </summary>
// <author>Markus Safar</author>
// -----------------------------------------------------------------------
namespace ConsoleWindows
{
    using System;

    /// <summary>
    /// Represents our ConsoleWindow class which contains the entry point of our application.
    /// </summary>
    public class ConsoleWindow
    {
        /// <summary>
        /// The entry point of our application.
        /// </summary>
        /// <param name="args">Specified command line arguments.</param>
        public static void Main(string[] args)
        {
            // Set buffer size
            Console.BufferWidth = 999;
            Console.BufferHeight = 999;

            // Disable cursor
            Console.CursorVisible = false;

            // Draw window
            DrawWindow(10, 10, 40, 10, title: "Das hier ist der Titel und der ist eindeutig zu lange :-)");

            // Wait for any key
            Console.ReadKey(true);
        }

        /// <summary>
        /// Draws a window into the console.
        /// </summary>
        /// <param name="left">The position from the left.</param>
        /// <param name="top">The position from the top.</param>
        /// <param name="width">The width of the window.</param>
        /// <param name="height">The height of the window.</param>
        /// <param name="foregroundColor">The foreground color of the window.</param>
        /// <param name="backgroundColor">The background color of the window.</param>
        /// <param name="leftTopChar">The character at the left upper edge.</param>
        /// <param name="rightTopChar">The character at the right upper edge.</param>
        /// <param name="leftBottomChar">The character at the left lower edge.</param>
        /// <param name="rightBottomChar">The character at the right lower edge.</param>
        /// <param name="leftChar">The character for the left border of the window.</param>
        /// <param name="rightChar">The character for the right border of the window.</param>
        /// <param name="topChar">The character for the upper border of the window.</param>
        /// <param name="bottomChar">The character for the lower border of the window.</param>
        /// <param name="shadowChar">The character for the shadow of the window.</param>
        /// <param name="title">The title of the window.</param>
        public static void DrawWindow(
            int left = 0,
            int top = 0,
            int width = 10,
            int height = 10,
            ConsoleColor foregroundColor = ConsoleColor.White,
            ConsoleColor backgroundColor = ConsoleColor.Blue,
            char? leftTopChar = null,
            char? rightTopChar = null,
            char? leftBottomChar = null,
            char? rightBottomChar = null,
            char? leftChar = null,
            char? rightChar = null,
            char? topChar = null,
            char? bottomChar = null,
            char? shadowChar = null,
            string title = null)
        {
            // Retrieve "old" characters from the DOS-720 encoding
            char[] x = System.Text.Encoding.GetEncoding("DOS-720").GetChars(new byte[] { 186, 187, 188, 200, 201, 205, 177 });
            char defaultCharLeftTop = x[4];
            char defaultCharRightTop = x[1];
            char defaultCharLeftBottom = x[3];
            char defaultCharRightBottom = x[2];
            char defaultCharLeft = x[0];
            char defaultCharRight = x[0];
            char defaultCharTop = x[5];
            char defaultCharBottom = x[5];
            char defaultCharShadow = x[6];

            // Set the background and foreground color
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;

            // Print the edges
            Console.SetCursorPosition(left, top);
            Console.Write(leftTopChar ?? defaultCharLeftTop);

            Console.SetCursorPosition(left + width, top);
            Console.Write(rightTopChar ?? defaultCharRightTop);

            Console.SetCursorPosition(left, top + height);
            Console.Write(leftBottomChar ?? defaultCharLeftBottom);

            Console.SetCursorPosition(left + width, top + height);
            Console.Write(rightBottomChar ?? defaultCharRightBottom);

            // Top and bottom border
            for (int i = left + 1; i < left + width; i++)
            {
                Console.SetCursorPosition(i, top);
                Console.Write(topChar ?? defaultCharTop);
                Console.SetCursorPosition(i, top + height);
                Console.Write(bottomChar ?? defaultCharBottom);
            }

            // Left and right border
            for (int i = top + 1; i < top + height; i++)
            {
                Console.SetCursorPosition(left, i);
                Console.Write(leftChar ?? defaultCharLeft);
                Console.SetCursorPosition(left + width, i);
                Console.Write(rightChar ?? defaultCharRight);
            }

            // Draw the inner rectangle
            for (int i = top + 1; i < top + height; i++)
            {
                for (int j = left + 1; j < left + width; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(" ");
                }
            }

            // Draw the shadow
            for (int i = left + 1; i <= left + width; i++)
            {
                Console.SetCursorPosition(i, top + height + 1);
                Console.Write(shadowChar ?? defaultCharShadow);
            }

            for (int i = top + 1; i <= top + height + 1; i++)
            {
                Console.SetCursorPosition(left + width + 1, i);
                Console.Write(shadowChar ?? defaultCharShadow);
            }

            // Draw the title
            if (title != null)
            {
                Console.SetCursorPosition(left + 2, top);
                Console.Write(title.Length + 2 > width - 4 ? String.Format(" {0}... ", title.Substring(0, width - 8)) : String.Format(" {0} ", title));
            }
        }
    }
}
