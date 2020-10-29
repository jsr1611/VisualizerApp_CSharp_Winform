using LiveCharts;
using LiveCharts.Geared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Axis = LiveCharts.Wpf.Axis;
using SeriesCollection = LiveCharts.SeriesCollection;
namespace VisualizerApp
{
    public partial class NewChartingForm : Form
    {
        public NewChartingForm() { }
        /// <summary>
        /// 시각화되는 센서들의 ID를 가지고 있는 배열
        /// </summary>
        public GearedValues<string> timeVal = new GearedValues<string>(); // 시간 데이터를 위한 배열 생성
        public List<GearedValues<double>> dblVals = new List<GearedValues<double>>(); // double 형식의 데이터를 위한 nested 배열 생성
        public List<GearedValues<Int64>> intVals = new List<GearedValues<Int64>>(); // int 형식의 데이터를 위한 nested 배열 생성
        public List<List<List<string[]>>> graphDataAll_RT = new List<List<List<string[]>>>(); // 실시간 데이터를 위한 임시 nested 배열 생성
        List<List<string[]>> data_RT; // 실시간 데이터를 위한 nested 배열
        /// <summary>
        /// 센서들의 ID를 가지고 있는 배열
        /// </summary>
        public List<int> IDs;
        /// <summary>
        /// 호출되는 센서 데이터: 온도, 습도, 파티클(0.3), 파티클(0.5)중에서 어느 하나 또는 모두
        /// </summary>
        public string WhatToShow;
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

