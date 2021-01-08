using ScottPlot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;

namespace DataVisualizerApp
{
    public partial class MainForm : Form
    {
        public string startTime = "";
        public string endTime = "";
        public string dbServerAddress = "";
        public string dbName = "";
        public string dbUID = "";
        public string dbPWD = "";

        public Button[] Btn1_time { get; set; }
        public Button[] Btn2_DataType = new Button[4];
        public Button[] Btn3_SensorLocation { get; set; }
        public Button button_show = new Button()
        {
            FlatAppearance = {
                BorderSize = 0,
                            MouseDownBackColor = Color.Transparent,
                            MouseOverBackColor = Color.Transparent,
                            BorderColor = Color.White },
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Transparent
        };
        public Button Btn_MinimizeMenuPanel = new Button();
        public Panel panel4peakVal = new Panel();

        public List<string> SensorLocation = new List<string>();
        public List<int> IDs_now = new List<int>();
        public List<int> IDs_next = new List<int>();
        public List<string> DataTypesNow = new List<string>();
        public List<string> DataTypesNext = new List<string>();
        public List<string> Sql_NamesNow = new List<string>();
        public string titleName = "";
        public List<List<TextBox>> RT_textBoxes = new List<List<TextBox>>();

        public List<Label> PeakValLabels = new List<Label>();
        public Color[] colorset { get; set; }
        public bool digital_flag { get; set; }

        public List<List<List<double[]>>> RTDataArray = new List<List<List<double[]>>>();
        public List<List<List<string[]>>> RT_Max = new List<List<List<string[]>>>();
        public List<List<List<string[]>>> RT_Min = new List<List<List<string[]>>>();

        List<SortedDictionary<int, double[]>> RTDataArray2 { get; set; }
        List<SortedDictionary<int, string[]>> RT_Max2 { get; set; }
        List<SortedDictionary<int, string[]>> RT_Min2 { get; set; }


        int nextDataIndex = 1;
        public List<FormsPlot> formsPlots { get; set; }
        public List<PlottableSignal> plts = new List<PlottableSignal>();
        public PlottableSignal signalPlot;
        public List<PlottableAnnotation> plottableAnnotationsMaxVal = new List<PlottableAnnotation>();
        public List<PlottableAnnotation> plottableAnnotationsMinVal = new List<PlottableAnnotation>();

        private static Thread progressbarThread;
        public Bitmap btnClicked_big = DataVisualizerApp.Properties.Resources._05;
        public Bitmap btnUnClicked_big = DataVisualizerApp.Properties.Resources._04;
        public Bitmap btnClicked_small = DataVisualizerApp.Properties.Resources.btn_sm7;
        public Bitmap btnUnClicked_small = DataVisualizerApp.Properties.Resources.btn_sm6;


