using ScottPlot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace VisualizerApp
{
    public partial class MainForm : Form
    {
        public string startTime = "";
        public string endTime = "";
        public Button[] Btn1_time { get; set; }
        public Button[] Btn2_data { get; set; }
        public Button[] Btn3_address { get; set; }
        public List<int> SensorIDs_available = new List<int>();
        public List<int> SensorIDs_selected = new List<int>();
        public List<int> IDs_SelectedNext = new List<int>();
        public List<int> Data_selected = new List<int>();
        public string whatToShow;
        public List<string> whatToShow2 = new List<string>();
        public List<string> whatToShowNext = new List<string>();
        public string titleName = "";
        public List<List<TextBox>> RT_textBoxes = new List<List<TextBox>>();
        public List<Label> RT_Labels = new List<Label>();
        public bool digital_flag { get; set; }

        public List<string> myObjectList { get; set; }
        public List<List<List<double[]>>> RTDataArr = new List<List<List<double[]>>>();
        List<List<List<double[]>>> RTDataArray = new List<List<List<double[]>>>();
        int nextDataIndex = 1;
        public List<FormsPlot> formsPlots = new List<FormsPlot>();
        public List<PlottableSignal> plts = new List<PlottableSignal>();
        public PlottableSignal signalPlot;

        public MainForm()
        {
            InitializeComponent();
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
            comboBox1.SelectedIndex = 0;

        }


        private void ScotPlot(List<List<List<string[]>>> MyDataArr, List<string> DataTypesArr, List<int> MySensorIDs_sel, bool MyRT_flag)
        {
            Color[] colorset = new Color[] { Color.DarkOrange, Color.Red, Color.Blue, Color.Green };
            panel4peakVal.Controls.Clear();
            int numOfElmnt = 0;
            if (DataTypesArr.Count > 1)
            {
                numOfElmnt = CountNumOfElmnt(MyDataArr, MySensorIDs_sel, "all"); //데이터 개수 : number of Elements(temp)
            }
            else
            {
                numOfElmnt = CountNumOfElmnt(MyDataArr, MySensorIDs_sel, DataTypesArr[0]);
            }

            string[] dataVal = new string[numOfElmnt];
            string[] timeVal = new string[numOfElmnt];
            panel2_ChartArea.Controls.Clear();
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            panel2_ChartArea.Controls.Add(tableLayoutPanel);
            formsPlots.Clear();
            plts.Clear();
            nextDataIndex = 0;
            RTDataArray.Clear();
            timer1.Stop();
            timer2.Stop();
            timer3_render.Stop();


            if (MyRT_flag == false) // 시간 설정 시각화
            {

                // 시각화 화면 세탕하기 1: TableLayoutPanel 구성 세팅
                if (DataTypesArr.Count < 2)
                {
                    tableLayoutPanel.RowCount = 1;
                    tableLayoutPanel.ColumnCount = 1;
                    tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                }
                else if (DataTypesArr.Count == 2)
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
                List<RichTextBox> PeakValTextBoxes = new List<RichTextBox>();
                List<Label> PeakValLabels = new List<Label>();
                // ScotPlot 차트 함수인 FormsPlot들을 위한 List 생성  
                List<FormsPlot> formsPlots = new List<FormsPlot>();

                string[][] dataArr2 = new string[MySensorIDs_sel.Count][]; //dataArr[0][0].Count
                string[][] timeArr2 = new string[MySensorIDs_sel.Count][];

                int peak_yAxis = 50;//button_show.Bounds.Y + button_show.Bounds.Height + 2*button_show.Bounds.Height;
                int label_yAxis = peak_yAxis - 24;

                for (int index_sensorID = 0; index_sensorID < MySensorIDs_sel.Count; index_sensorID++)
                {
                    dataArr2[index_sensorID] = new string[numOfElmnt];
                    timeArr2[index_sensorID] = new string[numOfElmnt];

                    // 최고값 시각화를 위한 textbox 및 label 생성 및 
                    RichTextBox richTextBox = new RichTextBox();
                    richTextBox.SetBounds(button3_solbi1.Bounds.X, peak_yAxis, button_show.Bounds.Width, button_show.Bounds.Height);
                    peak_yAxis += button_show.Bounds.Height + 25;
                    PeakValTextBoxes.Add(richTextBox);
                    panel4peakVal.Controls.Add(richTextBox);

                    Label label = new Label();
                    label.SetBounds(richTextBox.Bounds.X, label_yAxis, richTextBox.Bounds.Width, 24);
                    label_yAxis += button_show.Bounds.Height + 25;
                    PeakValLabels.Add(label);
                    panel4peakVal.Controls.Add(label);

                }
                // 시각화 화면 세탕하기 2: TableLayoutPanel의 구성요소들 생성
                for (int index_column = 0; index_column < tableLayoutPanel.ColumnCount; index_column++)
                {
                    for (int index_row = 0; index_row < tableLayoutPanel.RowCount; index_row++)
                    {
                        if (DataTypesArr.Count > index_column * tableLayoutPanel.RowCount + index_row)
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

                for (int index_DataType = 0; index_DataType < DataTypesArr.Count; index_DataType++)
                {

                    if (DataTypesArr[index_DataType].Contains("temp")) { titleName = "온도(°C)"; }
                    else if (DataTypesArr[index_DataType].Contains("humid")) { titleName = "습도(%)"; }
                    else if (DataTypesArr[index_DataType].Contains("part03")) { titleName = "파티클(0.3μm)"; }
                    else { titleName = "파티클(0.5μm)"; }

                    int annotY = 10;
                    int annotY2 = -10;
                    var plt = formsPlots[index_DataType];
                    for (int index_sensorID = 0; index_sensorID < MySensorIDs_sel.Count; index_sensorID++)
                    {
                        for (int index_DataElem = 0; index_DataElem < numOfElmnt; index_DataElem++)
                        {
                            dataArr2[index_sensorID][index_DataElem] = MyDataArr[index_DataType][index_sensorID][index_DataElem][0];
                            timeArr2[index_sensorID][index_DataElem] = MyDataArr[index_DataType][index_sensorID][index_DataElem][1];
                        }

                        //Console.WriteLine("num of data points: {0}", numOfElmnt);
                        double[] ys = dataArr2[index_sensorID].Select(x => double.Parse(x)).ToArray();
                        DateTime[] timeData = timeArr2[index_sensorID].Select(x => DateTime.Parse(x)).ToArray();
                        double[] xs = timeData.Select(x => x.ToOADate()).ToArray();
                        
                            formsPlots[index_DataType].plt.PlotSignalXYConst(xs, ys, lineStyle: LineStyle.Dot, color: colorset[index_sensorID]);                                    // Signal Chart
                            //formsPlots[index_DataType].plt.PlotSignalXYConst(xs, ys, lineStyle: LineStyle.Dot, color: colorset[index_sensorID]);                                    // Signal Chart
                        


                        // CHARTING Functions

                        //var sig = formsPlots[i].plt.PlotSignal(ys, sampleRate: 24*60, xOffset: xs[0]);     //                   --> Needs bug fixes
                        //formsPlots[i].plt.PlotScatter(xs, ys, lineWidth: 0);                          // Scatter Chart

                        //formsPlots[index_DataType].plt.PlotStep(xs, ys);                                    // Step Chart
                        //formsPlots[i].plt.PlotFill(xs, ys);                                    // Fill Chart
                        //formsPlots[index_DataType].plt.PlotScatterHighlight(xs, ys);                          // ScatterHighlight
                        //formsPlots[i].plt.PlotPolygon(Tools.Pad(xs, cloneEdges: true), Tools.Pad(ys));

                        //formsPlots[i].plt.Grid(enable: false);      //Enable-Disable Gridn


                        //formsPlots[i].plt.Ticks(dateTimeX: true); //formsPlot1.
                        formsPlots[index_DataType].plt.Ticks(dateTimeX: true); //formsPlot1.
                        formsPlots[index_DataType].plt.Title(titleName + " 센서 데이터 시각화", fontSize: 24); // formsPlot1.
                        formsPlots[index_DataType].plt.YLabel(titleName, fontSize: 20); // formsPlot1.
                        formsPlots[index_DataType].plt.XLabel("시간", fontSize: 20);
                        if (DataTypesArr.Count == 1)
                        {
                            formsPlots[index_DataType].plt.Style(figBg: Color.Black, tick: Color.White);
                        }
                        else
                        {
                            if (DataTypesArr.Count == 2)
                            {
                                formsPlots[index_DataType].plt.Style(figBg: Color.LimeGreen);
                            }
                            else
                            {
                                formsPlots[index_DataType].plt.Style(figBg: Color.LightBlue);
                            }

                        }


                        /*formsPlots[i].plt.PlotAnnotation("설비 " + IDs[i].ToString(), 10, 10, fontSize: 20);
                        formsPlots[i].plt.Title(titleName + " 센서 데이터 시각화", fontSize: 24); // formsPlot1.
                        formsPlots[i].plt.YLabel(titleName, fontSize: 20); // formsPlot1.
                        formsPlots[i].plt.XLabel("시간", fontSize: 20);
                        formsPlots[i].plt.Style(figBg: Color.LightBlue);*/
                        Tuple<double, int> tupleMax = FindMax(ys);
                        double max = tupleMax.Item1;
                        int indexOfMax = tupleMax.Item2;

                        PeakValTextBoxes[index_sensorID].AppendText("\n" + timeData[indexOfMax].ToString());
                        PeakValTextBoxes[index_sensorID].Text = max.ToString();
                        PeakValTextBoxes[index_sensorID].Font = new Font(PeakValTextBoxes[index_sensorID].Font.FontFamily, 25);
                        toolTip1.SetToolTip(PeakValTextBoxes[index_sensorID], timeData[indexOfMax].ToString());  // 마우스 포인팅 시 관련 시간을 표시함
                        PeakValLabels[index_sensorID].Font = new Font(PeakValLabels[index_sensorID].Font.FontFamily, 16);
                        PeakValTextBoxes[index_sensorID].SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Center;
                        PeakValLabels[index_sensorID].Text = "설비 " + SensorIDs_selected[index_sensorID].ToString() + " 최고값";
                        PeakValLabels[index_sensorID].TextAlign = ContentAlignment.MiddleCenter;

                        formsPlots[index_DataType].plt.PlotAnnotation("설비 " + MySensorIDs_sel[index_sensorID].ToString(), 10, annotY, fontSize: 20, fontColor: colorset[index_sensorID]);
                        formsPlots[index_DataType].plt.PlotAnnotation("최고값: " + max.ToString(), -10, annotY2, fontSize: 12, fontColor: colorset[index_sensorID]);
                        annotY += 35;
                        annotY2 -= 25;

                        //formsPlots[i].plt.SaveFig(titleName + "_" + i.ToString() + "_" + DateTime.Now.ToString("MMdd_HHmm") + ".png");
                    }
                    formsPlots[index_DataType].Render();
                }

            }
            else // 실시간 시각화
            {
                // 시각화 하려는 데이터 타입 갯수에 따라 textbox 컨테이너(List<T>) 생성
                //digital_flag = true;
                if (digital_flag)
                {
                    timer1.Start();

                    if (SensorIDs_selected.Count < 2)
                    {
                        tableLayoutPanel.RowCount = 1;
                        tableLayoutPanel.ColumnCount = 1;
                        tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    }
                    else if (SensorIDs_selected.Count == 2)
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
                    RT_Labels.Clear();
                    for (int i = 0; i < MySensorIDs_sel.Count; i++)
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
                            if (SensorIDs_selected.Count > index_column * tableLayoutPanel.RowCount + index_row)
                            {
                                int yBound = 0;

                                Panel panel = new Panel();
                                panel.BorderStyle = BorderStyle.FixedSingle;
                                panel.Dock = DockStyle.Fill;
                                tableLayoutPanel.Controls.Add(panel, index_row, index_column);

                                Label label = new Label();
                                RT_Labels.Add(label);
                                label.SetBounds(panel.Bounds.Width / 2 - 150, 20, 300, 50);
                                label.Font = new Font(label.Font.FontFamily, 20, System.Drawing.FontStyle.Bold);
                                label.TextAlign = ContentAlignment.MiddleCenter;

                                label.Text = "설비 " + SensorIDs_selected[index_column * tableLayoutPanel.RowCount + index_row];
                                panel.Controls.Add(label);
                                //Application.DoEvents();
                                // 시각화 하려는 센서 갯수에 따라 textbox 생성 
                                for (int boxIndex = 0; boxIndex < DataTypesArr.Count; boxIndex++)
                                {
                                    int counter = 0;
                                    if (DataTypesArr.Count > 1) { counter = DataTypesArr.Count; }
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
                else // Chart form
                {
                    timer2.Start();
                    timer3_render.Start();

                    //throw new NotImplementedException(); //Not Implemented Yet
                    // 시각화 화면 세탕하기 1: TableLayoutPanel 구성 세팅
                    if (DataTypesArr.Count < 2)
                    {
                        tableLayoutPanel.RowCount = 1;
                        tableLayoutPanel.ColumnCount = 1;
                        tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    }
                    else if (DataTypesArr.Count == 2)
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
                    List<RichTextBox> PeakValTextBoxes = new List<RichTextBox>();
                    List<Label> PeakValLabels = new List<Label>();
                    // ScotPlot 차트 함수인 FormsPlot들을 위한 List 생성  

                    string[][] dataArr2 = new string[MySensorIDs_sel.Count][]; //dataArr[0][0].Count
                    string[][] timeArr2 = new string[MySensorIDs_sel.Count][];

                    int peak_yAxis = 50;//button_show.Bounds.Y + button_show.Bounds.Height + 2*button_show.Bounds.Height;
                    int label_yAxis = peak_yAxis - 24;

                    try
                    {
                        for (int index_sensorID = 0; index_sensorID < MySensorIDs_sel.Count; index_sensorID++)
                        {
                            dataArr2[index_sensorID] = new string[numOfElmnt];
                            timeArr2[index_sensorID] = new string[numOfElmnt];

                            // 최고값 시각화를 위한 textbox 및 label 생성 및 
                            /*RichTextBox richTextBox = new RichTextBox();
                            richTextBox.SetBounds(button3_solbi1.Bounds.X, peak_yAxis, button_show.Bounds.Width, button_show.Bounds.Height);
                            peak_yAxis += button_show.Bounds.Height + 25;
                            PeakValTextBoxes.Add(richTextBox);
                            panel4peakVal.Controls.Add(richTextBox);

                            Label label = new Label();
                            label.SetBounds(richTextBox.Bounds.X, label_yAxis, richTextBox.Bounds.Width, 24);
                            label_yAxis += button_show.Bounds.Height + 25;
                            PeakValLabels.Add(label);
                            panel4peakVal.Controls.Add(label);*/

                        }
                        // 시각화 화면 세탕하기 2: TableLayoutPanel의 구성요소들 생성
                        for (int index_column = 0; index_column < tableLayoutPanel.ColumnCount; index_column++)
                        {
                            for (int index_row = 0; index_row < tableLayoutPanel.RowCount; index_row++)
                            {
                                if (DataTypesArr.Count > index_column * tableLayoutPanel.RowCount + index_row)
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


                        for (int index_DataType = 0; index_DataType < DataTypesArr.Count; index_DataType++)
                        {
                            List<List<double[]>> vs = new List<List<double[]>>();
                            RTDataArray.Add(vs);
                            if (DataTypesArr[index_DataType].Contains("temp")) { titleName = "온도(°C)"; }
                            else if (DataTypesArr[index_DataType].Contains("humid")) { titleName = "습도(%)"; }
                            else if (DataTypesArr[index_DataType].Contains("part03")) { titleName = "파티클(0.3μm)"; }
                            else { titleName = "파티클(0.5μm)"; }

                            /*int annotY = 10;
                            int annotY2 = -10;*/
                            //var plt = formsPlots[index_DataType].plt.PlotSignal(RTdata);  //formsPlots[index_DataType];

                            for (int index_sensorID = 0; index_sensorID < MySensorIDs_sel.Count; index_sensorID++)
                            {
                                List<double[]> vs0 = new List<double[]>();
                                RTDataArray[index_DataType].Add(vs0);
                                
                                    double[] vs1 = new double[100_000];
                                double[] vs2 = new double[100_000];
                                
                                RTDataArray[index_DataType][index_sensorID].Add(vs1);
                                RTDataArray[index_DataType][index_sensorID].Add(vs2);
                                DateTime timeData = DateTime.Parse(MyDataArr[index_DataType][index_sensorID][0][1]);
                                double xs = timeData.ToOADate();

                                /* for (int index_DataElem = 0; index_DataElem < numOfElmnt; index_DataElem++)
                                 {
                                     dataArr2[index_sensorID][index_DataElem] = MyDataArr[index_DataType][index_sensorID][index_DataElem][0];
                                     timeArr2[index_sensorID][index_DataElem] = MyDataArr[index_DataType][index_sensorID][index_DataElem][1];
                                 }

                                 //Console.WriteLine("num of data points: {0}", numOfElmnt);
                                 double[] ys = dataArr2[index_sensorID].Select(x => double.Parse(x)).ToArray();
                                 DateTime[] timeData = timeArr2[index_sensorID].Select(x => DateTime.Parse(x)).ToArray();
                                 double[] xs = timeData.Select(x => x.ToOADate()).ToArray();
                                 realtime_date = ys.ToArray();
                                 realtime_values = xs.ToArray();
                                 RTdata = ys.ToArray();
                                 RTtime = xs.ToArray();*/



                                // CHARTING Functions

                                //formsPlots[index_DataType].plt.PlotStep(xs, ys);                                    // Step Chart
                                //formsPlots[i].plt.PlotFill(xs, ys);                                    // Fill Chart
                                //formsPlots[index_DataType].plt.PlotScatterHighlight(xs, ys);                          // ScatterHighlight
                                //formsPlots[i].plt.PlotPolygon(Tools.Pad(xs, cloneEdges: true), Tools.Pad(ys));

                                //formsPlots[index_DataType].plt.PlotSignalXYConst(RTtime, RTdata);                                    // Signal Chart // , lineStyle: LineStyle.Dot, color: colorset[index_sensorID]
                                //formsPlots[index_DataType].plt.PlotScatter(RTtime, RTdata, lineWidth: 0);                          // Scatter Chart
                                //formsPlots[i].plt.Grid(enable: false);      //Enable-Disable Gridn
                                double samplesPerDay = TimeSpan.TicksPerDay / (TimeSpan.TicksPerSecond);
                                signalPlot = formsPlots[index_DataType].plt.PlotSignal(RTDataArray[index_DataType][index_sensorID][0], samplesPerDay, xs);
                                formsPlots[index_DataType].plt.Ticks(dateTimeX: true); 
                                formsPlots[index_DataType].plt.Title(titleName + " 센서 데이터 시각화", fontSize: 24);
                                formsPlots[index_DataType].plt.YLabel(titleName, fontSize: 20); 
                                formsPlots[index_DataType].plt.XLabel("시간", fontSize: 20);
                                if (DataTypesArr.Count == 1) {
                                    formsPlots[index_DataType].plt.Style(figBg: Color.Black, tick: Color.White);
                                }
                                else {
                                    if (DataTypesArr.Count == 2) {
                                        formsPlots[index_DataType].plt.Style(figBg: Color.LimeGreen);
                                    }
                                    else {
                                        formsPlots[index_DataType].plt.Style(figBg: Color.LightBlue);
                                    }
                                }

                                /*formsPlots[i].plt.PlotAnnotation("설비 " + IDs[i].ToString(), 10, 10, fontSize: 20);
                                formsPlots[i].plt.Title(titleName + " 센서 데이터 시각화", fontSize: 24); // formsPlot1.
                                formsPlots[i].plt.YLabel(titleName, fontSize: 20); // formsPlot1.
                                formsPlots[i].plt.XLabel("시간", fontSize: 20);
                                formsPlots[i].plt.Style(figBg: Color.LightBlue);*/
                                /* Tuple<double, int> tupleMax = FindMax(ys);
                                 double max = tupleMax.Item1;
                                 int indexOfMax = tupleMax.Item2;

                                 PeakValTextBoxes[index_sensorID].AppendText("\n" + timeData[indexOfMax].ToString());
                                 PeakValTextBoxes[index_sensorID].Text = max.ToString();
                                 PeakValTextBoxes[index_sensorID].Font = new Font(PeakValTextBoxes[index_sensorID].Font.FontFamily, 25);
                                 toolTip1.SetToolTip(PeakValTextBoxes[index_sensorID], timeData[indexOfMax].ToString());  // 마우스 포인팅 시 관련 시간을 표시함
                                 PeakValLabels[index_sensorID].Font = new Font(PeakValLabels[index_sensorID].Font.FontFamily, 16);
                                 PeakValTextBoxes[index_sensorID].SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Center;
                                 PeakValLabels[index_sensorID].Text = "설비 " + SensorIDs_selected[index_sensorID].ToString() + " 최고값";
                                 PeakValLabels[index_sensorID].TextAlign = ContentAlignment.MiddleCenter;

                                 formsPlots[index_DataType].plt.PlotAnnotation("설비 " + MySensorIDs_sel[index_sensorID].ToString(), 10, annotY, fontSize: 20, fontColor: colorset[index_sensorID]);
                                 formsPlots[index_DataType].plt.PlotAnnotation("최고값: " + max.ToString(), -10, annotY2, fontSize: 12, fontColor: colorset[index_sensorID]);
                                 annotY += 35;
                                 annotY2 -= 25;*/

                                //formsPlots[i].plt.SaveFig(titleName + "_" + i.ToString() + "_" + DateTime.Now.ToString("MMdd_HHmm") + ".png");
                                plts.Add(signalPlot);
                            }
                        }
                    }
                    catch(Exception ex) { MessageBox.Show(ex.Message, "Error Message"); }
                    //Console.WriteLine(RTDataArray.Count);
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
                if (button1_realtime.BackColor != Color.Transparent && SensorIDs_selected.Count > 0)
                {
                    Console.WriteLine("Now RealTime");
                    timer1.Enabled = true;
                    timer2.Enabled = true;
                    timer3_render.Enabled = true;
                    
                    SensorIDs_selected.Sort();
                    timeInterval[0] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    timeInterval[1] = "RT";
                    List<List<List<string[]>>> DataRetrieved_RT = myDataQuery.RealTimeDBQuery(SensorIDs_selected, whatToShow2);
                    IDs_SelectedNext = new List<int>(SensorIDs_selected);
                    whatToShowNext = new List<string>(whatToShow2);
                    ScotPlot(DataRetrieved_RT, whatToShow2, SensorIDs_selected, true);
                    //RealTimeCharting(DataRetrieved_RT, timeInterval, IDs_selected, whatToShow);

                    //NewChartingForm RTChart = new NewChartingForm(DataRetrieved_RT, timeInterval, SensorIDs_selected, whatToShow);
                    //RTChart.Show();
                }
                else if (button1_24h.BackColor != Color.Transparent && SensorIDs_selected.Count > 0)
                {
                    timer1.Stop();
                    Console.WriteLine("Now 24H");
                    SensorIDs_selected.Sort();
                    startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm");
                    endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    DataRetrieved_general = myDataQuery.DBQuery(startTime, endTime, SensorIDs_selected, whatToShow2);
                    Console.WriteLine(DataRetrieved_general.Count);
                    temp_max(DataRetrieved_general);

                    ScotPlot(DataRetrieved_general, whatToShow2, SensorIDs_selected, false);
                    //ChartingForm GeneralChart = new ChartingForm(DataRetrieved_general, timeInterval, IDs_selected, whatToShow);
                    //NewChartingForm GeneralChart = new NewChartingForm(DataRetrieved_general, timeInterval, IDs_selected, whatToShow);
                    //GeneralChart.Show();
                }
                else if (button1_datepicker.BackColor != Color.Transparent && SensorIDs_selected.Count > 0)
                {
                    timer1.Stop();
                    Console.WriteLine("Now from " + startTime + " to " + endTime);
                    SensorIDs_selected.Sort();
                    if (datePicker1_start.Value < datePicker2_end.Value)
                    {
                        startTime = datePicker1_start.Value.ToString("yyyy-MM-dd HH:mm");
                        endTime = datePicker2_end.Value.ToString("yyyy-MM-dd HH:mm");
                        Console.WriteLine(startTime + " " + endTime);
                        DataRetrieved_general = myDataQuery.DBQuery(startTime, endTime, SensorIDs_selected, whatToShow2);
                        Console.WriteLine(DataRetrieved_general.Count);
                        temp_max(DataRetrieved_general);

                        Console.WriteLine("Lens {0} {1}", DataRetrieved_general[0], DataRetrieved_general[0][0]);
                        //////////////////// new Chart ScotPlot ////////////////////////////////
                        ScotPlot(DataRetrieved_general, whatToShow2, SensorIDs_selected, false);
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

        }


        private void temp_max(List<List<List<string[]>>> data)
        {
            int dCount = 0;
            for (int k = 0; k < data[0].Count; k++)
            {
                dCount += data[0][k].Count;
            }
        }

        /// <summary>
        /// 최고값과 인덱스를 반환해 주는 함수
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Tuple<double, int> FindMax(double[] data)
        {
            if (data.Length == 0) { throw new InvalidOperationException("Empty list"); }
            double max = data[0];
            int index = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (max <= data[i])
                {
                    max = data[i];
                    index = i;
                }
            }
            Console.WriteLine("\nMax: {0}", max);
            return new Tuple<double, int>(max, index);
        }
        private void IDs_AvailCheck()
        {
            SensorIDs_available.Clear();
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
                                SensorIDs_available.Add(Convert.ToInt32(rowInfo[0]));
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
        private void highlightSelectedBtn(Button[] btnNames, int index, Color color)
        {
            for (int i = 0; i < btnNames.Length; i++)
            {
                if (index == i)
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
            button1_numRT.Visible = true;
            button1_chartRT.Visible = true;
            //"실시간" button
            highlightSelectedBtn(Btn1_time, 0, Color.Chartreuse);
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
            button1_numRT.Visible = false;
            button1_chartRT.Visible = false;
            //"24시간" button
            highlightSelectedBtn(Btn1_time, 1, Color.Chartreuse);

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
            button1_numRT.Visible = false;
            button1_chartRT.Visible = false;
            //"시간 설정" button
            highlightSelectedBtn(Btn1_time, 2, Color.Chartreuse);
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
            //"온도" button
            if (button2_temp.BackColor != Color.Transparent)
            {
                button2_temp.BackColor = Color.Transparent;
                whatToShow2.Remove("temp");
            }
            else
            {
                button2_temp.BackColor = Color.Chartreuse;
                whatToShow2.Add("temp");
            }
        }

        private void button5_humid_Click(object sender, EventArgs e)
        {
            if (button2_humid.BackColor != Color.Transparent)
            {
                button2_humid.BackColor = Color.Transparent;
                whatToShow2.Remove("humid");
            }
            else
            {
                button2_humid.BackColor = Color.Chartreuse;
                whatToShow2.Add("humid");
            }
        }

        private void button6_part03_Click(object sender, EventArgs e)
        {
            if (button2_part03.BackColor != Color.Transparent)
            {
                button2_part03.BackColor = Color.Transparent;
                whatToShow2.Remove("part03");
            }
            else
            {
                button2_part03.BackColor = Color.Chartreuse;
                whatToShow2.Add("part03");
            }
        }

        private void button6_part05_Click(object sender, EventArgs e)
        {
            if (button2_part05.BackColor != Color.Transparent)
            {
                button2_part05.BackColor = Color.Transparent;
                whatToShow2.Remove("part05");
            }
            else
            {
                button2_part05.BackColor = Color.Chartreuse;
                whatToShow2.Add("part05");
            }
        }

        private void button7_solbi1_Click(object sender, EventArgs e)
        {
            if (SensorIDs_available.Contains(1) == false)
            {
                MessageBox.Show("조회 불가한 센서 ID입니다.");
            }
            else
            {
                if (SensorIDs_selected.Contains(1) == false) // button3_solbi1.BackColor == Color.Transparent
                {
                    SensorIDs_selected.Add(1);
                    button3_solbi1.BackColor = Color.Chartreuse;
                }
                else
                {
                    SensorIDs_selected.Remove(1);
                    button3_solbi1.BackColor = Color.Transparent;
                }
            }
        }

        private void button8_solbi2_Click(object sender, EventArgs e)
        {
            if (SensorIDs_available.Contains(2) == false)
            {
                MessageBox.Show("조회 불가한 센서 ID입니다.");
            }
            else
            {
                if (SensorIDs_selected.Contains(2) == false)
                {
                    SensorIDs_selected.Add(2);
                    button3_solbi2.BackColor = Color.Chartreuse;
                }
                else
                {
                    SensorIDs_selected.Remove(2);
                    button3_solbi2.BackColor = Color.Transparent;
                }
            }
        }

        private void button9_solbi3_Click(object sender, EventArgs e)
        {
            if (SensorIDs_available.Contains(3) == false)
            {
                MessageBox.Show("조회 불가한 센서 ID입니다.");
            }
            else
            {

                if (SensorIDs_selected.Contains(3) == false)
                {
                    SensorIDs_selected.Add(3);
                    button3_solbi3.BackColor = Color.Chartreuse;
                }
                else
                {
                    SensorIDs_selected.Remove(3);
                    button3_solbi3.BackColor = Color.Transparent;
                }
            }
        }

        private void button10__solbi4_Click(object sender, EventArgs e)
        {
            if (SensorIDs_available.Contains(4) == false)
            {
                MessageBox.Show("조회 불가한 센서 ID입니다.", "에러 매시지");
            }
            else
            {
                if (SensorIDs_selected.Contains(4) == false)  //button3_solbi4.BackColor == Color.Transparent
                {
                    SensorIDs_selected.Add(4);
                    button3_solbi4.BackColor = Color.Chartreuse;
                }
                else
                {
                    SensorIDs_selected.Remove(4);
                    button3_solbi4.BackColor = Color.Transparent;
                }
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            MyDataQuery myDataQuery = new MyDataQuery();
            List<List<List<string[]>>> DataRetrieved_RT = new List<List<List<string[]>>>();
            DataRetrieved_RT = myDataQuery.RealTimeDBQuery(IDs_SelectedNext, whatToShowNext);
            string printedData = "";
            try
            {
                for (int ind = 0; ind < RT_textBoxes.Count; ind++)
                { //Data_selected
                    for (int i = 0; i < RT_textBoxes[ind].Count; i++) //whatToShow2
                    {
                        //string[][] dataArr2 = DataRetrieved_RT[ind][i].Select(x => x.ToArray()).ToArray();
                        if (whatToShowNext[i].Contains("temp"))
                        {
                            printedData = String.Concat(DataRetrieved_RT[i][ind][0][0].Where(c => !Char.IsWhiteSpace(c))) + " °C";
                            RT_textBoxes[ind][i].Text = printedData;
                        }
                        else if (whatToShowNext[i].Contains("humid"))
                        {
                            printedData = String.Concat(DataRetrieved_RT[i][ind][0][0].Where(c => !Char.IsWhiteSpace(c))) + " %";
                            RT_textBoxes[ind][i].Text = printedData;
                        }
                        else if (whatToShowNext[i].Contains("part03"))
                        {
                            printedData = String.Format("{0:n0}", Convert.ToInt64(DataRetrieved_RT[i][ind][0][0])) + " (0.3μm)";
                            RT_textBoxes[ind][i].Text = printedData;
                        }
                        else
                        {
                            //Int64 num = Convert.ToInt64(DataRetrieved_RT[ind][i][0][0]);
                            printedData = String.Format("{0:n0}", Convert.ToInt64(DataRetrieved_RT[i][ind][0][0])) + " (0.5μm)";
                            RT_textBoxes[ind][i].Text = printedData;
                        }
                        //Console.WriteLine("printedData " + printedData);
                    }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "에러 메시지");
            }
            //Console.WriteLine();
            timer1.Interval = 1000;
        }

        private void button1_numRT_Click(object sender, EventArgs e)
        {
            if (button1_numRT.BackColor != Color.Transparent)
            {
                button1_numRT.BackColor = Color.Transparent;

            }
            else
            {
                button1_numRT.BackColor = Color.Chartreuse;
                button1_chartRT.BackColor = Color.Transparent;
                digital_flag = true;
            }
        }

        private void button1_chartRT_Click(object sender, EventArgs e)
        {
            if (button1_chartRT.BackColor != Color.Transparent)
            {
                button1_chartRT.BackColor = Color.Transparent;
            }
            else
            {
                button1_chartRT.BackColor = Color.Chartreuse;
                button1_numRT.BackColor = Color.Transparent;
                digital_flag = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e){
            MyDataQuery myDataQuery = new MyDataQuery();
            List<List<List<string[]>>> DataRetrieved_RT = new List<List<List<string[]>>>();
            DataRetrieved_RT = myDataQuery.RealTimeDBQuery(IDs_SelectedNext, whatToShowNext);
            try {
                string now = DateTime.Now.ToString("HH:mm:ss");
                DateTime resetTime = Convert.ToDateTime(now);
                //Console.WriteLine(now + " {0}", (resetTime > Convert.ToDateTime("23:59:58") && resetTime < Convert.ToDateTime("23:59:59")));
                if (resetTime > Convert.ToDateTime("23:59:58") && resetTime < Convert.ToDateTime("23:59:59"))
                {
                    nextDataIndex = 0;
                }
                    for (int i=0; i<whatToShowNext.Count; i++) {
                    for(int id_index=0; id_index<IDs_SelectedNext.Count; id_index++) {
                        RTDataArray[i][id_index][0][nextDataIndex] = Convert.ToDouble(DataRetrieved_RT[i][id_index][0][0]);
                        DateTime dtime = Convert.ToDateTime(DataRetrieved_RT[i][id_index][0][1]);
                        RTDataArray[i][id_index][1][nextDataIndex] = dtime.ToOADate();
                    }
                }
            for(int pltIndex=0; pltIndex<plts.Count; pltIndex++) {
                plts[pltIndex].maxRenderIndex = nextDataIndex;
            }

            for (int i=0; i< formsPlots.Count; i++){
                //Console.WriteLine(formsPlots[i].Name);
                formsPlots[i].plt.AxisAuto();
                formsPlots[i].Render();
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message, "에러 메시지");
            }
            nextDataIndex += 1;
            timer2.Interval = 1000;
        }

        private void timer3_render_Tick(object sender, EventArgs e) {
            try {
                for (int i = 0; i < formsPlots.Count; i++) {
                    double[] autoAxisLimits = formsPlots[i].plt.AxisAuto(verticalMargin: .5);
                    double oldX2 = autoAxisLimits[1];
                    formsPlots[i].plt.Axis(x2: oldX2 + 1000);
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message, "에러 메시지");
            }
            timer3_render.Interval = 1000;
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
        public List<List<List<string[]>>> DBQuery(string startDate, string endDate, List<int> IDs, List<string> whatToQuery)
        {
            List<List<string[]>> DataArr = new List<List<string[]>>();
            List<List<List<string[]>>> DataArr2 = new List<List<List<string[]>>>();

            //    string sql_name = ""; // 데이터 변수명 
            //string whatToQuery = "temp";

            List<string> sql_names = new List<string>();

            for (int ind = 0; ind < whatToQuery.Count; ind++)
            {
                DataArr2.Add(new List<List<string[]>>());

                if (whatToQuery[ind].Contains("temp")) { sql_names.Add("Temperature"); }
                else if (whatToQuery[ind].Contains("humid")) { sql_names.Add("Humidity"); }
                else if (whatToQuery[ind].Contains("part03")) { sql_names.Add("Particle03"); }
                else { sql_names.Add("Particle05"); }

            }


            for (int index = 0; index < whatToQuery.Count; index++)
            {




                string sql_head = "select sensor_id, " + sql_names[index] + ", dateandtime from( ";
                string sql_connector = " union all "; // 테이블 연결하는 것
                string sql_tail = " )a order by dateandtime";

                for (int i = 0; i < IDs.Count; i++)
                {
                    sql_head += "select " + IDs[i].ToString() + " as sensor_id, " + sql_names[index] + ", dateandtime from dev_" + whatToQuery[index] + "_" + IDs[i].ToString() + " where dateandtime >= '" + startDate + "' and  dateandtime <= '" + endDate + "'";
                    if (IDs.Count > 1 && i != (IDs.Count - 1)) { sql_head += sql_connector; }
                }
                sql_head += sql_tail;

                Console.WriteLine("SQL query: " + sql_head);

                for (int i = 0; i < IDs.Count; i++)
                {
                    DataArr.Add(new List<string[]>());
                    DataArr2[index].Add(new List<string[]>());
                }
                try
                {
                    //SqlConnection myConnection = new SqlConnection(@"Data Source=DESKTOP-DLIT\SQLEXPRESS;Initial Catalog=SensorDataDB;Integrated Security=True");
                    SqlConnection myConnection = new SqlConnection(@"Data Source=10.1.55.174;Initial Catalog=SensorDataDB;User id=dlitdb;Password=dlitdb; Min Pool Size=20");
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
                                string[] myobj = { myReader.GetValue(1).ToString(), myReader.GetValue(2).ToString() };
                                //string[] all = { myReader.GetValue(0).ToString(), myReader.GetValue(1).ToString(), myReader.GetValue(2).ToString() };
                                //Console.WriteLine(myObj[0].ToString() + " " + myObj[1].ToString());
                                DataArr[i].Add(myobj);
                                DataArr2[index][i].Add(myobj);
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
            }
            return DataArr2;

        }
        /// <summary>
        /// 실시간 데이터 쿼리를 위한 쿼리 함수
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="whatToQuery"></param>
        /// <returns="List<List<string[]>> DataArr "></returns>
        public List<List<List<string[]>>> RealTimeDBQuery(List<int> IDs, List<string> whatToQuery)
        {
            List<List<string[]>> DataArr = new List<List<string[]>>();
            List<List<List<string[]>>> DataArr2 = new List<List<List<string[]>>>();

            List<string> sql_names = new List<string>();

            for (int ind = 0; ind < whatToQuery.Count; ind++)
            {
                DataArr2.Add(new List<List<string[]>>());

                if (whatToQuery[ind].Contains("temp")) { sql_names.Add("Temperature"); }
                else if (whatToQuery[ind].Contains("humid")) { sql_names.Add("Humidity"); }
                else if (whatToQuery[ind].Contains("part03")) { sql_names.Add("Particle03"); }
                else { sql_names.Add("Particle05"); }

            }

            for (int index = 0; index < whatToQuery.Count; index++)
            {
                string sql_head = "select sensor_id, " + sql_names[index] + ", dateandtime from( ";
                string sql_connector = " union all "; // 테이블 연결하는 것
                string sql_tail = " )a order by sensor_id";

                for (int i = 0; i < IDs.Count; i++)
                {
                    sql_head += "select top 1 " + IDs[i].ToString() + " as sensor_id, " + sql_names[index] + ", dateandtime from dev_" + whatToQuery[index] + "_" + IDs[i].ToString() + " order by dateandtime desc ";
                    if (IDs.Count > 1 && i != (IDs.Count - 1)) { sql_head += sql_connector; }
                    DataArr.Add(new List<string[]>());
                    DataArr2[index].Add(new List<string[]>());
                }
                sql_head += sql_tail;

                // select sensor_id, temperature, dateandtime from( select top 1 1 as sensor_id, temperature, DateAndTime from dev_temp_1 order by dateandtime desc union all select top 1 2 as sensor_id, temperature, dateandtime from dev_temp_2 order by dateandtime desc union all select top 1 3 as sensor_id, temperature, dateandtime from dev_temp_3 order by dateandtime desc) a order by sensor_id;


                //Console.WriteLine("SQL RT query: " + sql_head);
                //for (int i = 0; i < IDs.Count; i++) { }

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
                                DataArr2[index][i].Add(myobj);
                                i += 1;

                            }
                        }
                    }
                    myConnection.Close();
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString(), "에러 메시지");
                }
            }
            return DataArr2;
        }
    }


}

