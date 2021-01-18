using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Protograph.ProtoGrapher
{
    interface IDataset
    {
        string NewFileName { get; set; }
        string NewFilepath { get; set; }

        Chart CreateChart();

        void Interprete(string[] filetext);
    }
}
