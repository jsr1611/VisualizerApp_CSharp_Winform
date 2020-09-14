using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Brushes = System.Windows.Media.Brushes;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;

namespace VisualizerApp_3
{
    
    public partial class TestForm : Form
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public List<string[]> GraphData { get; set; }
        public Series TempSeries = new Series("Temperature");
        public List<double> yValue = new List<double>();
        public List<DateTime> xValue = new List<DateTime>();

        public TestForm(List<string[]> graphData, string startTime, string endTime)
        {
            InitializeComponent();
            this.AutoScroll = true;

            Console.WriteLine("\n\nNumber of elements: {0}", graphData.Count);

            TempSeries.IsValueShownAsLabel = true;
            TempSeries.ChartType = SeriesChartType.Line;
            chart1.Series.Add(TempSeries);
            this.chart1.Palette = ChartColorPalette.EarthTones;
            this.chart1.ChartAreas[0].AxisY.Minimum = -5;
            this.chart1.ChartAreas[0].AxisY.Maximum = 45;
            int j = 0;
            int arrtotal = graphData.Count / 10;

            for (int i=0; i< 10; i++) {
                if(graphData.Count > j) {
                    xValue.Add(Convert.ToDateTime(graphData[j][5]));
                    yValue.Add(Math.Round(Convert.ToSingle(graphData[j][1]), 2));
                    Console.WriteLine("{0} {1} ", graphData[j][1], graphData[j][5]);
                    j += (int)graphData.Count / 10;
                }
                else
                {
                    if (j == graphData.Count)
                    {
                        xValue.Add(Convert.ToDateTime(graphData[graphData.Count - 1][5]));
                        yValue.Add(Math.Round(Convert.ToSingle(graphData[graphData.Count - 1][1]), 2));
                        Console.WriteLine("{0} {1} ", graphData[graphData.Count - 1][1], graphData[graphData.Count - 1][5]);
                    }
                }
                
            }

            Console.WriteLine("Total elem:  {0}, showing:  {1} ", graphData.Count, yValue.Count);
            TempSeries.Points.DataBindXY(xValue, yValue);



            /*
            var dayConfig = Mappers.Xy<DateModel>()
                .X(dayModel => (double)dayModel.DateTime.Ticks / TimeSpan.FromHours(1).Ticks)
                .Y(dayModel => dayModel.Value);
            cartesianChart1_temp.Series = new SeriesCollection(dayConfig)
            {
                new LineSeries
                {
                    Values = new ChartValues<DateModel>
                    {
                        new DateModel
                        {
                            DateTime = System.DateTime.Now,
                            Value = 5
                        },
                        new DateModel
                        {
                            DateTime = System.DateTime.Now.AddHours(2),
                            Value = 9
                        }
                    },
                    Fill = Brushes.Transparent
                },
                new ColumnSeries
                {
                    Values = new ChartValues<DateModel>
                    {
                        new DateModel
                        {
                            DateTime = System.DateTime.Now,
                            Value = 4
                        },
                        new DateModel
                        {
                            DateTime = System.DateTime.Now.AddHours(1),
                            Value = 6
                        },
                        new DateModel
                        {
                            DateTime = System.DateTime.Now.AddHours(2),
                            Value = 8
                        }
                    }
                }
            };
            cartesianChart1_temp.AxisX.Add(new Axis
            {
                LabelFormatter = value => new System.DateTime((long)(value * TimeSpan.FromHours(1).Ticks)).ToString("t")
            });
            */


            /*
             * 
            this.AutoScroll = true;
            StartTime = startTime;
            EndTime = endTime;
            GraphData = graphData;
            textBox1.Text = startTime;
            textBox2.Text = endTime;

            var tempVal = new GearedValues<double>();
            var timeVal = new GearedValues<DateTime>();
            var humidVal = new GearedValues<double>();
            var part03Val = new GearedValues<Int64>();
            var part05Val = new GearedValues<Int64>();

            var defaultTemp = new GearedValues<double>();
            var defaultHumid = new GearedValues<double>();
            var defaultPart03 = new GearedValues<Int64>();
            var defaultPart05 = new GearedValues<Int64>();

            tempVal.Quality = Quality.Low;
            humidVal.Quality = Quality.Low;
            timeVal.Quality = Quality.Low;
            part03Val.Quality = Quality.Low;
            part05Val.Quality = Quality.Low;

            defaultTemp.Quality = Quality.Low;
            defaultHumid.Quality = Quality.Low;
            defaultPart03.Quality = Quality.Low;
            defaultPart05.Quality = Quality.Low;

            cartesianChart1_temp.LegendLocation = LegendLocation.Top;
            cartesianChart2_humid.LegendLocation = LegendLocation.Top;
            cartesianChart3_part03.LegendLocation = LegendLocation.Top;
            cartesianChart4_part05.LegendLocation = LegendLocation.Top;

            var tempTempVal = new double[graphData.Count];
            var tempHumidVal = new double[graphData.Count];
            var tempPart03Val = new Int64[graphData.Count];
            var tempPart05Val = new Int64[graphData.Count];
            var tempTimeVal = new DateTime[graphData.Count];

            var tempDefTemp = new double[graphData.Count];
            var tempDefHumid = new double[graphData.Count];
            var tempDefPart03Val = new Int64[graphData.Count];
            var tempDefPart05Val = new Int64[graphData.Count];

            Console.WriteLine("서버에서 불러오는 데이터 (행) 수량 (=Number of Rows Retrieved): " + graphData.Count.ToString() + "개 행");
            for (int i = 0; i < graphData.Count; i++)
            {
                tempDefTemp[i] = 21;
                tempDefHumid[i] = 40;
                tempDefPart03Val[i] = 10200;
                tempDefPart05Val[i] = 3520;

                tempTempVal[i] = Math.Round(Convert.ToSingle(graphData[i][1]), 2);
                tempHumidVal[i] = Math.Round(Convert.ToSingle(graphData[i][2]), 2);
                tempPart03Val[i] = Int64.Parse(graphData[i][3], NumberStyles.Any, new CultureInfo("en-au"));
                tempPart05Val[i] = Int64.Parse(graphData[i][4], NumberStyles.Any, new CultureInfo("en-au"));
                //string textim = tempPart03Val[i].ToString() + " " + tempPart05Val[i].ToString();
                //File.AppendAllText("myText01.txt", textim + Environment.NewLine);
                tempTimeVal[i] = DateTime.ParseExact(graphData[i][5], "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);

               
            }

            defaultTemp.AddRange(tempDefTemp);
            defaultHumid.AddRange(tempDefHumid);
            defaultPart03.AddRange(tempDefPart03Val);
            defaultPart05.AddRange(tempDefPart05Val);
            //Console.WriteLine("len2 " + tempDefTemp.Length.ToString());


            tempVal.AddRange(tempTempVal);
            humidVal.AddRange(tempHumidVal);
            part03Val.AddRange(tempPart03Val);
            part05Val.AddRange(tempPart05Val);
            timeVal.AddRange(tempTimeVal);
            //Console.WriteLine("len3 " + tempVal.Count.ToString());
            cartesianChart1_temp.DisableAnimations = true;
            cartesianChart1_temp.Hoverable = false;
            cartesianChart2_humid.DisableAnimations = true;
            cartesianChart2_humid.Hoverable = false;
            cartesianChart3_part03.DisableAnimations = true;
            cartesianChart3_part03.Hoverable = false;
            cartesianChart4_part05.DisableAnimations = true;
            cartesianChart4_part05.Hoverable = false;

            cartesianChart1_temp.AxisY.Add(new Axis
            {
                Title = "시간",
                Labels = tempVal,
            });

            cartesianChart1_temp.AxisX.Add(new Axis
            {
                Title = "시간",
                Labels = timeVal,
            }
            );
            cartesianChart2_humid.AxisX.Add(new Axis
            {
                Title = "시간",
                Labels = timeVal,
            }
            );
            cartesianChart3_part03.AxisX.Add(new Axis
            {
                Title = "시간",
                Labels = timeVal,
            }
            );
            cartesianChart4_part05.AxisX.Add(new Axis
            {
                Title = "시간",
                Labels = timeVal,
            }
            );
            
             */

        }

    }
    public class DateModel
    {
        public System.DateTime DateTime { get; set; }
        public double Value { get; set; }
    }
}
