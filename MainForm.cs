using LiveCharts;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualizerApp_3;

namespace VisualizerApp
{
    public partial class MainForm : Form
    {
        public string startTime = "";
        public string endTime = "";
        public Button[] Btn1_time;
        public Button[] Btn2_data;
        public Button[] Btn3_address;
        public List<int> IDs_available = new List<int>();
        public List<int> IDs_selected = new List<int>();
        public string whatToShow;
        public string titleName = "";
        public double[] realtime_values { get; set; }
        public double[] realtime_date { get; set; }

        public List<string> myObjectList { get; set; }
        public MainForm()
        {
            InitializeComponent();
            //this.SetBounds(0, 0, Screen.PrimaryScreen.Bounds.Width-10, Screen.PrimaryScreen.Bounds.Height-40);
            this.SetBounds(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height - 100);
            this.AutoScroll = true;
            datePicker1_start.Format = DateTimePickerFormat.Custom;
            datePicker1_start.CustomFormat = "yyyy-MM-dd HH:mm";
            datePicker2_end.Format = DateTimePickerFormat.Custom;
            datePicker2_end.CustomFormat = "yyyy-MM-dd HH:mm";
            Btn1_time = new Button[] { button1_realtime, button1_24h, button1_datepicker };
            Btn2_data = new Button[] { button2_temp, button2_humid, button2_part03, button2_part05 };
            Btn3_address = new Button[] { button3_solbi1, button3_solbi2, button3_solbi3, button3_solbi4 };
            foreach (var button in Btn3_address)
            { button.Enabled = false; }
            IDs_AvailCheck(); // 시각화 하려는 센서 ID 조회 및 배열에 ID번호 추가하기







        }
        private void ScotPlot(List<List<List<string[]>>> dataArr, string whatToShow, List<int> IDs, bool RT_flag)
        {


            int numOfElmnt = CountNumOfElmnt(dataArr, IDs, whatToShow); //데이터 개수 : number of Elements(temp)
            string[] dataVal = new string[numOfElmnt];
            string[] timeVal = new string[numOfElmnt];
            panel2_ChartArea.Controls.Clear();

            /*this.Controls.Add(panel1);
            this.Controls.Add(panel2);*/
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            panel2_ChartArea.Controls.Add(tableLayoutPanel);
            tableLayoutPanel.Dock = DockStyle.Fill;




            List<FormsPlot> formsPlots = new List<FormsPlot>();

            string[][] dataArr2 = new string[IDs.Count][]; //dataArr[0][0].Count
            string[][] timeArr2 = new string[IDs.Count][];


            for (int i = 0; i < IDs.Count; i++) { dataArr2[i] = new string[numOfElmnt]; timeArr2[i] = new string[numOfElmnt]; }

            if (IDs.Count < 2)
            {
                tableLayoutPanel.RowCount = 1;
                tableLayoutPanel.ColumnCount = 1;
                tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            }
            else if (IDs.Count == 2)
            {
                tableLayoutPanel.RowCount = 2;
                tableLayoutPanel.ColumnCount = 1;
                tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));

            }
            else if (IDs.Count > 2)
            {

                tableLayoutPanel.RowCount = 2;
                tableLayoutPanel.ColumnCount = 2;
                tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            }

            for (int i = 0; i < tableLayoutPanel.ColumnCount; i++)
            {
                for (int j = 0; j < tableLayoutPanel.RowCount; j++)
                {
                    int cnt = 0;
                    if (IDs.Count % 2 == 1 && cnt == 0)
                    {
                        Panel panel = new Panel();
                        panel.BorderStyle = BorderStyle.FixedSingle;
                        panel.Dock = DockStyle.Fill;

                        tableLayoutPanel.Controls.Add(panel, i, j);

                        FormsPlot formsPlot = new FormsPlot();
                        formsPlot.Dock = DockStyle.Fill;
                        tableLayoutPanel.Controls.Add(formsPlot);
                        //cartesianCharts.Add(cartesianChart);
                        formsPlots.Add(formsPlot);
                        panel.Controls.Add(formsPlot);
                        //this.Controls.Add(tableLayoutPanel);
                        cnt += 1;
                    }
                    else
                    {
                        Panel panel = new Panel();
                        panel.BorderStyle = BorderStyle.FixedSingle;
                        panel.Dock = DockStyle.Fill;

                        tableLayoutPanel.Controls.Add(panel, i, j);

                        FormsPlot formsPlot = new FormsPlot();
                        formsPlot.Dock = DockStyle.Fill;
                        tableLayoutPanel.Controls.Add(formsPlot);
                        formsPlots.Add(formsPlot);
                        panel.Controls.Add(formsPlot);
                        //this.Controls.Add(tableLayoutPanel);
                    }

                }
            }



