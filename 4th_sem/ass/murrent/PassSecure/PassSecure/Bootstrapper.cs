namespace PassSecure
{
    using PassSecure.Data;
    using PassSecure.Service;

    public class Bootstrapper
    {
        public Bootstrapper()
        {
            SimpleContainer.Register(new DataReader());
            SimpleContainer.Register(new DataStore());
        }
    }
}