        public MainForm()
        {
            InitializeComponent();

            // Initialize DB access variables
            dbServerAddress = "127.0.0.1";    //"10.1.55.174";
            dbName = "SensorDataDB";
            dbUID = "dlitdb01";
            dbPWD = "dlitdb01";

            //new SqlConnection(@"Data Source=10.1.55.174;Initial Catalog=SensorDataDB;User id=dlitdb;Password=dlitdb; Min Pool Size=20");

            this.SetBounds(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height - 50);
            this.AutoScroll = true;
            datePicker1_start.Format = DateTimePickerFormat.Custom;
            datePicker1_start.CustomFormat = "yyyy-MM-dd HH:mm";
            datePicker2_end.Format = DateTimePickerFormat.Custom;
            datePicker2_end.CustomFormat = "yyyy-MM-dd HH:mm";

            colorset = new Color[] { Color.Black, Color.DarkOrange, Color.Blue, Color.Green };
            Btn1_time = new Button[] { button1_realtime, button1_24h, button1_datepicker };
            // { button2_temp, button2_humid, button2_part03, button2_part05 };

            int x_btn = Btn1_time[0].Bounds.X;
            int y_btn = Btn1_time[0].Bounds.Y + Btn1_time[0].Bounds.Height * 2;

            for (int index_btn2 = 0; index_btn2 < 4; index_btn2++)
            {
                Button button = new Button()
                {
                    FlatAppearance = {
                            BorderSize = 0,
                            MouseDownBackColor = Color.Transparent,
                            MouseOverBackColor=Color.Transparent,
                            BorderColor=Color.White },
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Transparent,
                    Image = btnUnClicked_small
                };
                button.Name = (index_btn2 == 0) ? "temp" : ((index_btn2 == 1) ? "humid" : ((index_btn2 == 2) ? "part03" : "part05"));
                button.Text = (index_btn2 == 0) ? "온도" : ((index_btn2 == 1) ? "습도" : ((index_btn2 == 2) ? "파티클(0.3)" : "파티클(0.5)"));
                button.Font = new Font(button.Font.FontFamily, 12);
                button.SetBounds(x_btn, y_btn, Btn1_time[0].Bounds.Width * 3 / 4, Btn1_time[0].Bounds.Height);
                x_btn += ((Btn1_time[2].Bounds.X + Btn1_time[2].Bounds.Width) / 4);
                button.Click += new EventHandler(this.btn2_data_Click);
                panel1_menu.Controls.Add(button);
                button.Visible = false;
                Btn2_DataType[index_btn2] = button;

            }





            //(시각화) 보기 버튼 생성


            Btn_MinimizeMenuPanel.Text = "<";
            Btn_MinimizeMenuPanel.SetBounds(panel1_menu.Bounds.Width - 15, 0, 15, 20);
            Btn_MinimizeMenuPanel.Click += new EventHandler(this.MinimizeMenuPanel_Click);
            Btn_MinimizeMenuPanel.Dock = DockStyle.Right;
            toolTip1.SetToolTip(Btn_MinimizeMenuPanel, "선택메누 화면 숨기기");  // 마우스 포인팅 시 관련 내용 표시
            panel1_menu.Controls.Add(Btn_MinimizeMenuPanel);

            // Panel for peak values under the select menu
            //panel4peakVal.BackColor = Color.Transparent;
            panel4peakVal.SetBounds(15, 569, Btn_MinimizeMenuPanel.Bounds.X, 349);
            panel4peakVal.BorderStyle = BorderStyle.None;
            //panel4peakVal.Dock = DockStyle.Bottom;
            panel1_menu.Controls.Add(panel4peakVal);

            try
            {
                string slq_query = "SELECT ID, Name, Location, Other, Usage FROM SensorDataDB.dbo.SENSOR_INFO";
                SqlConnection con = new SqlConnection($@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20");

                using (var cmd = new SqlCommand(slq_query, con))
                {
                    cmd.CommandTimeout = 0;
                    con.Open();
                    //Console.WriteLine("Connection opened");
                    using (var myReader = cmd.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            ListViewItem item1 = new ListViewItem(myReader.GetValue(0).ToString());
                            item1.SubItems.Add(myReader.GetString(1));
                            item1.SubItems.Add(myReader.GetString(2));
                            item1.SubItems.Add(myReader.GetString(3));
                            item1.SubItems.Add(myReader.GetString(4));
                            listView1.Items.Add(item1);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "에러 메시지");
            }
            label_Title_main.Left = panel2_ChartArea.Bounds.Width / 2 - label_Title_main.Bounds.Width / 2;
            label_title_ver.Left = label_Title_main.Right + 15;
        }


        

         private void ScotPlot2(List<SortedDictionary<int, List<string>>> MyDataVals, List<SortedDictionary<int, List<string>>> MyDataTime, List<string> MyDataTypes, List<int> MyIDs, bool MyRT_flag)
        {
            panel2_ChartArea.Controls.Clear();
            panel4peakVal.Controls.Clear();
            //formsPlots.Clear();
            plts.Clear();
            nextDataIndex = 0;
            RTDataArray.Clear();

            int numOfElmnt = MyDataVals[0].Values.Count;
            /*if (MyDataTypes.Count > 1)
            {
                numOfElmnt = CountNumOfElmnt(MyData, MyIDs, "all"); //데이터 개수 : number of Elements(temp)
            }
            else
            {
                numOfElmnt = CountNumOfElmnt(MyData, MyIDs, MyDataTypes[0]);
            }

            string[] dataVal = new string[numOfElmnt];
            string[] timeVal = new string[numOfElmnt];
            */

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            panel2_ChartArea.Controls.Add(tableLayoutPanel);

            if (MyRT_flag == false) // 시간 설정 시각화
            {
                timer2.Stop();
                timer3_render.Stop();
                // 시각화 화면 세탕하기 1: TableLayoutPanel 구성 세팅
                if (MyDataTypes.Count < 2)
                {
                    tableLayoutPanel.RowCount = 1;
                    tableLayoutPanel.ColumnCount = 1;
                    tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                }
                else if (MyDataTypes.Count == 2)
                {
                    tableLayoutPanel.RowCount = 2;
                    tableLayoutPanel.ColumnCount = 1;
                    tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                }
                else
                {
                    tableLayoutPanel.RowCount = 2;
                    tableLayoutPanel.ColumnCount = 2;
                    tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                }

                formsPlots = new List<FormsPlot>();

                /*string[][] dataArr2 = new string[MyIDs.Count][]; //dataArr[0][0].Count
                string[][] timeArr2 = new string[MyIDs.Count][];*/

                string[] map_vals = new string[numOfElmnt]; //dataArr[0][0].Count
                string[] map_time = new string[numOfElmnt];

                /* for (int index_sensorID = 0; index_sensorID < MyIDs.Count; index_sensorID++)
                 {
                     dataArr2[index_sensorID] = new string[numOfElmnt]; 
                     timeArr2[index_sensorID] = new string[numOfElmnt];
                 } */
                // 시각화 화면 세탕하기 2: TableLayoutPanel의 구성요소들 생성
                for (int index_column = 0; index_column < tableLayoutPanel.ColumnCount; index_column++)
                {
                    for (int index_row = 0; index_row < tableLayoutPanel.RowCount; index_row++)
                    {
                        if (MyDataTypes.Count > index_column * tableLayoutPanel.RowCount + index_row)
                        {
                            Panel panel = new Panel();
                            panel.BorderStyle = BorderStyle.FixedSingle;
                            panel.Dock = DockStyle.Fill;
                            tableLayoutPanel.Controls.Add(panel, index_row, index_column);
                            FormsPlot formsPlot = new FormsPlot();
                            formsPlot.Dock = DockStyle.Fill;
                            tableLayoutPanel.Controls.Add(panel, index_row, index_column);
                            formsPlots.Add(formsPlot);
                            panel.Controls.Add(formsPlot);

                        }
                    }
                }

                for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
                {

                    if (MyDataTypes[index_DataType].Contains("temp")) { titleName = "온도(°C)"; }
                    else if (MyDataTypes[index_DataType].Contains("humid")) { titleName = "습도(%)"; }
                    else if (MyDataTypes[index_DataType].Contains("part03")) { titleName = "파티클(0.3μm)"; }
                    else { titleName = "파티클(0.5μm)"; }


                    var plt = formsPlots[index_DataType];
                    for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
                    {

                        /* for (int index_DataElem = 0; index_DataElem < numOfElmnt; index_DataElem++)
                          {
                            dataArr2[index_ID][index_DataElem] = MyData[index_DataType][index_ID][index_DataElem][0];
                            timeArr2[index_ID][index_DataElem] = MyData[index_DataType][index_ID][index_DataElem][1];
                         } */


                        /*double[] ys = dataArr2[index_ID].Select(x => double.Parse(x)).ToArray();
                        DateTime[] timeData = timeArr2[index_ID].Select(x => DateTime.Parse(x)).ToArray();*/

                        double[] ys = MyDataVals[index_DataType][MyIDs[index_ID]].Select(x => double.Parse(x)).ToArray();
                        DateTime[] timeData = MyDataTime[index_DataType][MyIDs[index_ID]].Select(x => DateTime.Parse(x)).ToArray();
                        double[] xs = timeData.Select(x => x.ToOADate()).ToArray();

                        // CHARTING Functions

                        formsPlots[index_DataType].plt.PlotSignalXYConst(xs, ys, label: Btn3_SensorLocation[MyIDs[index_ID] - 1].Text, color: colorset[index_ID]);             // Signal Chart

                        //formsPlots[index_DataType].plt.PlotScatter(xs, ys, lineWidth: 0);                          // Scatter Chart
                        //formsPlots[index_DataType].plt.PlotStep(xs, ys);                                    // Step Chart
                        //formsPlots[index_DataType].plt.PlotFill(xs, ys);                                    // Fill Chart
                        //formsPlots[index_DataType].plt.PlotScatterHighlight(xs, ys);                          // ScatterHighlight
                        //formsPlots[index_DataType].plt.PlotPolygon(Tools.Pad(xs, cloneEdges: true), Tools.Pad(ys));
                        //formsPlots[index_DataType].plt.Grid(enable: false);      //Enable-Disable Grid


                        formsPlots[index_DataType].plt.Ticks(dateTimeX: true);
                        formsPlots[index_DataType].plt.Title(titleName, fontSize: 24);
                        formsPlots[index_DataType].plt.YLabel(titleName, fontSize: 20);
                        formsPlots[index_DataType].plt.XLabel("시간", fontSize: 20);

                        if (MyDataTypes.Count == 1)
                        {
                            formsPlots[index_DataType].plt.Style(figBg: Color.GhostWhite); //tick: Color.White, label: Color.White, title: Color.White
                        }
                        else
                        {
                            if (MyDataTypes.Count == 2)
                            {
                                formsPlots[index_DataType].plt.Style(figBg: Color.WhiteSmoke);
                            }
                            else
                            {
                                formsPlots[index_DataType].plt.Style(figBg: Color.FloralWhite);
                            }
                        }
                    }
                    formsPlots[index_DataType].plt.Legend(location: legendLocation.lowerLeft, fontSize: 14);
                    formsPlots[index_DataType].plt.Layout(y2LabelWidth: 80);
                    formsPlots[index_DataType].plt.AxisAuto();
                }
                for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
                {
                    /*for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
                    {
                        for (int index_DataElem = 0; index_DataElem < numOfElmnt; index_DataElem++)
                        {
                            dataArr2[index_ID][index_DataElem] = MyData[index_DataType][index_ID][index_DataElem][0];
                        }
                    }*/

                    
                    AnnotationBackground(MyDataTypes, MyIDs);
                    AnnotationsMinMax(MyDataTypes, MyIDs, MyDataVals, false);
                    /*int annotY = -10 - 25 * (MyIDs.Count - 1);
                    for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
                    {

                        //double[] ys = dataArr2[index_ID].Select(x => double.Parse(x)).ToArray();
                        Tuple<string, int> tupleMax = FindMax2(MyDataVals[index_DataType][MyIDs[index_ID]], MyDataTypes, index_DataType);
                        string max = tupleMax.Item1;
                        int indexOfMax = tupleMax.Item2;

                        Tuple<string, int> tupleMin = FindMin2(MyDataVals[index_DataType][MyIDs[index_ID]], MyDataTypes, index_DataType);
                        string min = tupleMin.Item1;
                        int indexOfMin = tupleMin.Item2;

                        formsPlots[index_DataType].plt.PlotAnnotation(max + " " + char.ConvertFromUtf32(0x2191), -10, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);
                        formsPlots[index_DataType].plt.PlotAnnotation(min + " " + char.ConvertFromUtf32(0x2193), -75, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);

                        annotY += 25;
                        
                    }*/
                    formsPlots[index_DataType].Render();
                }
                progressbarThread.Abort();
                progressbarThread = null;

            }
            else // 실시간 시각화
            {

                // 시각화 하려는 데이터 타입 갯수에 따라 textbox 컨테이너(List<T>) 생성
                if (digital_flag) // 숫자만 display됨
                {
                    try
                    {
                        timer1.Start();
                        timer2.Stop();
                        timer3_render.Stop();

                        if (IDs_now.Count < 2)
                        {
                            tableLayoutPanel.RowCount = 1;
                            tableLayoutPanel.ColumnCount = 1;
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        }
                        else if (IDs_now.Count == 2)
                        {
                            tableLayoutPanel.RowCount = 2;
                            tableLayoutPanel.ColumnCount = 1;
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        }
                        else
                        {
                            tableLayoutPanel.RowCount = 2;
                            tableLayoutPanel.ColumnCount = 2;
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        }

                        RT_textBoxes.Clear();
                        for (int i = 0; i < MyIDs.Count; i++)
                        {
                            List<TextBox> textBoxes = new List<TextBox>();
                            RT_textBoxes.Add(textBoxes);
                        }
                        // 시각화 화면 세탕하기 2: TableLayoutPanel의 구성요소들 생성 - 실시간
                        int txtBoxCount = 0;
                        for (int index_column = 0; index_column < tableLayoutPanel.ColumnCount; index_column++)
                        {
                            for (int index_row = 0; index_row < tableLayoutPanel.RowCount; index_row++)
                            {
                                if (IDs_now.Count > index_column * tableLayoutPanel.RowCount + index_row)
                                {
                                    int yBound = 0;

                                    Panel panel = new Panel();
                                    panel.BorderStyle = BorderStyle.FixedSingle;
                                    panel.Dock = DockStyle.Fill;
                                    tableLayoutPanel.Controls.Add(panel, index_row, index_column);

                                    Label label = new Label();
                                    label.SetBounds(panel.Bounds.Width / 2 - 150, 20, 300, 50);
                                    label.Font = new Font(label.Font.FontFamily, 20, System.Drawing.FontStyle.Bold);
                                    label.TextAlign = ContentAlignment.MiddleCenter;

                                    label.Text = Btn3_SensorLocation[MyIDs[index_column * tableLayoutPanel.RowCount + index_row] - 1].Text;

                                    panel.Controls.Add(label);
                                    //Application.DoEvents();
                                    // 시각화 하려는 센서 갯수에 따라 textbox 생성 
                                    for (int boxIndex = 0; boxIndex < MyDataTypes.Count; boxIndex++)
                                    {
                                        int counter = 0;
                                        if (MyDataTypes.Count > 1) { counter = MyDataTypes.Count; }
                                        TextBox textBox1 = new TextBox();
                                        textBox1.SetBounds(panel.Bounds.Width / 2 - 275, 100 + yBound, 550, 70);
                                        textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                                        textBox1.Font = new Font(textBox1.Font.FontFamily, 50);
                                        textBox1.BackColor = this.BackColor;
                                        textBox1.BorderStyle = BorderStyle.None;
                                        textBox1.Name = "textBox" + txtBoxCount + "_" + boxIndex;
                                        yBound += 75;
                                        panel.Controls.Add(textBox1);
                                        RT_textBoxes[txtBoxCount].Add(textBox1);
                                        //textBox1.Text = "textBox" + bbox_count + "_" + boxIndex;
                                        //Application.DoEvents();

                                    }
                                    txtBoxCount += 1;
                                }
                            }
                        }
                        timer1.Tick += timer1_Tick_1;
                    }
                    catch (Exception ex)
                    {
                        timer1.Stop();
                        MessageBox.Show(ex.Message, "Error message");
                    }
                }
                //plottableAnnotations.Add(formsPlots[index_DataType].plt.PlotAnnotation(label: "최고값: " + RT_Max[index_DataType][index_ID][0][RT_Max[index_DataType][index_ID][0].Count - 1].ToString(), -10, annotY2, fontSize: 12, fontColor: colorset[index_ID]));
                else // Chart form - 차트가 시각화됨
                {
                    try
                    {
                        formsPlots = new List<FormsPlot>();
                        plottableAnnotationsMaxVal.Clear();
                        plottableAnnotationsMinVal.Clear();
                        RT_Max.Clear();
                        RT_Min.Clear();
                        timer2.Start();
                        timer3_render.Start();
                        timer1.Stop();

                        // 시각화 화면 세탕하기 1: TableLayoutPanel 구성 세팅
                        if (MyDataTypes.Count < 2)
                        {
                            tableLayoutPanel.RowCount = 1;
                            tableLayoutPanel.ColumnCount = 1;
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        }
                        else if (MyDataTypes.Count == 2)
                        {
                            tableLayoutPanel.RowCount = 2;
                            tableLayoutPanel.ColumnCount = 1;
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        }
                        else
                        {
                            tableLayoutPanel.RowCount = 2;
                            tableLayoutPanel.ColumnCount = 2;
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        }

                        // 최대값 표시를 위한 textbox 및 label 


                        int peak_yAxis = 50;//button_show.Bounds.Y + button_show.Bounds.Height + 2*button_show.Bounds.Height;
                        int label_yAxis = peak_yAxis - 24;

                        // 시각화 화면 세탕하기 2: TableLayoutPanel의 구성요소들 생성
                        for (int index_column = 0; index_column < tableLayoutPanel.ColumnCount; index_column++)
                        {
                            for (int index_row = 0; index_row < tableLayoutPanel.RowCount; index_row++)
                            {
                                if (MyDataTypes.Count > index_column * tableLayoutPanel.RowCount + index_row)
                                {
                                    Panel panel = new Panel();
                                    panel.BorderStyle = BorderStyle.FixedSingle;
                                    panel.Dock = DockStyle.Fill;
                                    tableLayoutPanel.Controls.Add(panel, index_row, index_column);
                                    FormsPlot formsPlot = new FormsPlot();
                                    formsPlot.Name = "formPlot" + index_column * tableLayoutPanel.RowCount + index_row;
                                    formsPlot.Dock = DockStyle.Fill;
                                    tableLayoutPanel.Controls.Add(panel, index_row, index_column);
                                    formsPlots.Add(formsPlot);
                                    panel.Controls.Add(formsPlot);

                                }
                            }
                        }

                        





                        
                        for (int i_DataType = 0; i_DataType < MyDataTypes.Count; i_DataType++)
                        {
                            RTDataArray2.Add(new SortedDictionary<int, double[]>());
                            RT_Max2.Add(new SortedDictionary<int, string[]>());
                            RT_Min2.Add(new SortedDictionary<int, string[]>());
                            

/*

                            List<List<double[]>> vs = new List<List<double[]>>();
                            RTDataArray.Add(vs);
                            List<List<string[]>> vs_max = new List<List<string[]>>();
                            List<List<string[]>> vs_min = new List<List<string[]>>();
                            RT_Max.Add(vs_max);
                            RT_Min.Add(vs_min);*/

                            if (MyDataTypes[i_DataType].Contains("temp")) { titleName = "온도(°C)"; }
                            else if (MyDataTypes[i_DataType].Contains("humid")) { titleName = "습도(%)"; }
                            else if (MyDataTypes[i_DataType].Contains("part03")) { titleName = "파티클(0.3μm)"; }
                            else { titleName = "파티클(0.5μm)"; }

                            for (int i_sensorID = 0; i_sensorID < MyIDs.Count; i_sensorID++)
                            {
                                double[] dataArr;
                                string[] maxArr;
                                string[] minArr;
                                if (!RTDataArray2[i_DataType].TryGetValue(MyIDs[i_sensorID], out dataArr))
                                {
                                    dataArr = new double[100_00];
                                    RTDataArray2[i_DataType].Add(MyIDs[i_sensorID], dataArr);
                                }
                                if (!RT_Max2[i_DataType].TryGetValue(MyIDs[i_sensorID], out maxArr))
                                {
                                    maxArr = new string[1];
                                    RT_Max2[i_DataType].Add(MyIDs[i_sensorID], maxArr);
                                }

                                if (!RT_Min2[i_DataType].TryGetValue(MyIDs[i_sensorID], out minArr))
                                {
                                    minArr = new string[1];
                                    RT_Min2[i_DataType].Add(MyIDs[i_sensorID], minArr);
                                }
/*

                                List<double[]> vs0 = new List<double[]>();
                                RTDataArray[i_DataType].Add(vs0);

                                double[] vs1 = new double[100_000];
                                double[] vs2 = new double[100_000];
                                RTDataArray[i_DataType][i_sensorID].Add(vs1);
                                RTDataArray[i_DataType][i_sensorID].Add(vs2);

                                List<string[]> vs0_max = new List<string[]>();
                                List<string[]> vs0_min = new List<string[]>();
                                RT_Max[i_DataType].Add(vs0_max);
                                RT_Min[i_DataType].Add(vs0_min);

                                string[] vs1_max = new string[1];
                                string[] vs2_max = new string[1];
                                string[] vs1_min = new string[1];
                                string[] vs2_min = new string[1];


                                RT_Max[i_DataType][i_sensorID].Add(vs1_max);
                                RT_Max[i_DataType][i_sensorID].Add(vs2_max);

                                RT_Min[i_DataType][i_sensorID].Add(vs1_min);
                                RT_Min[i_DataType][i_sensorID].Add(vs2_min);


                                //Max values



                                //var RT_Max2 = MyDataVals;
                                //List<SortedDictionary<int, List<string>>> RT_Max2;
*/
                                //RTDataArray2[i_DataType][MyIDs[i_sensorID]] = MyDataVals[i_DataType][MyIDs[i_sensorID]].Select();
                                RT_Max2[i_DataType][MyIDs[i_sensorID]][0] = MyDataVals[i_DataType][MyIDs[i_sensorID]][0];
                                RT_Min2[i_DataType][MyIDs[i_sensorID]][0] = MyDataTime[i_DataType][MyIDs[i_sensorID]][0];
                                //(List<string> values, List<string> times) temp = ;
                                
                                
                                /*
                                RT_Max[i_DataType][i_sensorID][0][0] = MyData[i_DataType][i_sensorID][0][0];
                                //DateTime dtime_maxi = DateTime.Parse(MyData[index_DataType][index_ID][0][1]);
                                RT_Max[i_DataType][i_sensorID][1][0] = MyData[i_DataType][i_sensorID][0][1];// dtime_maxi.ToOADate();

                                //Min values
                                RT_Min[i_DataType][i_sensorID][0][0] = MyData[i_DataType][i_sensorID][0][0];
                                DateTime dtime_min = DateTime.Parse(MyData[i_DataType][i_sensorID][0][1]);
                                RT_Min[i_DataType][i_sensorID][1][0] = MyData[i_DataType][i_sensorID][0][1]; //dtime_min.ToOADate();
                                *//*RT_Max[index_DataType][index_ID][0].Add(Convert.ToDouble(MyData[index_DataType][index_ID][0][0]));
                                DateTime dtime_maxi = DateTime.Parse(MyData[i_DataType][i_sensorID][0][1]);
                                RT_Max[i_DataType][i_sensorID][1].Add(dtime_maxi.ToOADate());
                                
                                RT_Min[i_DataType][i_sensorID][0].Add(Convert.ToDouble(MyData[i_DataType][i_sensorID][0][0]));
                                DateTime dtime_min = DateTime.Parse(MyData[i_DataType][i_sensorID][0][1]);
                                RT_Min[i_DataType][i_sensorID][1].Add(dtime_min.ToOADate());*/




                                //Console.WriteLine($"RT_Max {RT_Max[i_DataType][i_sensorID][0][0]} , RT_Min  {RT_Min[i_DataType][i_sensorID][0][0]} were initialized at {DateTime.Now.ToString("HH:mm:ss") }");
                                //시간 데이터
                                //DateTime timeData = DateTime.Parse(MyData[0][0][0][1]);
                                //double xs = timeData.ToOADate();
                                
                                DateTime timeData2 = DateTime.Parse(MyDataTime[i_DataType][MyIDs[i_sensorID]][0]);
                                double xs2 = timeData2.ToOADate();

                                // CHARTING Functions

                                //formsPlots[index_DataType].plt.PlotStep(xs, ys);                                    // Step Chart
                                //formsPlots[i].plt.PlotFill(xs, ys);                                    // Fill Chart
                                //formsPlots[index_DataType].plt.PlotScatterHighlight(xs, ys);                          // ScatterHighlight
                                //formsPlots[i].plt.PlotPolygon(Tools.Pad(xs, cloneEdges: true), Tools.Pad(ys));
                                //formsPlots[index_DataType].plt.PlotSignalXYConst(RTtime, RTdata);                                    // Signal Chart // , lineStyle: LineStyle.Dot, color: colorset[index_sensorID]

                                double samplesPerDay = TimeSpan.TicksPerDay / (TimeSpan.TicksPerSecond);
                                
                                



                                
                                //signalPlot = formsPlots[i_DataType].plt.PlotSignal(RTDataArray[i_DataType][i_sensorID][0], samplesPerDay, xs, label: Btn3_SensorLocation[MyIDs[i_sensorID] - 1].Text, color: colorset[i_sensorID]);
                                signalPlot = formsPlots[i_DataType].plt.PlotSignal(RTDataArray2[i_DataType][MyIDs[i_sensorID]], samplesPerDay, xs2, label: Btn3_SensorLocation[MyIDs[i_sensorID] - 1].Text, color: colorset[i_sensorID]);
                                
                                
                                
                                
                                
                                formsPlots[i_DataType].plt.Ticks(dateTimeX: true);
                                //formsPlots[i].plt.Grid(enable: false);      //Enable-Disable Gridn
                                formsPlots[i_DataType].plt.Title(titleName, fontSize: 24);
                                formsPlots[i_DataType].plt.YLabel(titleName, fontSize: 20);
                                formsPlots[i_DataType].plt.XLabel("시간", fontSize: 20);
                                if (MyDataTypes.Count == 1)
                                {
                                    formsPlots[i_DataType].plt.Style(figBg: Color.GhostWhite); //tick: Color.White, label: Color.White, title: Color.White
                                }
                                else
                                {
                                    if (MyDataTypes.Count == 2)
                                    {
                                        formsPlots[i_DataType].plt.Style(figBg: Color.WhiteSmoke);
                                    }
                                    else
                                    {
                                        formsPlots[i_DataType].plt.Style(figBg: Color.FloralWhite);
                                    }
                                }

                                formsPlots[i_DataType].plt.Title(titleName, fontSize: 24); // formsPlot1.

                                formsPlots[i_DataType].plt.YLabel(titleName, fontSize: 20); // formsPlot1.
                                formsPlots[i_DataType].plt.XLabel("시간", fontSize: 20);
                                plts.Add(signalPlot);

                            }
                            formsPlots[i_DataType].plt.Legend(location: legendLocation.lowerLeft, fontSize: 14);
                            formsPlots[i_DataType].plt.Layout(y2LabelWidth: 80);
                            formsPlots[i_DataType].plt.AxisAuto();
                        }


                        AnnotationBackground(MyDataTypes, MyIDs);
                        AnnotationsMinMax(MyDataTypes, MyIDs, MyDataVals, true);
                        // Plot Annotations separately to put them above the charts.
                        /*for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
                        {
                            int annotY = -10 - 25 * (MyIDs.Count - 1);
                            for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
                            {

                                //Console.WriteLine($"New Max: {RT_Max[index_DataType][index_ID][0][0]} at {RT_Max[index_DataType][index_ID][1][0]} ");
                                Console.WriteLine($"New Max: {RT_Max2[index_DataType][MyIDs[index_ID]][0]} at {RTDataArray2[index_DataType][MyIDs[index_ID]][0]} ");
                                string numberStrMax = RT_Max2[index_DataType][MyIDs[index_ID]][0];
                                string numberStrMin = RT_Min2[index_DataType][MyIDs[index_ID]][0];
                                string maxLabel = (numberStrMax.Contains(".") == false && numberStrMax.Length > 3) ? numberStrMax.Insert(numberStrMax.Length - 3, ",") : numberStrMax;
                                string minLabel = (numberStrMin.Contains(".") == false && numberStrMin.Length > 3) ? numberStrMin.Insert(numberStrMin.Length - 3, ",") : numberStrMin;

                                PlottableAnnotation pltAnnotMax = formsPlots[index_DataType].plt.PlotAnnotation(label: maxLabel + " " + char.ConvertFromUtf32(0x2191), -10, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);
                                PlottableAnnotation pltAnnotMin = formsPlots[index_DataType].plt.PlotAnnotation(label: minLabel + " " + char.ConvertFromUtf32(0x2193), -75, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);
                                //Console.WriteLine("Lbl: " + pltAnnot.label + ", vis: " + pltAnnot.visible + ", x: " + pltAnnot.xPixel + ", y: " + pltAnnot.yPixel);
                                plottableAnnotationsMaxVal.Add(pltAnnotMax);
                                plottableAnnotationsMinVal.Add(pltAnnotMin);
                                annotY += 25;
                            }
                        }*/

                        Console.WriteLine("\n\n\n");


                    }
                    catch (Exception ex)
                    {
                        timer2.Stop();
                        timer3_render.Stop();
                        throw new Exception(ex.Message);
                    }
                    //Console.WriteLine(RTDataArray.Count);
                }
            }
        }







        private void ScotPlot(List<List<List<string[]>>> MyData, List<string> MyDataTypes, List<int> MyIDs, bool MyRT_flag)
        {
            panel2_ChartArea.Controls.Clear();
            panel4peakVal.Controls.Clear();
            //formsPlots.Clear();
            plts.Clear();
            nextDataIndex = 0;
            RTDataArray.Clear();

            int numOfElmnt = 0;
            if (MyDataTypes.Count > 1)
            {
                numOfElmnt = CountNumOfElmnt(MyData, MyIDs, "all"); //데이터 개수 : number of Elements(temp)
            }
            else
            {
                numOfElmnt = CountNumOfElmnt(MyData, MyIDs, MyDataTypes[0]);
            }

            string[] dataVal = new string[numOfElmnt];
            string[] timeVal = new string[numOfElmnt];

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            panel2_ChartArea.Controls.Add(tableLayoutPanel);

            if (MyRT_flag == false) // 시간 설정 시각화
            {
                timer2.Stop();
                timer3_render.Stop();
                // 시각화 화면 세탕하기 1: TableLayoutPanel 구성 세팅
                if (MyDataTypes.Count < 2)
                {
                    tableLayoutPanel.RowCount = 1;
                    tableLayoutPanel.ColumnCount = 1;
                    tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                }
                else if (MyDataTypes.Count == 2)
                {
                    tableLayoutPanel.RowCount = 2;
                    tableLayoutPanel.ColumnCount = 1;
                    tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                }
                else
                {
                    tableLayoutPanel.RowCount = 2;
                    tableLayoutPanel.ColumnCount = 2;
                    tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                }

                // ScotPlot 차트 함수인 FormsPlot들을 위한 List 생성  
                formsPlots = new List<FormsPlot>();

                string[][] dataArr2 = new string[MyIDs.Count][]; //dataArr[0][0].Count
                string[][] timeArr2 = new string[MyIDs.Count][];

                for (int index_sensorID = 0; index_sensorID < MyIDs.Count; index_sensorID++)
                {
                    dataArr2[index_sensorID] = new string[numOfElmnt];
                    timeArr2[index_sensorID] = new string[numOfElmnt];
                }
                // 시각화 화면 세탕하기 2: TableLayoutPanel의 구성요소들 생성
                for (int index_column = 0; index_column < tableLayoutPanel.ColumnCount; index_column++)
                {
                    for (int index_row = 0; index_row < tableLayoutPanel.RowCount; index_row++)
                    {
                        if (MyDataTypes.Count > index_column * tableLayoutPanel.RowCount + index_row)
                        {
                            Panel panel = new Panel();
                            panel.BorderStyle = BorderStyle.FixedSingle;
                            panel.Dock = DockStyle.Fill;
                            tableLayoutPanel.Controls.Add(panel, index_row, index_column);
                            FormsPlot formsPlot = new FormsPlot();
                            formsPlot.Dock = DockStyle.Fill;
                            tableLayoutPanel.Controls.Add(panel, index_row, index_column);
                            formsPlots.Add(formsPlot);
                            panel.Controls.Add(formsPlot);

                        }
                    }
                }

                for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
                {

                    if (MyDataTypes[index_DataType].Contains("temp")) { titleName = "온도(°C)"; }
                    else if (MyDataTypes[index_DataType].Contains("humid")) { titleName = "습도(%)"; }
                    else if (MyDataTypes[index_DataType].Contains("part03")) { titleName = "파티클(0.3μm)"; }
                    else { titleName = "파티클(0.5μm)"; }


                    var plt = formsPlots[index_DataType];
                    for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
                    {
                        for (int index_DataElem = 0; index_DataElem < numOfElmnt; index_DataElem++)
                        {
                            dataArr2[index_ID][index_DataElem] = MyData[index_DataType][index_ID][index_DataElem][0];
                            timeArr2[index_ID][index_DataElem] = MyData[index_DataType][index_ID][index_DataElem][1];
                        }

                        double[] ys = dataArr2[index_ID].Select(x => double.Parse(x)).ToArray();
                        DateTime[] timeData = timeArr2[index_ID].Select(x => DateTime.Parse(x)).ToArray();
                        double[] xs = timeData.Select(x => x.ToOADate()).ToArray();

                        // CHARTING Functions

                        formsPlots[index_DataType].plt.PlotSignalXYConst(xs, ys, label: Btn3_SensorLocation[MyIDs[index_ID] - 1].Text, color: colorset[index_ID]);             // Signal Chart

                        //formsPlots[index_DataType].plt.PlotScatter(xs, ys, lineWidth: 0);                          // Scatter Chart
                        //formsPlots[index_DataType].plt.PlotStep(xs, ys);                                    // Step Chart
                        //formsPlots[index_DataType].plt.PlotFill(xs, ys);                                    // Fill Chart
                        //formsPlots[index_DataType].plt.PlotScatterHighlight(xs, ys);                          // ScatterHighlight
                        //formsPlots[index_DataType].plt.PlotPolygon(Tools.Pad(xs, cloneEdges: true), Tools.Pad(ys));
                        //formsPlots[index_DataType].plt.Grid(enable: false);      //Enable-Disable Grid

                        formsPlots[index_DataType].plt.Ticks(dateTimeX: true);
                        formsPlots[index_DataType].plt.Title(titleName, fontSize: 24);
                        formsPlots[index_DataType].plt.YLabel(titleName, fontSize: 20);
                        formsPlots[index_DataType].plt.XLabel("시간", fontSize: 20);

                        if (MyDataTypes.Count == 1)
                        {
                            formsPlots[index_DataType].plt.Style(figBg: Color.GhostWhite); //tick: Color.White, label: Color.White, title: Color.White
                        }
                        else
                        {
                            if (MyDataTypes.Count == 2)
                            {
                                formsPlots[index_DataType].plt.Style(figBg: Color.WhiteSmoke);
                            }
                            else
                            {
                                formsPlots[index_DataType].plt.Style(figBg: Color.FloralWhite);
                            }
                        }
                    }
                    formsPlots[index_DataType].plt.Legend(location: legendLocation.lowerLeft, fontSize: 14);
                    formsPlots[index_DataType].plt.Layout(y2LabelWidth: 80);
                    formsPlots[index_DataType].plt.AxisAuto();
                }
                for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
                {
                    for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
                    {
                        for (int index_DataElem = 0; index_DataElem < numOfElmnt; index_DataElem++)
                        {
                            dataArr2[index_ID][index_DataElem] = MyData[index_DataType][index_ID][index_DataElem][0];
                        }
                    }

                    int annotY = -10 - 25 * (MyIDs.Count - 1);
                    AnnotationBackground(MyDataTypes, MyIDs);
                    
                    for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
                    {

                        //double[] ys = dataArr2[index_ID].Select(x => double.Parse(x)).ToArray();
                        Tuple<string, int> tupleMax = FindMax(dataArr2[index_ID], MyDataTypes, index_DataType);
                        string max = tupleMax.Item1;
                        int indexOfMax = tupleMax.Item2;

                        Tuple<string, int> tupleMin = FindMin(dataArr2[index_ID], MyDataTypes, index_DataType);
                        string min = tupleMin.Item1;
                        int indexOfMin = tupleMin.Item2;

                        formsPlots[index_DataType].plt.PlotAnnotation(max + " " + char.ConvertFromUtf32(0x2191), -10, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);
                        formsPlots[index_DataType].plt.PlotAnnotation(min + " " + char.ConvertFromUtf32(0x2193), -75, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);

                        annotY += 25;
                        formsPlots[index_DataType].Render();
                    }
                }
                progressbarThread.Abort();
                progressbarThread = null;

            }
            else // 실시간 시각화
            {

                // 시각화 하려는 데이터 타입 갯수에 따라 textbox 컨테이너(List<T>) 생성
                if (digital_flag) // 숫자만 display됨
                {
                    try
                    {
                        timer1.Start();
                        timer2.Stop();
                        timer3_render.Stop();

                        if (IDs_now.Count < 2)
                        {
                            tableLayoutPanel.RowCount = 1;
                            tableLayoutPanel.ColumnCount = 1;
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        }
                        else if (IDs_now.Count == 2)
                        {
                            tableLayoutPanel.RowCount = 2;
                            tableLayoutPanel.ColumnCount = 1;
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        }
                        else
                        {
                            tableLayoutPanel.RowCount = 2;
                            tableLayoutPanel.ColumnCount = 2;
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        }

                        RT_textBoxes.Clear();
                        for (int i = 0; i < MyIDs.Count; i++)
                        {
                            List<TextBox> textBoxes = new List<TextBox>();
                            RT_textBoxes.Add(textBoxes);
                        }
                        // 시각화 화면 세탕하기 2: TableLayoutPanel의 구성요소들 생성 - 실시간
                        int txtBoxCount = 0;
                        for (int index_column = 0; index_column < tableLayoutPanel.ColumnCount; index_column++)
                        {
                            for (int index_row = 0; index_row < tableLayoutPanel.RowCount; index_row++)
                            {
                                if (IDs_now.Count > index_column * tableLayoutPanel.RowCount + index_row)
                                {
                                    int yBound = 0;

                                    Panel panel = new Panel();
                                    panel.BorderStyle = BorderStyle.FixedSingle;
                                    panel.Dock = DockStyle.Fill;
                                    tableLayoutPanel.Controls.Add(panel, index_row, index_column);

                                    Label label = new Label();
                                    label.SetBounds(panel.Bounds.Width / 2 - 150, 20, 300, 50);
                                    label.Font = new Font(label.Font.FontFamily, 20, System.Drawing.FontStyle.Bold);
                                    label.TextAlign = ContentAlignment.MiddleCenter;

                                    label.Text = Btn3_SensorLocation[MyIDs[index_column * tableLayoutPanel.RowCount + index_row] - 1].Text;

                                    panel.Controls.Add(label);
                                    //Application.DoEvents();
                                    // 시각화 하려는 센서 갯수에 따라 textbox 생성 
                                    for (int boxIndex = 0; boxIndex < MyDataTypes.Count; boxIndex++)
                                    {
                                        int counter = 0;
                                        if (MyDataTypes.Count > 1) { counter = MyDataTypes.Count; }
                                        TextBox textBox1 = new TextBox();
                                        textBox1.SetBounds(panel.Bounds.Width / 2 - 275, 100 + yBound, 550, 70);
                                        textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                                        textBox1.Font = new Font(textBox1.Font.FontFamily, 50);
                                        textBox1.BackColor = this.BackColor;
                                        textBox1.BorderStyle = BorderStyle.None;
                                        textBox1.Name = "textBox" + txtBoxCount + "_" + boxIndex;
                                        yBound += 75;
                                        panel.Controls.Add(textBox1);
                                        RT_textBoxes[txtBoxCount].Add(textBox1);
                                        //textBox1.Text = "textBox" + bbox_count + "_" + boxIndex;
                                        //Application.DoEvents();

                                    }
                                    txtBoxCount += 1;
                                }
                            }
                        }
                        timer1.Tick += timer1_Tick_1;
                    }
                    catch (Exception ex)
                    {
                        timer1.Stop();
                        MessageBox.Show(ex.Message, "Error message");
                    }
                }
                //plottableAnnotations.Add(formsPlots[index_DataType].plt.PlotAnnotation(label: "최고값: " + RT_Max[index_DataType][index_ID][0][RT_Max[index_DataType][index_ID][0].Count - 1].ToString(), -10, annotY2, fontSize: 12, fontColor: colorset[index_ID]));
                else // Chart form - 차트가 시각화됨
                {
                    try
                    {
                        formsPlots = new List<FormsPlot>();
                        plottableAnnotationsMaxVal.Clear();
                        plottableAnnotationsMinVal.Clear();
                        RT_Max.Clear();
                        RT_Min.Clear();
                        timer2.Start();
                        timer3_render.Start();
                        timer1.Stop();

                        // 시각화 화면 세탕하기 1: TableLayoutPanel 구성 세팅
                        if (MyDataTypes.Count < 2)
                        {
                            tableLayoutPanel.RowCount = 1;
                            tableLayoutPanel.ColumnCount = 1;
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        }
                        else if (MyDataTypes.Count == 2)
                        {
                            tableLayoutPanel.RowCount = 2;
                            tableLayoutPanel.ColumnCount = 1;
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        }
                        else
                        {
                            tableLayoutPanel.RowCount = 2;
                            tableLayoutPanel.ColumnCount = 2;
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        }

                        // 최대값 표시를 위한 textbox 및 label 


                        int peak_yAxis = 50;//button_show.Bounds.Y + button_show.Bounds.Height + 2*button_show.Bounds.Height;
                        int label_yAxis = peak_yAxis - 24;

                        // 시각화 화면 세탕하기 2: TableLayoutPanel의 구성요소들 생성
                        for (int index_column = 0; index_column < tableLayoutPanel.ColumnCount; index_column++)
                        {
                            for (int index_row = 0; index_row < tableLayoutPanel.RowCount; index_row++)
                            {
                                if (MyDataTypes.Count > index_column * tableLayoutPanel.RowCount + index_row)
                                {
                                    Panel panel = new Panel();
                                    panel.BorderStyle = BorderStyle.FixedSingle;
                                    panel.Dock = DockStyle.Fill;
                                    tableLayoutPanel.Controls.Add(panel, index_row, index_column);
                                    FormsPlot formsPlot = new FormsPlot();
                                    formsPlot.Name = "formPlot" + index_column * tableLayoutPanel.RowCount + index_row;
                                    formsPlot.Dock = DockStyle.Fill;
                                    tableLayoutPanel.Controls.Add(panel, index_row, index_column);
                                    formsPlots.Add(formsPlot);
                                    panel.Controls.Add(formsPlot);

                                }
                            }
                        }


                        for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
                        {

                            List<List<double[]>> vs = new List<List<double[]>>();
                            RTDataArray.Add(vs);
                            List<List<string[]>> vs_max = new List<List<string[]>>();
                            List<List<string[]>> vs_min = new List<List<string[]>>();
                            RT_Max.Add(vs_max);
                            RT_Min.Add(vs_min);

                            if (MyDataTypes[index_DataType].Contains("temp")) { titleName = "온도(°C)"; }
                            else if (MyDataTypes[index_DataType].Contains("humid")) { titleName = "습도(%)"; }
                            else if (MyDataTypes[index_DataType].Contains("part03")) { titleName = "파티클(0.3μm)"; }
                            else { titleName = "파티클(0.5μm)"; }

                            for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
                            {
                                List<double[]> vs0 = new List<double[]>();
                                RTDataArray[index_DataType].Add(vs0);

                                double[] vs1 = new double[100_000];
                                double[] vs2 = new double[100_000];
                                RTDataArray[index_DataType][index_ID].Add(vs1);
                                RTDataArray[index_DataType][index_ID].Add(vs2);

                                List<string[]> vs0_max = new List<string[]>();
                                List<string[]> vs0_min = new List<string[]>();
                                RT_Max[index_DataType].Add(vs0_max);
                                RT_Min[index_DataType].Add(vs0_min);

                                string[] vs1_max = new string[1];
                                string[] vs2_max = new string[1];
                                string[] vs1_min = new string[1];
                                string[] vs2_min = new string[1];


                                RT_Max[index_DataType][index_ID].Add(vs1_max);
                                RT_Max[index_DataType][index_ID].Add(vs2_max);

                                RT_Min[index_DataType][index_ID].Add(vs1_min);
                                RT_Min[index_DataType][index_ID].Add(vs2_min);


                                //Max values

                                RT_Max[index_DataType][index_ID][0][0] = MyData[index_DataType][index_ID][0][0];
                                //DateTime dtime_maxi = DateTime.Parse(MyData[index_DataType][index_ID][0][1]);
                                RT_Max[index_DataType][index_ID][1][0] = MyData[index_DataType][index_ID][0][1];// dtime_maxi.ToOADate();

                                //Min values
                                RT_Min[index_DataType][index_ID][0][0] = MyData[index_DataType][index_ID][0][0];
                                DateTime dtime_min = DateTime.Parse(MyData[index_DataType][index_ID][0][1]);
                                RT_Min[index_DataType][index_ID][1][0] = MyData[index_DataType][index_ID][0][1]; //dtime_min.ToOADate();
                                /*RT_Max[index_DataType][index_ID][0].Add(Convert.ToDouble(MyData[index_DataType][index_ID][0][0]));
                                DateTime dtime_maxi = DateTime.Parse(MyData[index_DataType][index_ID][0][1]);
                                RT_Max[index_DataType][index_ID][1].Add(dtime_maxi.ToOADate());
                                
                                RT_Min[index_DataType][index_ID][0].Add(Convert.ToDouble(MyData[index_DataType][index_ID][0][0]));
                                DateTime dtime_min = DateTime.Parse(MyData[index_DataType][index_ID][0][1]);
                                RT_Min[index_DataType][index_ID][1].Add(dtime_min.ToOADate());*/

                                Console.WriteLine($"RT_Max {RT_Max[index_DataType][index_ID][0][0]} , RT_Min  {RT_Min[index_DataType][index_ID][0][0]} were initialized at {DateTime.Now.ToString("HH:mm:ss") }");
                                //시간 데이터
                                DateTime timeData = DateTime.Parse(MyData[0][0][0][1]);
                                double xs = timeData.ToOADate();

                                // CHARTING Functions

                                //formsPlots[index_DataType].plt.PlotStep(xs, ys);                                    // Step Chart
                                //formsPlots[i].plt.PlotFill(xs, ys);                                    // Fill Chart
                                //formsPlots[index_DataType].plt.PlotScatterHighlight(xs, ys);                          // ScatterHighlight
                                //formsPlots[i].plt.PlotPolygon(Tools.Pad(xs, cloneEdges: true), Tools.Pad(ys));
                                //formsPlots[index_DataType].plt.PlotSignalXYConst(RTtime, RTdata);                                    // Signal Chart // , lineStyle: LineStyle.Dot, color: colorset[index_sensorID]

                                double samplesPerDay = TimeSpan.TicksPerDay / (TimeSpan.TicksPerSecond);
                                signalPlot = formsPlots[index_DataType].plt.PlotSignal(RTDataArray[index_DataType][index_ID][0], samplesPerDay, xs, label: Btn3_SensorLocation[MyIDs[index_ID] - 1].Text, color: colorset[index_ID]);
                                formsPlots[index_DataType].plt.Ticks(dateTimeX: true);

                                //formsPlots[i].plt.Grid(enable: false);      //Enable-Disable Gridn

                                formsPlots[index_DataType].plt.Title(titleName, fontSize: 24);
                                formsPlots[index_DataType].plt.YLabel(titleName, fontSize: 20);
                                formsPlots[index_DataType].plt.XLabel("시간", fontSize: 20);
                                if (MyDataTypes.Count == 1)
                                {
                                    formsPlots[index_DataType].plt.Style(figBg: Color.GhostWhite); //tick: Color.White, label: Color.White, title: Color.White
                                }
                                else
                                {
                                    if (MyDataTypes.Count == 2)
                                    {
                                        formsPlots[index_DataType].plt.Style(figBg: Color.WhiteSmoke);
                                    }
                                    else
                                    {
                                        formsPlots[index_DataType].plt.Style(figBg: Color.FloralWhite);
                                    }
                                }

                                formsPlots[index_DataType].plt.Title(titleName, fontSize: 24); // formsPlot1.

                                formsPlots[index_DataType].plt.YLabel(titleName, fontSize: 20); // formsPlot1.
                                formsPlots[index_DataType].plt.XLabel("시간", fontSize: 20);
                                plts.Add(signalPlot);

                            }
                            formsPlots[index_DataType].plt.Legend(location: legendLocation.lowerLeft, fontSize: 14);
                            formsPlots[index_DataType].plt.Layout(y2LabelWidth: 80);
                            formsPlots[index_DataType].plt.AxisAuto();
                        }


                        AnnotationBackground(MyDataTypes, MyIDs);

                        
                        // Plot Annotations separately to put them above the charts.
                        for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
                        {
                            int annotY = -10 - 25 * (MyIDs.Count - 1);
                            for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
                            {
                                
                                Console.WriteLine($"New Max: {RT_Max[index_DataType][index_ID][0][0]} at {RT_Max[index_DataType][index_ID][1][0]} ");
                                string numberStrMax = RT_Max[index_DataType][index_ID][0][0];
                                string numberStrMin = RT_Min[index_DataType][index_ID][0][0];
                                string maxLabel = (numberStrMax.Contains(".") == false && RT_Min[index_DataType][index_ID][0][0].Length > 3) ? numberStrMax.Insert(numberStrMax.Length - 3, ",") : numberStrMax;
                                string minLabel = (numberStrMin.Contains(".") == false && RT_Min[index_DataType][index_ID][0][0].Length > 3) ? numberStrMin.Insert(numberStrMin.Length - 3, ",") : numberStrMin;

                                PlottableAnnotation pltAnnot = formsPlots[index_DataType].plt.PlotAnnotation(label: maxLabel + " " + char.ConvertFromUtf32(0x2191), -10, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);
                                PlottableAnnotation pltAnnot_min = formsPlots[index_DataType].plt.PlotAnnotation(label: minLabel + " " + char.ConvertFromUtf32(0x2193), -75, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);
                                //Console.WriteLine("Lbl: " + pltAnnot.label + ", vis: " + pltAnnot.visible + ", x: " + pltAnnot.xPixel + ", y: " + pltAnnot.yPixel);
                                plottableAnnotationsMaxVal.Add(pltAnnot);
                                plottableAnnotationsMinVal.Add(pltAnnot_min);
                                annotY += 25;
                            }
                        }
                        Console.WriteLine("\n\n\n");
                    }
                    catch (Exception ex)
                    {
                        timer2.Stop();
                        timer3_render.Stop();
                        throw new Exception(ex.Message);
                    }
                    //Console.WriteLine(RTDataArray.Count);
                }
            }
        }
       
        



        public void AnnotationBackground(List<string> MyDataTypes, List<int> MyIDs)
        {
            for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
            {
                if (MyIDs.Count == 4)
                {
                    formsPlots[index_DataType].plt.PlotAnnotation(label: "ANN", -10, -10, fontSize: 60, fontColor: Color.White, fillColor: Color.White, fillAlpha: 1);
                }
                else if (MyIDs.Count == 3)
                {
                    formsPlots[index_DataType].plt.PlotAnnotation(label: "ANN", -10, -10, fontSize: 50, fontColor: Color.White, fillColor: Color.White, fillAlpha: 1);
                }
                else if (MyIDs.Count == 2)
                {
                    formsPlots[index_DataType].plt.PlotAnnotation(label: "ANNOTA", -10, -10, fontSize: 30, fontColor: Color.White, fillColor: Color.White, fillAlpha: 1);
                }
                else
                {
                    formsPlots[index_DataType].plt.PlotAnnotation(label: "ANNOTATION BOX", -10, -10, fontSize: 13, fontColor: Color.White, fillColor: Color.White, fillAlpha: 1);
                }
            }
        }
        
        
        public void AnnotationsMinMax(List<string> MyDataTypes, List<int> MyIDs, List<SortedDictionary<int, List<string>>> MyDataVals, bool realtime)
        {
            if (realtime == false)
            {
                for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
                {
                    int annotY = -10 - 25 * (MyIDs.Count - 1);
                    for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
                    {
                        //double[] ys = dataArr2[index_ID].Select(x => double.Parse(x)).ToArray();
                        Tuple<string, int> tupleMax = FindMax2(MyDataVals[index_DataType][MyIDs[index_ID]], MyDataTypes, index_DataType);
                        string max = tupleMax.Item1;
                        int indexOfMax = tupleMax.Item2;

                        Tuple<string, int> tupleMin = FindMin2(MyDataVals[index_DataType][MyIDs[index_ID]], MyDataTypes, index_DataType);
                        string min = tupleMin.Item1;
                        int indexOfMin = tupleMin.Item2;

                        formsPlots[index_DataType].plt.PlotAnnotation(max + " " + char.ConvertFromUtf32(0x2191), -10, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);
                        formsPlots[index_DataType].plt.PlotAnnotation(min + " " + char.ConvertFromUtf32(0x2193), -75, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);

                        annotY += 25;

                    }
                }
            }
            else
            {
                for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
                {
                    int annotY = -10 - 25 * (MyIDs.Count - 1);
                    for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
                    {

                        //Console.WriteLine($"New Max: {RT_Max[index_DataType][index_ID][0][0]} at {RT_Max[index_DataType][index_ID][1][0]} ");
                        Console.WriteLine($"New Max: {RT_Max2[index_DataType][MyIDs[index_ID]][0]} at {RTDataArray2[index_DataType][MyIDs[index_ID]][0]} ");
                        string numberStrMax = RT_Max2[index_DataType][MyIDs[index_ID]][0];
                        string numberStrMin = RT_Min2[index_DataType][MyIDs[index_ID]][0];
                        string maxLabel = (numberStrMax.Contains(".") == false && numberStrMax.Length > 3) ? numberStrMax.Insert(numberStrMax.Length - 3, ",") : numberStrMax;
                        string minLabel = (numberStrMin.Contains(".") == false && numberStrMin.Length > 3) ? numberStrMin.Insert(numberStrMin.Length - 3, ",") : numberStrMin;

                        PlottableAnnotation pltAnnotMax = formsPlots[index_DataType].plt.PlotAnnotation(label: maxLabel + " " + char.ConvertFromUtf32(0x2191), -10, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);
                        PlottableAnnotation pltAnnotMin = formsPlots[index_DataType].plt.PlotAnnotation(label: minLabel + " " + char.ConvertFromUtf32(0x2193), -75, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);
                        //Console.WriteLine("Lbl: " + pltAnnot.label + ", vis: " + pltAnnot.visible + ", x: " + pltAnnot.xPixel + ", y: " + pltAnnot.yPixel);
                        plottableAnnotationsMaxVal.Add(pltAnnotMax);
                        plottableAnnotationsMinVal.Add(pltAnnotMin);
                        annotY += 25;
                    }
                }
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
        /// Progress Bar
        /// </summary>
        private static void WaitForm()
        {
            //Application.Run(new ProgressBarForm());
            ProgressBarForm progressBarForm = new ProgressBarForm();
            progressBarForm.ShowDialog();
        }


         


        //확인 (시각화) 버튼 누를 때에의 행위
        /// <summary>
        /// Function to display results in form of chart for the selected time interval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_show_Click(object sender, EventArgs e)
        {
            MyDataQuery myDataQuery = new MyDataQuery();
            List<List<List<string[]>>> DataRetrieved_general = new List<List<List<string[]>>>();
                       

            endTime = "RT";
            string[] timeInterval = { startTime, endTime };
            
            // 실시간 버튼 눌렀을 때
            if (button1_realtime.Image == btnClicked_big)// BackColor != Color.Transparent
            {
                Console.WriteLine("Now RealTime");
                timer1.Enabled = true;
                timer2.Enabled = true;
                timer3_render.Enabled = true;

                IDs_now.Sort();
                timeInterval[0] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                timeInterval[1] = "RT";

                List<string> sql_names = new List<string>();
                for (int ind = 0; ind < DataTypesNow.Count; ind++)
                {
                    if (DataTypesNow[ind].Contains("temp")) { sql_names.Add("Temperature"); }
                    else if (DataTypesNow[ind].Contains("humid")) { sql_names.Add("Humidity"); }
                    else if (DataTypesNow[ind].Contains("part03")) { sql_names.Add("Particle03"); }
                    else { sql_names.Add("Particle05"); }
                }
                Sql_NamesNow = new List<string>(sql_names);
                List<List<List<string[]>>> DataRetrieved_RT = myDataQuery.RealTimeDBQuery(IDs_now, DataTypesNow, Sql_NamesNow);
                IDs_next = new List<int>(IDs_now);
                DataTypesNext = new List<string>(DataTypesNow);
                ScotPlot(DataRetrieved_RT, DataTypesNow, IDs_now, true);
                //RealTimeCharting(DataRetrieved_RT, timeInterval, IDs_selected, whatToShow);

                //NewChartingForm RTChart = new NewChartingForm(DataRetrieved_RT, timeInterval, SensorIDs_selected, whatToShow);
                //RTChart.Show();
            }

            //24시간 버튼 눌렀을 때
            else if (button1_24h.Image == btnClicked_big) //BackColor != Color.Transparent
            {
                timer1.Stop();
                timer2.Stop();
                timer3_render.Stop();

                Console.WriteLine("Now 24H");
                IDs_now.Sort();
                startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm");
                endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                progressbarThread = new Thread(new ThreadStart(WaitForm));
                progressbarThread.Start();
                //DataRetrieved_general = myDataQuery.DBQuery(startTime, endTime, IDs_now, DataTypesNow);
                // Console.WriteLine(DataRetrieved_general.Count);
                //temp_max(DataRetrieved_general);
                //ScotPlot(DataRetrieved_general, DataTypesNow, IDs_now, false);

                var (mapVals, mapTime) = myDataQuery.DBQuery2(startTime, endTime, IDs_now, DataTypesNow);
                //Console.WriteLine("mapvals: ", mapVals.Count, mapTime.Count);
                if (mapVals[0][IDs_now[0]].Count > 0)
                {
                    ScotPlot2(mapVals, mapTime, DataTypesNow, IDs_now, false);
                }
                else
                {
                    progressbarThread.Abort();
                    progressbarThread = null;
                    MessageBox.Show("No data for visualization.", "Error Message");
                }

                //ChartingForm GeneralChart = new ChartingForm(DataRetrieved_general, timeInterval, IDs_selected, whatToShow);
                //NewChartingForm GeneralChart = new NewChartingForm(DataRetrieved_general, timeInterval, IDs_selected, whatToShow);
                //GeneralChart.Show();
            }
            // 기간 설정 버튼 눌렀을 때
            else
            {
                timer1.Stop();
                timer2.Stop();
                timer3_render.Stop();
                IDs_now.Sort();
                if (datePicker1_start.Value < datePicker2_end.Value)
                {
                    startTime = datePicker1_start.Value.ToString("yyyy-MM-dd HH:mm");
                    endTime = datePicker2_end.Value.ToString("yyyy-MM-dd HH:mm");

                    progressbarThread = new Thread(new ThreadStart(WaitForm));
                    progressbarThread.Start();
                    //DataRetrieved_general = myDataQuery.DBQuery(startTime, endTime, IDs_now, DataTypesNow);
                    //////////////////// new Chart ScotPlot ////////////////////////////////
                    //ScotPlot(DataRetrieved_general, DataTypesNow, IDs_now, false);
                    var (mapVals, mapTime) = myDataQuery.DBQuery2(startTime, endTime, IDs_now, DataTypesNow);
                    Console.WriteLine("mapvals: ", mapVals.Count, mapTime.Count);
                    
                    if (mapVals[0][IDs_now[0]].Count > 0)
                    {
                        ScotPlot2(mapVals, mapTime, DataTypesNow, IDs_now, false);
                    }
                    else
                    {
                        progressbarThread.Abort();
                        progressbarThread = null;
                        MessageBox.Show("No data for visualization.", "Error Message");
                    }

                    //ChartingForm GeneralChart = new ChartingForm(DataRetrieved_general, timeInterval, IDs_selected, whatToShow);
                    //NewChartingForm GeneralChart = new NewChartingForm(DataRetrieved_general, timeInterval, IDs_selected, whatToShow);
                    //GeneralChart.Show();
                }
                else
                {
                    MessageBox.Show("잘못된 날짜가 선택되었습니다. 확인해 보세요!", "에러 메시지");
                }
            }

        }



        /// <summary>
        /// 최대값과 인덱스를 반환해 주는 함수
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Tuple<string, int> FindMax(string[] data, List<string> MyDT, int index_MyDT)
        {
            //var DATA = data[0][1];

            if (data.Length == 0) { throw new InvalidOperationException("Empty list"); }
            double max = Convert.ToDouble(data[0]);
            int index = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (max <= Convert.ToDouble(data[i]))
                {
                    max = Convert.ToDouble(data[i]);
                    index = i;
                }
            }
            if (MyDT[index_MyDT].Contains("temp") || MyDT[index_MyDT].Contains("humid"))
            {
                return new Tuple<string, int>(max.ToString("F", CultureInfo.InvariantCulture), index);
            }
            else
            {
                return new Tuple<string, int>(String.Format("{0:n0}", max), index);
            }

            //Console.WriteLine("\nMax: {0}", max);

        }


        private Tuple<string, int> FindMax2(List<string> data, List<string> MyDT, int index_MyDT)
        {
            //var DATA = data[0][1];

            if (data.Count == 0) { throw new InvalidOperationException("Empty list"); }
            double max = Convert.ToDouble(data[0]);
            int index = 0;
            for (int i = 0; i < data.Count; i++)
            {
                if (max <= Convert.ToDouble(data[i]))
                {
                    max = Convert.ToDouble(data[i]);
                    index = i;
                }
            }
            if (MyDT[index_MyDT].Contains("temp") || MyDT[index_MyDT].Contains("humid"))
            {
                return new Tuple<string, int>(max.ToString("F", CultureInfo.InvariantCulture), index);
            }
            else
            {
                return new Tuple<string, int>(String.Format("{0:n0}", max), index);
            }

            //Console.WriteLine("\nMax: {0}", max);

        }


        /// <summary>
        /// 최소값과 인덱스를 반환해 주는 함수
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Tuple<string, int> FindMin(string[] data, List<string> MyDT, int index_MyDT)
        {
            if (data.Length == 0) { throw new InvalidOperationException("Empty list"); }
            double min = Convert.ToDouble(data[0]);
            int index = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (min >= Convert.ToDouble(data[i]))
                {
                    min = Convert.ToDouble(data[i]);
                    index = i;
                }
            }
            //Console.WriteLine("\nMax: {0}", max);
            if (MyDT[index_MyDT].Contains("temp") || MyDT[index_MyDT].Contains("humid"))
            {
                return new Tuple<string, int>(min.ToString("F", CultureInfo.InvariantCulture), index);
            }
            else
            {

                return new Tuple<string, int>(String.Format("{0:n0}", min), index);
            }
        }


        private Tuple<string, int> FindMin2(List<string> data, List<string> MyDT, int index_MyDT)
        {
            if (data.Count == 0) { throw new InvalidOperationException("Empty list"); }
            double min = Convert.ToDouble(data[0]);
            int index = 0;
            for (int i = 0; i < data.Count; i++)
            {
                if (min >= Convert.ToDouble(data[i]))
                {
                    min = Convert.ToDouble(data[i]);
                    index = i;
                }
            }
            //Console.WriteLine("\nMax: {0}", max);
            if (MyDT[index_MyDT].Contains("temp") || MyDT[index_MyDT].Contains("humid"))
            {
                return new Tuple<string, int>(min.ToString("F", CultureInfo.InvariantCulture), index);
            }
            else
            {

                return new Tuple<string, int>(String.Format("{0:n0}", min), index);
            }
        }


        private List<int> IDs_AvailCheck()
        {
            List<int> SensorIDs_available = new List<int>();
            SqlConnection myConnection = new SqlConnection($@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20");
            string sql_getIDs = "SELECT * FROM SensorDataDB.dbo.SENSOR_INFO a WHERE a.Usage = 'YES'";
            try
            {
                using (var cmd = new SqlCommand(sql_getIDs, myConnection))
                {
                    //if(true){ //myConnection.State == ConnectionState.Open

                    myConnection.Open();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            while (myReader.Read())
                            {
                                //string[] rowInfo = { myReader.GetValue(0).ToString(), myReader.GetValue(1).ToString(), myReader.GetValue(2).ToString(), myReader.GetValue(3).ToString() };
                                SensorIDs_available.Add(Convert.ToInt32(myReader["ID"]));
                                SensorLocation.Add(myReader["Location"].ToString());
                                // SensorIDs_available.Add(Convert.ToInt32(rowInfo[0])+3); //test
                            }
                        }
                        else
                        {
                            Console.WriteLine("조회할 데이터가 없습니다.");
                        }
                    }
                    myConnection.Close();
                    /*}
                    else
                    {
                        if(CheckInternetConnection() == false)
                        {
                            MessageBox.Show("인터넷 연결에 실폐했습니다. 본 컴퓨터가 인터넷에 연결되어 있는지 확인하십시오.", "인터넷 연결 에러");
                        }
                        else
                        {
                            MessageBox.Show("SQL Server에 연결을 설정하는 중에 네트워크 관련 또는 인스턴스 관련 오류가 발생했습니다. " +
                            "서버를 찾을 수 없거나 액세스할 수 없습니다. 인스턴스 이름이 올바르고 SQL Server가 원격 " +
                            "연결을 허용하도록 구성되어 있는지 확인하십시오.\n(provider: Named Pipes Provider, error: " +
                            "40 - SQL Server에 대한 연결을 열 수 없습니다.)", "SQL Server Connection Error");
                        }
                        
                    }*/
                    // 
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "에러 매시지"); }
            return SensorIDs_available;
        }


        public static bool CheckInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch (WebException ex)
            {
                return false;
            }
        }

       
        //간편한 시간 간격 선택 시 button.BackColor를 다른색으로 표시해 주는 함수
        private void highlightSelectedBtn(Button[] btnNames, int index, string btnSize)
        {
            if (btnNames != null)
            {
                for (int i = 0; i < btnNames.Length; i++)
                {
                    if (index == i)
                    {
                        //btnNames[i].BackColor = color;
                        if (btnSize == "big")
                        {
                            btnNames[i].Image = btnClicked_big;
                        }
                        else
                        {
                            btnNames[i].Image = btnClicked_small;
                        }

                    }
                    else
                    {
                        if (btnSize == "big")
                        {
                            btnNames[i].Image = btnUnClicked_big; //.BackColor = Color.Transparent;
                        }
                        else
                        {
                            btnNames[i].Image = btnUnClicked_small; //.BackColor = Color.Transparent;
                        }
                    }
                }
            }

        }


