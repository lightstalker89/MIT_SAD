using System;

namespace BiOWheelsFileWatcher.CustomEventArgs
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateProgressEventArgs : EventArgs
    {
        public UpdateProgressEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Message { get; set; }
    }
}
