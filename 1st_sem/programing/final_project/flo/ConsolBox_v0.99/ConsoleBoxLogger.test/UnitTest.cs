using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;


namespace ConsoleBoxLogger.test
{
    [TestFixture]
    public class UnitTest
    {
        ILogger log = new Logger();
        [TestCase]
        public void TestLogger()
        {
            log.SetFileSize<FileLogger>(10);
            for (int i = 0; i < 20000; i++)
            {
                //log.Log(i + " | ", MessageType.Error);
                Parallel.Invoke(() =>
                {
                    log.Log(i + " | ", MessageType.Error);
                },  // close first Action

                             () =>
                             {
                                 log.Log(i + " | ", MessageType.Error);
                             }, //close second Action

                             () =>
                             {
                                 log.Log(i + " | ", MessageType.Error);
                             } //close third Action
                         ); //close parallel.invoke*/

            }

        }

        private void Start(int i)
        {

        }
    }
}
