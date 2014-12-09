// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

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
            SimpleIoc.Default.Register<ILogger, ConsoleLogger>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
        }

        public MainWindowViewModel MainViewModel { get { return ServiceLocator.Current.GetInstance<MainWindowViewModel>(); } }
    }
}