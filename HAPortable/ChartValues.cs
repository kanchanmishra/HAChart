
using System;
using System.Collections.Generic;


namespace HAPortable
{
    public class ChartValues
    {
        
        //public List<string> LineChartBarcolors = new List<string>();// { "#E28200", "#2aa467", "#e28200", "#d5503b" };
        //public List<string> LineChartBarText = new List<string>(); //{ "UNDERWEIGHT", "NORMAL", "OVERWEIGHT", "OBESE" };
        //public List<string> LineChartXValues = new List<string>();// { "10", "25", "17", "250" };
        //public List<string> LineChartYValues = new List<string>();//{ "18.5", "25", "30", "40" };
        //public List<string> LineChartLegends = new List<string>();// { "1999", "2000", "2010", "2012" };

        private int LineChartCircleRadius = 30;
        private int LineChartPercentage = 0;
       
        public ChartData SetChartValues(string chartType)
        {
            HAJsonManager hAJsonManager = new HAJsonManager();
            HAReport appointmentFromJson = hAJsonManager.AppointmentFromJson();
            ChartData chartData = new ChartData();
            switch (chartType)
            {
                case "bmi":
                    // BMI chart Values
                    var historic = appointmentFromJson.graphs.body_composition.bmi.historic;
                    chartData = GetData(historic);
                    break;
                case "body_fat":
                    // BMI chart Values
                    historic = appointmentFromJson.graphs.body_composition.body_fat.historic;
                    chartData = GetData(historic);
                    break;
            }

            return chartData;

        }

        private ChartData GetData(Historic historic)
        {
            ChartData chartData = new ChartData();
            var yRanges = historic.ranges.y;
            // get first min value and all the max values for LineChartYValues
            System.Globalization.NumberFormatInfo _nFormat = new System.Globalization.NumberFormatInfo();
            _nFormat.NumberDecimalDigits = 2;

            if (yRanges.Count > 0)
            {
                chartData.BarColors = new List<string>();
                chartData.BarText = new List<string>();
                chartData.YValues = new List<string>();
                chartData.BaseValue = float.Parse(yRanges[0].min);
                for (int i = 0; i < yRanges.Count; i++)
                {
                    chartData.BarColors.Add(string.Format("#{0}", yRanges[i].color));
                    chartData.BarText.Add(yRanges[i].name);
                    chartData.YValues.Add((decimal.Parse(yRanges[i].max)).ToString("##.#"));
                }
            }
            // data
            
            var data = historic.data;
            if (data.Count > 0)
            {
                chartData.XValues = new List<string>();
                for (int i = 0; i < data.Count; i++)
                {
                    chartData.XValues.Add(data[i].y);
                }
            }

            // lagend 
            var lagend = historic.legend;
            if (lagend.Count > 0)
            {
                chartData.Legends = new List<string>();
                for (int i = 0; i < lagend.Count; i++)
                {
                    chartData.Legends.Add(lagend[i]);
                }
            }

            chartData.Radius = LineChartCircleRadius;
            chartData.Percentage = LineChartPercentage;
            
            return chartData;
        }

        

        
    }
}