        public NewChartingForm(List<List<List<string[]>>> graphDataAll, string[] timeInterval, List<int> ids, string whatToShow)
        {
            InitializeComponent();
            //this.Location = new Point(MainForm.ActiveForm.Bounds.Width, 0);
            IDs = ids; // 시각화 하려는 센서들의 ID가 들어가 있는 배열
            WhatToShow = whatToShow; // 시각화 하려는 데이터 구분(온습도 또는 파티클)
            List<LiveCharts.WinForms.CartesianChart> cartesianCharts = new List<LiveCharts.WinForms.CartesianChart>();
            Panel headerPanel = new Panel(); // 해더 패널 생성 및 주소 지정, 이 패널에 관련 콘트롤 추가
            headerPanel.Dock = DockStyle.Top; 
            headerPanel.SetBounds(0, 0, this.Bounds.Width, 30);
            headerPanel.Controls.Add(label1_selectedTimeInter);
            headerPanel.Controls.Add(textBox1_startTime);
            headerPanel.Controls.Add(label2_from);
            headerPanel.Controls.Add(textBox2_endTime);
            headerPanel.Controls.Add(label3_upto);
            this.Controls.Add(headerPanel); 
            TableLayoutPanel tbpanel = new TableLayoutPanel(); //테이블레이아웃패널 생성
            tbpanel.Dock = DockStyle.Fill;
            tbpanel.BringToFront();

            if (IDs.Count < 2) {
                tbpanel.RowCount = 1;
                tbpanel.ColumnCount = 1;
                tbpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tbpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            }
            else if (IDs.Count== 2) {
                tbpanel.RowCount = 2; 
                tbpanel.ColumnCount = 1;
                tbpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tbpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tbpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            
            }
            else if (IDs.Count > 2) {
            
                tbpanel.RowCount = 2; 
                tbpanel.ColumnCount = 2;
                tbpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tbpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tbpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tbpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            }

            textBox1_startTime.Text = timeInterval[0];
            //실시간 시각화 구분, if문은 시간 간격 시각화, else문은 실시간 시각화
            if (timeInterval[1].Contains("RT") == false) {
                textBox2_endTime.Text = timeInterval[1];
            }
            else
            {
                timer1.Enabled = true;
                timer1.Start();
                textBox2_endTime.Text = "현재";
                graphDataAll_RT.Add(MyDataGetter(ids, whatToShow));
                graphDataAll = new List<List<List<string[]>>>(graphDataAll_RT);
            }

            List<Label> avgLabels = new List<Label>();
            List<Label> maxLabels = new List<Label>();
            List<Label> minLabels = new List<Label>();
            List<TextBox> avgTextBoxes = new List<TextBox>();
            List<TextBox> maxTextBoxes = new List<TextBox>();
            List<TextBox> minTextBoxes = new List<TextBox>();
            
            //textBox, Label, CartesianChart 등 컨트롤 생성
            for (int i = 0; i < tbpanel.ColumnCount; i++)
                {
                    for (int j = 0; j < tbpanel.RowCount; j++)
                    {
                    int cnt = 0;
                    if (IDs.Count % 2 == 1 && cnt == 0) {
                        Panel panel = new Panel();
                        panel.BorderStyle = BorderStyle.FixedSingle;
                        panel.Dock = DockStyle.Fill;

                        tbpanel.Controls.Add(panel, i, j);

                        LiveCharts.WinForms.CartesianChart cartesianChart = new LiveCharts.WinForms.CartesianChart();
                        cartesianCharts.Add(cartesianChart);
                        cartesianChart.Dock = DockStyle.Fill;

                        panel.Controls.Add(cartesianChart);
                        this.Controls.Add(tbpanel);
                        cnt += 1;
                        }
                    else
                    {
                        Panel panel = new Panel();
                        panel.BorderStyle = BorderStyle.FixedSingle;
                        panel.Dock = DockStyle.Fill;

                        tbpanel.Controls.Add(panel, i, j);

                        LiveCharts.WinForms.CartesianChart cartesianChart = new LiveCharts.WinForms.CartesianChart();
                        cartesianCharts.Add(cartesianChart);
                        cartesianChart.Dock = DockStyle.Fill;

                        panel.Controls.Add(cartesianChart);
                        this.Controls.Add(tbpanel);
                    }
                        
                    }
                }
            for (int i = 0; i < IDs.Count; i++) {
                var avg_label = new Label();
                var max_label = new Label();
                var min_label = new Label();

                var avg_textbox = new TextBox();
                var max_textbox = new TextBox();
                var min_textbox = new TextBox();

                avg_label.Text = "평균";
                max_label.Text = "최고";
                min_label.Text = "최소";

                //cartesianCharts.Add(cartesianChart);

                avgLabels.Add(avg_label);
                maxLabels.Add(max_label);
                minLabels.Add(min_label);

                avgTextBoxes.Add(avg_textbox);
                maxTextBoxes.Add(max_textbox);
                minTextBoxes.Add(min_textbox);
                /*
                this.Controls.Add(cartesianChart);
                this.Controls.Add(avg_label);
                this.Controls.Add(max_label);
                this.Controls.Add(min_label);
                this.Controls.Add(avg_textbox);
                this.Controls.Add(max_textbox);
                this.Controls.Add(min_textbox);
                */


            }
            if (cartesianCharts.Count >= 3) {
                            //cartesianChart.LegendLocation = LegendLocation.Bottom;
                /*
                            int y = 60;
                            int avg_x = x+40; int avg_y = Screen.PrimaryScreen.Bounds.Height / 2 - 80;
                            int max_x = x+310; int max_y = avg_y +5;
                            int min_x = x+580; int min_y = avg_y +5;

                            for (int i = 0; i < cartesianCharts.Count; i++)
                            {
                                avgLabels[i].SetBounds(avg_x, avg_y, 30, 50);
                                maxLabels[i].SetBounds(max_x, max_y, 30, 30);
                                minLabels[i].SetBounds(min_x, min_y, 30, 30);

                                avgTextBoxes[i].SetBounds(avg_x + 40, avg_y - 3, 150, 50);
                                maxTextBoxes[i].SetBounds(max_x + 40, max_y - 4, 140, 40);
                                minTextBoxes[i].SetBounds(min_x + 40, min_y - 4, 140, 40);
                                avgTextBoxes[i].BorderStyle = BorderStyle.None;
                                //avgTextBoxes[i].BackColor = Color.Transparent;
                                //cartesianCharts[i].SetBounds(x, y, Screen.PrimaryScreen.Bounds.Width/2 - 60, Screen.PrimaryScreen.Bounds.Height/2-180);
                                cartesianCharts[i].LegendLocation = LegendLocation.Top;
                                //avgLabels[0].SetBounds(x+40, Screen.PrimaryScreen.Bounds.Height / 2 - 85, 100, 50);

                                if (i == 0 || i % 2 == 0)
                                {
                                    x += Screen.PrimaryScreen.Bounds.Width/2 - 60;
                                    avg_x += Screen.PrimaryScreen.Bounds.Width / 2 - 60;
                                    max_x += Screen.PrimaryScreen.Bounds.Width / 2 - 60;
                                    min_x += Screen.PrimaryScreen.Bounds.Width / 2 - 60;
                                }
                                else {
                                    x = 60;
                                    avg_x = 100;
                                    max_x = 370;
                                    min_x = 370;
                                    max_y += Screen.PrimaryScreen.Bounds.Height / 2 - 50;
                                    min_y += Screen.PrimaryScreen.Bounds.Height / 2 - 50;
                                    avg_y += Screen.PrimaryScreen.Bounds.Height / 2 - 50;
                                    y += Screen.PrimaryScreen.Bounds.Height/2 - 50;
                                }
                            }*/
            }
            else if (cartesianCharts.Count == 2)
            {
                //int x = 60; //543;
                int y = 60;
                int avg_x = Screen.PrimaryScreen.Bounds.Width / 2 - 400; int avg_y = Screen.PrimaryScreen.Bounds.Height / 2 - 60;
                int max_x = Screen.PrimaryScreen.Bounds.Width / 2 - 100; int max_y = Screen.PrimaryScreen.Bounds.Height / 2 - 60;
                int min_x = Screen.PrimaryScreen.Bounds.Width / 2 + 200; int min_y = Screen.PrimaryScreen.Bounds.Height / 2 - 60;


                for (int i = 0; i < 2; i++)
                {
                    avgLabels[i].SetBounds(avg_x, avg_y, 30, 50);
                    maxLabels[i].SetBounds(max_x, max_y, 30, 50);
                    minLabels[i].SetBounds(min_x, min_y, 30, 50);

                    avgTextBoxes[i].SetBounds(avg_x + 40, avg_y - 4, 150, 50);
                    maxTextBoxes[i].SetBounds(max_x + 40, max_y - 4, 150, 50);
                    minTextBoxes[i].SetBounds(min_x + 40, min_y - 4, 150, 50);

                    //cartesianCharts[i].SetBounds(x, y, Screen.PrimaryScreen.Bounds.Width-130, Screen.PrimaryScreen.Bounds.Height/2 - 130);
                    //cartesianCharts[i].LegendLocation = LegendLocation.Top;
                    y += Screen.PrimaryScreen.Bounds.Height/2 - 50;
                    
                    avg_y += Screen.PrimaryScreen.Bounds.Height / 2 - 50;
                    max_y += Screen.PrimaryScreen.Bounds.Height / 2 - 50;
                    min_y += Screen.PrimaryScreen.Bounds.Height / 2 - 50;
                }

                //MessageBox.Show("You have just two charts to plot. Please implement them one under another.", "Message");
            }
            else if (cartesianCharts.Count == 1)
            {
                

            //   int x = 60; //543;
               //int y = 60;
                int avg_x = Screen.PrimaryScreen.Bounds.Width / 2 - 400; int avg_y = Screen.PrimaryScreen.Bounds.Height - 100;
                int max_x = Screen.PrimaryScreen.Bounds.Width / 2 - 100; int max_y = Screen.PrimaryScreen.Bounds.Height - 100;
                int min_x = Screen.PrimaryScreen.Bounds.Width / 2 + 200; int min_y = Screen.PrimaryScreen.Bounds.Height - 100;

                avgLabels[0].SetBounds(avg_x, avg_y, 30, 50);
                maxLabels[0].SetBounds(max_x, max_y, 30, 50);
                minLabels[0].SetBounds(min_x, min_y, 30, 50);

                avgTextBoxes[0].SetBounds(avg_x + 40, avg_y - 4, 150, 50);
                maxTextBoxes[0].SetBounds(max_x + 40, max_y - 4, 150, 50);
                minTextBoxes[0].SetBounds(min_x + 40, min_y - 4, 150, 50);

                //cartesianCharts[0].SetBounds(x, y, Screen.PrimaryScreen.Bounds.Width - 100, Screen.PrimaryScreen.Bounds.Height - 190);
                //cartesianCharts[0].LegendLocation = LegendLocation.Top;
                //MessageBox.Show("You have just a single chart to plot. Please implement it as a full screen chart.", "Message");
            }

            int numOfElmnt = CountNumOfElmnt(graphDataAll, ids, whatToShow); //데이터 개수 : number of Elements(temp)
            double[][] tempDblVals = new double[IDs.Count][]; // double형식의 데이터를 위한 임시 배열 생성
            Int64[][] tempIntVals = new Int64[IDs.Count][];   // int형식의 데이터를 위한 임시 배열 생성
            //실제 배열 생성
            for (int i = 0; i < IDs.Count; i++)
            {
                dblVals.Add(new GearedValues<double>());
                intVals.Add(new GearedValues<Int64>());
                tempDblVals[i] = new double[numOfElmnt];
                tempIntVals[i] = new Int64[numOfElmnt];
            }

            //var defaultTemp = new GearedValues<double>();
            // 차트 Legend 주소 설정하기
            for (int i = 0; i < IDs.Count; i++)
            {
                cartesianCharts[i].LegendLocation = LegendLocation.Bottom;
            }

            var tempTimeVal1 = new string[numOfElmnt];
            //var tempDefTemp = new double[numOfElmnt];
            Console.WriteLine("서버에서 불러오는 데이터 (행) 수량 (=Number of Rows Retrieved): {0}개 행 * {1}개 센서 = {2}", numOfElmnt, IDs.Count, numOfElmnt*IDs.Count);
            //실시간이 아닌 시간 간격 선택 시 불러운 데이터를 임시 배열에 담기기 //임시 배열 사용이유: 속도
            if (timeInterval[1].Contains("RT") == false)
            {
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
                timeVal.AddRange(tempTimeVal1);
            }
            //defaultTemp.AddRange(tempDefTemp);
            // 임시 배열에 있는 데이터를 dblVals 및 intVals (실제) 배열에 한번에 담기기
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
                xAxesFunc(cartesianCharts[index], timeVal);
            }
            if (whatToShow.Contains("temp")) {
                this.Text = "온도 시각화 화면";
                List<double> avgDbl = Avg(dblVals);
                List<double> maxDbl = Max(dblVals);
                List<double> minDbl = Min(dblVals);
                for (int i = 0; i < IDs.Count; i++) {
                    avgTextBoxes[i].Text = Math.Round(avgDbl[i], 2).ToString() + " °C";
                    maxTextBoxes[i].Text = Math.Round(maxDbl[i], 2).ToString() + " °C";
                    minTextBoxes[i].Text = Math.Round(minDbl[i], 2).ToString() + " °C";
                    yAxisFuncTmpHmd(cartesianCharts[i], "온도센서 (°C) " + IDs[i].ToString(), dblVals[i]);
                }
            }
            else if (whatToShow.Contains("humid"))
            {
                this.Text = "습도 시각화 화면";
                List<double> avgDbl = Avg(dblVals);
                List<double> maxDbl = Max(dblVals);
                List<double> minDbl = Min(dblVals);
                for (int i = 0; i < IDs.Count; i++)
                {
                    avgTextBoxes[i].Text = Math.Round(avgDbl[i], 2).ToString() + " %";
                    maxTextBoxes[i].Text = Math.Round(avgDbl[i], 2).ToString() + " %";
                    minTextBoxes[i].Text = Math.Round(minDbl[i], 2).ToString() + " %";
                    yAxisFuncTmpHmd(cartesianCharts[i], "습도센서 (%) " + IDs[i].ToString(), dblVals[i]);
                }
            }
            else if (whatToShow.Contains("part03"))
            {
                this.Text = "파티클 (0.3μm) 시각화 화면";
                List<Int64> avgInt = Avg(intVals);
                List<Int64> maxInt = Max(intVals);
                List<Int64> minInt = Min(intVals);
                for (int i = 0; i < IDs.Count; i++)
                {
                    avgTextBoxes[i].Text = String.Format("{0:n0}", avgInt[i]) + " 0.3μm";
                    maxTextBoxes[i].Text = String.Format("{0:n0}", maxInt[i]) + " 0.3μm";
                    minTextBoxes[i].Text = String.Format("{0:n0}", minInt[i]) + " 0.3μm";
                    yAxisFuncPcl(cartesianCharts[i], "파티클센서 (0.3μm) " + IDs[i].ToString(), intVals[i]);
                }
            }
            else if (whatToShow.Contains("part05"))
            {
                this.Text = "파티클 (0.5μm) 시각화 화면";
                List<Int64> avgInt = Avg(intVals);
                List<Int64> maxInt = Max(intVals);
                List<Int64> minInt = Min(intVals);
                for (int i = 0; i < IDs.Count; i++)
                {
                    avgTextBoxes[i].Text = String.Format("{0:n0}", avgInt[i]) + " 0.5μm";
                    maxTextBoxes[i].Text = String.Format("{0:n0}", maxInt[i]) + " 0.5μm";
                    minTextBoxes[i].Text = String.Format("{0:n0}", minInt[i]) + " 0.5μm";
                    yAxisFuncPcl(cartesianCharts[i], "파티클센서 (0.5μm) " + IDs[i].ToString(), intVals[i]);
                }
            }
            //모두 시각화 기능, 아직 작업중임.
            else
            {
                MessageBox.Show("Currently this feature is not implemented. ", "에러 매시지");
            }
            //timeVal 배열에 있는 데이터를 해당 차트의 X axis Values 부분에 추가하기 
           /* for (int i = 0; i < IDs.Count; i++)
            {
                xAxesFunc(cartesianCharts[i], timeVal);
            }*/
            Console.WriteLine("Finished");
        }

