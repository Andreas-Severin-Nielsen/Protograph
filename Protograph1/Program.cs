using Protograph.ProtoGrapher;
using Protograph.ThreadControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Protograph
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPoolManager poolManager = new ThreadPoolManager();
            poolManager.ConFigurePoolByNumberOfCPUs();
            foreach(string s in args)
            {
                IThreadPoolWorker pg = new GraphCreator(s);
                poolManager.AddWorker(pg);
            }
            
            poolManager.Wait();
            System.Environment.Exit(0);
        }
    }
}
