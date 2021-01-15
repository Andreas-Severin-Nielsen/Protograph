using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protograph.ThreadControl
{
    interface IThreadPoolWorker
    {
        void ThreadPoolJob(object callback);
    }
}
