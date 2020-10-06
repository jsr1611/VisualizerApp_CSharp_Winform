using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using System.Globalization;

namespace VisualizerApp
{
    public partial class UserControl_Main : UserControl
    {
        public UserControl_Main() { } // 기본 컨스트럭터
        /// <summary>
        /// 시각화되는 센서들의 ID를 가지고 있는 배열
        /// </summary>
        public List<int> IDs { get; set; }
        /// <summary>
        /// 호출되는 센서 데이터: 온도, 습도, 파티클(0.3), 파티클(0.5)중에서 어느 하나 또는 모두
        /// </summary>
        public string WhatToShow { get; set; }
        
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
        public UserControl_Main(string[] timeInterval, List<List<List<string[]>>> graphDataAll, List<int> ids, string whatToShow)
        {
            InitializeComponent();

            this.AutoScroll = true; // 시각화화면 스크럴링가능 여부
            TextBox[] textboxes_avg = { textBox_CF_1, textBox_CF_2, textBox_CF_3, textBox_CF_4 };
            TextBox[] textboxes_max = { textBox_CF_1_max, textBox_CF_2_max, textBox_CF_3_max, textBox_CF_4_max };
            TextBox[] textboxes_min = { textBox_CF_1_min, textBox_CF_2_min, textBox_CF_3_min, textBox_CF_4_min };
            LiveCharts.WinForms.CartesianChart[] cartesianCharts = { cartesianChart1, cartesianChart2, cartesianChart3, cartesianChart4 };

            textBox1_startTime.Text = timeInterval[0];
            textBox2_endTime.Text = timeInterval[1];

            int numOfElmnt = CountNumOfElmnt(graphDataAll, ids, whatToShow); //데이터 개수 : number of Elements(temp)
            IDs = ids;                                                      //시각화되는 데이터 개수를 동일하게 하기위해 최소 데이터 개수(Min num of elmnts) 계산하기
            Console.WriteLine("\n\nNumber of DataPoints: {0}", numOfElmnt);
            var timeVal = new GearedValues<string>(); //시간 데이터를 위한 배열
            var dblVals = new List<GearedValues<double>>(); // double형식의 데이터를 위한 nested배열
            var intVals = new List<GearedValues<Int64>>(); //int형식의 데이터를 위한 nested배열
            double[][] tempDblVals = new double[IDs.Count][]; //double형식의 데이터를 위한 nested임시 배열
            Int64[][] tempIntVals = new Int64[IDs.Count][]; ////int형식의 데이터를 위한 임시 nested배열
            //각 데이터를 위한 실제 배열
            for (int i = 0; i < IDs.Count; i++)
            {
                dblVals.Add(new GearedValues<double>());
                intVals.Add(new GearedValues<Int64>());
                tempDblVals[i] = new double[numOfElmnt];
                tempIntVals[i] = new Int64[numOfElmnt];
            }

            //차트의 legend 주소 지정
            for (int i = 0; i < IDs.Count; i++)
            {
                cartesianCharts[i].LegendLocation = LegendLocation.Top;
            }

            var tempTimeVal1 = new string[numOfElmnt]; // 시간 데이터를 위한 임시 배열

            Console.WriteLine("서버에서 불러오는 데이터 (행) 수량 (=Number of Rows Retrieved): " + numOfElmnt.ToString() + "개 행");
            //실제 데이터를 차트의 임시 배열에 추가하기(추가 대신 배열 값 변경) :임시 배열 사용 이유: 속도
            for (int i = 0; i < numOfElmnt; i++)
            {
                tempTimeVal1[i] = Convert.ToString(graphDataAll[0][0][i][1]);
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
            //실제 데이터를 차트의 Values부분에 추가하기
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

            // 실제 시각화를 하는 부분
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
                    textboxes_avg[i].Text = Math.Round(avgDbl[i], 2).ToString() + " %";
                    textboxes_max[i].Text = Math.Round(maxDbl[i], 2).ToString() + " %";
                    textboxes_min[i].Text = Math.Round(minDbl[i], 2).ToString() + " %";
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
                MessageBox.Show("아직 구현이 안 된 부분입니다. 에러가 발생했습니다. ");
            }
            for (int i = 0; i < IDs.Count; i++)
            {
                xAxesFunc(cartesianCharts[i], timeVal);
            }
            Console.WriteLine("작업 끝");
        }







        /// <summary>
        /// 시각화 되는 데이터 갯수를 계산해 주는 함수
        /// </summary>
        /// <param name="datalist"></param>
        /// <param name="IDs"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
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
        // 평균값, 최고 값, 그리고 최소 값을 계산해 주는 함수들.
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
            for (int h = 0; h < IDs.Count; h++)
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
            for (int h = 0; h < IDs.Count; h++)
            {
                double max = data[h][0];
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
