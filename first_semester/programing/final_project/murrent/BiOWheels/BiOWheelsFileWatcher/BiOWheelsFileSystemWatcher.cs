// *******************************************************
// * <copyright file="BiOWheelsFileSystemWatcher.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System;
using System.Linq;
using System.Timers;
using BiOWheelsFileWatcher.CustomEventArgs;

namespace BiOWheelsFileWatcher
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// </summary>
    public class BiOWheelsFileSystemWatcher : FileSystemWatcher
    {
        #region Private Fields

        /// <summary>
        /// Field representing the timers and mapping information for each file
        /// </summary>
        private Dictionary<string, EventInformationMapping> eventInformationMappings;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BiOWheelsFileSystemWatcher"/> class
        /// </summary>
        /// <param name="path">
        /// </param>
        public BiOWheelsFileSystemWatcher(string path)
            : base(path)
        {
            this.Changed += this.BiOWheelsFileSystemWatcherChanged;
            this.Created += this.BiOWheelsFileSystemWatcherCreated;
            this.Deleted += this.BiOWheelsFileSystemWatcherDeleted;
            this.Renamed += this.BiOWheelsFileSystemWatcherRenamed;

            this.EventInformationMappings = new Dictionary<string, EventInformationMapping>();
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
        /// Gets or sets the timer and mapping information for each file
        /// </summary>
        internal Dictionary<string, EventInformationMapping> EventInformationMappings
        {
            get
            {
                return this.eventInformationMappings;
            }

            set
            {
                this.eventInformationMappings = value;
            }
        }

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
            Timer timer;

            if (this.EventInformationMappings.ContainsKey(e.FullPath))
            {
                EventInformationMapping eventInformationMapping = this.EventInformationMappings[e.FullPath];
                timer = eventInformationMapping.Timer;
            }
            else
            {
                CustomFileSystemEventArgs customEventArgs = new CustomFileSystemEventArgs(e.FullPath, e.Name)
                      {
                          CompareInBlocks = this.MustCompareFileInBlocks(e.FullPath)
                      };

                timer = new Timer { Interval = 200, AutoReset = false };
                this.EventInformationMappings.Add(e.FullPath, new EventInformationMapping(timer, customEventArgs, FileAction.COPY));
                timer.Elapsed += TimerElapsed;
            }

            timer.Stop();
            timer.Start();
        }

        ///
        protected void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Timer timer = sender as Timer;

            EventInformationMapping mapping = this.EventInformationMappings.First(p => p.Value.Timer == timer).Value;

            if (mapping != null)
            {
                if (mapping.FileAction == FileAction.CREATE)
                {
                    this.ObjectCreated(this, mapping.CustomFileSystemEventArgs);
                }
                else
                {
                    this.ObjectChanged(this, mapping.CustomFileSystemEventArgs);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BiOWheelsFileSystemWatcherRenamed(object sender, RenamedEventArgs e)
        {
            CustomRenamedEventArgs customEventArgs = new CustomRenamedEventArgs(e.FullPath, e.Name, e.OldName, e.OldFullPath);

            this.OnObjectRenamed(customEventArgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BiOWheelsFileSystemWatcherDeleted(object sender, FileSystemEventArgs e)
        {
            CustomFileSystemEventArgs customEventArgs = new CustomFileSystemEventArgs(e.FullPath, e.Name);

            this.OnObjectDeleted(customEventArgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BiOWheelsFileSystemWatcherCreated(object sender, FileSystemEventArgs e)
        {
            Timer timer;

            if (this.EventInformationMappings.ContainsKey(e.FullPath))
            {
                EventInformationMapping eventInformationMapping = this.EventInformationMappings[e.FullPath];
                timer = eventInformationMapping.Timer;
            }
            else
            {
                CustomFileSystemEventArgs customEventArgs = new CustomFileSystemEventArgs(e.FullPath, e.Name)
                {
                    CompareInBlocks = false
                };

                timer = new Timer() { Interval = 200, AutoReset = false };
                this.EventInformationMappings.Add(e.FullPath, new EventInformationMapping(timer, customEventArgs, FileAction.CREATE));
                timer.Elapsed += TimerElapsed;
            }

            timer.Stop();
            timer.Start();
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnObjectRenamed(CustomRenamedEventArgs e)
        {
            if (this.ObjectRenamed != null)
            {
                this.ObjectRenamed(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnObjectDeleted(CustomFileSystemEventArgs e)
        {
            if (this.ObjectDeleted != null)
            {
                this.ObjectDeleted(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnObjectCreated(CustomFileSystemEventArgs e)
        {
            if (this.ObjectCreated != null)
            {
                this.ObjectCreated(this, e);
            }
        }
        #endregion

        /// <summary>
        /// Checks if the file must be compared in blocks
        /// </summary>
        /// <param name="file">
        /// Full qualified file name
        /// </param>
        /// <returns>
        /// A value whether the file must be compared in blocks or not
        /// </returns>
        private bool MustCompareFileInBlocks(string file)
        {
            double length;

            using (Stream actualFileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                length = Math.Round((actualFileStream.Length / 1024f) / 1024f, 2, MidpointRounding.AwayFromZero);
            }

            if (length > this.BlockCompareFileSizeInMB)
            {
                return true;
            }

            return false;
        }
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

        /// <summary>
        /// Gets or sets the block size in MB
        /// </summary>
        public long BlockCompareFileSizeInMB { get; set; }

        #endregion
    }
}