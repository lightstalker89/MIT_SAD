//-----------------------------------------------------------------------
// <copyright file="WindowManager.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Prototyp.Log;

    /// <summary>
    /// Window manager
    /// </summary>
    public class WindowManager
    {
        /// <summary>
        /// Logger instance
        /// </summary>
        private Logger logger = Logger.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowManager"/> class
        /// </summary>
        public WindowManager()
        {
            this.Windows = new List<IWindow>();
        }

        /// <summary>
        /// Gets or sets a collection of all windows
        /// </summary>
        public List<IWindow> Windows { get; set; }

        /// <summary>
        /// Creates a new Window and sets it to active
        /// </summary>
        /// <param name="topLeft">Position where to place the Window</param>
        /// <param name="width">Width of the Window</param>
        /// <param name="height">Height of the Window</param>
        /// <param name="backgroundColor">BackgroundColor of the Window</param>
        /// <param name="title">Title of the window</param>
        public void CreateWindow(Position topLeft, int width, int height, ConsoleColor backgroundColor, string title)
        {
            Console.ResetColor();
            if (this.Windows != null)
            {
                foreach (var window in this.Windows)
                {
                    if (window.Active == true)
                    {
                        window.Active = false;
                    }
                }
            }

            Position topleft = topLeft;
            IWindow consoleWindow = new ConsoleWindow(topleft, width, height);
            ((ConsoleWindow)consoleWindow).BackgroundColor = backgroundColor;
            consoleWindow.Active = true;
            consoleWindow.DrawWindow(title);
            this.Windows.Add(consoleWindow);
        }

        /// <summary>
        /// Deletes a specific Window
        /// </summary>
        /// <param name="index">Index of the Window in the list</param>
        public void DeleteWindow(int index)
        {
            this.Windows.RemoveAt(index);
        }

        /// <summary>
        /// Clears the console and draws the windows and it changes again
        /// </summary>
        public void Refresh()
        {
            // TODO Remove dependencies from System.Console
            Console.Clear();
            Console.ResetColor();
            foreach (var window in this.Windows)
            {
                window.DrawWindow(window.Title);
                if (window == this.Windows.Last())
                {
                    window.Active = true;
                }
            }
        }

        /// <summary>
        /// Get the current active Window
        /// </summary>
        /// <returns>Returns the current active Window</returns>
        public IWindow GetActiveWindow()
        {
            foreach (var window in this.Windows)
            {
                if (window.Active)
                {
                    return window;
                }
            }

            return null;
        }

        /// <summary>
        /// Moves a specific window one step to the foreground
        /// </summary>
        /// <param name="index">Index of the window in the list of Windows</param>
        public void MoveWindowToForeGround(int index)
        {
            // TODO Remove dependencies from System.Console
            Console.ResetColor();
            IWindow temp;
            for (int i = 0; i < this.Windows.Count(); i++)
            {
                if (this.Windows.ElementAt(index) == this.Windows.ElementAt(i))
                {
                    if ((i + 1) < this.Windows.Count())
                    {
                        temp = this.Windows.ElementAt(i + 1);
                        if (this.Windows.Last() == temp && temp.Active)
                        {
                            temp.Active = false;
                            this.Windows.ElementAt(index).Active = true;
                        }

                        this.Windows[i + 1] = this.Windows.ElementAt(index);
                        this.Windows[i] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// Moves a specific window one step to the background
        /// </summary>
        /// <param name="index">Index of the window in the list of windows</param>
        public void MoveWindowToBackGround(int index)
        {
            // TODO Remove dependencies from System.Console
            Console.ResetColor();

            IWindow temp;
            for (int i = 0; i < this.Windows.Count(); i++)
            {
                if (this.Windows.ElementAt(index) == this.Windows.ElementAt(i))
                {
                    if ((i - 1) >= 0)
                    {
                        temp = this.Windows.ElementAt(i - 1);
                        if (this.Windows.ElementAt(index) == this.Windows.Last() && this.Windows.ElementAt(index).Active)
                        {
                            this.Windows.ElementAt(index).Active = false;
                            temp.Active = true;
                        }

                        this.Windows[i - 1] = this.Windows.ElementAt(index);
                        this.Windows[i] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// Select a specific window
        /// </summary>
        /// <param name="index">Index of the window in the list of windows</param>
        /// <returns>Returns the selected Window</returns>
        public IWindow SelectWindow(int index)
        {
            // TODO Remove dependencies from System.Console
            IWindow selectedWindow = this.Windows.ElementAt(index);
            if (selectedWindow != null)
            {              
                if (selectedWindow is ConsoleWindow)
                {
                    Console.ResetColor();
                    ((ConsoleWindow)selectedWindow).BackgroundColor = ConsoleColor.Blue;
                }

                return selectedWindow;
            }

            return null;
        }

        /// <summary>
        /// Select the index of a specific Window
        /// </summary>
        /// <param name="window">The window</param>
        /// <returns>The index of the window</returns>
        public int SelectWindow(IWindow window)
        {
            if (this.Windows.Contains(window))
            {
                return this.Windows.FindIndex(m => m == window);
            }

            return -1;
        }
    }
}
