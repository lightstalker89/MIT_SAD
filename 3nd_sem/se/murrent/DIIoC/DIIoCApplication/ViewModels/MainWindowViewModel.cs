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

        public MainWindowViewModel(ILogger loggerToSet)
        {
            this.logger = loggerToSet;
            this.logger.Log("Logging from constructor", Enums.LogType.DEBUG);
            this.timer = new Timer(1000);
            this.timer.Elapsed += timer_Elapsed;
            this.StartLoggingButtonCaption = "Start Logging";
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
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

        private RelayCommand startLoggingCommand;
        public RelayCommand StartLoggingCommand
        {
            get { return startLoggingCommand ?? (new RelayCommand(StartPeriodicLogging)); }
        }

        private RelayCommand injectConsoleLoggerCommand;
        public RelayCommand InjectConsoleLoggerCommand
        {
            get { return injectConsoleLoggerCommand ?? (new RelayCommand(InjectConsoleLogger)); }
        }

        private RelayCommand injectFileLoggerCommand;
        public RelayCommand InjectFileLoggerCommand
        {
            get { return injectConsoleLoggerCommand ?? (new RelayCommand(InjectFileLogger)); }
        }

        private RelayCommand injectEventLoggerCommand;
        public RelayCommand InjectEventLoggerCommand
        {
            get { return injectEventLoggerCommand ?? (new RelayCommand(InjectEventLogger)); }
        }

        public void InjectConsoleLogger()
        {
            this.logger = SimpleIoc.Default.GetInstance<ILogger>("ConsoleLogger");
        }

        public void InjectFileLogger()
        {
            this.logger = SimpleIoc.Default.GetInstance<ILogger>("FileLogger");
        }

        public void InjectEventLogger()
        {
            this.logger = SimpleIoc.Default.GetInstance<ILogger>("EventLogger");
        }
    }
}
