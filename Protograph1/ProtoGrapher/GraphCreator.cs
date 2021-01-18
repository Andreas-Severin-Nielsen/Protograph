using Protograph.ThreadControl;
using Protograph.Filemanager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Protograph.ProtoGrapher
{
    class GraphCreator : IThreadPoolWorker
    {
        private string lognote; // process result log note
        private string originalFile;  // original file location and path as string
        private IDataset dataset; // dataset containing data
        private Chart chart;
        
        public GraphCreator(string filepath)
        {
            lognote = filepath;
            originalFile = filepath;
        }

        /// <summary>
        /// Used for thread jobs
        /// </summary>
        /// <param name="callback"></param>
        public void ThreadPoolJob(object callback)
        {
            CreateGraph();
            ((AutoResetEvent)callback).Set();
        }

        public void CreateGraph()
        {
            /// DATA
            /// 0: Open file
            /// 1: Define type
            /// 2: Interprete data
            /// CHART
            /// 3: Import data to chart
            /// 4: Configure chart
            /// EXPORT TO FILE
            /// 5: Chart export
            /// FINISH
            /// 6: Cleanup files
            /// 7: Write log
            /// 

            Filehandler filehandler = new Filehandler();
            try
            {
                // 0: Open file
                string[] filetext = filehandler.OpenFile(originalFile);

                // 1-2: Interprete data
                DataInterpreter dataInterpreter = new DataInterpreter();
                dataset = DataInterpreter.Interprete(filetext);

                // 3: Import data to chart
                chart = dataset.CreateChart();

                // 5: Chart export
                string newfile = Path.GetDirectoryName(originalFile);
                newfile += dataset.NewFilepath;
                filehandler.CreatePath(newfile);
                newfile += dataset.NewFileName;
                filehandler.saveChart(chart, newfile);
                filehandler.Archive(originalFile, newfile);
                lognote += " Processed OK: " + newfile;
            }
            catch(Exception e)
            {
                lognote += " "+e.Message + " -*- "+e.StackTrace;
            }
            finally
            {
                lognote += Environment.NewLine;
                DateTime dt = DateTime.Now;
                string logname = $"{dt.Year}-{dt.Month}-{dt.Day}.log";
                filehandler.Log(logname, lognote);
                
            }
            

        }
    }
}
