// *******************************************************
// * <copyright file="ItemFinalizedEventArgs.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher.CustomEventArgs
{
    using System;

    using BiOWheelsFileWatcher.Enums;

    /// <summary>
    ///  Class representing the <see cref="ItemFinalizedEventArgs"/>
    /// </summary>
    public class ItemFinalizedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemFinalizedEventArgs"/> class.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        /// <param name="fileAction">
        /// The file action.
        /// </param>
        public ItemFinalizedEventArgs(string fileName, FileAction fileAction)
        {
            this.FileName = fileName;
            this.FileAction = fileAction;
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the items left in queue.
        /// </summary>
        /// <value>
        /// The items left in queue.
        /// </value>
        public int ItemsLeftInQueue { get; set; }

        /// <summary>
        /// Gets or sets the file action.
        /// </summary>
        /// <value>
        /// The file action.
        /// </value>
        public FileAction FileAction { get; set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get
            {
                return "Items left in queue: " + this.ItemsLeftInQueue + " - Item with name " + this.FileName
                       + " and file action " + this.FileAction + " has sucessfully been processed.";
            }
        }
    }
}