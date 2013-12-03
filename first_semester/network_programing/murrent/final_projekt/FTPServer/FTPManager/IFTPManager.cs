namespace FTPManager
{
    public interface IFTPManager
    {
        /// <summary>
        /// </summary>
        void Start();

        /// <summary> 
        /// </summary>
        event FTPManager.ServerStartedEventHandler ServerStarted;

        /// <summary>
        /// </summary>
        event FTPManager.ProgressUpdateHandler ProgressUpdate;

    }
}