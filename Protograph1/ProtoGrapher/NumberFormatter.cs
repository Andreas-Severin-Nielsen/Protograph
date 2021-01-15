using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protograph.ProtoGrapher
{
    class NumberFormatter
    {
        internal static string DefineDateTimeFormat(DateTime[] x)
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

            string format = "";
            if (daysChange) format += "dd";
            if (monthsChange) format += " MMMM";
            if (yearsChange) format += " yyyy";
            if (minutesChange || hoursChange) format += " HH:mm";
            format.Trim();

            return format;
        }

        internal static string DefineFloatFormat(double[] y)
        {
            return "N";
        }
    }
}
