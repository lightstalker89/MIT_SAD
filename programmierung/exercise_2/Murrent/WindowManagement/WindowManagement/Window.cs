// *******************************************************
// * <copyright file="Window.cs" company="MDMCoWorks">
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

    /// <summary>
    /// Base class with logic
    /// </summary>
    public class Window
    {
        /// <summary>
        /// Title of the window
        /// </summary>
        private string title;

        /// <summary>
        /// Window content
        /// </summary>
        private string content;

        /// <summary>
        /// Foreground color for the window
        /// </summary>
        private ConsoleColor foregroundColor;

        /// <summary>
        /// Background color for the window
        /// </summary>
        private ConsoleColor backgroundColor;

        /// <summary>
        /// Left offset
        /// </summary>
        private int left;

        /// <summary>
        /// Right offset
        /// </summary>
        private int top;

        /// <summary>
        /// The height of the window
        /// </summary>
        private int height;

        /// <summary>
        /// The width of the window
        /// </summary>
        private int width;

        /// <summary>
        /// Gets or sets the title of the window
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
            }
        }

        /// <summary>
        /// Gets or sets the content of the window
        /// </summary>
        public string Content
        {
            get
            {
                return this.content;
            }

            set
            {
                this.content = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color of the window
        /// </summary>
        public ConsoleColor BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }

            set
            {
                this.backgroundColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the foreground color of the window
        /// </summary>
        public ConsoleColor ForegroundColor
        {
            get
            {
                return this.foregroundColor;
            }

            set
            {
                this.foregroundColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the left position of the window
        /// </summary>
        public int Left
        {
            get
            {
                return this.left;
            }

            set
            {
                this.left = value;
            }
        }

        /// <summary>
        /// Gets or sets the top position of the window
        /// </summary>
        public int Top
        {
            get
            {
                return this.top;
            }

            set
            {
                this.top = value;
            }
        }

        /// <summary>
        /// Gets or sets the height of the window
        /// </summary>
        public int Height
        {
            get
            {
                return this.height;
            }

            set
            {
                this.height = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the window
        /// </summary>
        public int Width
        {
            get
            {
                return this.width;
            }

            set
            {
                this.width = value;
            }
        }

        /// <summary>
        /// Draws a window into the console.
        /// </summary>
        /// <param name="leftTopChar">The character at the left upper edge.</param>
        /// <param name="rightTopChar">The character at the right upper edge.</param>
        /// <param name="leftBottomChar">The character at the left lower edge.</param>
        /// <param name="rightBottomChar">The character at the right lower edge.</param>
        /// <param name="leftChar">The character for the left border of the window.</param>
        /// <param name="rightChar">The character for the right border of the window.</param>
        /// <param name="topChar">The character for the upper border of the window.</param>
        /// <param name="bottomChar">The character for the lower border of the window.</param>
        /// <param name="shadowChar">The character for the shadow of the window.</param>
        public virtual void Draw(
            char? leftTopChar = null,
            char? rightTopChar = null,
            char? leftBottomChar = null,
            char? rightBottomChar = null,
            char? leftChar = null,
            char? rightChar = null,
            char? topChar = null,
            char? bottomChar = null,
            char? shadowChar = null)
        {
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
            Console.BackgroundColor = this.backgroundColor;
            Console.ForegroundColor = this.foregroundColor;

            // Print the edges
            Console.SetCursorPosition(this.Left, this.Top);
            Console.Write(leftTopChar ?? defaultCharLeftTop);

            Console.SetCursorPosition(this.Left + this.Width, this.Top);
            Console.Write(rightTopChar ?? defaultCharRightTop);

            Console.SetCursorPosition(this.Left, this.Top + this.Height);
            Console.Write(leftBottomChar ?? defaultCharLeftBottom);

            Console.SetCursorPosition(this.Left + this.Width, this.Top + this.Height);
            Console.Write(rightBottomChar ?? defaultCharRightBottom);

            // Top and bottom border
            for (int i = this.Left + 1; i < this.Left + this.Width; i++)
            {
                Console.SetCursorPosition(i, this.Top);
                Console.Write(topChar ?? defaultCharTop);
                Console.SetCursorPosition(i, this.Top + this.Height);
                Console.Write(bottomChar ?? defaultCharBottom);
            }

            // Left and right border
            for (int i = this.Top + 1; i < this.Top + this.Height; i++)
            {
                Console.SetCursorPosition(this.Left, i);
                Console.Write(leftChar ?? defaultCharLeft);
                Console.SetCursorPosition(this.Left + this.Width, i);
                Console.Write(rightChar ?? defaultCharRight);
            }

            int drawIndex = this.Left + 1;

            // Draw the inner rectangle
            for (int i = this.Top + 1; i < this.Top + this.Height; i++)
            {
                for (int j = this.Left + 1; j < this.Left + this.Width; j++)
                {
                    Console.SetCursorPosition(j, i);

                    // Draw the text
                    if (j < this.Content.Length + this.Left + 1 && drawIndex <= this.Content.Length + this.Left)
                    {
                        Console.Write(this.Content[drawIndex - this.Left - 1]);
                        drawIndex++;
                    }
                    else
                    {
                        Console.WriteLine(" ");
                    }
                }
            }

            // Draw the shadow
            for (int i = this.Top + 1; i <= this.Top + this.Width; i++)
            {
                Console.SetCursorPosition(i, this.Top + this.Height + 1);
                Console.Write(shadowChar ?? defaultCharShadow);
            }

            for (int i = this.Top + 1; i <= this.Top + this.Height + 1; i++)
            {
                Console.SetCursorPosition(this.Left + this.Width + 1, i);
                Console.Write(shadowChar ?? defaultCharShadow);
            }

            // Draw the title
            if (this.Title != null)
            {
                Console.SetCursorPosition(this.Left + 2, this.Top);
                Console.Write(this.Title.Length + 2 > this.Width - 4 ? string.Format(" {0}... ", this.Title.Substring(0, this.Width - 8)) : string.Format(" {0} ", this.Title));
            }
        }
    }
}