        /// <summary>
        /// Clears highlighting color for all buttons in the given List<T>
        /// </summary>
        /// <param name="btns"></param>
        private void clearHighlighting(Button[] btns, string btnSize)
        {
            if (btns != null)
            {
                if (btnSize == "big")
                {
                    foreach (var btn in btns)
                    {
                        btn.Image = btnUnClicked_big; // .BackColor = Color.Transparent;
                    }
                }
                else
                {
                    foreach (var btn in btns)
                    {
                        btn.Image = btnUnClicked_small; // .BackColor = Color.Transparent;
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (Btn2_DataType != null && Btn2_DataType[0].Visible == true)
            {
                foreach (var btn2 in Btn2_DataType)
                {
                    btn2.Visible = false;
                    btn2.Image = btnUnClicked_small; // BackColor = Color.Transparent;
                }
                if (Btn3_SensorLocation != null)
                {
                    foreach (var btn3 in Btn3_SensorLocation)
                    {
                        btn3.Visible = false;
                        btn3.Image = btnUnClicked_small; // BackColor = Color.Transparent;
                    }
                }
            }

            button1_numRT.Visible = true;
            button1_chartRT.Visible = true;
            button1_numRT.Image = btnUnClicked_small; // //BackColor = Color.Transparent;
            button1_chartRT.Image = btnUnClicked_small; // BackColor = Color.Transparent;
            //"실시간" button
            highlightSelectedBtn(Btn1_time, 0, "big");
            datePicker1_start.Visible = false;
            datePicker2_end.Visible = false;
            label_between.Visible = false;
            //LinkLabelVisited(5);
            datePicker1_start.Value = DateTime.Now;
            datePicker2_end.Value = DateTime.Now;
            endTime = "RT";
            button_show.Visible = false;
            clearHighlighting(Btn2_DataType, "small");
            clearHighlighting(Btn3_SensorLocation, "small");
            DataTypesNow.Clear();
            IDs_now.Clear();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (Btn2_DataType != null)
            {
                if (Btn2_DataType[0].Visible == false)
                {
                    foreach (var btn in Btn2_DataType)
                    {
                        btn.Visible = true;
                    }
                }
                else
                {
                    foreach (var btn2 in Btn2_DataType)
                    {
                        btn2.Image = btnUnClicked_small; // BackColor = Color.Transparent;
                    }
                    if (Btn3_SensorLocation != null)
                    {
                        foreach (var btn3 in Btn3_SensorLocation)
                        {
                            btn3.Visible = false;
                            btn3.Image = btnUnClicked_small; // BackColor = Color.Transparent;
                        }
                    }
                }
            }
            button1_numRT.Visible = false;
            button1_chartRT.Visible = false;
            //"24시간" button
            highlightSelectedBtn(Btn1_time, 1, "big");
            datePicker1_start.Visible = false;
            datePicker2_end.Visible = false;
            label_between.Visible = false;
            //LinkLabelVisited(2);
            startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm");
            datePicker1_start.Value = Convert.ToDateTime(startTime);
            endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            datePicker2_end.Value = Convert.ToDateTime(endTime);
            //Console.WriteLine(startTime + " " + endTime);
            button_show.Visible = false;
            DataTypesNow.Clear();
            IDs_now.Clear();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (Btn2_DataType[0].Visible == false)
            {
                foreach (var btn in Btn2_DataType)
                {
                    btn.Visible = true;
                }
            }
            else
            {
                foreach (var btn2 in Btn2_DataType)
                {
                    btn2.Image = btnUnClicked_small; // BackColor = Color.Transparent;
                }
                if (Btn3_SensorLocation != null)
                {
                    foreach (var btn3 in Btn3_SensorLocation)
                    {
                        btn3.Visible = false;
                        btn3.Image = btnUnClicked_small; // BackColor = Color.Transparent;
                    }
                }
            }
            button1_numRT.Visible = false;
            button1_chartRT.Visible = false;
            //"시간 설정" button
            highlightSelectedBtn(Btn1_time, 2, "big");
            datePicker1_start.Visible = true;
            datePicker2_end.Visible = true;
            label_between.Visible = true;
            startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm");
            datePicker1_start.Value = Convert.ToDateTime(startTime);
            endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            datePicker2_end.Value = Convert.ToDateTime(endTime);
            button_show.Visible = false;
            DataTypesNow.Clear();
            IDs_now.Clear();

        }
       
        
        private void button1_numRT_Click(object sender, EventArgs e)
        {
            if (button1_numRT.Image == btnClicked_small)//BackColor != Color.Transparent
            {
                if (Btn3_SensorLocation != null)
                {
                    foreach (var btn in Btn3_SensorLocation)
                    {
                        btn.Visible = false;
                    }
                }


                button_show.Visible = false;
                DataTypesNow.Clear();
                IDs_now.Clear();
                clearHighlighting(Btn2_DataType, "small");
                clearHighlighting(Btn3_SensorLocation, "small");
            }
            else
            {
                button1_numRT.Image = btnClicked_small; // BackColor = Color.Chartreuse;
                button1_chartRT.Image = btnUnClicked_small; // BackColor = Color.Transparent;
                digital_flag = true;

                if (Btn2_DataType != null)
                {
                    foreach (var btn in Btn2_DataType)
                    {
                        btn.Visible = true;
                    }
                    if (Btn3_SensorLocation != null)
                    {
                        foreach (var btn in Btn3_SensorLocation)
                        {
                            btn.Visible = false;
                        }
                    }

                }

                clearHighlighting(Btn2_DataType, "small");
                clearHighlighting(Btn3_SensorLocation, "small");
                DataTypesNow.Clear();
                IDs_now.Clear();
            }
        }


        private void button1_chartRT_Click(object sender, EventArgs e)
        {
            if (button1_chartRT.Image == btnClicked_small)// BackColor != Color.Transparent
            {
                if (Btn3_SensorLocation != null)
                {
                    foreach (var btn in Btn3_SensorLocation)
                    {
                        btn.Visible = false;
                    }
                }

                button_show.Visible = false;
                DataTypesNow.Clear();
                IDs_now.Clear();
                clearHighlighting(Btn2_DataType, "small");
                clearHighlighting(Btn3_SensorLocation, "small");

            }
            else
            {
                button1_chartRT.Image = btnClicked_small; // BackColor = Color.Chartreuse;
                button1_numRT.Image = btnUnClicked_small; // BackColor = Color.Transparent;
                digital_flag = false;
                if (Btn2_DataType != null)
                {
                    foreach (var btn in Btn2_DataType)
                    {
                        btn.Visible = true;
                    }
                    if (Btn3_SensorLocation != null)
                    {
                        foreach (var btn in Btn3_SensorLocation)
                        {
                            btn.Visible = false;
                        }
                    }
                }

                button_show.Visible = false;
                DataTypesNow.Clear();
                IDs_now.Clear();
                clearHighlighting(Btn2_DataType, "small");
                clearHighlighting(Btn3_SensorLocation, "small");
            }
        }


        private void btn2_data_Click(object sender, EventArgs e)
        {
            //"온도" button
            Button button = (Button)sender; // receive clicked button properties
            if (button.Image == btnClicked_small) //BackColor != Color.Transparent
            {
                button.Image = btnUnClicked_small; //BackColor = Color.Transparent;
                DataTypesNow.Remove(button.Name);
                if (DataTypesNow.Count < 1)
                {
                    if (Btn3_SensorLocation != null)
                    {
                        foreach (var btn in Btn3_SensorLocation)
                        {
                            btn.Visible = false;
                        }
                    }
                    button_show.Visible = false;
                    IDs_now.Clear();
                    clearHighlighting(Btn3_SensorLocation, "small");

                }
            }
            else
            {
                button.Image = btnClicked_small; //BackColor = Color.Chartreuse;
                DataTypesNow.Add(button.Name);
            }
            if (Btn3_SensorLocation == null || Btn3_SensorLocation.Length == 0)
            {
                List<int> btn_addresses = new List<int>();
                btn_addresses = IDs_AvailCheck(); // 시각화 하려는 센서 ID 조회 및 배열에 ID번호 추가하기
                int x_btn = Btn2_DataType[0].Bounds.X;
                int y_btn = Btn2_DataType[0].Bounds.Y + Btn2_DataType[0].Bounds.Height * 3 / 2;

                Btn3_SensorLocation = new Button[btn_addresses.Count];
                for (int index_btn3 = 0; index_btn3 < btn_addresses.Count; index_btn3++)
                {
                    Button button1 = new Button()
                    {
                        FlatAppearance = {
                            BorderSize = 0,
                            MouseDownBackColor = Color.Transparent,
                            MouseOverBackColor=Color.Transparent,
                            BorderColor=Color.White },
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.Transparent,
                        Image = btnUnClicked_small
                    };
                    button1.SetBounds(x_btn,
                                        y_btn,
                                        Btn2_DataType[0].Bounds.Width,
                                        Btn2_DataType[0].Bounds.Height);
                    if (Btn1_time[2].Bounds.X <= x_btn)
                    {
                        x_btn = Btn2_DataType[0].Bounds.X;
                        y_btn += Btn2_DataType[0].Bounds.Y - (button1_24h.Bounds.Y + button1_24h.Bounds.Height);
                    }
                    else
                    {
                        x_btn += Btn2_DataType[1].Bounds.X - Btn2_DataType[0].Bounds.X;
                    }
                    button1.Text = SensorLocation[index_btn3];
                    button1.Font = new Font(button1.Font.FontFamily, 12);
                    button1.Name = btn_addresses[index_btn3].ToString();
                    //button1.FlatStyle = FlatStyle.Flat;
                    button1.Click += new EventHandler(this.btn3_address_Click);
                    panel1_menu.Controls.Add(button1);
                    button1.Visible = false;
                    Btn3_SensorLocation[index_btn3] = button1;

                }
                if (Btn3_SensorLocation.Length != 0)
                {
                    button_show.Image = btnUnClicked_big;

                    button_show.SetBounds(panel1_menu.Bounds.Width / 2 - Btn1_time[0].Bounds.Width / 2,
                                          Btn3_SensorLocation[Btn3_SensorLocation.Length - 1].Bounds.Y + Btn3_SensorLocation[Btn3_SensorLocation.Length - 1].Bounds.Height * 3 / 2,
                                          Btn1_time[0].Bounds.Width,
                                          Btn1_time[0].Bounds.Height
                                          );
                    listView1.SetBounds(panel1_menu.Bounds.X,
                                button_show.Bounds.Y + 200,
                                panel1_menu.Bounds.Width - 15,
                                panel1_menu.Bounds.Height - button_show.Bounds.Y + 200);

                }

                button_show.Text = "확인";
                button_show.Font = new Font(button_show.Font.FontFamily, 15);
                button_show.Click += new EventHandler(this.button_show_Click);
                panel1_menu.Controls.Add(button_show);
                button_show.Visible = false;
            }

            if (Btn3_SensorLocation.Length != 0 && Btn3_SensorLocation[0].Visible == false)
            {
                foreach (var btn in Btn3_SensorLocation)
                {
                    btn.Visible = true;
                }
            }
        }


        private void btn3_address_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender; // receive clicked button properties
            int btn_num = Convert.ToInt32(button.Name);
            button_show.Visible = true;
            if (IDs_now.Contains(btn_num) == false) // button3_solbi1.BackColor == Color.Transparent
            {
                IDs_now.Add(btn_num);
                button.Image = btnClicked_small; //BackColor = Color.Chartreuse;
            }
            else
            {
                IDs_now.Remove(btn_num);
                button.Image = btnUnClicked_small; //BackColor = Color.Transparent;
                if (IDs_now.Count < 1) { button_show.Visible = false; }
            }

        }


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            MyDataQuery myDataQuery = new MyDataQuery();
            List<List<List<string[]>>> DataRetrieved_RT = new List<List<List<string[]>>>();
            DataRetrieved_RT = myDataQuery.RealTimeDBQuery(IDs_next, DataTypesNext, Sql_NamesNow);
            string printedData = "";
            try
            {
                for (int RT_textBox_index = 0; RT_textBox_index < RT_textBoxes.Count; RT_textBox_index++)
                { 
                    for (int i = 0; i < RT_textBoxes[RT_textBox_index].Count; i++) //whatToShow2
                    {
                        if (DataTypesNext[i].Contains("temp"))
                        {
                            printedData = String.Concat(DataRetrieved_RT[i][RT_textBox_index][0][0].Where(c => !Char.IsWhiteSpace(c))) + " °C";
                            RT_textBoxes[RT_textBox_index][i].Text = printedData;
                        }
                        else if (DataTypesNext[i].Contains("humid"))
                        {
                            printedData = String.Concat(DataRetrieved_RT[i][RT_textBox_index][0][0].Where(c => !Char.IsWhiteSpace(c))) + " %";
                            RT_textBoxes[RT_textBox_index][i].Text = printedData;
                        }
                        else if (DataTypesNext[i].Contains("part03"))
                        {
                            printedData = String.Format("{0:n0}", Convert.ToInt64(DataRetrieved_RT[i][RT_textBox_index][0][0])) + " (0.3μm)";
                            RT_textBoxes[RT_textBox_index][i].Text = printedData;
                        }
                        else
                        {
                            printedData = String.Format("{0:n0}", Convert.ToInt64(DataRetrieved_RT[i][RT_textBox_index][0][0])) + " (0.5μm)";
                            RT_textBoxes[RT_textBox_index][i].Text = printedData;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            timer1.Interval = 1000;
        }


        private void timer2_Tick(object sender, EventArgs e)
        {
            MyDataQuery myDataQuery = new MyDataQuery();
            List<List<List<string[]>>> DataRetrieved_RT = new List<List<List<string[]>>>();
            DataRetrieved_RT = myDataQuery.RealTimeDBQuery(IDs_next, DataTypesNext, Sql_NamesNow);
            try
            {
                string now = DateTime.Now.ToString("HH:mm:ss");
                DateTime resetTime = Convert.ToDateTime(now);
                if (resetTime > Convert.ToDateTime("23:59:58") && resetTime <= Convert.ToDateTime("23:59:59"))
                {
                    Console.WriteLine("Timer Reset Successful. Charts reset.");
                    nextDataIndex = 0;
                    timer2.Stop();
                    timer3_render.Stop();
                    ScotPlot(DataRetrieved_RT, DataTypesNext, IDs_next, true);
                }
                for (int index_DataType = 0; index_DataType < DataTypesNext.Count; index_DataType++)
                {
                    for (int index_ID = 0; index_ID < IDs_next.Count; index_ID++)
                    {
                        RTDataArray[index_DataType][index_ID][0][nextDataIndex] = Convert.ToDouble(DataRetrieved_RT[index_DataType][index_ID][0][0]);
                        DateTime dtime = Convert.ToDateTime(DataRetrieved_RT[index_DataType][index_ID][0][1]);
                        RTDataArray[index_DataType][index_ID][1][nextDataIndex] = dtime.ToOADate();
                        Console.WriteLine($"\nnextDataIndex: {nextDataIndex}, Data: {RTDataArray[index_DataType][index_ID][0][nextDataIndex]} at {dtime.ToString("yyyy-MM-dd HH:mm:ss")}");

                        if (Convert.ToDouble(DataRetrieved_RT[index_DataType][index_ID][0][0]) > Convert.ToDouble(RT_Max[index_DataType][index_ID][0][0]))
                        {
                            RT_Max[index_DataType][index_ID][0][0] = DataRetrieved_RT[index_DataType][index_ID][0][0];
                            RT_Max[index_DataType][index_ID][1][0] = DataRetrieved_RT[index_DataType][index_ID][0][1];
                            string numberStrMax = RT_Max[index_DataType][index_ID][0][0];
                            string maxLabel = (numberStrMax.Contains(".") == false && RT_Min[index_DataType][index_ID][0][0].Length > 3) ? numberStrMax.Insert(numberStrMax.Length - 3, ",") : numberStrMax;
                            //Console.WriteLine($"New Max: {RT_Max[index_DataType][index_ID][0][0]} at {RT_Max[index_DataType][index_ID][1][0]} ");
                            plottableAnnotationsMaxVal[index_DataType * IDs_next.Count + index_ID].label = maxLabel + " " + char.ConvertFromUtf32(0x2191);
                        }
                        if (Convert.ToDouble(DataRetrieved_RT[index_DataType][index_ID][0][0]) < Convert.ToDouble(RT_Min[index_DataType][index_ID][0][0]))
                        {
                            RT_Min[index_DataType][index_ID][0][0] = DataRetrieved_RT[index_DataType][index_ID][0][0];
                            RT_Min[index_DataType][index_ID][1][0] = DataRetrieved_RT[index_DataType][index_ID][0][1];

                            //Console.WriteLine($"New Max: {RT_Min[index_DataType][index_ID][0][0]} at {RT_Min[index_DataType][index_ID][1][0]} ");
                            string numberStrMin = RT_Min[index_DataType][index_ID][0][0];
                            string minLabel = (numberStrMin.Contains(".") == false && RT_Min[index_DataType][index_ID][0][0].Length > 3) ? numberStrMin.Insert(numberStrMin.Length - 3, ",") : numberStrMin;
                            plottableAnnotationsMinVal[index_DataType * IDs_next.Count + index_ID].label = minLabel + " " + char.ConvertFromUtf32(0x2193);

                        }
                    }
                }
                for (int pltIndex = 0; pltIndex < plts.Count; pltIndex++)
                {
                    plts[pltIndex].maxRenderIndex = nextDataIndex - 1;
                }

                for (int formPltIndex = 0; formPltIndex < formsPlots.Count; formPltIndex++)
                {
                    //Console.WriteLine(formsPlots[i].Name);
                    formsPlots[formPltIndex].plt.AxisAuto();
                    formsPlots[formPltIndex].Render();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "에러 메시지");
                throw new Exception(ex.Message);
            }
            nextDataIndex += 1;
            timer2.Interval = 1000;
        }
        
        
        private void timer3_render_Tick(object sender, EventArgs e)
        {
            try
            {
                for (int formPltIndex = 0; formPltIndex < formsPlots.Count; formPltIndex++)
                {
                    double[] autoAxisLimits = formsPlots[formPltIndex].plt.AxisAuto(verticalMargin: .5);
                    double oldX2 = autoAxisLimits[1];
                    formsPlots[formPltIndex].plt.Axis(x2: oldX2 + 1000);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "에러 메시지");
                throw new Exception(ex.Message);
            }
            timer3_render.Interval = 1000;
        }


        private void MinimizeMenuPanel_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (panel1_menu.Bounds.Width > 15)
            {
                panel1_menu.SetBounds(panel1_menu.Bounds.X, 
                                      panel1_menu.Bounds.Y, 
                                      15, 
                                      panel1_menu.Bounds.Height);
                listView1.Visible = false;
                button.Text = ">";
                toolTip1.SetToolTip(button, "선택메누 화면 보이기");  // 마우스 포인팅 시 관련 내용 표시
            }
            else
            {
                panel1_menu.SetBounds(panel1_menu.Bounds.X, 
                                      panel1_menu.Bounds.Y, 
                                      415, 
                                      panel1_menu.Bounds.Height);
                listView1.Visible = true;
                button.Text = "<";
                toolTip1.SetToolTip(button, "선택메누 화면 숨기기");  // 마우스 포인팅 시 관련 내용 표시
            }
        }
    }

    public class Device
    {
        public (List<string> Values, List<string> Times) Temperature { set; get; }
        public (List<string> Values, List<string> Times) Humidity { set; get; }
        public (List<string> Values, List<string> Times) Particle03 { set; get; }
        public (List<string> Values, List<string> Times) Particle05 { set; get; }

        public Device()
        {
            Temperature = (new List<string>(), new List<string>());
            Humidity = (new List<string>(), new List<string>());
            Particle03 = (new List<string>(), new List<string>());
            Particle05 = (new List<string>(), new List<string>());
        }
    }

    /// <summary>
    /// 데이터 쿼리를 위한 만든 클래스
    /// </summary>
    public class MyDataQuery : MainForm
    {
        /// <summary>
        /// 데이터 쿼리 함수
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<List<List<string[]>>> DBQuery(string startDate, string endDate, List<int> IDs, List<string> whatToQuery)
        {

            Device[] devices = new Device[3];
            for (int i = 0; i < devices.Length; i++)
            {
                devices[i] = new Device();
            }
            List<string> vals = new List<string> { "12", "23", "22", "20" };
            List<string> tims = new List<string> { "2020-10-10 15:17:00.000", "2020-10-10 16:16:01.000", "2020-10-10 17:16:11.000", "2020-10-11 15:26:00.000" };

            devices[0].Temperature.Values.AddRange(vals);
            devices[0].Temperature.Times.AddRange(tims);

            Console.WriteLine(devices[0].Temperature.Values.Count.ToString() + devices[0].Temperature.Times.Count);


            List<List<List<string[]>>> DataArr = new List<List<List<string[]>>>(); // new List<List<List<string[]>>>();
            List<SortedDictionary<int, List<string>>> mapVals = new List<SortedDictionary<int, List<string>>>();
            List<SortedDictionary<int, List<string>>> mapTime = new List<SortedDictionary<int, List<string>>>();

            List<string> sql_names = new List<string>();

            for (int i_DataType = 0; i_DataType < whatToQuery.Count; i_DataType++)   //temperature, humidity, particle03, particle05
            {
                DataArr.Add(new List<List<string[]>>());
                mapVals.Add(new SortedDictionary<int, List<string>>());
                mapTime.Add(new SortedDictionary<int, List<string>>());
                if (whatToQuery[i_DataType].Contains("temp")) { sql_names.Add("Temperature"); }
                else if (whatToQuery[i_DataType].Contains("humid")) { sql_names.Add("Humidity"); }
                else if (whatToQuery[i_DataType].Contains("part03")) { sql_names.Add("Particle03"); }
                else { sql_names.Add("Particle05"); }
            }
            for (int i_DataType = 0; i_DataType < whatToQuery.Count; i_DataType++)
            {
                try
                {
                    string sql_head = "SELECT " +
                                            "sensor_id" +
                                            ", " + sql_names[i_DataType] +
                                            ", dateandtime " +
                                        "FROM( ";
                    string sql_connector = " UNION ALL "; // 테이블 연결하는 것
                    string sql_tail = " )a ORDER BY dateandtime";

                    for (int i_ID = 0; i_ID < IDs.Count; i_ID++) // 1,2,3, ...
                    {
                        sql_head += "SELECT " +
                                            IDs[i_ID].ToString() + " AS sensor_id" +
                                            ", " + "AVG(CAST(" + sql_names[i_DataType] + " AS DECIMAL(18, 2))) AS " + sql_names[i_DataType] +
                                            ", SUBSTRING(dateandtime, 1,16) AS dateandtime " +
                                    "FROM dev_" + whatToQuery[i_DataType] + "_" + IDs[i_ID].ToString() +
                                   " WHERE dateandtime BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                                   "GROUP BY SUBSTRING(dateandtime, 1, 16)";
                        if (IDs.Count > 1 && i_ID != (IDs.Count - 1)) { sql_head += sql_connector; }

                        DataArr[i_DataType].Add(new List<string[]>());
                        List<string> mylist1;
                        List<string> mylist2;
                        if (!mapVals[i_DataType].TryGetValue(IDs[i_ID], out mylist1))
                        {
                            mylist1 = new List<string>();
                            mapVals[i_DataType].Add(IDs[i_ID], mylist1);
                        }
                        if (!mapTime[i_DataType].TryGetValue(IDs[i_ID], out mylist2))
                        {
                            mylist2 = new List<string>();
                            mapTime[i_DataType].Add(IDs[i_ID], mylist2);
                        }
                    }
                    sql_head += sql_tail;
                    
                    Console.WriteLine("SQL 쿼리문: " + sql_head);
                    //로컬 db접속 방식
                    //SqlConnection myConnection = new SqlConnection($@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20");
                    SqlConnection myConnection = new SqlConnection($@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20");
                    using (var cmd = new SqlCommand(sql_head, myConnection))
                    {
                        cmd.CommandTimeout = 0;
                        myConnection.Open();
                        Console.WriteLine("Connection opened");
                        using (var myReader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("Executing Reader() method...");
                            int i_ID2 = 0;
                            while (myReader.Read())
                            {
                                Console.WriteLine("Reading data...");
                                if (i_ID2 == IDs.Count) { i_ID2 = 0; }
                                string[] allDataRead = { myReader.GetValue(0).ToString(), myReader[sql_names[i_DataType]].ToString(), myReader["DateAndTime"].ToString() };
                                // Known issue. Fix is under progress.

                                mapVals[i_DataType][Convert.ToInt32(allDataRead[0])].Add(allDataRead[1]);
                                mapTime[i_DataType][Convert.ToInt32(allDataRead[0])].Add(allDataRead[2]);

                                DataArr[i_DataType][i_ID2].Add(new string[] { allDataRead[1], allDataRead[2], allDataRead[0] });
                                Console.WriteLine(i_ID2 + " " + sql_names[i_DataType] + " : " + allDataRead[1] + " " + allDataRead[2] + " " + allDataRead[0]);
                                i_ID2 += 1;
                            }
                        }
                        myConnection.Close();
                    }
                    Console.WriteLine("mapVals, mapTime, dataArr:", mapVals.Count, mapTime.Count, DataArr.Count);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString(), "에러 매시지");
                    //throw new Exception("에러 메시지:\n" + ee.ToString());
                }
            }
            return DataArr;
        }


        public (List<SortedDictionary<int, List<string>>>, List<SortedDictionary<int, List<string>>>) DBQuery2(string startDate, string endDate, List<int> IDs, List<string> whatToQuery)
        {
            List<SortedDictionary<int, List<string>>> mapVals = new List<SortedDictionary<int, List<string>>>();
            List<SortedDictionary<int, List<string>>> mapTime = new List<SortedDictionary<int, List<string>>>();
            List<string> sql_names = new List<string>();

            for (int i_DataType = 0; i_DataType < whatToQuery.Count; i_DataType++)   //temperature, humidity, particle03, particle05
            {
                mapVals.Add(new SortedDictionary<int, List<string>>());
                mapTime.Add(new SortedDictionary<int, List<string>>());
                if (whatToQuery[i_DataType].Contains("temp")) { sql_names.Add("Temperature"); }
                else if (whatToQuery[i_DataType].Contains("humid")) { sql_names.Add("Humidity"); }
                else if (whatToQuery[i_DataType].Contains("part03")) { sql_names.Add("Particle03"); }
                else { sql_names.Add("Particle05"); }
            }
            for (int i_DataType = 0; i_DataType < whatToQuery.Count; i_DataType++)
            {
                try
                {
                    string sql_head = "SELECT " +
                                            "sensor_id" +
                                            ", " + sql_names[i_DataType] +
                                            ", dateandtime " +
                                        "FROM( ";
                    string sql_connector = " UNION ALL "; // 테이블 연결하는 것
                    string sql_tail = " )a ORDER BY dateandtime";

                    for (int i_sensorID = 0; i_sensorID < IDs.Count; i_sensorID++) // 1,2,3, ...
                    {
                        sql_head += "SELECT " +
                                            IDs[i_sensorID].ToString() + " AS sensor_id" +
                                            ", " + "AVG(CAST(" + sql_names[i_DataType] + " AS DECIMAL(18, 2))) AS " + sql_names[i_DataType] +
                                            ", SUBSTRING(dateandtime, 1,16) AS dateandtime " +
                                    "FROM dev_" + whatToQuery[i_DataType] + "_" + IDs[i_sensorID].ToString() +
                                   " WHERE dateandtime BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                                   "GROUP BY SUBSTRING(dateandtime, 1, 16)";
                        if (IDs.Count > 1 && i_sensorID != (IDs.Count - 1)) { sql_head += sql_connector; }

                        List<string> mylist1;
                        List<string> mylist2;
                        if (!mapVals[i_DataType].TryGetValue(IDs[i_sensorID], out mylist1))
                        {
                            mylist1 = new List<string>();
                            mapVals[i_DataType].Add(IDs[i_sensorID], mylist1);
                        }
                        if (!mapTime[i_DataType].TryGetValue(IDs[i_sensorID], out mylist2))
                        {
                            mylist2 = new List<string>();
                            mapTime[i_DataType].Add(IDs[i_sensorID], mylist2);
                        }
                    }
                    sql_head += sql_tail;

                    Console.WriteLine("SQL 쿼리문: " + sql_head);
                    //로컬 db접속 방식
                    //SqlConnection myConnection = new SqlConnection($@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20");
                    SqlConnection myConnection = new SqlConnection($@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20");
                    using (var cmd = new SqlCommand(sql_head, myConnection))
                    {
                        cmd.CommandTimeout = 0;
                        myConnection.Open();
                        Console.WriteLine("Connection opened");
                        using (var myReader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("Executing Reader() method...");
                            while (myReader.Read())
                            {
                                Console.WriteLine("Reading data...");
                                string[] allDataRead = { myReader.GetValue(0).ToString(), myReader[sql_names[i_DataType]].ToString(), myReader["DateAndTime"].ToString() };

                                mapVals[i_DataType][Convert.ToInt32(allDataRead[0])].Add(allDataRead[1]);
                                mapTime[i_DataType][Convert.ToInt32(allDataRead[0])].Add(allDataRead[2]);
                                //Console.WriteLine(sql_names[i_DataType] + " : " + allDataRead[1] + " " + allDataRead[2] + " " + allDataRead[0]);
                            }
                        }
                        myConnection.Close();
                    }
                    Console.WriteLine("mapVals, mapTime, dataArr:", mapVals.Count, mapTime.Count);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString(), "에러 매시지");
                    //throw new Exception("에러 메시지:\n" + ee.ToString());
                }
            }
            return (mapVals, mapTime);
        }
        
        
        /// <summary>
            /// 실시간 데이터 쿼리를 위한 쿼리 함수
            /// </summary>
            /// <param name="IDs"></param>
            /// <param name="whatToQuery"></param>
            /// <returns="List<List<string[]>> DataArr "></returns>
        public List<List<List<string[]>>> RealTimeDBQuery(List<int> IDs, List<string> whatToQuery, List<string> sql_names)
        {
            List<List<List<string[]>>> DataArrRT = new List<List<List<string[]>>>();
            for (int ind = 0; ind < whatToQuery.Count; ind++)
            {
                DataArrRT.Add(new List<List<string[]>>());
            }

            for (int index = 0; index < whatToQuery.Count; index++)
            {
                try
                {
                    string sql_head = "SELECT sensor_id, " + sql_names[index] + ", dateandtime FROM( ";
                    string sql_connector = " UNION ALL "; // 테이블 연결하는 것
                    string sql_tail = " )a ORDER BY sensor_id";

                    for (int i = 0; i < IDs.Count; i++)
                    {
                        DataArrRT[index].Add(new List<string[]>());
                        sql_head += "SELECT TOP 1 " + IDs[i].ToString() + " AS sensor_id, " + sql_names[index] + ", dateandtime " +
                                    "FROM dev_" + whatToQuery[index] + "_" + IDs[i].ToString() +
                                    " ORDER BY dateandtime DESC ";
                        if (IDs.Count > 1 && i != (IDs.Count - 1)) { sql_head += sql_connector; }
                    }
                    sql_head += sql_tail;
                    //Console.WriteLine("SQL RT query: " + sql_head);
                    SqlConnection myConnection = new SqlConnection($@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20");
                    myConnection.Open();
                    using (var cmd = new SqlCommand(sql_head, myConnection))
                    {
                        using (var myReader = cmd.ExecuteReader())
                        {
                            int i = 0;
                            while (myReader.Read())
                            {
                                DataArrRT[index][i].Add(new string[] { myReader[sql_names[index]].ToString(), myReader["DateAndTime"].ToString() });
                                i += 1;
                            }
                        }
                    }
                    myConnection.Close();
                }
                catch (Exception ee)
                {
                    MessageBox.Show("에러 메시지:\n" + ee.ToString());
                    //MessageBox.Show(ee.Message, "에러 메시지");
                }
            }
            return DataArrRT;
        }
    }
}


