//-----------------------------------------------------------------------
// <copyright file="ConsoleWindow.cs" company="MD Development">
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
    /// Console window
    /// </summary>
    public class ConsoleWindow : IWindow
    {
        /// <summary>
        /// Write logs to the window
        /// </summary>
        private static Logger logger = Logger.Instance;

        /// <summary>
        /// Top left position of the window
        /// </summary>
        private Position topLeft;

        /// <summary>
        /// Top right position of the window
        /// </summary>
        private Position topRight;

        /// <summary>
        /// Bottom left position of the window
        /// </summary>
        private Position bottomLeft;

        /// <summary>
        /// Bottom right position of the window
        /// </summary>
        private Position bottomRight;

        /// <summary>
        /// Left top default char for the window border 
        /// </summary>
        private char defaultCharLeftTop;

        /// <summary>
        /// Right top default char for the window border
        /// </summary>
        private char defaultCharRightTop;

        /// <summary>
        /// Left Bottom default char for the window border
        /// </summary>
        private char defaultCharLeftBottom;

        /// <summary>
        /// Right bottom default char for the window border
        /// </summary>
        private char defaultCharRightBottom;

        /// <summary>
        /// Left default char for the window border
        /// </summary>
        private char defaultCharLeft;

        /// <summary>
        /// Right default char for the window border
        /// </summary>
        private char defaultCharRight;

        /// <summary>
        /// Top default char for the window border
        /// </summary>
        private char defaultCharTop;

        /// <summary>
        /// Bottom default char for the window border
        /// </summary>
        private char defaultCharBottom;

        /// <summary>
        /// Shadow default char for the window
        /// </summary>
        private char defaultCharShadow;

        /// <summary>
        /// Defines if the window is active
        /// </summary>
        private bool active = false;

        /// <summary>
        /// The title of the window
        /// </summary>
        private string title;

        /// <summary>
        /// Handles the entered commands
        /// </summary>
        private CommandInterpreter commandInterpreter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleWindow"/> class
        /// </summary>
        /// <param name="topLeft">Position where to start to draw</param>
        /// <param name="width">Width of the window</param>
        /// <param name="height">Height of the window</param>
        public ConsoleWindow(Position topLeft, int width, int height)
        {
            this.WindowWidth = width;
            this.WindowHeight = height;
            this.topLeft = topLeft;
            this.topRight = new Position(topLeft.X + this.WindowWidth, topLeft.Y);
            this.bottomLeft = new Position(topLeft.X, topLeft.Y + this.WindowHeight);
            this.bottomRight = new Position(topLeft.X + this.WindowWidth, topLeft.Y + this.WindowHeight);

            // Retrieve "old" characters from the DOS-720 encoding
            char[] x = System.Text.Encoding.GetEncoding("DOS-720").GetChars(new byte[] { 186, 187, 188, 200, 201, 205, 177 });
            this.defaultCharLeftTop = x[4];
            this.defaultCharRightTop = x[1];
            this.defaultCharLeftBottom = x[3];
            this.defaultCharRightBottom = x[2];
            this.defaultCharLeft = x[0];
            this.defaultCharRight = x[0];
            this.defaultCharTop = x[5];
            this.defaultCharBottom = x[5];
            this.defaultCharShadow = x[6];
        }

        // Properties 

        /// <summary>
        /// Gets or sets the width of the window
        /// </summary>
        public int WindowWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the window
        /// </summary>
        public int WindowHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the window is active or not
        /// </summary>
        public bool Active 
        { 
            get { return this.active; } 
            set { this.active = value; } 
        }

        /// <summary>
        /// Gets the top left corner position of the window
        /// </summary>
        public Position TopLeft
        {
            get { return this.topLeft; }
        }

        /// <summary>
        /// Gets the top right corner position of the window
        /// </summary>
        public Position TopRight
        {
            get { return this.topRight; }
        }

        /// <summary>
        /// Gets the bottom left corner position of the window
        /// </summary>
        public Position BottomLeft
        {
            get { return this.bottomLeft; }
        }

        /// <summary>
        /// Gets the bottom right corner position of the window
        /// </summary>
        public Position BottomRight
        {
            get { return this.bottomRight; }
        }

        /// <summary>
        /// Gets or sets a value for the window title
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        /// <summary>
        /// Gets or sets the background color of the window
        /// </summary>
        public ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the window
        /// </summary>
        public ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets a new CommandHandler
        /// </summary>
        public CommandInterpreter CommandInterpreter
        {
            get { return this.commandInterpreter; }
            set { this.commandInterpreter = value; }
        }

        // Methods

        /// <summary>
        /// Draws the window in the console
        /// </summary>
        /// <param name="title">Title of the window</param>
        public void DrawWindow(string title)
        {
            Console.ResetColor();
            this.topRight = new Position(this.topLeft.X + this.WindowWidth, this.topLeft.Y);
            this.bottomLeft = new Position(this.topLeft.X, this.topLeft.Y + this.WindowHeight);
            this.bottomRight = new Position(this.topLeft.X + this.WindowWidth, this.topLeft.Y + this.WindowHeight);
            Position startPosition = new Position(this.topLeft.X + 1, this.topLeft.Y + 1);
            Console.SetCursorPosition(this.topLeft.X, this.topLeft.Y);
            Console.ForegroundColor = ConsoleColor.White;

            this.DrawX(this.topLeft, this.topRight);
            this.DrawY(this.topLeft, this.bottomLeft);
            this.PrintEdges();
            Console.ResetColor();
            this.DrawShadowX(this.bottomLeft, this.bottomRight);
            this.DrawShadowY(this.topRight, this.bottomRight);
            Console.ResetColor();
            this.DrawRectangle(startPosition, this.WindowWidth, this.WindowHeight);
            Console.ResetColor();

            // Draw the title
            if (title != null)
            {
                Console.SetCursorPosition(this.topLeft.X + 2, this.topLeft.Y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(title.Length + 2 > this.WindowWidth - 4 ? string.Format(" {0}... ", title.Substring(0, this.WindowWidth - 8)) : string.Format(" {0} ", title));
            }

            Console.SetCursorPosition(startPosition.X, startPosition.Y);
            Console.ResetColor();
        }

        /// <summary>
        /// Draws the x-aches on the console
        /// </summary>
        /// <param name="startPosition">Position where to start to draw</param>
        /// <param name="endPosition">Position where to stop to draw</param>
        public void DrawX(Position startPosition, Position endPosition)
        {
            for (int x = 0; x < (endPosition.X - startPosition.X); x++)
            {
                Console.SetCursorPosition(startPosition.X + x, startPosition.Y);
                Console.Write(this.defaultCharTop);
                Console.SetCursorPosition(startPosition.X + x, startPosition.Y + this.WindowHeight);
                Console.Write(this.defaultCharBottom);
            }
        }

        /// <summary>
        /// Draws the y-aches on the console
        /// </summary>
        /// <param name="startPosition">Position where to start to draw</param>
        /// <param name="endPosition">Position where to stop to draw</param>
        public void DrawY(Position startPosition, Position endPosition)
        {
            for (int y = 0; y <= (endPosition.Y - startPosition.Y); y++)
            {
                Console.SetCursorPosition(startPosition.X, startPosition.Y + y);
                Console.Write(this.defaultCharLeft);
                Console.SetCursorPosition(startPosition.X + this.WindowWidth, startPosition.Y + y);
                Console.Write(this.defaultCharRight);
            }
        }

        /// <summary>
        /// Draws the edges
        /// </summary>
        public void PrintEdges()
        {
            // Print the edges
            Console.SetCursorPosition(this.topLeft.X, this.topLeft.Y);
            Console.Write(this.defaultCharLeftTop);

            Console.SetCursorPosition(this.topLeft.X + this.WindowWidth, this.topLeft.Y);
            Console.Write(this.defaultCharRightTop);

            Console.SetCursorPosition(this.topLeft.X, this.topLeft.Y + this.WindowHeight);
            Console.Write(this.defaultCharLeftBottom);

            Console.SetCursorPosition(this.topLeft.X + this.WindowWidth, this.topLeft.Y + this.WindowHeight);
            Console.Write(this.defaultCharRightBottom);
        }

        /// <summary>
        /// Draws the shadow on the x-aches
        /// </summary>
        /// <param name="startPosition">Position where to start to draw</param>
        /// <param name="endPosition">Position where to stop to draw</param>
        public void DrawShadowX(Position startPosition, Position endPosition)
        {
            Console.SetCursorPosition(startPosition.X + 1, startPosition.Y + 1);
            for (int x = 0; x < (endPosition.X - startPosition.X); x++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write(this.defaultCharShadow);
            }
        }

        /// <summary>
        /// Draws the shadow on the y-aches
        /// </summary>
        /// <param name="startPosition">Position where to start to draw</param>
        /// <param name="endPosition">Position where to stop to draw</param>
        public void DrawShadowY(Position startPosition, Position endPosition)
        {
            for (int y = 0; y < ((endPosition.Y + 1) - startPosition.Y); y++)
            {
                Console.SetCursorPosition(startPosition.X + 1, startPosition.Y + 1 + y);
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.WriteLine(this.defaultCharShadow);
            }
        }

        /// <summary>
        /// Draws the inner rectangle 
        /// </summary>
        /// <param name="startPosition">Position where to start to draw</param>
        /// <param name="windowWidth">Width of the drawing window</param>
        /// <param name="windowHeight">Height of the drawing window</param>
        public void DrawRectangle(Position startPosition, int windowWidth, int windowHeight)
        {
            string rectangle = string.Empty;
            Console.SetCursorPosition(startPosition.X, startPosition.Y);
            for (int i = 0; i < windowHeight - 1; ++i)
            {
                // Zeile (x)
                for (int j = 0; j < windowWidth - 1; ++j)
                { 
                    // Spalte (y)
                    rectangle += " ";
                    Console.SetCursorPosition(startPosition.X, startPosition.Y + i);
                    Console.BackgroundColor = this.BackgroundColor;
                    Console.ForegroundColor = this.ForegroundColor;
                    Console.WriteLine(rectangle);
                }

                rectangle = string.Empty;
            }
        }

        /// <summary>
        /// Write text to the console window
        /// </summary>
        /// <param name="output">Text to display in the console</param>
        /// <param name="color">Foreground color of the console</param>
        public void Write(string output, ConsoleColor color)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.BackgroundColor = this.BackgroundColor;

            if ((Console.CursorTop + 1) < this.BottomLeft.Y)
            {
                Console.SetCursorPosition(this.TopLeft.X + 1, Console.CursorTop);
            }
            else
            {
                Position startPosition = new Position(this.TopLeft.X + 1, this.TopLeft.Y + 1);
                this.DrawRectangle(startPosition, this.WindowWidth, this.WindowHeight);
                Console.SetCursorPosition(this.TopLeft.X + 1, this.TopLeft.Y + 1);
            }

            if (output.Length < this.WindowWidth)
            {
                string[] lines = output.Split(new string[] { "\n" }, StringSplitOptions.None);
                foreach (string line in lines)
                {
                    Console.Write(output);
                    Console.SetCursorPosition(this.topLeft.X + 1, Console.CursorTop + 1);
                }
            }
            else
            {
                string[] lines = output.Split(new string[] { "\n" }, StringSplitOptions.None);
                foreach (string line in lines)
                {
                    IEnumerable<string> splittedString = this.SplitStringAtLenght(line, this.WindowWidth - 1);
                    foreach (string str in splittedString)
                    {
                        Console.Write(str);
                        Console.SetCursorPosition(this.TopLeft.X + 1, Console.CursorTop + 1);
                    }
                }
            }

            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Clear ConsoleWindow
        /// </summary>
        public void ClearWindow()
        {
            Position temp = new Position(this.topLeft.X + 1, this.topLeft.Y + 1);
            Console.SetCursorPosition(temp.X, temp.Y);
            this.DrawRectangle(temp, this.WindowWidth, this.WindowHeight);
        }

        /// <summary>
        /// Resets the console color
        /// </summary>
        public void ResetColor()
        {
            Console.ResetColor();
        }

        /// <summary>
        /// Set the cursor to a new Position
        /// </summary>
        public void SetCursorPosition()
        {
            Console.SetCursorPosition(this.topLeft.X + 1, Console.CursorTop + 1);
        }

        /// <summary>
        /// Sets the cursor position to the top of the window
        /// </summary>
        public void SetCursorToTop()
        {
            Console.SetCursorPosition(this.topLeft.X + 1, this.topLeft.Y + 1);
        }

        /// <summary>
        /// Split strings which are too long for the window
        /// </summary>
        /// <param name="str">String string</param>
        /// <param name="chunkSize">Chunksize</param>
        /// <returns>Split strings</returns>
        private IEnumerable<string> SplitStringAtLenght(string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
            {
                yield return str.Substring(i, Math.Min(chunkSize, str.Length - i));
            }
        }
    }
}
