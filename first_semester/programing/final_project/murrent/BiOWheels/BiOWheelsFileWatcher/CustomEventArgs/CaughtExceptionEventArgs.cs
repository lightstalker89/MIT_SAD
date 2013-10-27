using System;

namespace BiOWheelsFileWatcher.CustomEventArgs
{
    public class CaughtExceptionEventArgs : EventArgs 
    {
        public CaughtExceptionEventArgs(Type exceptionType, string exceptionMessage)
        {
            this.ExceptionType = exceptionType;
            this.ExceptionMessage = exceptionMessage;
        }

        public Type ExceptionType { get; set; }

        public string ExceptionMessage { get; set; }
    }
}
