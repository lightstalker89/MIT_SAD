// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

using System;
using System.Timers;
using System.Windows;
using DIIoCApplication.Interfaces;
using DIIoCApplication.Logger;
using GalaSoft.MvvmLight.Ioc;

namespace DIIoCApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private int intervallCount = 0;
        private Timer timer;
        public App()
        {
            this.InitTimer();
            this.timer.Start();
        }

        private void CheckTime()
        {
            SimpleIoc.Default.Unregister<ILogger>();
            switch (intervallCount % 3)
            {
                case 0:
                    SimpleIoc.Default.Register<ILogger, FileLogger>();
                    break;

                case 1:
                    SimpleIoc.Default.Register<ILogger, EventLogger>();
                    break;

                case 2:
                    SimpleIoc.Default.Register<ILogger, ConsoleLogger>();
                    break;
            }
        }

        private void InitTimer()
        {
            this.timer = new Timer(5000);
            this.timer.Elapsed += timer_Elapsed;
        }

        protected void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            intervallCount++;
            CheckTime();
        }
    }
}
