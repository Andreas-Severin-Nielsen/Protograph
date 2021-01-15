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
        private string originalFilePath;  // original file location and path as string
        private string ArchiveFileName; // Original file when archived
        private string ChartFileName; // Chart file name
        private IDataset dataset; // dataset containing data
        private Chart chart;
        
        public GraphCreator(string filepath)
        {
            lognote = filepath;
            originalFilePath = filepath;

        }

        /// <summary>
        /// Used for thread jobs
        /// </summary>
        /// <param name="callback"></param>
        public void ThreadPoolJob(object callback)
        {
            CreateGraph();
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
                string[] filetext = filehandler.OpenFile(originalFilePath);

                // 1-2: Interprete data
                DataInterpreter dataInterpreter = new DataInterpreter();
                dataset = DataInterpreter.Interprete(filetext);

                // 3: Import data to chart
                chart = dataset.CreateChart();

                // 5: Chart export
                chart.SaveImage(dataset.Name + ".png", ChartImageFormat.Png);

                lognote += " Processed OK.";
            }
            catch(Exception e)
            {
                lognote += e.Message;
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
