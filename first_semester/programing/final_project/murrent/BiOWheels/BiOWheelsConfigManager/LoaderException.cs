namespace BiOWheelsConfigManager
{
    using System;

    public class LoaderException
    {
        public LoaderException(string message, Type exceptionType)
        {
            this.Message = message;
            this.ExceptionType = exceptionType;
        }

        public string Message { get; set; }

        public Type ExceptionType { get; set; }
    }
}
