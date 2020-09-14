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
        public MainForm ParentFormm { get; set; }
        public List<List<string[]>> GraphData { get; set; }
        public List<int> IDs { get; set; }
        /// <summary>
        /// 센서 데이터: 온도, 습도, 파티클(0.3), 파티클(0.5)중에서 어느 하나 또는 모두
        /// </summary>
        public string WhatToShow { get; set; }
        public ChartingForm() { }

        /// <summary>
        /// x axis에 string형식인 시간 값을 추가해 주는 함수. 
        /// </summary>
        /// <param name="cartesianChart"></param>
        /// <param name="timeVal"></param>
        void xAxesFunc(LiveCharts.WinForms.CartesianChart cartesianChart, GearedValues<string> timeVal)
        {
            cartesianChart.AxisX.Add(new Axis
            {
                Title = "시간",
                Labels = timeVal,
            }
            );
        }
        // "온도 센서 1 (°C)"
        /// <summary>
        /// Values 값이 double형식인 데이터를 y axis에 추가해 주는 함수.
        /// </summary>
        /// <param name="cartesianChart"></param>
        /// <param name="title"></param>
        /// <param name="tempVal"></param>
        void yAxisFuncTmpHmd(LiveCharts.WinForms.CartesianChart cartesianChart, string title, GearedValues<double> tempVal)
        {
            cartesianChart.Series = new SeriesCollection
                {
                    new GStepLineSeries
                    {
                        Stroke = System.Windows.Media.Brushes.Blue,
                        StrokeThickness = 1,
                        Fill = System.Windows.Media.Brushes.Transparent,
                        Title = title,
                        Values = tempVal,
                    }
                };
        }
        /// <summary>
        /// Values 값이 int형식인 데이터를 y axis에 추가해 주는 함수.
        /// </summary>
        /// <param name="cartesianChart"></param>
        /// <param name="title"></param>
        /// <param name="tempVal"></param>
        void yAxisFuncPcl(LiveCharts.WinForms.CartesianChart cartesianChart, string title, GearedValues<Int64> tempVal)
        {
            cartesianChart.Series = new SeriesCollection
                {
                    new GStepLineSeries
                    {
                        Stroke = System.Windows.Media.Brushes.Blue,
                        StrokeThickness = 1,
                        Fill = System.Windows.Media.Brushes.Transparent,
                        Title = title,
                        Values = tempVal,
                    }
                };
        }




        public ChartingForm(string[] timeInterval, List<List<List<string[]>>> graphDataAll, List<int> ids, string whatToShow)
        {
            InitializeComponent();
            this.AutoScroll = true; // 시각화화면 스크럴링가능 여부
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(270, 0);
            TextBox[] textboxes = { textBox_CF_1, textBox_CF_2, textBox_CF_3, textBox_CF_4 };
            LiveCharts.WinForms.CartesianChart[] cartesianCharts = { cartesianChart1, cartesianChart2, cartesianChart3, cartesianChart4 };

            textBox1_startTime.Text = timeInterval[0];
            textBox2_endTime.Text = timeInterval[1];

            int numOfElmnt = CountNumOfElmnt(graphDataAll, ids, whatToShow); //데이터 개수 : number of Elements(temp)
            IDs = ids;                                                      //시각화되는 데이터 개수를 동일하게 하기위해 최소 데이터 개수(Min num of elmnts) 계산하기
            Console.WriteLine("\n\nNumber of DataPoints: {0}", numOfElmnt);

            var timeVal = new GearedValues<string>();
            var dblVal1 = new GearedValues<double>();
            var dblVal2 = new GearedValues<double>();
            var dblVal3 = new GearedValues<double>();
            var dblVal4 = new GearedValues<double>();
            List<GearedValues<double>> dblVal = new List<GearedValues<double>> { dblVal1, dblVal2, dblVal3, dblVal4 };

            var intVal1 = new GearedValues<Int64>();
            var intVal2 = new GearedValues<Int64>();
            var intVal3 = new GearedValues<Int64>();
            var intVal4 = new GearedValues<Int64>();
            List<GearedValues<Int64>> intVal = new List<GearedValues<Int64>> { intVal1, intVal2, intVal3, intVal4 };


            //var defaultTemp = new GearedValues<double>();

            for(int i=0; i<IDs.Count; i++)
            {
                cartesianCharts[i].LegendLocation = LegendLocation.Top;
            }
            
            var tempTimeVal1 = new string[numOfElmnt];

            var tempDblVal1 = new double[numOfElmnt];
            var tempDblVal2 = new double[numOfElmnt];
            var tempDblVal3 = new double[numOfElmnt];
            var tempDblVal4 = new double[numOfElmnt];
            double[][] tempDblVal = { tempDblVal1, tempDblVal2, tempDblVal3, tempDblVal4 };

            var tempIntVal1 = new Int64[numOfElmnt];
            var tempIntVal2 = new Int64[numOfElmnt];
            var tempIntVal3 = new Int64[numOfElmnt];
            var tempIntVal4 = new Int64[numOfElmnt];
            Int64[][] tempIntVal = { tempIntVal1, tempIntVal2, tempIntVal3, tempIntVal4 };


            //var tempDefTemp = new double[numOfElmnt];
            Console.WriteLine("서버에서 불러오는 데이터 (행) 수량 (=Number of Rows Retrieved): " + numOfElmnt.ToString() + "개 행");
            for (int i = 0; i < numOfElmnt; i++)
            {
                //tempDefTemp[i] = 21;
                tempTimeVal1[i] = Convert.ToString(graphDataAll[0][IDs[0]-1][i][1]);
                //tempTempVal1[i] = Math.Round(Convert.ToSingle(graphData_temp[0][i][0]), 2);

                for (int index = 0; index < IDs.Count; index++)
                {
                    if (whatToShow.Contains("temp") || whatToShow.Contains("humid"))
                    {
                        tempDblVal[index][i] = Math.Round(Convert.ToSingle(graphDataAll[0][IDs[index]-1][i][0]), 2);
                    }
                    else
                    {
                        tempIntVal[index][i] = Int64.Parse(graphDataAll[0][IDs[index] - 1][i][0], NumberStyles.Any, new CultureInfo("en-au"));
                    }
                }
            }
            Console.WriteLine("\n\n Num: {0} {1} \n", tempIntVal[0].Length, intVal.Count);
                //defaultTemp.AddRange(tempDefTemp);
            timeVal.AddRange(tempTimeVal1);
            for (int index = 0; index < IDs.Count; index++)
            {
                if (whatToShow.Contains("temp") || whatToShow.Contains("humid"))
                {
                    dblVal[index].AddRange(tempDblVal[index]);
                }
                else
                {
                    intVal[index].AddRange(tempIntVal[index]);
                }

            }
                Console.WriteLine("\n\nNumber of elem2plot: {0} {1} ", dblVal[0].Count, dblVal.Count);
                            
                if (whatToShow.Contains("temp"))
                {
                    for(int i=0; i<IDs.Count; i++) {
                        textboxes[i].Text = dblVal[i][0].ToString();
                        yAxisFuncTmpHmd(cartesianCharts[i], "온도센서 (°C) "+IDs[i].ToString(), dblVal[i]);
                    }

                }
                else if (whatToShow.Contains("humid"))
                {
                    
                    
                    for (int i = 0; i < IDs.Count; i++){
                        textboxes[i].Text = dblVal[i][0].ToString();
                        yAxisFuncTmpHmd(cartesianCharts[i], "습도센서 (%) " + IDs[i].ToString(), dblVal[i]);
                    }
                    
                }
                else if (whatToShow.Contains("part03"))
                {
                for (int i = 0; i < IDs.Count; i++)
                {
                    textboxes[i].Text = intVal[i][0].ToString();
                    yAxisFuncPcl(cartesianCharts[i], "파티클센서 (0.3μm) " + IDs[i].ToString(), intVal[i]);
                }

               /* textBox_CF_1.Text = intVal[0][0].ToString();
                    textBox_CF_2.Text = intVal[1][0].ToString();
                    textBox_CF_3.Text = intVal[2][0].ToString();
                    if (IDs.Count == 4) { textBox_CF_4.Text = intVal[3][0].ToString(); }

                    yAxisFuncPcl(cartesianChart1, "파티클센서 1 (0.3μm) ", intVal[0]);
                    yAxisFuncPcl(cartesianChart1, "파티클센서 2 (0.3μm) ", intVal[0]);
                    yAxisFuncPcl(cartesianChart1, "파티클센서 3 (0.3μm) ", intVal[0]);
                    yAxisFuncPcl(cartesianChart1, "파티클센서 4 (0.3μm) ", intVal[0]);*/

                }

                else if (whatToShow.Contains("part05"))
                {
                for (int i = 0; i < IDs.Count; i++)
                {
                    textboxes[i].Text = intVal[i][0].ToString();
                    yAxisFuncPcl(cartesianCharts[i], "파티클센서 (0.5μm) " + IDs[i].ToString(), intVal[i]);
                }
            }
                else
                {
                MessageBox.Show("Currently this feature is under construction. ");
                }

                for(int i=0; i<IDs.Count; i++) {
                
                xAxesFunc(cartesianCharts[i], timeVal);
            }
            Console.WriteLine("Finished");

            
        }
        public int CountNumOfElmnt(List<List<List<string[]>>> datalist, List<int> IDs, string flag)
        {
            if(flag.Contains("all"))
            {
                int cntr_tmp = 0; //임시 카운터변수
                int numOfElmnt = datalist[0][0].Count; // 데이터 개수 : number of Elements(temp, humid, part03, part05)
                                                           //시각화되는 데이터 개수를 동일하게 하기위해 최소 데이터 개수(Min num of elmnts) 계산하기
                for (int ind = 0; ind < datalist.Count; ind++)
                {
                    for (int ind2 = 0; ind2 < IDs.Count; ind2++)
                    {
                        if (numOfElmnt > datalist[ind][ind2].Count && datalist[ind][ind2].Count != 0)
                        {
                            numOfElmnt = datalist[ind][ind2].Count;
                        }
                        //Console.WriteLine("\n\n\nCOUNTERlength: "+graphDataAll[ind][ind2].Count.ToString());
                        cntr_tmp += 1;
                    }
                }
                return numOfElmnt;
            }
            else
            {
                int numOfElmnt = datalist[0][IDs[0]-1].Count; // 데이터 개수 : number of Elements(temp, humid, part03, part05)
                //시각화되는 데이터 개수를 동일하게 하기위해 최소 데이터 개수(Min num of elmnts) 계산하기
                for (int ind = 0; ind < IDs.Count; ind++)
                {
                    if (numOfElmnt > datalist[0][ind].Count && datalist[0][ind].Count != 0)
                    {
                        numOfElmnt = datalist[0][ind].Count;
                    }
                    //Console.WriteLine("\n\n\nCOUNTERlength: "+graphDataAll[ind][ind2].Count.ToString());
                }
                return numOfElmnt;
            }
        }
        
    }
}





