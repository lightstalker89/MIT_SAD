using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public class Watch
    {
        public List<WatchSource> WatchSourceList { get; private set; }
        private DataLogger lg { get; set; }
        public QueueWorker QueueWorker { get; private set; }

        public Watch(DataLogger logger, QueueWorker queue)
        {
            this.WatchSourceList = new List<WatchSource>();
            this.lg = logger;
            this.QueueWorker = queue;
        }


        public void AddNewWatchSource(Source source)
        {
            SourceWatcher sourceWatcher = new SourceWatcher(source);
            WatchSource watchSource = new WatchSource(sourceWatcher);
            
            watchSource.OnNewCommingJob +=  new WatchSource.NewJobDelegate(watchSource_OnNewCommingInJob)  ;
            //watchSource.DoSomething();

            this.WatchSourceList.Add(watchSource);
        }

        void watchSource_OnNewCommingInJob(JobTask value)
        {
            this.QueueWorker.AddJob(value);

            //Console.Clear();
            //foreach (JobTask item in this.QueueWorker.QueueList)
            //{
            //    Console.WriteLine(item);
            //}
        }

        public void EndAllToWatch()
        {
            foreach (WatchSource item in WatchSourceList)
            {
                item.EndWatch();
            }
        }


    }
}
