namespace FTPManager
{
    public class FTPManagerFactory
    {
        public static IFTPManager CreateFTPManager()
        {
            return new FTPManager();
        }
    }
}