/*
    new GStepLineSeries
    {
        Stroke = System.Windows.Media.Brushes.Red,
        StrokeThickness = 1,
        Fill = System.Windows.Media.Brushes.Transparent,
        Title = "최고 임계치",
        Values = defaultHumid,
    }
            
*/





/*  public ChartingForm(string startTime, string endTime, List<List<List<string[]>>> graphDataAll, List<int> IDs, string whatToShow)
        {
            InitializeComponent();
            List<List<string[]>> graphData_temp = graphDataAll[0];
            List<List<string[]>> graphData_humid = graphDataAll[1];
            List<List<string[]>> graphData_part03 = graphDataAll[2];
            List<List<string[]>> graphData_part05 = graphDataAll[3];
            int numOfElmnt = CountNumOfElmnt(graphDataAll, IDs, whatToShow);

            this.AutoScroll = true; // 시각화화면 스크럴링가능 여부
            StartTime = startTime;
            EndTime = endTime;

            //선택한 시간을 화면에 표시하기
            textBox1_startTime.Text = startTime;
            textBox2_endTime.Text = endTime;

            var timeVal = new GearedValues<string>();
            var tempVal1 = new GearedValues<double>();
            var tempVal2 = new GearedValues<double>();
            var tempVal3 = new GearedValues<double>();
            var tempVal4 = new GearedValues<double>();
            List<GearedValues<double>> tempVal = new List<GearedValues<double>> { tempVal1, tempVal2, tempVal3, tempVal4 };

            var humidVal1 = new GearedValues<double>();
            var humidVal2 = new GearedValues<double>();
            var humidVal3 = new GearedValues<double>();
            var humidVal4 = new GearedValues<double>();
            List<GearedValues<double>> humidVal = new List<GearedValues<double>> { humidVal1, humidVal2, humidVal3, humidVal4 };

            var part03Val1 = new GearedValues<Int64>();
            var part03Val2 = new GearedValues<Int64>();
            var part03Val3 = new GearedValues<Int64>();
            var part03Val4 = new GearedValues<Int64>();
            List<GearedValues<Int64>> part03Val = new List<GearedValues<Int64>> { part03Val1, part03Val2, part03Val3, part03Val4 };

            var part05Val1 = new GearedValues<Int64>();
            var part05Val2 = new GearedValues<Int64>();
            var part05Val3 = new GearedValues<Int64>();
            var part05Val4 = new GearedValues<Int64>();
            List<GearedValues<Int64>> part05Val = new List<GearedValues<Int64>> { part05Val1, part05Val2, part05Val3, part05Val4 };

            var defaultTemp = new GearedValues<double>();
            var defaultHumid = new GearedValues<double>();
            var defaultPart03 = new GearedValues<Int64>();
            var defaultPart05 = new GearedValues<Int64>();

            timeVal.Quality = Quality.Low;

            for (int i = 0; i < IDs.Count; i++)
            {
                tempVal[IDs[i]].Quality = Quality.Low;
                humidVal[IDs[i]].Quality = Quality.Low;
                part03Val[IDs[i]].Quality = Quality.Low;
                part05Val[IDs[i]].Quality = Quality.Low;
            }

            defaultTemp.Quality = Quality.Low;
            defaultHumid.Quality = Quality.Low;
            defaultPart03.Quality = Quality.Low;
            defaultPart05.Quality = Quality.Low;

            cartesianChart1.LegendLocation = LegendLocation.Top;
            cartesianChart2.LegendLocation = LegendLocation.Top;
            cartesianChart3.LegendLocation = LegendLocation.Top;
            cartesianChart4.LegendLocation = LegendLocation.Top;
            
            var tempTimeVal1 = new string[numOfElmnt];

            if (whatToShow.Contains("temp"))
            {
                var tempTempVal1 = new double[numOfElmnt];
                var tempTempVal2 = new double[numOfElmnt];
                var tempTempVal3 = new double[numOfElmnt];
                var tempTempVal4 = new double[numOfElmnt];
                double[][] tempTempVal = { tempTempVal1, tempTempVal2, tempTempVal3, tempTempVal4 };
                
                var tempDefTemp = new double[numOfElmnt];
                Console.WriteLine("서버에서 불러오는 데이터 (행) 수량 (=Number of Rows Retrieved): " + numOfElmnt.ToString() + "개 행");
                for (int i = 0; i < numOfElmnt; i++)
                {
                    //Console.WriteLine("Some info: cnt, 1 row cnt, 1st elem: {0} {1} {2}", numOfElmnt, graphData_temp[0].Count, graphData_temp[0][i][0]);
                    tempDefTemp[i] = 21;
                    tempTimeVal1[i] = Convert.ToString(graphData_temp[0][i][1]);

                    //tempTempVal1[i] = Math.Round(Convert.ToSingle(graphData_temp[0][i][0]), 2);
                    for (int index = 0; index < IDs.Count; index++)
                    {
                        tempTempVal[index][i] = Math.Round(Convert.ToSingle(graphData_temp[index][i][0]), 2);
                    }
                }
                defaultTemp.AddRange(tempDefTemp);
                timeVal.AddRange(tempTimeVal1);
                for (int index = 0; index < IDs.Count; index++)
                {
                    tempVal[index].AddRange(tempTempVal[index]);
                }
            }

            if (whatToShow.Contains("all"))
            {
                var tempTempVal1 = new double[numOfElmnt];
                var tempTempVal2 = new double[numOfElmnt];
                var tempTempVal3 = new double[numOfElmnt];
                var tempTempVal4 = new double[numOfElmnt];
                double[][] tempTempVal = {tempTempVal1, tempTempVal2, tempTempVal3, tempTempVal4 };
            
                var tempHumidVal1 = new double[numOfElmnt];
                var tempHumidVal2 = new double[numOfElmnt];
                var tempHumidVal3 = new double[numOfElmnt];
                var tempHumidVal4 = new double[numOfElmnt];
                double[][] tempHumidVal = { tempHumidVal1, tempHumidVal2, tempHumidVal3, tempHumidVal4 };

                var tempPart03Val1 = new Int64[numOfElmnt];
                var tempPart03Val2 = new Int64[numOfElmnt];
                var tempPart03Val3 = new Int64[numOfElmnt];
                var tempPart03Val4 = new Int64[numOfElmnt];
                Int64[][] tempPart03Val = {tempPart03Val1, tempPart03Val2, tempPart03Val3, tempPart03Val4 };

                var tempPart05Val1 = new Int64[numOfElmnt];
                var tempPart05Val2 = new Int64[numOfElmnt];
                var tempPart05Val3 = new Int64[numOfElmnt];
                var tempPart05Val4 = new Int64[numOfElmnt];
                Int64[][] tempPart05Val = { tempPart05Val1, tempPart05Val2, tempPart05Val3, tempPart05Val4 };

                var tempDefTemp = new double[numOfElmnt];
                var tempDefHumid = new double[numOfElmnt];
                var tempDefPart03Val = new Int64[numOfElmnt];
                var tempDefPart05Val = new Int64[numOfElmnt];
            
                Console.WriteLine("서버에서 불러오는 데이터 (행) 수량 (=Number of Rows Retrieved): " + numOfElmnt.ToString() + "개 행");
                for (int i =0; i< numOfElmnt; i++)
                {
                    //Console.WriteLine("Some info: cnt, 1 row cnt, 1st elem: {0} {1} {2}", numOfElmnt, graphData_temp[0].Count, graphData_temp[0][i][0]);
                    tempDefTemp[i] = 21;
                    tempDefHumid[i] = 40;
                    tempDefPart03Val[i] = 10200;
                    tempDefPart05Val[i] = 3520;
                    tempTimeVal1[i] = Convert.ToString(graphData_temp[0][i][1]);

                    //tempTempVal1[i] = Math.Round(Convert.ToSingle(graphData_temp[0][i][0]), 2);
                    for(int index=0; index<IDs.Count; index++)
                        {
                            tempTempVal[index][i] = Math.Round(Convert.ToSingle(graphData_temp[index][i][0]), 2);
                            tempHumidVal[index][i] = Math.Round(Convert.ToSingle(graphData_humid[index][i][0]), 2);
                            tempPart03Val[index][i] = Int64.Parse(graphData_part03[index][i][0], NumberStyles.Any, new CultureInfo("en-au"));
                            tempPart05Val[index][i] = Int64.Parse(graphData_part05[index][i][0], NumberStyles.Any, new CultureInfo("en-au")); 
                        }

                }

                //add default (fixed) values here
                defaultTemp.AddRange(tempDefTemp);
                defaultHumid.AddRange(tempDefHumid);
                defaultPart03.AddRange(tempDefPart03Val);
                defaultPart05.AddRange(tempDefPart05Val);
               Console.WriteLine("\n\n\n tempDefTemp.len {0}, tempVal.len {1} tempTempValLen {2}\n\n\n\n",defaultTemp.Count,tempVal1.Count, tempTempVal.Length);

                //Add sensor values here
                //tempVal1.AddRange(tempTempVal1);
                timeVal.AddRange(tempTimeVal1);
                for (int index = 0; index < IDs.Count; index++)
                {
                tempVal[index].AddRange(tempTempVal[index]);
                humidVal[index].AddRange(tempHumidVal[index]);
                part03Val[index].AddRange(tempPart03Val[index]);
                part05Val[index].AddRange(tempPart05Val[index]);
                }
                /*
                //Console.WriteLine("len3 " + tempVal.Count.ToString());
                cartesianChart1.DisableAnimations = true;
                cartesianChart1.Hoverable = false;
                //cartesianChart1_temp.DisplayRectangle = t

                cartesianChart2.DisableAnimations = true;
                cartesianChart2.Hoverable = false;
                cartesianChart3.DisableAnimations = true;
                cartesianChart3.Hoverable = false;
                cartesianChart4.DisableAnimations = true;
                cartesianChart4.Hoverable = false;
                
               


            }
            //Create charts Series(lines)
            //MainForm aform = new MainForm();
            if (whatToShow.Contains("temp"))
{
    cartesianChart1.Series = new SeriesCollection
                {
                    new GStepLineSeries
                    {
                        Stroke = System.Windows.Media.Brushes.Blue,
                        StrokeThickness = 1,
                        Fill = System.Windows.Media.Brushes.Transparent,
                        Title = "온도 센서 1 (°C)",
                        Values = tempVal1,
                    },

                };
    cartesianChart2.Series = new SeriesCollection
                {
                    new GStepLineSeries
                    {
                        Stroke = System.Windows.Media.Brushes.Green,
                        StrokeThickness = 1,
                        Fill = System.Windows.Media.Brushes.Transparent,
                        Title = "온도 센서 2 (°C)",
                        Values = tempVal2,
                    },

                };
    cartesianChart3.Series = new SeriesCollection
                {
                    new GStepLineSeries
                    {
                        Stroke = System.Windows.Media.Brushes.Black,
                        StrokeThickness = 1,
                        Fill = System.Windows.Media.Brushes.Transparent,
                        Title = "온도 센서 3 (°C)",
                        Values = tempVal3,
                    },

                };
    cartesianChart4.Series = new SeriesCollection
                {
                    new GStepLineSeries
                    {
                        Stroke = System.Windows.Media.Brushes.Black,
                        StrokeThickness = 1,
                        Fill = System.Windows.Media.Brushes.Transparent,
                        Title = "온도 센서 4 (°C)",
                        Values = tempVal4,
                    },

                };
}
else if (whatToShow.Contains("humid"))
{
    cartesianChart1.Series = new SeriesCollection
            {
                new GStepLineSeries
                {
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = "습도 센서 1 (%)",
                    Values = humidVal1

                },
                };
    cartesianChart2.Series = new SeriesCollection
            {
                new GStepLineSeries
                {
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = "습도 센서 2 (%)",
                    Values = humidVal2

                },
            };
    cartesianChart3.Series = new SeriesCollection
            {
                new GStepLineSeries
                {
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = "습도 센서 3 (%)",
                    Values = humidVal3
                },

            };
    cartesianChart4.Series = new SeriesCollection
            {
                new GStepLineSeries
                {
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = "습도 센서 4 (%)",
                    Values = humidVal4,
                },

            };
}
else if (whatToShow.Contains("part03"))
{
    cartesianChart1.Series = new SeriesCollection
            {
                new GStepLineSeries
                {
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = "파티클 센서 1 (0.3μm)",
                    Values = part03Val1

                },
                };
    cartesianChart2.Series = new SeriesCollection
            {
                new GStepLineSeries
                {
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = "파티클 센서 2 (0.3μm)",
                    Values = part03Val2

                },
            };
    cartesianChart3.Series = new SeriesCollection
            {
                new GStepLineSeries
                {
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = "파티클 센서 3 (0.3μm)",
                    Values = part03Val3,
                },

            };
    cartesianChart4.Series = new SeriesCollection
            {
                new GStepLineSeries
                {
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = "파티클 센서 4 (0.3μm)",
                    Values = part03Val4,
                },

            };
}
else if (whatToShow.Contains("part05"))
{
    cartesianChart1.Series = new SeriesCollection
            {
                new GStepLineSeries
                {
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = "파티클 센서 1 (0.5μm)",
                    Values = part05Val1

                },
                };
    cartesianChart2.Series = new SeriesCollection
            {
                new GStepLineSeries
                {
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = "파티클 센서 2 (0.5μm)",
                    Values = part05Val2

                },
            };
    cartesianChart3.Series = new SeriesCollection
            {
                new GStepLineSeries
                {
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = "파티클 센서 3 (0.5μm)",
                    Values = part05Val3,
                },

            };
    cartesianChart4.Series = new SeriesCollection
            {
                new GStepLineSeries
                {
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = "파티클 센서 4 (0.5μm)",
                    Values = part05Val4,
                },

            };
}
else if (whatToShow.Contains("all"))
{

    textBox_CF_big.Text = "아직 구현이 안되어 있습니다.";
}
            

            

        }
            */