            for (int i = 0; i < IDs.Count; i++)
            {

                var plt = formsPlots[i]; // new ScottPlot.Plot(600, 400);

                for (int j = 0; j < numOfElmnt; j++)
                {
                    dataArr2[i][j] = dataArr[0][i][j][0];
                    timeArr2[i][j] = dataArr[0][i][j][1];
                    //dataVal[j] = dataArr[0][0][j][0];
                }


                timeVal[0] = dataArr[0][0][0][1];
                //if(whatToShow.Contains("temp") || whatToShow.Contains("humid")) { }
                Console.WriteLine("num of data points: {0}", numOfElmnt);
                double[] ys = dataArr2[i].Select(x => double.Parse(x)).ToArray();
                DateTime[] timeData = timeArr2[i].Select(x => DateTime.Parse(x)).ToArray();
                double[] xs = timeData.Select(x => x.ToOADate()).ToArray();
                realtime_date = ys;
                realtime_values = xs;

                if (RT_flag == true)
                {
                    formsPlots[i].plt.PlotSignalXYConst(realtime_date, realtime_values, lineStyle: LineStyle.Dot);            // Signal Chart
                }
                else
                {
                    formsPlots[i].plt.PlotSignalXYConst(xs, ys, lineStyle: LineStyle.Dot);                                    // Signal Chart
                }
                // CHARTING Functions
                //var sig = formsPlots[i].plt.PlotSignal(ys, sampleRate: 24*60, xOffset: xs[0]);     //                   --> Needs bug fixes
                //formsPlots[i].plt.PlotScatter(xs, ys, lineWidth: 0);                          // Scatter Chart

                //formsPlots[i].plt.PlotStep(xs, ys);                                    // Step Chart
                //formsPlots[i].plt.PlotFill(xs, ys);                                    // Fill Chart
                //formsPlots[i].plt.PlotScatterHighlight(xs, ys);                          // ScatterHighlight
                //formsPlots[i].plt.PlotPolygon(Tools.Pad(xs, cloneEdges: true), Tools.Pad(ys));

                //formsPlots[i].plt.Grid(enable: false);      //Enable-Disable Gridn
                formsPlots[i].plt.Ticks(dateTimeX: true); //formsPlot1.

                //sig.minRenderIndex = 4000;
                //sig.maxRenderIndex = 5000;
                /*sig.fillType = FillType.FillBelow;
                sig.fillColor1 = Color.Blue;
                sig.gradientFillColor1 = Color.Transparent;*/

                if (whatToShow.Contains("temp")) { titleName = "온도(°C)"; }
                else if (whatToShow.Contains("humid")) { titleName = "습도(%)"; }
                else if (whatToShow.Contains("part03")) { titleName = "파티클(0.3μm)"; }
                else { titleName = "파티클(0.5μm)"; }
                formsPlots[i].plt.PlotAnnotation("설비 " + IDs[i].ToString(), 10, 10, fontSize: 18);

                formsPlots[i].plt.Title(titleName + " 센서 데이터 시각화"); // formsPlot1.
                formsPlots[i].plt.YLabel(titleName); // formsPlot1.
                formsPlots[i].plt.XLabel("시간");
                formsPlots[i].plt.SaveFig(titleName + "_" + i.ToString() + "_" + DateTime.Now.ToString("MMdd_HHmm") + ".png");
                formsPlots[i].Render();

            }

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
        /// Function to display results in form of chart for the selected time interval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //보기 버튼 누를 때에의 행위
        private void show_button_Click(object sender, EventArgs e)
        {

            if (button1_realtime.BackColor != Color.Transparent || button1_24h.BackColor != Color.Transparent || button1_datepicker.BackColor != Color.Transparent)
            {
                MyDataQuery myDataQuery = new MyDataQuery();
                List<List<List<string[]>>> DataRetrieved_general = new List<List<List<string[]>>>();
                endTime = "RT";
                string[] timeInterval = { startTime, endTime };
                if (button1_realtime.BackColor != Color.Transparent && IDs_selected.Count > 0)
                {
                    Console.WriteLine("Now RealTime");
                    /*timer1.Enabled = true;
                    timer1.Start();*/
                    IDs_selected.Sort();
                    timeInterval[0] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    timeInterval[1] = "RT";
                    List<List<List<string[]>>> DataRetrieved_RT = new List<List<List<string[]>>>();
                    List<List<string[]>> rtData = myDataQuery.RealTimeDBQuery(IDs_selected, whatToShow);
                    DataRetrieved_RT.Add(rtData);
                    //ScotPlot(DataRetrieved_RT, whatToShow, IDs_selected, true);
                    RealTimeCharting(DataRetrieved_RT, timeInterval, IDs_selected, whatToShow);

                    //NewChartingForm RTChart = new NewChartingForm(DataRetrieved_RT, timeInterval, IDs_selected, whatToShow);
                    //RTChart.Show();
                }
                else if (button1_24h.BackColor != Color.Transparent && IDs_selected.Count > 0)
                {
                    Console.WriteLine("Now 24H");
                    IDs_selected.Sort();
                    startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm");
                    endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    DataRetrieved_general.Add(myDataQuery.DBQuery(whatToShow, startTime, endTime, IDs_selected, whatToShow));
                    ScotPlot(DataRetrieved_general, whatToShow, IDs_selected, false);
                    //ChartingForm GeneralChart = new ChartingForm(DataRetrieved_general, timeInterval, IDs_selected, whatToShow);
                    //NewChartingForm GeneralChart = new NewChartingForm(DataRetrieved_general, timeInterval, IDs_selected, whatToShow);
                    //GeneralChart.Show();
                }
                else if (button1_datepicker.BackColor != Color.Transparent && IDs_selected.Count > 0)
                {
                    Console.WriteLine("Now from " + startTime + " to " + endTime);
                    IDs_selected.Sort();
                    if (datePicker1_start.Value < datePicker2_end.Value)
                    {
                        startTime = datePicker1_start.Value.ToString("yyyy-MM-dd HH:mm");
                        endTime = datePicker2_end.Value.ToString("yyyy-MM-dd HH:mm");
                        Console.WriteLine(startTime + " " + endTime);
                        DataRetrieved_general.Add(myDataQuery.DBQuery(whatToShow, startTime, endTime, IDs_selected, whatToShow));

                        Console.WriteLine("Lens {0} {1}", DataRetrieved_general[0], DataRetrieved_general[0][0]);
                        //////////////////// new Chart ScotPlot ////////////////////////////////
                        ScotPlot(DataRetrieved_general, whatToShow, IDs_selected, false);
                        //ChartingForm GeneralChart = new ChartingForm(DataRetrieved_general, timeInterval, IDs_selected, whatToShow);
                        //NewChartingForm GeneralChart = new NewChartingForm(DataRetrieved_general, timeInterval, IDs_selected, whatToShow);
                        //GeneralChart.Show();
                    }
                    else
                    {
                        MessageBox.Show("잘못된 날짜가 선택되었습니다. 확인해 보세요!", "에러 메시지");
                    }
                }
                else
                {
                    MessageBox.Show("센서 설치 주소를 선택하지 않으셨습니다.", "에러 매시지");
                }

            }
            else { MessageBox.Show("시간을 선택하지 않으셨습니다. 실시간 또는 기간 설정을 해 주세요!", "에러 메시지"); }

            foreach (var item in IDs_selected)
            {
                Console.WriteLine("IDs selected: {0} ", item);
            }
        }
        private void IDs_AvailCheck()
        {
            IDs_available.Clear();
            try
            {
                SqlConnection myConnection = new SqlConnection(@"Data Source=10.1.55.174;Initial Catalog=SensorDataDB;User id=dlitdb;Password=dlitdb; Min Pool Size=20");
                string sql_getIDs = "SELECT * FROM SensorDataDB.dbo.SENSOR_INFO a WHERE a.Usage = 'YES'";

                using (var cmd = new SqlCommand(sql_getIDs, myConnection))
                {
                    myConnection.Open();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            while (myReader.Read())
                            {
                                string[] rowInfo = { myReader.GetValue(0).ToString(), myReader.GetValue(1).ToString(), myReader.GetValue(3).ToString() };
                                Btn3_address[Convert.ToInt32(rowInfo[0]) - 1].Enabled = true;
                                IDs_available.Add(Convert.ToInt32(rowInfo[0]));
                            }
                        }
                        else
                        {
                            Console.WriteLine("조회할 데이터가 없습니다.");
                        }
                    }
                    myConnection.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "에러 매시지"); }
        }
        /// <summary>
        /// 똑같은 장이 1회 이상 열리지 않게 막는 함수.
        /// </summary>
        /// <param name="all"></param>
        /// <param name="one"></param>
        /// <returns></returns>
        private bool EqualityChecker(List<string[]> all, string[] one)
        {
            if (all.Count > 0)
            {
                for (int i = 0; i < all.Count; i++)
                {
                    if (all[i][0].SequenceEqual(one[0]) && all[i][1].SequenceEqual(one[1]) && all[i][2].SequenceEqual(one[2]) && all[i][3].SequenceEqual(one[3]))
                    {
                        return false;
                    }
                }
                return true;
            }
            else { return true; }
        }
        //간편한 시간 간격 선택 시 button.BackColor를 다른색으로 표시해 주는 함수
        private void buttonSelector(Button[] btnNames, int index, Color color)
        {
            for (int i = 0; i < btnNames.Length; i++)
            {
                if (i == index)
                {
                    btnNames[i].BackColor = color;

                }
                else
                {
                    btnNames[i].BackColor = Color.Transparent;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            buttonSelector(Btn1_time, 0, Color.Chartreuse);
            datePicker1_start.Visible = false;
            datePicker2_end.Visible = false;
            label1_from.Visible = false;
            label2_end.Visible = false;
            //LinkLabelVisited(5);
            datePicker1_start.Value = DateTime.Now;
            datePicker2_end.Value = DateTime.Now;
            endTime = "RT";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            buttonSelector(Btn1_time, 1, Color.Chartreuse);

            datePicker1_start.Visible = false;
            datePicker2_end.Visible = false;
            label1_from.Visible = false;
            label2_end.Visible = false;
            //LinkLabelVisited(2);
            startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm");
            datePicker1_start.Value = Convert.ToDateTime(startTime);
            endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            datePicker2_end.Value = Convert.ToDateTime(endTime);
            Console.WriteLine(startTime + " " + endTime);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            buttonSelector(Btn1_time, 2, Color.Chartreuse);
            datePicker1_start.Visible = true;
            datePicker2_end.Visible = true;
            label1_from.Visible = true;
            label2_end.Visible = true;
            startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            datePicker1_start.Value = Convert.ToDateTime(startTime);
            endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            datePicker2_end.Value = Convert.ToDateTime(endTime);

        }

        private void button4_temp_Click(object sender, EventArgs e)
        {
            buttonSelector(Btn2_data, 0, Color.Chartreuse);
            whatToShow = "temp";
        }

        private void button5_humid_Click(object sender, EventArgs e)
        {

            buttonSelector(Btn2_data, 1, Color.Chartreuse);
            whatToShow = "humid";
        }

        private void button6_part03_Click(object sender, EventArgs e)
        {
            buttonSelector(Btn2_data, 2, Color.Chartreuse);
            whatToShow = "part03";
        }

        private void button6_part05_Click(object sender, EventArgs e)
        {
            buttonSelector(Btn2_data, 3, Color.Chartreuse);
            whatToShow = "part05";
        }

        private void button7_solbi1_Click(object sender, EventArgs e)
        {
            if (IDs_available.Contains(1) == false)
            {
                MessageBox.Show("조회 불가한 센서 ID입니다.");
            }
            else
            {
                if (IDs_selected.Contains(1) == false) // button3_solbi1.BackColor == Color.Transparent
                {
                    IDs_selected.Add(1);
                    button3_solbi1.BackColor = Color.Chartreuse;
                }
                else
                {
                    IDs_selected.Remove(1);
                    button3_solbi1.BackColor = Color.Transparent;
                }
            }
        }

        private void button8_solbi2_Click(object sender, EventArgs e)
        {
            if (IDs_available.Contains(2) == false)
            {
                MessageBox.Show("조회 불가한 센서 ID입니다.");
            }
            else
            {
                if (IDs_selected.Contains(2) == false)
                {
                    IDs_selected.Add(2);
                    button3_solbi2.BackColor = Color.Chartreuse;
                }
                else
                {
                    IDs_selected.Remove(2);
                    button3_solbi2.BackColor = Color.Transparent;
                }
            }
        }

        private void button9_solbi3_Click(object sender, EventArgs e)
        {
            if (IDs_available.Contains(3) == false)
            {
                MessageBox.Show("조회 불가한 센서 ID입니다.");
            }
            else
            {

                if (IDs_selected.Contains(3) == false)
                {
                    IDs_selected.Add(3);
                    button3_solbi3.BackColor = Color.Chartreuse;
                }
                else
                {
                    IDs_selected.Remove(3);
                    button3_solbi3.BackColor = Color.Transparent;
                }
            }
        }

        private void button10__solbi4_Click(object sender, EventArgs e)
        {
            if (IDs_available.Contains(4) == false)
            {
                MessageBox.Show("조회 불가한 센서 ID입니다.", "에러 매시지");
            }
            else
            {
                if (IDs_selected.Contains(4) == false)  //button3_solbi4.BackColor == Color.Transparent
                {
                    IDs_selected.Add(4);
                    button3_solbi4.BackColor = Color.Chartreuse;
                }
                else
                {
                    IDs_selected.Remove(4);
                    button3_solbi4.BackColor = Color.Transparent;
                }
            }
        }

       /* private void timer1_Tick(object sender, EventArgs e)
        {


            MyDataQuery myDataQuery = new MyDataQuery();
            List<List<List<string[]>>> DataRetrieved_RT = new List<List<List<string[]>>>();
            List<List<string[]>> rtData = myDataQuery.RealTimeDBQuery(IDs_selected, whatToShow);
            DataRetrieved_RT.Add(rtData);
            //ScotPlot(DataRetrieved_RT, whatToShow, IDs_selected, true);
            for (int i = 0; i < IDs_selected.Count; i++)
            {

                string[][] dataArr2 = DataRetrieved_RT[0][i].Select(x => x.ToArray()).ToArray();

                string[] dataArr3 = dataArr2[i];
            }
        }*/


//    RealTime Viz function props



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

        //          RealTime CHARTING FUNCTION HERE
        /// <summary>
        /// REAL-TIME CHARTING FUNCTION
        /// </summary>
        /// <param name="graphDataAll"></param>
        /// <param name="timeInterval"></param>
        /// <param name="ids"></param>
        /// <param name="whatToShow"></param>
        public void RealTimeCharting(List<List<List<string[]>>> graphDataAll, string[] timeInterval, List<int> ids, string whatToShow)
        {
            IDs = ids; // 시각화 하려는 센서들의 ID가 들어가 있는 배열
            WhatToShow = whatToShow; // 시각화 하려는 데이터 구분(온습도 또는 파티클)
            List<LiveCharts.WinForms.CartesianChart> cartesianCharts = new List<LiveCharts.WinForms.CartesianChart>();
            
            TableLayoutPanel tbpanel = new TableLayoutPanel(); //테이블레이아웃패널 생성
            tbpanel.Dock = DockStyle.Fill;
            //tbpanel.BringToFront();
            panel2_ChartArea.Controls.Add(tbpanel);

            if (IDs.Count < 2)
            {
                tbpanel.RowCount = 1;
                tbpanel.ColumnCount = 1;
                tbpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tbpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            }
            else if (IDs.Count == 2)
            {
                tbpanel.RowCount = 2;
                tbpanel.ColumnCount = 1;
                tbpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tbpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tbpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));

            }
            else if (IDs.Count > 2)
            {

                tbpanel.RowCount = 2;
                tbpanel.ColumnCount = 2;
                tbpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tbpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tbpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                tbpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            }

            //실시간 시각화 구분, if문은 시간 간격 시각화, else문은 실시간 시각화
            if (timeInterval[1].Contains("RT") == false)
            {
                
            }
            else
            {
                timer1.Enabled = true;
                timer1.Start();
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
                    if (IDs.Count % 2 == 1 && cnt == 0)
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
            for (int i = 0; i < IDs.Count; i++)
            {
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
            if (cartesianCharts.Count >= 3)
            {
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
                int x = 60; //543;
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
                    y += Screen.PrimaryScreen.Bounds.Height / 2 - 50;

                    avg_y += Screen.PrimaryScreen.Bounds.Height / 2 - 50;
                    max_y += Screen.PrimaryScreen.Bounds.Height / 2 - 50;
                    min_y += Screen.PrimaryScreen.Bounds.Height / 2 - 50;
                }

                //MessageBox.Show("You have just two charts to plot. Please implement them one under another.", "Message");
            }
            else if (cartesianCharts.Count == 1)
            {


                int x = 60; //543;
                int y = 60;
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
            Console.WriteLine("서버에서 불러오는 데이터 (행) 수량 (=Number of Rows Retrieved): {0}개 행 * {1}개 센서 = {2}", numOfElmnt, IDs.Count, numOfElmnt * IDs.Count);
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
            if (whatToShow.Contains("temp"))
            {
                this.Text = "온도 시각화 화면";
                List<double> avgDbl = Avg(dblVals);
                List<double> maxDbl = Max(dblVals);
                List<double> minDbl = Min(dblVals);
                for (int i = 0; i < IDs.Count; i++)
                {
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

            for (int i = 0; i < IDs.Count; i++)
            {
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

            for (int index = 0; index < IDs.Count; index++)
            {
                if (WhatToShow.Contains("temp") || WhatToShow.Contains("humid"))
                {
                    dblVals[index].Add(Math.Round(Convert.ToSingle(data_RT[index][0][0]), 2));
                    if (dblVals[index].Count > 1000 || dblVals[index][0] == 0) { dblVals[index].RemoveAt(0); }

                }
                else
                {
                    intVals[index].Add(Int64.Parse(data_RT[index][0][0], NumberStyles.Any, new CultureInfo("en-au")));
                    if (intVals[index].Count > 1000 || intVals[index][0] == 0) { intVals[index].RemoveAt(0); }

                }
            }
            timer1.Interval = 1000; //1초 시간 간격 
        }














    }
    /// <summary>
    /// 데이터 쿼리를 위한 만든 클래스
    /// </summary>
    public class MyDataQuery
        {
            /// <summary>
            /// 데이터 쿼리 함수
            /// </summary>
            /// <param name="startDate"></param>
            /// <param name="endDate"></param>
            /// <returns></returns>
            public virtual List<List<string[]>> DBQuery(string what, string startDate, string endDate, List<int> IDs, string whatToQuery)
            {
                List<List<string[]>> DataArr = new List<List<string[]>>();

                string sql_name = ""; // 데이터 변수명 
                                      //string whatToQuery = "temp";

                if (whatToQuery.Contains("temp")) { sql_name = "Temperature"; }
                else if (whatToQuery.Contains("humid")) { sql_name = "Humidity"; }
                else if (whatToQuery.Contains("part03")) { sql_name = "Particle03"; }
                else { sql_name = "Particle05"; }

                string sql_head = "select sensor_id, " + sql_name + ", dateandtime from( ";
                string sql_connector = " union all "; // 테이블 연결하는 것
                string sql_tail = " )a order by dateandtime";

                for (int i = 0; i < IDs.Count; i++)
                {
                    sql_head += "select " + IDs[i].ToString() + " as sensor_id, " + sql_name + ", dateandtime from dev_" + whatToQuery + "_" + IDs[i].ToString() + " where dateandtime >= '" + startDate + "' and  dateandtime <= '" + endDate + "'";
                    if (IDs.Count > 1 && i != (IDs.Count - 1)) { sql_head += sql_connector; }
                }
                sql_head += sql_tail;

                Console.WriteLine("SQL query: " + sql_head);





                for (int i = 0; i < IDs.Count; i++)
                {
                    DataArr.Add(new List<string[]>());
                }
                try
                {
                    //SqlConnection myConnection = new SqlConnection(@"Data Source=DESKTOP-DLIT\SQLEXPRESS;Initial Catalog=SensorDataDB;Integrated Security=True");
                    SqlConnection myConnection = new SqlConnection(@"Data Source=10.1.55.174;Initial Catalog=SensorDataDB;User id=dlitdb;Password=dlitdb; Min Pool Size=20");

                    //센서 ID와 데이터 (온,습도,타티클03, 05), 그리고 산텍된 시간 간격(interval)에 따른 퀴리하기
                    /*for (int i=0; i<IDs.Count; i++) 
                        { 
                        if(what.Contains("temp")) { sql = "select * from SensorDataDB.dbo.DEV_TEMP_" + IDs[i].ToString() + " a where CONVERT(datetime, DateAndTime) >= '" + startDate + "' and CONVERT(datetime, DateAndTime) <= '" + endDate + "' order by DateAndTime ASC";
                        }
                        else if (what.Contains("humid")) { sql = "select * from SensorDataDB.dbo.DEV_HUMID_" + IDs[i].ToString() + " a where DateAndTime >= '" + startDate + "' and DateAndTime <= '" + endDate + "' order by DateAndTime ASC"; }
                        else if (what.Contains("part03")) { sql = "select * from SensorDataDB.dbo.DEV_PART03_" + IDs[i].ToString() + " a where DateAndTime >= '" + startDate + "' and DateAndTime <= '" + endDate + "' order by DateAndTime ASC"; }
                        else { sql = "select * from SensorDataDB.dbo.DEV_PART05_" + IDs[i].ToString() + " a where DateAndTime >= '" + startDate + "' and DateAndTime <= '" + endDate + "' order by DateAndTime ASC"; }
                        Console.WriteLine(sql);*/

                    using (var cmd = new SqlCommand(sql_head, myConnection))
                    {
                        myConnection.Open();
                        using (var myReader = cmd.ExecuteReader())
                        {
                            int i = 0;
                            while (myReader.Read())
                            {
                                //각 배열(array) 변수에 2가지 데이터가 들어가 있어서, 0과 1인덕스만 불러우면 된다. 
                                //0은 데이터, 1은 시간임.
                                if (i == IDs.Count) { i = 0; }
                                string[] myObj = { myReader.GetValue(1).ToString(), myReader.GetValue(2).ToString() };
                                //Console.WriteLine("{0}", (int)myReader.GetValue(0) - 1);
                                string[] all = { myReader.GetValue(0).ToString(), myReader.GetValue(1).ToString(), myReader.GetValue(2).ToString() };
                                DataArr[i].Add(myObj);
                                i += 1;
                            }
                        }
                        myConnection.Close();
                    }
                    //}
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString(), "에러 매시지");
                }
                return new List<List<string[]>>(DataArr);
            }
            /// <summary>
            /// 실시간 데이터 쿼리를 위한 쿼리 함수
            /// </summary>
            /// <param name="IDs"></param>
            /// <param name="whatToQuery"></param>
            /// <returns="List<List<string[]>> DataArr "></returns>
            public List<List<string[]>> RealTimeDBQuery(List<int> IDs, string whatToQuery)
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

                for (int i = 0; i < IDs.Count; i++)
                {
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
                    MessageBox.Show(ee.ToString());
                }
                return new List<List<string[]>>(DataArr);
            }
        }

    
}

