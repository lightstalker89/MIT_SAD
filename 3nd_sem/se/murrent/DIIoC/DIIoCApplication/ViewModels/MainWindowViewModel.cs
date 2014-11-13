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
        private ILogger logger;
        private ILogger fileLoger;
        private ILogger consoleLogger;
        private ILogger eventLogger;

        public MainWindowViewModel(ILogger loggerToSet)
        {
            this.logger = loggerToSet;
            this.logger.Log("Logging from constructor", Enums.LogType.DEBUG);
            this.InitTimer();
            this.StartLoggingButtonCaption = "Start Logging";
            this.InitLogger();
            ;
        }

        protected void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            logger.Log("Periodic logging from timer", Enums.LogType.INFO);
        }

        public void SetLogger(ILogger loggerToSet)
        {
            this.logger = loggerToSet;
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

        private RelayCommand injectConsoleLoggerCommand;
        public RelayCommand InjectConsoleLoggerCommand
        {
            get { return injectConsoleLoggerCommand ?? ( injectConsoleLoggerCommand = new RelayCommand(InjectConsoleLogger)); }
        }

        private RelayCommand injectFileLoggerCommand;
        public RelayCommand InjectFileLoggerCommand
        {
            get { return injectFileLoggerCommand ?? (injectFileLoggerCommand = new RelayCommand(InjectFileLogger)); }
        }

        private RelayCommand injectEventLoggerCommand;
        public RelayCommand InjectEventLoggerCommand
        {
            get { return injectEventLoggerCommand ?? (injectEventLoggerCommand = new RelayCommand(InjectEventLogger)); }
        }

        private void InjectConsoleLogger()
        {
            this.logger = consoleLogger;
            this.LoggerChanges.Add(DateTime.Now + " - " + "Injected ConsoleLogger");
        }

        private void InjectFileLogger()
        {
            this.logger = fileLoger;
            this.LoggerChanges.Add(DateTime.Now + " - " + "Injected FileLogger");
        }

        private void InjectEventLogger()
        {
            this.logger = eventLogger;
            this.LoggerChanges.Add(DateTime.Now + " - " + "Injected EventLogger");
        }

        private void InitLogger()
        {
            this.consoleLogger = SimpleIoc.Default.GetInstance<ILogger>("ConsoleLogger");
            this.fileLoger = SimpleIoc.Default.GetInstance<ILogger>("FileLogger");
            this.eventLogger = SimpleIoc.Default.GetInstance<ILogger>("EventLogger");
        }

        private void InitTimer()
        {
            this.timer = new Timer(1000);
            this.timer.Elapsed += TimerElapsed;
        }
    }
}
