using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp4
{
    public class Timing
    {
        TimeSpan startTime;
        TimeSpan stopTime;
        TimeSpan duration;

        public Timing()
        {
            startTime = new TimeSpan(0);
            duration = new TimeSpan(0);
        }

        public void StopTime()
        {
            stopTime = Process.GetCurrentProcess().TotalProcessorTime;
            duration = stopTime.Subtract(startTime);
        }

        public void StartTime()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            startTime = Process.GetCurrentProcess().TotalProcessorTime;
        }

        public TimeSpan Result()
        {
            return duration;
        }
    }
}
