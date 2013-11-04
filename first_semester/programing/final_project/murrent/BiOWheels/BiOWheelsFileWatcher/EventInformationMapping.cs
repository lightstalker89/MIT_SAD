using System.Timers;
using BiOWheelsFileWatcher.CustomEventArgs;

namespace BiOWheelsFileWatcher
{
    public class EventInformationMapping
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="eventArgs"></param>
        /// <param name="fileAction"></param>
        public EventInformationMapping(Timer timer, CustomFileSystemEventArgs eventArgs, FileAction fileAction)
        {
            this.Timer = timer;
            this.CustomFileSystemEventArgs = eventArgs;
            this.FileAction = fileAction;
        }

        /// <summary>
        /// 
        /// </summary>
        public Timer Timer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CustomFileSystemEventArgs CustomFileSystemEventArgs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FileAction FileAction { get; set; }
    }
}
