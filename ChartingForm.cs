using LiveCharts;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualizerApp_3
{
    public partial class ChartingForm : Form
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public List<string[]> GraphData { get; set; }

        

        public ChartingForm(string startTime, string endTime, List<string[]> graphData)
        {
            InitializeComponent();
            this.AutoScroll = true;
            StartTime = startTime;
            EndTime = endTime;
            GraphData = graphData;
            textBox1.Text = startTime;
            textBox2.Text = endTime;

            var tempVal = new GearedValues<double>();
            var timeVal = new GearedValues<string>();
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
            var tempTimeVal = new string[graphData.Count];

            var tempDefTemp = new double[graphData.Count];
            var tempDefHumid = new double[graphData.Count];
            var tempDefPart03Val = new Int64[graphData.Count];
            var tempDefPart05Val = new Int64[graphData.Count];

            Console.WriteLine("서버에서 불러오는 데이터 (행) 수량 (=Number of Rows Retrieved): " + graphData.Count.ToString() + "개 행");
            for (int i =0; i<graphData.Count; i++)
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
                tempTimeVal[i] = graphData[i][5];

                /*defaultTemp.Add(21);
                defaultHumid.Add(40);
                defaultPart03.Add(10200);
                defaultPart05.Add(3520);
            
                tempVal.Add(Math.Round(Convert.ToSingle(graphData[i][1]),2));
                humidVal.Add(Math.Round(Convert.ToSingle(graphData[i][2]), 2));
                part03Val.Add(Int32.Parse(graphData[i][3], NumberStyles.AllowThousands, new CultureInfo("en-au")));
                part05Val.Add(Int32.Parse(graphData[i][4], NumberStyles.AllowThousands, new CultureInfo("en-au")));
                timeVal.Add(graphData[i][5]);
                */
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

            cartesianChart1_temp.Series = new SeriesCollection
            {
                new GLineSeries
                {
                    Title = "온도(°C)",
                    Values = tempVal,
                },
                new GLineSeries
                {
                    Title = "최고 임계치",
                    Values = defaultTemp,
                }
            };

            cartesianChart2_humid.Series = new SeriesCollection
            {
                new GLineSeries
                {
                    Title = "습도(%)",
                    Values = humidVal

                },
                new GLineSeries
                {
                    Title = "최고 임계치",
                    Values = defaultHumid,
                }
            };

            cartesianChart3_part03.Series = new SeriesCollection
            {
                new GLineSeries
                {
                    Title = "파티클(0.3μm)",
                    Values = part03Val
                },
                new GLineSeries
                {
                    Title = "최고 임계치",
                    Values = defaultPart03,
                }
            };

            cartesianChart4_part05.Series = new SeriesCollection
            {
                new GLineSeries
                {
                    Title = "파티클(0.5μm)",
                    Values = part05Val
                },
                new GLineSeries
                {
                    Title = "최고 임계치",
                    Values = defaultPart05,
                }
            };


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

        }
    }
}
