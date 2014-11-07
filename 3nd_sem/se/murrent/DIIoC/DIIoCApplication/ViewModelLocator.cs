using DIIoCApplication.Interfaces;
using DIIoCApplication.Logger;
using DIIoCApplication.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace DIIoCApplication
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<ILogger>(()=> new ConsoleLogger(), "ConsoleLogger");
            SimpleIoc.Default.Register<ILogger>(() => new EventLogger(), "EventLogger");
            SimpleIoc.Default.Register<ILogger>(() => new FileLogger(), "FileLogger");
            SimpleIoc.Default.Register(() =>new MainWindowViewModel(SimpleIoc.Default.GetInstance<ILogger>("ConsoleLogger")));
        }

        public MainWindowViewModel MainViewModel { get { return ServiceLocator.Current.GetInstance<MainWindowViewModel>(); } }
    }
}