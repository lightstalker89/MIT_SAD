// *******************************************************
// * <copyright file="EventInformationMapping.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher
{
    using System.Timers;

    using BiOWheelsFileWatcher.CustomEventArgs;

    /// <summary>
    /// </summary>
    public class EventInformationMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventInformationMapping"/> class.
        /// </summary>
        /// <param name="timer">
        /// The timer.
        /// </param>
        /// <param name="eventArgs">
        /// The <see cref="CustomFileSystemEventArgs"/> instance containing the event data.
        /// </param>
        /// <param name="fileAction">
        /// The file action.
        /// </param>
        public EventInformationMapping(Timer timer, CustomFileSystemEventArgs eventArgs, FileAction fileAction)
        {
            this.Timer = timer;
            this.CustomFileSystemEventArgs = eventArgs;
            this.FileAction = fileAction;
        }

        /// <summary>
        /// Gets or sets the timer.
        /// </summary>
        /// <value>
        /// The timer.
        /// </value>
        public Timer Timer { get; set; }

        /// <summary>
        /// Gets or sets the custom file system event arguments.
        /// </summary>
        /// <value>
        /// The custom file system event arguments.
        /// </value>
        public CustomFileSystemEventArgs CustomFileSystemEventArgs { get; set; }

        /// <summary>
        /// Gets or sets the file action.
        /// </summary>
        /// <value>
        /// The file action.
        /// </value>
        public FileAction FileAction { get; set; }
    }
}