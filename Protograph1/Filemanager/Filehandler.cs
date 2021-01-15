using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protograph.Filemanager
{
    class Filehandler
    {
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
                File.AppendAllText(logname, lognote);
            }
            catch { };
        }
    }
}
