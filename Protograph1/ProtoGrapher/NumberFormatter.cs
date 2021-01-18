using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protograph.ProtoGrapher
{
    class NumberFormatter
    {
        internal static (string, string) DefineDateTimeFormat(DateTime[] x)
        {
            bool minutesChange = false;
            bool hoursChange = false;
            bool daysChange = false;
            bool monthsChange = false;
            bool yearsChange = false;

            for (int i = 1; i < x.Length; i++)
            {
                if (x[i].Minute != x[i - 1].Minute) minutesChange = true;
                if (x[i].Hour != x[i - 1].Hour) hoursChange = true;
                if (x[i].Day != x[i - 1].Day) daysChange = true;
                if (x[i].Month != x[i - 1].Month) monthsChange = true;
                if (x[i].Year != x[i - 1].Year) yearsChange = true;
            }

            string labelFormat = "";
            if (daysChange) labelFormat += "dd";
            if (monthsChange) labelFormat += " MMMM";
            if (yearsChange) labelFormat += " yyyy";
            if (minutesChange || hoursChange) labelFormat += " HH:mm";
            labelFormat.Trim();

            string dsc = @"\\";
            string pathFormat = $@"{dsc}yyyy{dsc}";
            if (hoursChange) pathFormat += $@"MM-MMMM{dsc}dd-dddd{dsc}";
            else if (daysChange) pathFormat += $@"MM-MMMM{dsc}";

            return (labelFormat, pathFormat);
        }

        internal static string DefineFloatFormat(double[] y)
        {
            return "N";
        }

        internal static string DefineFilename(DateTime minX, DateTime maxX)
        {
            string newFileName;
            newFileName = $"{minX.ToString("yyyy-MM-dd-HH-mm")} -- " +
                $"{maxX.ToString("yyyy-MM-dd-HH-mm")}";
            return newFileName;
        }
    }
}
