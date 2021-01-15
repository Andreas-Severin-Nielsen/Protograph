using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Protograph.ThreadControl
{
    class ThreadPoolManager
    {
        List<AutoResetEvent> autoEvents;

        public ThreadPoolManager()
        {
            autoEvents = new List<AutoResetEvent>();
        }

        internal void ConFigurePoolByNumberOfCPUs()
        {
            int cpuCount = Environment.ProcessorCount;
            ThreadPool.SetMinThreads(cpuCount, cpuCount + 2);
            ThreadPool.SetMaxThreads(cpuCount, cpuCount + 2);
        }

        internal void Wait()
        {
            foreach(AutoResetEvent arv in autoEvents)
            {
                arv.WaitOne();
            }
        }

        internal void AddWorker(IThreadPoolWorker pg)
        {
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            autoEvents.Add(autoEvent);
            ThreadPool.QueueUserWorkItem(new WaitCallback(pg.ThreadPoolJob), autoEvent);
        }
    }
}
