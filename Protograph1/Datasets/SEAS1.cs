using Protograph.ProtoGrapher;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Protograph.Datasets
{
    class SEAS1 : IDataset
    {
        private int Count { get; set; }
        private string XFormat { get; set; }
        private DateTime MaxX { get; set; }
        private DateTime MinX { get; set; }
        private string XUnit { get; set; }
        private string YFormat { get; set; }
        private double MaxY { get; set; }
        private double MinY { get; set; }
        private string YUnit { get; set; }
        private CultureInfo UsedCulture { get; set; }

        public DateTime[] X { get; set; }
        public double[] Y { get; set; }
        public string Note { get; set; }

        public string Name { get; set; }


        public Chart CreateChart()
        {
            Chart chart = new Chart();
            chart.Size = new System.Drawing.Size(3508, 2480);

            var chartArea = new ChartArea();
            chartArea.AxisX.LabelStyle.Format = XFormat;
            chartArea.AxisY.LabelStyle.Format = YFormat;
            chartArea.AxisX.Name = XUnit;
            chartArea.AxisY.Name = YUnit;
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Arial", 30);
            chartArea.AxisY.LabelStyle.Font = new Font("Arial", 30);
            chartArea.AxisX.Interval = 1;
            chart.ChartAreas.Add(chartArea);

            var series = new Series();
            series.Name = Note;
            series.ChartType = SeriesChartType.Column;
            series.XValueType = ChartValueType.DateTime;
            series.YValueType = ChartValueType.Double;
            series.Font = new Font("Arial", 20);
            series.LabelFormat = "N2";
            series.LabelAngle = 90;
            series.IsValueShownAsLabel = true;
            series.IsXValueIndexed = false;
            chart.Series.Add(series);

            // bind the datapoints
            chart.Series[Note].Points.DataBindXY(X, Y);

            // draw!
            chart.Invalidate();
            return chart;
        }

        public void Interprete(string[] filetext)
        {
            UsedCulture = new CultureInfo("da-DK");
            string firstline = filetext[0].Trim();
            string[] metadata = firstline.Split('\t');
            YUnit = metadata[2];
            Count = filetext.Length - 1;
            X = new DateTime[Count];
            Y = new double[Count];

            string[] currentline;
            for (int i = 1; i < filetext.Length; i++)
            {
                currentline = filetext[i].Split('\t');
                string datetime = currentline[0].Trim().Replace('.', '/') + " " + 
                    currentline[1].Trim().Replace('.',':');
                X[i - 1] = DateTime.Parse(datetime);
                Y[i - 1] = float.Parse(currentline[2].Trim());
                Note = currentline[4];
            }

            

            MinX = X[0];
            MaxX = X[Count - 1];

            MinY = Y[0];
            MaxY = Y[0];
            foreach(double f in Y)
            {
                if (f < MinY) MinY = f;
                if (MaxY < f) MaxY = f;
            }

            XFormat = NumberFormatter.DefineDateTimeFormat(X);
            YFormat = NumberFormatter.DefineFloatFormat(Y);
            XUnit = "Dato/tid";
            Note = metadata[4] + ": " + Note;
            Name = $"{MinX.Year}_{MinX.Month}_{MinX.Day}_{MinX.Hour}_{MinX.Minute} - " +
                $"{MaxX.Year}_{MaxX.Month}_{MaxX.Day}_{MaxX.Hour}_{MaxX.Minute}";
        }
    }
}
