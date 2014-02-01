//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp.EntryPoint
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Prototyp.Log;
    using Prototyp.View;

    /// <summary>
    /// Entry point of the program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry function of the program
        /// </summary>
        /// <param name="args">Startup arguments</param>
        public static void Main(string[] args)
        {
            try
            {
                Console.SetBufferSize(700, 700);
                Console.SetWindowSize(150, 65);
            }
            catch (Exception e)
            {
                Console.SetBufferSize(200, 200);
                Console.SetWindowPosition(50, 40);

                Console.WriteLine("Program: Error on program start up {0}", e.Message);
            }

            WindowManager winManager = new WindowManager();
            SyncManager syncManager = new SyncManager(winManager);
            syncManager.Init();
            syncManager.DoWork();
        }
    }
}
