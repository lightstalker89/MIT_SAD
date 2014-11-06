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
            SimpleIoc.Default.Register<MainWindowViewModel>();
        }


        public MainWindowViewModel MainViewModel { get { return ServiceLocator.Current.GetInstance<MainWindowViewModel>(); } }
    }
}
