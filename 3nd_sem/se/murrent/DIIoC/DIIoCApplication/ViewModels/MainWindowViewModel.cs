// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

using System;
using System.Collections.ObjectModel;
using System.Timers;
using DIIoCApplication.Interfaces;
using DIIoCApplication.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;

namespace DIIoCApplication.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Timer timer;

        public MainWindowViewModel()
        {
            this.InitTimer();
            this.StartLoggingButtonCaption = "Start Logging";
        }

        protected void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            SimpleIoc.Default.GetInstance<ILogger>().Log("Periodic logging from timer", Enums.LogType.INFO);
        }

        public void StartPeriodicLogging()
        {
            if (timer.Enabled)
            {
                timer.Stop();
                this.StartLoggingButtonCaption = "Start Logging";
            }
            else
            {
                timer.Start();
                this.StartLoggingButtonCaption = "Stop Logging";
            }
        }

        private string startLoggingButtonCaption;
        public string StartLoggingButtonCaption
        {
            get { return startLoggingButtonCaption; }
            set
            {
                startLoggingButtonCaption = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<string> loggerChanges = new ObservableCollection<string>();
        public ObservableCollection<string> LoggerChanges
        {
            get { return this.loggerChanges; }
            set
            {
                this.loggerChanges = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand startLoggingCommand;
        public RelayCommand StartLoggingCommand
        {
            get { return startLoggingCommand ?? (startLoggingCommand = new RelayCommand(StartPeriodicLogging)); }
        }

        private void InitTimer()
        {
            this.timer = new Timer(1000);
            this.timer.Elapsed += TimerElapsed;
        }
    }
}
