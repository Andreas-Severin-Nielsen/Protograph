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
        public string NewFilepath { get; set; }

        public DateTime[] X { get; set; }
        public double[] Y { get; set; }
        public string Note { get; set; }

        public string NewFileName { get; set; }
        public double Total { get; set; }


        public Chart CreateChart()
        {
            Chart chart = new Chart();
            chart.Size = new System.Drawing.Size(3508, 2480);

            var chartArea = new ChartArea();
            chartArea.AxisX.LabelStyle.Format = XFormat;
            chartArea.AxisY.LabelStyle.Format = YFormat;
            chartArea.AxisX.LabelStyle.Font = new Font("Arial", 26);
            chartArea.AxisY.LabelStyle.Font = new Font("Arial", 30);
            chartArea.AxisX.LabelStyle.Angle = 90;
            chartArea.AxisX.TitleFont = new Font("Arial", 30, FontStyle.Bold);
            chartArea.AxisY.Title = YUnit;
            chartArea.AxisY.TitleFont = new Font("Arial", 30, FontStyle.Bold);
            chartArea.AxisX.LabelAutoFitMinFontSize = 20;
            chartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;
            chartArea.AxisY.LabelAutoFitMinFontSize = 10;
            chartArea.AxisX.LabelAutoFitMaxFontSize = 30;
            chartArea.AxisY.LabelAutoFitMaxFontSize = 30;
            chartArea.AxisX.MajorGrid.LineColor = Color.DarkGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.DarkGray;
            TimeSpan ts = MaxX - MinX;
            if (ts.TotalHours < 30)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
                chartArea.AxisX.Title = "Time";
            }
            else if (ts.TotalDays < 40)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Days;
                chartArea.AxisX.Title = "Dato";
            }
            else if (ts.TotalDays < 370)
            {
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Months;
                chartArea.AxisX.Title = "Måned";
            }
            else chartArea.AxisX.IntervalType = DateTimeIntervalType.Years;
            chartArea.AxisX.Interval = 1;
            chart.ChartAreas.Add(chartArea);

            var series = new Series();
            series.Name = Note;
            series.ChartType = SeriesChartType.Column;
            series.XValueType = ChartValueType.DateTime;
            series.YValueType = ChartValueType.Double;
            series.Font = new Font("Arial", 25);
            series.LabelFormat = "N2";
            series.LabelAngle = 90;
            series.IsValueShownAsLabel = true;
            Title title = new Title();
            title.Font = new Font("Arial", 40);
            title.Text = $"{MinX.ToLongDateString()}, {MinX.ToShortTimeString()} - " +
                $"{MaxX.ToLongDateString()}, {MaxX.ToShortTimeString()}{Environment.NewLine}" +
                $"{Note}{Environment.NewLine}" +
                $"Total forbrug: {Total.ToString(YFormat)}";
            chart.Titles.Add(title);
            chart.Series.Add(series);


            // bind the datapoints
            chart.Series[Note].Points.DataBindXY(X, Y);

            if (25 < ts.TotalDays && ts.TotalDays < 35)
            {
                for (int i = 0; i < X.Length; i++)
                {
                    if (X[i].DayOfWeek == DayOfWeek.Saturday || (X[i].DayOfWeek == DayOfWeek.Sunday))
                    {
                        chart.Series[Note].Points[i].Color = Color.DarkSlateGray;
                    }
                } 
            }

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
                    currentline[1].Trim().Replace('.', ':');
                X[i - 1] = DateTime.Parse(datetime);
                Y[i - 1] = float.Parse(currentline[2].Trim());
                Note = currentline[4];
            }

            Total = 0.0;
            foreach (double d in Y) Total += d;

            MinX = X[0];
            MaxX = X[Count - 1];

            MinY = Y[0];
            MaxY = Y[0];
            foreach (double f in Y)
            {
                if (f < MinY) MinY = f;
                if (MaxY < f) MaxY = f;
            }

            string newPathFormat;
            (XFormat, newPathFormat) = NumberFormatter.DefineDateTimeFormat(X);
            YFormat = NumberFormatter.DefineFloatFormat(Y);
            XUnit = "Dato/tid";
            Note = metadata[4] + ": " + Note;
            NewFileName = NumberFormatter.DefineFilename(MinX, MaxX);
            NewFilepath = MinX.ToString(newPathFormat);
        }
    }
}
