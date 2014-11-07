using System.Timers;
using System.Windows.Media.Animation;
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
            timer.Start();
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

        public void InjectConsoleLogger()
        {
            SimpleIoc.Default.GetInstance<ILogger>("ConsoleLogger");
        }

        public void InjectFileLogger()
        {
            SimpleIoc.Default.GetInstance<ILogger>("FileLogger");
        }

        public void InjectEventLogger()
        {
            SimpleIoc.Default.GetInstance<ILogger>("EventLogger");
        }
    }
}
