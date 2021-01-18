using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Protograph.Filemanager
{
    class Filehandler
    {
        private static readonly object originalFileWriterLock = new object();
        private static readonly object newPathLock = new object();
        private static readonly object chartFileWriterLock = new object();
        private static readonly object logWriterLock = new object();

        public string[] OpenFile(string originalFilePath)
        {
            if (!File.Exists(originalFilePath))
            {
                throw new Exception("File not found: " + originalFilePath);
            }
            string[] filetext = File.ReadAllLines(originalFilePath);
            if (filetext.Length < 2) throw new Exception("Not enough data i file.");
            return filetext;
        }

        public void Log(string logname, string lognote)
        {
            try
            {
                lock (logWriterLock)
                {
                    File.AppendAllText(logname, lognote); 
                }
            }
            catch (Exception e) { throw e; };
        }

        internal void Archive(string originalFile, string newFilepath)
        {
            try
            {
                if (!File.Exists(originalFile))
                {
                    throw new Exception("Original file doesn't exist: " + originalFile);
                }

                string newFile = newFilepath + Path.GetExtension(originalFile);

                lock (originalFileWriterLock)
                {
                    if (File.Exists(newFile)) File.Delete(newFile);

                    // Move the file.
                    File.Move(originalFile, newFile); 
                }

                if (File.Exists(originalFile))
                {
                    throw new Exception("error moving: file still exists after moving. " + originalFile);
                }
            }
            catch (Exception e) { throw e; }
        }

        internal void CreatePath(string newpath)
        {
            lock(newPathLock)
            {
                if(!Directory.Exists(newpath))
                {
                    Directory.CreateDirectory(newpath);
                }
            }
        }

        internal void saveChart(Chart chart, string newfile)
        {
            lock (chartFileWriterLock)
            {
                chart.SaveImage(newfile + ".png", ChartImageFormat.Png); 
            }
        }
    }
}