        /// <summary>
        /// 시각화하려는 데이터 갯수를 계산하는 함수
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
        /// <summary>
        /// double형식의 데이타의 평균 값을 계산하는 함수.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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
                //Console.WriteLine("\nAvgDbl[{0}]: {1}", h, total / data[h].Count);
            }
            return AvgArr;
        }
        /// <summary>
        /// Int형식의 데이터의 평균 값을 계산하는 함수.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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
                //Console.WriteLine("\nAvgInt[{0}]: {1}", h, total / data[h].Count);
            }
            return AvgArr;
        }
        /// <summary>
        /// double 형식의 데이터의 최고 값을 계산하는 함수.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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
                //Console.WriteLine("\nMaxDbl[{0}]: {1}", h, max);
            }
            return MaxArr;
        }
        /// <summary>
        /// int 형식의 데이터의 최고 값을 계산하는 함수.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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
                //Console.WriteLine("\nMaxDbl[{0}]: {1}", h, max);
            }
            return MaxArr;
        }
        /// <summary>
        /// double 형식의 데이터의 최소 값을 계산하는 함수
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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
               // Console.WriteLine("\nMaxDbl[{0}]: {1}", h, min);
            }
            return MinArr;
        }
        /// <summary>
        /// int 형식의 데이터의 최소 값을 계산하는 함수
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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
               // Console.WriteLine("\nMaxDbl[{0}]: {1}", h, min);
            }
            return MinArr;
        }
        /// <summary>
        /// 실시간 데이터 쿼리를 위한 쿼리 함수
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="whatToQuery"></param>
        /// <returns></returns>
        public List<List<string[]>> MyDataGetter(List<int> IDs, string whatToQuery)
        {
            List<List<string[]>> DataArr = new List<List<string[]>>();
            string sql_name = ""; // 데이터 변수명 

            if (whatToQuery.Contains("temp")) { sql_name = "Temperature"; }
            else if (whatToQuery.Contains("humid")) { sql_name = "Humidity"; }
            else if (whatToQuery.Contains("part03")) { sql_name = "Particle03"; }
            else { sql_name = "Particle05"; }
            
            string sql_head = "select sensor_id, " + sql_name + ", dateandtime from( "; 
            string sql_connector = " union all "; // 테이블 연결하는 것
            string sql_tail = " )a";

            for (int i = 0; i < IDs.Count; i++) {
                sql_head += "select " + IDs[i].ToString() + " as sensor_id, " + sql_name + ", dateandtime from dev_" + whatToQuery + "_" + IDs[i].ToString() + " where dateandtime = (select max(dateandtime) from dev_" + whatToQuery + "_" + IDs[i].ToString() + ") ";
                if (IDs.Count > 1 && i != (IDs.Count - 1)) { sql_head += sql_connector; }
            }
            sql_head += sql_tail;

            Console.WriteLine("sql_temp0" + sql_head);
            for (int i = 0; i < IDs.Count; i++)
            {
                DataArr.Add(new List<string[]>());
            }

            // 사용 가능한 센서 ID 조회하기
            try
            {
                //SqlConnection myConnection = new SqlConnection(@"Data Source=DESKTOP-DLIT\SQLEXPRESS;Initial Catalog=SensorDataDB;Integrated Security=True");
                SqlConnection myConnection = new SqlConnection(@"Data Source=10.1.55.174;Initial Catalog=SensorDataDB;User id=dlitdb;Password=dlitdb; Min Pool Size=20");
                myConnection.Open();
                
                using (var cmd = new SqlCommand(sql_head, myConnection))
                {
                    using (var myReader = cmd.ExecuteReader())
                    {
                        int i = 0;
                        while (myReader.Read())
                        {
                            string[] myobj = { myReader.GetValue(1).ToString(), myReader.GetValue(2).ToString() };
                            DataArr[i].Add(myobj);
                            i += 1;
                        }
                    }
                }
                myConnection.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString(), "에러 매시지");
            }
            return new List<List<string[]>>(DataArr);
        }
        /// <summary>
        /// 실시간 시각화를 위한 타이머 세팅. 1초 시간 간격
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("\n\nIDs.Count {0}, dblVals.Count {1}, intVals.Count {2}\n\n", IDs.Count, dblVals.Count, intVals.Count);
            data_RT = MyDataGetter(IDs, WhatToShow);
            timeVal.Add(Convert.ToString(data_RT[0][0][1]));

                for (int index = 0; index < IDs.Count; index++) {
                    if (WhatToShow.Contains("temp") || WhatToShow.Contains("humid")) {
                    dblVals[index].Add(Math.Round(Convert.ToSingle(data_RT[index][0][0]), 2));
                    if (dblVals[index].Count > 1000 || dblVals[index][0] == 0) { dblVals[index].RemoveAt(0); }
                        
                    }
                    else {
                    intVals[index].Add(Int64.Parse(data_RT[index][0][0], NumberStyles.Any, new CultureInfo("en-au")));
                    if (intVals[index].Count > 1000 || intVals[index][0] == 0) { intVals[index].RemoveAt(0); }
                        
                    }
                }
            timer1.Interval = 1000; //1초 시간 간격 
        }
    }
}

