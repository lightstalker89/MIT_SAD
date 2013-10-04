// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace WindowManagement
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the Application which contains the entry point of the application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// WindowManager holds all windows
        /// </summary>
        private static readonly WindowManager WindowManager = new WindowManager();

        /// <summary>
        /// Entry point for the program
        /// </summary>
        private static void Main()
        {
            Console.CursorVisible = false;

            WindowManager.Windows = new List<Window>
                {
                    new WindowWithSigns(new[] { '*', '.', '<' }, 10) { Content = "Window 1", ForegroundColor = ConsoleColor.Yellow, BackgroundColor = ConsoleColor.DarkBlue, Title = "Window Blue/Yellow", Left = 0, Top = 0, Width = 10, Height = 10 },
                    new WindowWithSigns(new[] { '-', '+', '>' }, 5) { Content = "Window 2 wants to be a window, but will never be one because windows dont look like that. The only chance is to change the window immediately", ForegroundColor = ConsoleColor.Cyan, BackgroundColor = ConsoleColor.Red, Title = "Window Cyan/Red", Left = 0, Top = 0, Width = 15, Height = 10 },
                    new WindowWithTextContent { Content = "Window 3 has a green background and a border", ForegroundColor = ConsoleColor.DarkRed, BackgroundColor = ConsoleColor.Green, Title = "Window Red/Green", Left = 2, Top = 2, Width = 15, Height = 15 },
                    new WindowWithTextContent { Content = "Window 4 has a gray background", ForegroundColor = ConsoleColor.DarkMagenta, BackgroundColor = ConsoleColor.DarkGray, Title = "Window Magenta/Gray", Left = 1, Top = 1, Width = 20, Height = 20 },
                    new WindowWithTextContent { Content = "Window 5 has a gray background", ForegroundColor = ConsoleColor.DarkGreen, BackgroundColor = ConsoleColor.DarkGray, Title = "Window Darkgreen/Gray", Left = 1, Top = 1, Width = 20, Height = 20 },
                    new WindowWithTextContent { Content = "Window 6 has a gray background", ForegroundColor = ConsoleColor.White, BackgroundColor = ConsoleColor.DarkGray, Title = "Window White/Gray", Left = 1, Top = 1, Width = 20, Height = 20 }
                };

            WindowManager.DrawAll();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        WindowManager.DrawNext();
                        break;
                    case ConsoleKey.DownArrow:
                        WindowManager.DrawPrevious();
                        break;

                    case ConsoleKey.F4:
                        Environment.Exit(0);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}