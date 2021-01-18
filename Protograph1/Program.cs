using Protograph.ProtoGrapher;
using Protograph.ThreadControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Protograph
{
    class Program
    {
        [STAThread] // needed to show open folder dialogue
        static void Main(string[] args)
        {
            string[] files = args;
            ThreadPoolManager poolManager = new ThreadPoolManager();
            poolManager.ConFigurePoolByNumberOfCPUs();
            if (files.Length == 0) files = getFiles();
            foreach(string s in files)
            {
                IThreadPoolWorker pg = new GraphCreator(s);
                poolManager.AddWorker(pg);
            }
            
            poolManager.Wait();
            System.Environment.Exit(0);
        }

        private static string[] getFiles()
        {
            string[] files = new string[0];
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    files = Directory.GetFiles(fbd.SelectedPath, "*.csv");
                }
            }

            return files;
        }
    }
}
