// *******************************************************
// * <copyright file="WindowManager.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace WindowManagement
{
    using System.Collections.Generic;

    /// <summary>
    /// WindowManager class and interaction logic
    /// </summary>
    public class WindowManager
    {
        /// <summary>
        /// Holds all the windows which should be drawn to the console
        /// </summary>
        private List<Window> windows = new List<Window>();

        /// <summary>
        /// Holds the index for the current window
        /// </summary>
        private int currentWindowIndex;

        /// <summary>
        /// Gets the window count for the elements in the window list
        /// </summary>
        public int WindowCount
        {
            get
            {
                return this.Windows.Count - 1;
            }
        }

        /// <summary>
        /// Gets or sets the windows which should be drawn to the console
        /// </summary>
        public List<Window> Windows
        {
            get
            {
                return this.windows;
            }

            set
            {
                this.windows = value;
            }
        }

        /// <summary>
        /// Draws the first window from the list
        /// </summary>
        public void DrawAll()
        {
            this.Windows.ForEach(p => p.Draw());

            this.currentWindowIndex = this.WindowCount;
        }

        /// <summary>
        /// Draws a specific window according to the index
        /// </summary>
        /// <param name="index">
        /// Window which should be drawn
        /// </param>
        public void Draw(int index)
        {
            this.Windows[index].Draw();
        }

        /// <summary>
        /// Draws the next window in the list
        /// </summary>
        public void DrawNext()
        {
            this.currentWindowIndex++;

            if (this.currentWindowIndex > this.WindowCount)
            {
                this.currentWindowIndex = 0;
            }

            this.Draw(this.currentWindowIndex);
        }

        /// <summary>
        /// Draws the previous window
        /// </summary>
        public void DrawPrevious()
        {
            this.currentWindowIndex--;

            if (this.currentWindowIndex < 0)
            {
                this.currentWindowIndex = this.WindowCount;
            }

            this.Draw(this.currentWindowIndex);
        }
    }
}