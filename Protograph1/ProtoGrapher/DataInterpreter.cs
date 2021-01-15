using Protograph.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protograph.ProtoGrapher
{
    class DataInterpreter
    {
        internal static IDataset Interprete(string[] filetext)
        {
            string firstline = filetext[0].Trim();
            IDataset dataset;

            // 1: Define type
            switch (firstline)
            {
                case "Dato	Tidspunkt	kWh	kg CO2	Adresse":
                    dataset = new SEAS1();
                    break;
                default:
                    throw new Exception("File not recognized for interpretion.");
            }

            // 2: Interprete data
            dataset.Interprete(filetext);
            return dataset;
        }
    }
}
