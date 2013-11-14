// *******************************************************
// * <copyright file="BiOWheelsFileSystemWatcher.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher
{
    using System.Collections.Generic;
    using System.IO;

    using BiOWheelsFileWatcher.CustomEventArgs;

    /// <summary>
    /// </summary>
    public class BiOWheelsFileSystemWatcher : FileSystemWatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiOWheelsFileSystemWatcher"/> class
        /// </summary>
        /// <param name="path">
        /// The directory to monitor, in standard or Universal Naming Convention (UNC) notation.
        /// </param>
        internal BiOWheelsFileSystemWatcher(string path)
            : base(path)
        {
            this.Created += this.BiOWheelsFileSystemWatcherCreated;
            this.Deleted += this.BiOWheelsFileSystemWatcherDeleted;
            this.Renamed += this.BiOWheelsFileSystemWatcherRenamed;
            //this.Changed += this.BiOWheelsFileSystemWatcherChanged;
        }

        #region Delegates

        /// <summary>
        /// Delegate for the <see cref="ObjectChangedHandler"/> event
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="data">Data from the event</param>
        public delegate void ObjectChangedHandler(object sender, CustomFileSystemEventArgs data);

        /// <summary>
        /// Delegate for the <see cref="ObjectRenamedHandler"/> event
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="data">Data from the event</param>
        public delegate void ObjectRenamedHandler(object sender, CustomRenamedEventArgs data);

        /// <summary>
        /// Delegate for the <see cref="ObjectDeletedHandler"/> event
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="data">Data from the event</param>
        public delegate void ObjectDeletedHandler(object sender, CustomFileSystemEventArgs data);

        /// <summary>
        /// Delegate for the <see cref="ObjectCreatedHandler"/> event
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="data">Data from the event</param>
        public delegate void ObjectCreatedHandler(object sender, CustomFileSystemEventArgs data);

        #endregion

        #region Event Handler

        /// <summary>
        /// Event handler for an object that changed
        /// </summary>
        public event ObjectChangedHandler ObjectChanged;

        /// <summary>
        /// Event handler for an object that has been renamed
        /// </summary>
        public event ObjectRenamedHandler ObjectRenamed;

        /// <summary>
        /// Event handler for an object that has been deleted
        /// </summary>
        public event ObjectDeletedHandler ObjectDeleted;

        /// <summary>
        /// Event handler for an object that has been created
        /// </summary>
        public event ObjectCreatedHandler ObjectCreated;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the destination folder
        /// </summary>
        public List<string> Destinations { get; set; }

        /// <summary>
        /// Gets or sets the excluded directories
        /// </summary>
        public List<string> ExcludedDirectories { get; set; }

        #endregion

        #region Methods

        #region Event Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BiOWheelsFileSystemWatcherChanged(object sender, FileSystemEventArgs e)
        {
            CustomFileSystemEventArgs customEventArgs = new CustomFileSystemEventArgs(e.FullPath, e.Name);

            //this.OnObjectChanged(customEventArgs);

        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected void BiOWheelsFileSystemWatcherRenamed(object sender, RenamedEventArgs e)
        {
            CustomRenamedEventArgs customEventArgs = new CustomRenamedEventArgs(
                e.FullPath, e.Name, e.OldName, e.OldFullPath);

            this.OnObjectRenamed(customEventArgs);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected void BiOWheelsFileSystemWatcherDeleted(object sender, FileSystemEventArgs e)
        {
            CustomFileSystemEventArgs customEventArgs = new CustomFileSystemEventArgs(e.FullPath, e.Name);

            this.OnObjectDeleted(customEventArgs);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected void BiOWheelsFileSystemWatcherCreated(object sender, FileSystemEventArgs e)
        {
            CustomFileSystemEventArgs customEventArgs = new CustomFileSystemEventArgs(e.FullPath, e.Name);

            this.OnObjectCreated(customEventArgs);
        }

        /// <summary>
        ///  Raises the <see cref="OnObjectChanged"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnObjectChanged(CustomFileSystemEventArgs e)
        {
            if (this.ObjectChanged != null)
            {
                this.ObjectChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ObjectRenamed"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="CustomRenamedEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnObjectRenamed(CustomRenamedEventArgs e)
        {
            if (this.ObjectRenamed != null)
            {
                this.ObjectRenamed(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ObjectDeleted"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="CustomFileSystemEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnObjectDeleted(CustomFileSystemEventArgs e)
        {
            if (this.ObjectDeleted != null)
            {
                this.ObjectDeleted(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ObjectCreated"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="CustomFileSystemEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnObjectCreated(CustomFileSystemEventArgs e)
        {
            if (this.ObjectCreated != null)
            {
                this.ObjectCreated(this, e);
            }
        }

        #endregion

        #endregion
    }
}