using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiOWheelsOpenFileHelper
{
    public class OpenFileHelper
    {
        public OpenFileHelper()
        {
            
        }

        internal void StartProcess()
        {
            Process getHandleProcess = new Process
                                       {
                                           StartInfo =
                                           {
                                               FileName = "/HelperApps/handle.exe",
                                               Arguments = "-a",
                                               UseShellExecute = false,
                                               RedirectStandardOutput = true,
                                               CreateNoWindow = true,
                                               WindowStyle = ProcessWindowStyle.Hidden,
                                               RedirectStandardError = true,
                                           },
                                           EnableRaisingEvents = true
                                       };

            getHandleProcess.OutputDataReceived += GetHandleProcessOutputDataReceived;
            getHandleProcess.ErrorDataReceived += GetHandleProcessErrorDataReceived;
            getHandleProcess.Start();
            getHandleProcess.BeginOutputReadLine();
            getHandleProcess.BeginErrorReadLine();
        }

        protected void GetHandleProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
          
        }

        protected void GetHandleProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            
        }
    }
}
