using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public class QueueWorker
    {
        public List<JobTask> QueueList { get; set; }

        public QueueWorker()
        {
            QueueList = new List<JobTask>();
        }

        public void AddJob(JobTask newJob)
        {
            QueueList.Add(newJob);
        }

        public JobTask GetNextJob()
        {
            if (QueueList.Count > 0)
            {
                JobTask job = QueueList.First();
                QueueList.RemoveAt(0);
                return job;
            }
            else
            {
                return null;
            }
        }

    }
}
