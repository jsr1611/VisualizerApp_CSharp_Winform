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
        /// <summary>
        /// 시각화되는 데이터를 가지고 있는 배열
        /// </summary>
        public List<List<string[]>> GraphData { get; set; }
        /// <summary>
        /// 시각화되는 센서들의 ID를 가지고 있는 배열
        /// </summary>
        public List<int> IDs { get; set; }
        /// <summary>
        /// 호출되는 센서 데이터: 온도, 습도, 파티클(0.3), 파티클(0.5)중에서 어느 하나 또는 모두
        /// </summary>
        public string WhatToShow { get; set; }

        // 기본 컨스트럭터
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
        /// <summary>
        /// 시각화 함수
        /// </summary>
        /// <param name="timeInterval"></param>
        /// <param name="graphDataAll"></param>
        /// <param name="ids"></param>
        /// <param name="whatToShow"></param>
        public ChartingForm(string[] timeInterval, List<List<List<string[]>>> graphDataAll, List<int> ids, string whatToShow)
        {
            InitializeComponent();
            this.AutoScroll = true; // 시각화화면 스크럴링가능 여부
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(270, 0);
            TextBox[] textboxes_avg = { textBox_CF_1, textBox_CF_2, textBox_CF_3, textBox_CF_4 };
            TextBox[] textboxes_max = { textBox_CF_1_max, textBox_CF_2_max, textBox_CF_3_max, textBox_CF_4_max };
            TextBox[] textboxes_min = { textBox_CF_1_min, textBox_CF_2_min, textBox_CF_3_min, textBox_CF_4_min };
            LiveCharts.WinForms.CartesianChart[] cartesianCharts = { cartesianChart1, cartesianChart2, cartesianChart3, cartesianChart4 };

            textBox1_startTime.Text = timeInterval[0];
            textBox2_endTime.Text = timeInterval[1];

            int numOfElmnt = CountNumOfElmnt(graphDataAll, ids, whatToShow); //데이터 개수 : number of Elements(temp)
            IDs = ids;                                                      //시각화되는 데이터 개수를 동일하게 하기위해 최소 데이터 개수(Min num of elmnts) 계산하기
            Console.WriteLine("\n\nNumber of DataPoints: {0}", numOfElmnt);
            
            var timeVal = new GearedValues<string>();

            var dblVals = new List<GearedValues<double>>();
            var intVals = new List<GearedValues<Int64>>();
            double[][] tempDblVals = new double[IDs.Count][];
            Int64[][] tempIntVals = new Int64[IDs.Count][];
            for (int i=0; i<IDs.Count; i++)
            {
                dblVals.Add(new GearedValues<double>());
                intVals.Add(new GearedValues<Int64>());
                tempDblVals[i] = new double[numOfElmnt];
                tempIntVals[i] = new Int64[numOfElmnt];
            }

            //var defaultTemp = new GearedValues<double>();

            for (int i = 0; i < IDs.Count; i++)
            {
                cartesianCharts[i].LegendLocation = LegendLocation.Top;
            }

            var tempTimeVal1 = new string[numOfElmnt];

            //var tempDefTemp = new double[numOfElmnt];
            Console.WriteLine("서버에서 불러오는 데이터 (행) 수량 (=Number of Rows Retrieved): " + numOfElmnt.ToString() + "개 행");
            for (int i = 0; i < numOfElmnt; i++)
            {
                //tempDefTemp[i] = 21;
                tempTimeVal1[i] = Convert.ToString(graphDataAll[0][0][i][1]);

                //tempTempVal1[i] = Math.Round(Convert.ToSingle(graphData_temp[0][i][0]), 2);

                for (int index = 0; index < IDs.Count; index++)
                {
                    if (whatToShow.Contains("temp") || whatToShow.Contains("humid"))
                    {
                        tempDblVals[index][i] = Math.Round(Convert.ToSingle(graphDataAll[0][index][i][0]), 2);
                    }
                    else
                    {
                        tempIntVals[index][i] = Int64.Parse(graphDataAll[0][index][i][0], NumberStyles.Any, new CultureInfo("en-au"));

                    }
                }
            }
            Console.WriteLine("\n\n Num: {0} {1} \n", tempIntVals[0].Length, intVals.Count);
            //defaultTemp.AddRange(tempDefTemp);
            timeVal.AddRange(tempTimeVal1);
            for (int index = 0; index < IDs.Count; index++)
            {
                if (whatToShow.Contains("temp") || whatToShow.Contains("humid"))
                {
                    dblVals[index].AddRange(tempDblVals[index]);

                }
                else
                {
                    intVals[index].AddRange(tempIntVals[index]);
                }

            }
            Console.WriteLine("\n\nNumber of elem2plot: {0} {1} ", dblVals[0].Count, dblVals.Count);
            
            
            if (whatToShow.Contains("temp"))
            {
                List<double> avgDbl = Avg(dblVals);
                List<double> maxDbl = Max(dblVals);
                List<double> minDbl = Min(dblVals);
                for (int i = 0; i < IDs.Count; i++)
                {
                    textboxes_avg[i].Text = Math.Round(avgDbl[i], 2).ToString() + " °C";
                    textboxes_max[i].Text = Math.Round(maxDbl[i], 2).ToString() + " °C";
                    textboxes_min[i].Text = Math.Round(minDbl[i], 2).ToString() + " °C";
                    yAxisFuncTmpHmd(cartesianCharts[i], "온도센서 (°C) " + IDs[i].ToString(), dblVals[i]);
                }
            }
            else if (whatToShow.Contains("humid"))
            {
                List<double> avgDbl = Avg(dblVals);
                List<double> maxDbl = Max(dblVals);
                List<double> minDbl = Min(dblVals);
                for (int i = 0; i < IDs.Count; i++)
                {
                    textboxes_avg[i].Text = Math.Round(avgDbl[i], 2).ToString("{0:n0}") + " %";
                    textboxes_avg[i].Text = Math.Round(avgDbl[i], 2).ToString("{0:n0}") + " %";
                    textboxes_min[i].Text = Math.Round(minDbl[i], 2).ToString("{0:n0}") + " %";
                    yAxisFuncTmpHmd(cartesianCharts[i], "습도센서 (%) " + IDs[i].ToString(), dblVals[i]);
                }
            }
            else if (whatToShow.Contains("part03"))
            {
                List<Int64> avgInt = Avg(intVals);
                List<Int64> maxInt = Max(intVals);
                List<Int64> minInt = Min(intVals);
                for (int i = 0; i < IDs.Count; i++)
                {
                    textboxes_avg[i].Text = String.Format("{0:n0}", avgInt[i]) + " 0.3μm";
                    textboxes_max[i].Text = String.Format("{0:n0}", maxInt[i]) + " 0.3μm";
                    textboxes_min[i].Text = String.Format("{0:n0}", minInt[i]) + " 0.3μm";
                    yAxisFuncPcl(cartesianCharts[i], "파티클센서 (0.3μm) " + IDs[i].ToString(), intVals[i]);
                }
            }
            else if (whatToShow.Contains("part05"))
            {
                List<Int64> avgInt = Avg(intVals);
                List<Int64> maxInt = Max(intVals);
                List<Int64> minInt = Min(intVals);
                for (int i = 0; i < IDs.Count; i++)
                {
                    textboxes_avg[i].Text = String.Format("{0:n0}", avgInt[i]) + " 0.5μm";
                    textboxes_max[i].Text = String.Format("{0:n0}", maxInt[i]) + " 0.5μm";
                    textboxes_min[i].Text = String.Format("{0:n0}", minInt[i]) + " 0.5μm";
                    yAxisFuncPcl(cartesianCharts[i], "파티클센서 (0.5μm) " + IDs[i].ToString(), intVals[i]);
                }
            }
            else
            {
                MessageBox.Show("Currently this feature is under construction. ");
            }
            for (int i = 0; i < IDs.Count; i++)
            {
                xAxesFunc(cartesianCharts[i], timeVal);
            }
            Console.WriteLine("Finished");
        }
        public int CountNumOfElmnt(List<List<List<string[]>>> datalist, List<int> IDs, string flag)
        {
            if (flag.Contains("all"))
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
                int numOfElmnt = datalist[0][0].Count; // 데이터 개수 : number of Elements(temp, humid, part03, part05)
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
        public List<double> Avg(List<GearedValues<double>> data)
        {
            List<double> AvgArr = new List<double>();
            for (int h = 0; h < IDs.Count; h++)
            {
                double total = 0;
                for (int i = 0; i < data[h].Count; i++)
                {
                    total += data[h][i];
                }
                AvgArr.Add(total / data[h].Count);
                Console.WriteLine("\nAvgDbl[{0}]: {1}", h, total / data[h].Count);
            }
            return AvgArr;
        }
        public List<Int64> Avg(List<GearedValues<Int64>> data)
        {
            List<Int64> AvgArr = new List<Int64>();
            for (int h=0; h<IDs.Count; h++)
            {
                Int64 total = 0;
                for (int i = 0; i < data[h].Count; i++)
                {
                    total += data[h][i];
                }
                AvgArr.Add(total / data[h].Count);
                Console.WriteLine("\nAvgInt[{0}]: {1}", h, total / data[h].Count);
            }
            return AvgArr;
        }

        public List<double> Max(List<GearedValues<double>> data)
        {
            List<double> MaxArr = new List<double>();
            for(int h=0; h<IDs.Count; h++)
            {
                double max = data[h][0];
                for(int i=0; i < data[h].Count; i++)
                {
                    if(max < data[h][i])
                    {
                        max = data[h][i];
                    }
                }
                MaxArr.Add(max);
                Console.WriteLine("\nMaxDbl[{0}]: {1}", h, max);
            }
            return MaxArr;
        }

        public List<Int64> Max(List<GearedValues<Int64>> data)
        {
            List<Int64> MaxArr = new List<Int64>();
            for (int h = 0; h < IDs.Count; h++)
            {
                Int64 max = data[h][0];
                for (int i = 0; i < data[h].Count; i++)
                {
                    if (max < data[h][i])
                    {
                        max = data[h][i];
                    }
                }
                MaxArr.Add(max);
                Console.WriteLine("\nMaxDbl[{0}]: {1}", h, max);
            }
            return MaxArr;
        }
        
        public List<double> Min(List<GearedValues<double>> data)
        {
            List<double> MinArr = new List<double>();
            for (int h = 0; h < IDs.Count; h++)
            {
                double min = data[h][0];
                for (int i = 0; i < data[h].Count; i++)
                {
                    if (min > data[h][i])
                    {
                        min = data[h][i];
                    }
                }
                MinArr.Add(min);
                Console.WriteLine("\nMaxDbl[{0}]: {1}", h, min);
            }
            return MinArr;
        }
        public List<Int64> Min(List<GearedValues<Int64>> data)
        {
            List<Int64> MinArr = new List<Int64>();
            for (int h = 0; h < IDs.Count; h++)
            {
                Int64 min = data[h][0];
                for (int i = 0; i < data[h].Count; i++)
                {
                    if (min > data[h][i])
                    {
                        min = data[h][i];
                    }
                }
                MinArr.Add(min);
                Console.WriteLine("\nMaxDbl[{0}]: {1}", h, min);
            }
            return MinArr;
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