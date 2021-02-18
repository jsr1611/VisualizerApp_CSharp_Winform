using ScottPlot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
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
        public SqlConnection myConn { get; set; }

        private string _deviceTable;

        public string S_DeviceTable
        {
            get { return _deviceTable; }
            set { _deviceTable = value; }
        }
        public List<string> S_DeviceTableColumn { get; set; }
        private string _sensorUsage;

        public string SensorUsage
        {
            get { return _sensorUsage; }
            set { _sensorUsage = value; }
        }

        

        private List<string> _sensorUsageColumn;

        public List<string> SensorUsageColumn
        {
            get { return _sensorUsageColumn; }
            set { _sensorUsageColumn = value; }
        }

        public List<string> SensorNames { get; set; }
        public Button[] Btn1_time { get; set; }
        public Button[] Btn2_DataType { get; set; }
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

        public Dictionary<string, List<string>> DeviceZoneLocInfo = new Dictionary<string, List<string>>();
        
        public List<int> IDs_now = new List<int>();
        public List<int> IDs_next = new List<int>();
        public List<int> allIDs = new List<int>();
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

        public Dictionary<int, Dictionary<string, Dictionary<string, long>>> RangeLimitData { get; set; }
        public List<string> RangeNames { get; set; }
        public int nextDataIndex = 1;
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

        private DataQuery _dataQuery;

        public DataQuery G_DataQuery
        {
            get { return _dataQuery; }
            set { _dataQuery = value; }
        }



        public MainForm()
        {
            InitializeComponent();

            // Initialize DB access variables
            dbServerAddress = "localhost\\SQLEXPRESS"; //"127.0.0.1";    //"10.1.55.174";
            dbName = "SensorDataDB";
            dbUID = "dlitadmin";
            dbPWD = "dlitadmin";
            myConn = new SqlConnection($@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20"); // ; Integrated Security=True ");

            S_DeviceTable = "SENSOR_INFO";
            SensorUsage = "SensorUsage";
            SensorUsageColumn = new List<string>();
            SensorUsageColumn = GetTableColumnNames(SensorUsage);
            S_DeviceTableColumn = GetTableColumnNames(S_DeviceTable);
            var S_FourRangeColmn = new List<string>() { "higherLimit2", "higherLimit1", "lowerLimit1", "lowerLimit2" };
            RangeNames = new List<string>() { "상한2", "상한1", "하한1", "하한2" };
            SensorNames = new List<string>() { "온도(°C)", "습도(%)", "파티클(0.3μm)", "파티클(0.5μm)", "파티클(1.0μm)", "파티클(2.5μm)", "파티클(5.0μm)", "파티클(10.0μm)" };
            
          //RangeLimitData = new Dictionary<int, Dictionary<string, long>>();

          DeviceZoneLocInfo = new Dictionary<string, List<string>>();
          string getZoneLocation = $"SELECT {S_DeviceTableColumn[2]}, COUNT(*) FROM {S_DeviceTable} GROUP BY {S_DeviceTableColumn[2]};";
          List<string> sZones = GetColumnDataAsList(getZoneLocation, S_DeviceTableColumn[2]);
          for(int i=0; i<sZones.Count; i++)
          {
              string getLocations = $"SELECT {S_DeviceTableColumn[3]} FROM {S_DeviceTable} WHERE {S_DeviceTableColumn[2]} = '{sZones[i]}';";
              List<string> sLocations = GetColumnDataAsList(getLocations, S_DeviceTableColumn[3]);
              DeviceZoneLocInfo.Add(sZones[i], sLocations);
          }








          this.SetBounds(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height - 50);
          this.AutoScroll = true;
          datePicker1_start.Format = DateTimePickerFormat.Custom;
          datePicker1_start.CustomFormat = "yyyy-MM-dd HH:mm";
          datePicker2_end.Format = DateTimePickerFormat.Custom;
          datePicker2_end.CustomFormat = "yyyy-MM-dd HH:mm";

          colorset = new Color[] { Color.Black, Color.DarkOrange, Color.Blue, Color.Green, Color.Brown, Color.Yellow, Color.Purple, Color.Red, Color.Azure, Color.Chocolate, Color.DarkCyan, Color.Gold, Color.Gray, Color.GreenYellow, Color.Ivory };

          /*KnownColor[] colors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
          colorset = new Color[colors.Length];
          int i = 0;
          foreach (KnownColor knowColor in colors)
          {
              Color color = Color.FromKnownColor(knowColor);
              colorset[i] = color;
              i += 1;
          }*/

            Console.WriteLine(colorset.Length);




            Btn1_time = new Button[] { button1_realtime, button1_24h, button1_datepicker };
            Btn2_DataType = new Button[SensorUsageColumn.Count - 1];
            // { button2_temp, button2_humid, button2_part03, button2_part05 };

            int btn_X = Btn1_time[0].Bounds.X;
            int btn_Y = Btn1_time[0].Bounds.Y + Btn1_time[0].Bounds.Height * 2;

            for (int btn2_index = 0; btn2_index < Btn2_DataType.Length; btn2_index++)
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

                button.Name = $"{SensorUsageColumn[btn2_index+1]}";
                button.Text = SensorNames[btn2_index];
                button.Font = new Font(button.Font.FontFamily, 12);
                button.SetBounds(btn_X, btn_Y, Btn1_time[0].Bounds.Width * 3 / 4, Btn1_time[0].Bounds.Height);
                btn_X += ((Btn1_time[2].Bounds.X + Btn1_time[2].Bounds.Width) / 4);
                if(button.Right + (btn_X-button.Right) + button.Width >= panel1_menu.Right)
                {
                    btn_X = Btn1_time[0].Bounds.X;
                    btn_Y += button.Height;
                }
                button.Click += new EventHandler(this.btn2_data_Click);
                panel1_menu.Controls.Add(button);
                button.Visible = false;
                Btn2_DataType[btn2_index] = button;

            }





            G_DataQuery = new DataQuery(myConn, dbName, S_DeviceTable, SensorUsage, SensorUsageColumn, S_FourRangeColmn);





            //(시각화) 보기 버튼 생성


            Btn_MinimizeMenuPanel.Text = "<";
            Btn_MinimizeMenuPanel.SetBounds(panel1_menu.Bounds.Width - 15, 0, 15, 20);
            Btn_MinimizeMenuPanel.Click += new EventHandler(this.MinimizeMenuPanel_Click);
            Btn_MinimizeMenuPanel.Dock = DockStyle.Right;
            toolTip1.SetToolTip(Btn_MinimizeMenuPanel, "선택메누 화면 숨기기");  // 마우스 포인팅 시 관련 내용 표시
            panel1_menu.Controls.Add(Btn_MinimizeMenuPanel);
/*
            // Panel for peak values under the select menu
            panel4peakVal.SetBounds(15, 569, Btn_MinimizeMenuPanel.Bounds.X, 349);
            panel4peakVal.BorderStyle = BorderStyle.None;
            //panel4peakVal.Dock = DockStyle.Bottom;
            panel1_menu.Controls.Add(panel4peakVal);
*/

            string slq_query = $"SELECT * FROM {S_DeviceTable} ORDER BY {S_DeviceTableColumn[0]}";
            var cmd = new SqlCommand(slq_query, myConn);
            cmd.CommandTimeout = 0;
            try
            {
                if (myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }
                using (var myReader = cmd.ExecuteReader())
                {
                    while (myReader.Read())
                    {
                        ListViewItem item1 = new ListViewItem(myReader.GetValue(0).ToString());
                        for (int i = 1; i < myReader.FieldCount; i++)
                        {
                            item1.SubItems.Add(myReader.GetString(i));
                        }
                        
                        listView1.Items.Add(item1);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "에러 메시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            label_Title_main.Left = panel2_ChartArea.Bounds.Width / 2 - label_Title_main.Bounds.Width / 2;
            label_title_ver.Left = label_Title_main.Right + 15;
        }

        private List<string> GetColumnDataAsList(string sqlStr, string ColumnName)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            List<string> res = new List<string>();

            SqlCommand cmd = new SqlCommand(sqlStr, myConn);
            try
            {
                if(myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(ds);
                try
                {
                    res = ds.Tables[0].AsEnumerable().Select(x => x.Field<string>(ColumnName)).ToList();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Could not get data as type String. Now trying to get data as type Int");
                    res = ds.Tables[0].AsEnumerable().Select(x => x.Field<int>(ColumnName).ToString()).ToList();
                }

            }
            catch(System.Exception ex)
            {
                MessageBox.Show(ex.Message, "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if(myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return res;
        }




        /// <summary>
        /// 상한 및 하한 정보 반환해 주는 함수
        /// </summary>
        /// <param name="s_id"></param>
        /// <returns></returns>
        private Dictionary<string, Dictionary<string, long>> GetRangeLimitData(int s_id)
        {

            var result = new Dictionary<string, Dictionary<string, long>>();
            
            string sqlGetRangeLimit;
            for (int i = 1; i < SensorUsageColumn.Count; i++)
            {
                Dictionary<string, long> res = new Dictionary<string, long>();
                sqlGetRangeLimit = $"SELECT * FROM {SensorUsageColumn[i]} WHERE {SensorUsageColumn[0]} = {s_id};";

                SqlCommand cmd = new SqlCommand(sqlGetRangeLimit, myConn);

                try
                {
                    if (myConn.State != ConnectionState.Open)
                    {
                        myConn.Open();
                    }
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            for (int j = 1; j < r.FieldCount; j++)
                            {
                                res.Add(r.GetName(j), Convert.ToInt64(r.GetValue(j)));
                            }

                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    result.Add(SensorUsageColumn[i], res);
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }


                }

                
            }

            return result;
            
        }




        /// <summary>
        /// 주어진 테이블의 모든 Column명들을 List형태로 반환함.
        /// </summary>
        /// <param name="tableName">테이블명</param>
        /// <returns></returns>
        private List<string> GetTableColumnNames(string tableName)
        {
            List<string> tbColNames = new List<string>();

            try
            {
                string[] restrictions = new string[4] { null, null, $"{tableName}", null };
                if (myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }
                DataTable dt = myConn.GetSchema("Columns", restrictions);
                var dv = dt.DefaultView;
                dv.Sort = "ORDINAL_POSITION ASC";
                dt = dv.ToTable();
                tbColNames = dt.AsEnumerable().Select(x => x.Field<string>("COLUMN_NAME")).ToList();
            }

            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                myConn.Close();
            }

            return tbColNames;
        }




        private void ScotPlot3(List<string> MyDataTypes, List<string> MySqlNames, List<int> MyIDs, string[] startEndDate, bool MyRT_flag)
        {
            panel2_ChartArea.Controls.Clear();
            panel4peakVal.Controls.Clear();
            //formsPlots.Clear();
            plts.Clear();
            nextDataIndex = 0;
            RTDataArray.Clear();

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            panel2_ChartArea.Controls.Add(tableLayoutPanel);

            if (MyRT_flag == false) // 시간 설정 시각화
            {
                timer2.Stop();
                timer3_render.Stop();
                formsPlots = new List<FormsPlot>();
                TableLayoutPrep(tableLayoutPanel, MyDataTypes);
                DataQuery dataQuery = new DataQuery();

                for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
                {

                    for (int i = 1; i < SensorUsageColumn.Count; i++)
                    {
                        if (MyDataTypes[index_DataType].Contains(SensorUsageColumn[i])) { titleName = SensorNames[i - 1]; }
                    }


                    /*if (MyDataTypes[index_DataType].Contains("temp")) { titleName = "온도(°C)"; }
                    else if (MyDataTypes[index_DataType].Contains("humid")) { titleName = "습도(%)"; }
                    else if (MyDataTypes[index_DataType].Contains("part03")) { titleName = "파티클(0.3μm)"; }
                    else { titleName = "파티클(0.5μm)"; }*/

                    List<string> maxValues = new List<string>();
                    List<string> minValues = new List<string>();

                    System.Data.DataSet ds = dataQuery.GetValues(startEndDate[0], startEndDate[1], MySqlNames[index_DataType], MyIDs);
                    for (int i = 0; i < MyIDs.Count; i++)
                    {
                        double[] xs_time = ds.Tables[0].AsEnumerable().Where(r => r.Field<int>("sensor_id") == MyIDs[i]).Select(r => Convert.ToDateTime(r.Field<string>("dateandtime")).ToOADate()).ToArray();
                        double[] ys_data = ds.Tables[0].AsEnumerable().Where(r => r.Field<int>("sensor_id") == MyIDs[i]).Select(r => Convert.ToDouble(r.Field<decimal>(MySqlNames[index_DataType]))).ToArray();
                        formsPlots[index_DataType].plt.PlotSignalXYConst(xs_time, ys_data, label: Btn3_SensorLocation[MyIDs[i] - 1].Text, color: colorset[i]); //              // Signal Chart

                        var (indexOfMax, max) = minMaxIndex(ys_data, true);
                        var (indexOfMin, min) = minMaxIndex(ys_data, false);

                        if (MySqlNames[index_DataType] == "Temperature" || MySqlNames[index_DataType] == "Humidity")
                        {
                            maxValues.Add(max.ToString("F", CultureInfo.InvariantCulture));
                            minValues.Add(min.ToString("F", CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            maxValues.Add(String.Format("{0:n0}", max));
                            minValues.Add(String.Format("{0:n0}", min));
                        }

                        // Console.WriteLine(i + "Max: " + max + " at " + DateTime.FromOADate(xs[indexOfMax]).ToString("yyyy-MM-dd HH:mm:ss.sss"));
                        // Console.WriteLine(i + "Min: " +  min + " at " + DateTime.FromOADate(xs[indexOfMin]).ToString("yyyy-MM-dd HH:mm:ss.sss"));

                    }

                    pltStyler(MyDataTypes, index_DataType);


                    DrawAnnotationBackground(index_DataType, MyIDs);
                    AnnotateMinMax(index_DataType, MyIDs, maxValues, minValues);



                }

                for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
                {
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

                        TableLayoutPrep(tableLayoutPanel, MyDataTypes);
                        /*int peak_yAxis = 50;//button_show.Bounds.Y + button_show.Bounds.Height + 2*button_show.Bounds.Height;
                        int label_yAxis = peak_yAxis - 24;*/



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

                            for (int i = 1; i < SensorUsageColumn.Count; i++)
                            {
                                if (MyDataTypes[i_DataType].Contains(SensorUsageColumn[i])) { titleName = SensorNames[i - 1]; }
                            }


                            for (int i_sensorID = 0; i_sensorID < MyIDs.Count; i_sensorID++)
                            {
                                /*
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



                                //                                RT_Max2[i_DataType][MyIDs[i_sensorID]][0] = MyDataVals[i_DataType][MyIDs[i_sensorID]][0];
                                //                                RT_Min2[i_DataType][MyIDs[i_sensorID]][0] = MyDataTime[i_DataType][MyIDs[i_sensorID]][0];







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

                                //                                DateTime timeData2 = DateTime.Parse(MyDataTime[i_DataType][MyIDs[i_sensorID]][0]);
                                //                                double xs2 = timeData2.ToOADate();

                                // CHARTING Functions

                                //formsPlots[index_DataType].plt.PlotStep(xs, ys);                                    // Step Chart
                                //formsPlots[i].plt.PlotFill(xs, ys);                                    // Fill Chart
                                //formsPlots[index_DataType].plt.PlotScatterHighlight(xs, ys);                          // ScatterHighlight
                                //formsPlots[i].plt.PlotPolygon(Tools.Pad(xs, cloneEdges: true), Tools.Pad(ys));
                                //formsPlots[index_DataType].plt.PlotSignalXYConst(RTtime, RTdata);                                    // Signal Chart // , lineStyle: LineStyle.Dot, color: colorset[index_sensorID]

                                double samplesPerDay = TimeSpan.TicksPerDay / (TimeSpan.TicksPerSecond);






                                //signalPlot = formsPlots[i_DataType].plt.PlotSignal(RTDataArray[i_DataType][i_sensorID][0], samplesPerDay, xs, label: Btn3_SensorLocation[MyIDs[i_sensorID] - 1].Text, color: colorset[i_sensorID]);
                                //                                signalPlot = formsPlots[i_DataType].plt.PlotSignal(RTDataArray2[i_DataType][MyIDs[i_sensorID]], samplesPerDay, xs2, label: Btn3_SensorLocation[MyIDs[i_sensorID] - 1].Text, color: colorset[i_sensorID]);




                                //formsPlots[i].plt.Grid(enable: false);      //Enable-Disable Gridn

                                plts.Add(signalPlot);

                            }
                            pltStyler(MyDataTypes, i_DataType);

                            /*formsPlots[i_DataType].plt.Ticks(dateTimeX: true);
                            formsPlots[i_DataType].plt.Title(titleName, fontSize: 24);
                            formsPlots[i_DataType].plt.YLabel(titleName, fontSize: 20);
                            formsPlots[i_DataType].plt.XLabel("시간", fontSize: 20);

                            

                            formsPlots[i_DataType].plt.Title(titleName, fontSize: 24); // formsPlot1.

                            formsPlots[i_DataType].plt.YLabel(titleName, fontSize: 20); // formsPlot1.
                            formsPlots[i_DataType].plt.XLabel("시간", fontSize: 20);

                            formsPlots[i_DataType].plt.Legend(location: legendLocation.lowerLeft, fontSize: 14);
                            formsPlots[i_DataType].plt.Layout(y2LabelWidth: 80);
                            formsPlots[i_DataType].plt.AxisAuto();*/
                        }


                        //                        AnnotationBackground(MyDataTypes, MyIDs);
                        //                        AnnotationsMinMax(MyDataTypes, MyIDs, MyDataVals, true);
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




        /// <summary>
        /// 차트 배경 색갈 세팅함: FormsPlot figure background color setter
        /// </summary>
        /// <param name="MyDataTypes"></param>
        /// <param name="index"></param>
        private void pltStyler(List<string> MyDataTypes, int index)
        {

            if (MyDataTypes.Count == 1)
            {
                formsPlots[index].plt.Style(figBg: Color.GhostWhite); //tick: Color.White, label: Color.White, title: Color.White
            }
            else
            {
                if (MyDataTypes.Count == 2)
                {
                    formsPlots[index].plt.Style(figBg: Color.WhiteSmoke);
                }
                else
                {
                    formsPlots[index].plt.Style(figBg: Color.FloralWhite);
                }
            }

            formsPlots[index].plt.Ticks(dateTimeX: true);
            //formsPlots[i].plt.Grid(enable: false);      //Enable-Disable Gridn
            formsPlots[index].plt.Title(titleName, fontSize: 24);
            formsPlots[index].plt.YLabel(titleName, fontSize: 20);
            formsPlots[index].plt.XLabel("시간", fontSize: 20);
            formsPlots[index].plt.Legend(location: legendLocation.lowerLeft, fontSize: 14);
            formsPlots[index].plt.Layout(y2LabelWidth: 80);
            formsPlots[index].plt.AxisAuto();

        }


        /// <summary>
        /// 시각화 화면 세탕하기: TableLayoutPanel 구성 세팅함
        /// </summary>
        /// <param name="tableLayoutPanel"></param>
        /// <param name="MyDataTypes"></param>
        private void TableLayoutPrep(TableLayoutPanel tableLayoutPanel, List<string> MyDataTypes)
        {
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

            // Panel 및 FormsPlot 차트 생성 및 TableLayoutPanel 구성 세팅하기
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
                        formsPlot.Name = "formPlot " + index_column * tableLayoutPanel.RowCount + index_row;
                        tableLayoutPanel.Controls.Add(panel, index_row, index_column);
                        formsPlots.Add(formsPlot);
                        panel.Controls.Add(formsPlot);
                    }
                }
            }

        }


        private void DrawAnnotationBackground(int index_DataType, List<int> MyIDs)
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


        private (int, double) minMaxIndex(double[] items, bool maxMin)
        {
            int index = 0;
            double target = items[0];
            if (maxMin)
            {
                for (int i = 1; i < items.Length; i++)
                {
                    if (target < items[i])
                    {
                        target = items[i];
                        index = i;
                    }
                }
            }
            else
            {
                for (int i = 1; i < items.Length; i++)
                {
                    if (target > items[i])
                    {
                        target = items[i];
                        index = i;
                    }
                }
            }
            return (index, target);
        }

        private void AnnotateMinMax(int index_DataType, List<int> MyIDs, List<string> maxVals, List<string> minVals)
        {
            int annotY = -10 - 25 * (MyIDs.Count - 1);
            for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
            {
                formsPlots[index_DataType].plt.PlotAnnotation(maxVals[index_ID] + " " + char.ConvertFromUtf32(0x2191), -10, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);
                formsPlots[index_DataType].plt.PlotAnnotation(minVals[index_ID] + " " + char.ConvertFromUtf32(0x2193), -75, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);

                annotY += 25;
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


        public void AnnotationsMinMax(List<string> MyDataTypes, List<int> MyIDs, List<string> MyDataVals, bool realtime)
        {
            if (realtime == false)
            {
                for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
                {
                    int annotY = -10 - 25 * (MyIDs.Count - 1);
                    for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
                    {
                        //double[] ys = dataArr2[index_ID].Select(x => double.Parse(x)).ToArray();
                        //Tuple<string, int> tupleMax = FindMax(MyDataVals[index_DataType][MyIDs[index_ID]], MyDataTypes, index_DataType);
                        string max = MyDataVals[0]; //tupleMax.Item1;
                        int indexOfMax = Convert.ToInt32(MyDataVals[1]); //tupleMax.Item2;

                        //Tuple<string, int> tupleMin = FindMin(MyDataVals[index_DataType][MyIDs[index_ID]], MyDataTypes, index_DataType);
                        string min = MyDataVals[0]; //tupleMin.Item1;
                        int indexOfMin = Convert.ToInt32(MyDataVals[1]); //tupleMin.Item2;

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
            List<List<List<string[]>>> DataRetrieved_general = new List<List<List<string[]>>>();

            RangeLimitData = new Dictionary<int, Dictionary<string, Dictionary<string, long>>>();
            for (int i=0; i<IDs_now.Count; i++)
            {
                RangeLimitData.Add(IDs_now[i],GetRangeLimitData(IDs_now[i]));
            }

            


            endTime = "RT";
            string[] timeInterval = { startTime, endTime };

            List<string> sql_names = new List<string>();
            for (int i = 0; i < DataTypesNow.Count; i++)
            {
                for (int j = 1; j < SensorUsageColumn.Count; j++)
                {
                    if (DataTypesNow[i].Contains(SensorUsageColumn[j])) { sql_names.Add(SensorUsageColumn[j]); }
                }
            }
            Sql_NamesNow = new List<string>(sql_names);

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


                List<List<List<string[]>>> DataRetrieved_RT = G_DataQuery.RealTimeDBQuery(IDs_now, DataTypesNow, Sql_NamesNow);
                IDs_next = new List<int>(IDs_now);
                DataTypesNext = new List<string>(DataTypesNow);
                if(DataRetrieved_RT.Count != 0)
                {
                    ScotPlot(DataRetrieved_RT, DataTypesNext, IDs_next, true);
                }
                else
                {
                    timer2.Stop();
                    timer3_render.Stop();
                }
                
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
                string[] startEndDate = { startTime, endTime };
                progressbarThread = new Thread(new ThreadStart(WaitForm));
                progressbarThread.Start();

                //Console.WriteLine("mapvals: ", mapVals.Count, mapTime.Count);

                ScotPlot3(DataTypesNow, Sql_NamesNow, IDs_now, startEndDate, false);

                /*progressbarThread.Abort();
                progressbarThread = null;
                MessageBox.Show("No data for visualization.", "Error Message");*/


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

                    string[] startEndDate = { startTime, endTime };
                    progressbarThread = new Thread(new ThreadStart(WaitForm));
                    progressbarThread.Start();

                    /* System.Data.DataSet ds = myDataQuery.GetTempValues();
                     double[] xs = ds.Tables[0].AsEnumerable().Select(r => Convert.ToDateTime(r.Field<string>("dateandtime")).ToOADate()).ToArray();
                     double[] ys = ds.Tables[0].AsEnumerable().Select(r => Convert.ToDouble(r.Field<decimal>("Temperature"))).ToArray();*/
                    //Form form = new Form();

                    //FormsPlot formsPlot = new FormsPlot();
                    //formsPlot.plt.PlotScatter(xs, ys);
                    //DataRetrieved_general = myDataQuery.DBQuery(startTime, endTime, IDs_now, DataTypesNow);
                    //////////////////// new Chart ScotPlot ////////////////////////////////
                    //ScotPlot(DataRetrieved_general, DataTypesNow, IDs_now, false);
                    //var (mapVals, mapTime) = myDataQuery.DBQuery2(startTime, endTime, IDs_now, DataTypesNow);
                    //Console.WriteLine("mapvals: ", mapVals.Count, mapTime.Count);

                    ScotPlot3(DataTypesNow, Sql_NamesNow, IDs_now, startEndDate, false);
                    //if (mapVals[0][IDs_now[0]].Count > 0)
                    //{

                    //}
                    //else
                    //{
                    //    progressbarThread.Abort();
                    //    progressbarThread = null;
                    //    MessageBox.Show("No data for visualization.", "Error Message");
                    //}

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
            List<string> sLocations = new List<string>();
            List<string> sZones = new List<string>();
            string sql_getIDs = $"SELECT {S_DeviceTableColumn[0]} FROM {S_DeviceTable} WHERE {S_DeviceTableColumn[S_DeviceTableColumn.Count-1]} = 'YES'";
            var cmd = new SqlCommand(sql_getIDs, myConn);
            try
            {
                if(myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }
                using (var myReader = cmd.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            //string[] rowInfo = { myReader.GetValue(0).ToString(), myReader.GetValue(1).ToString(), myReader.GetValue(2).ToString(), myReader.GetValue(3).ToString() };
                            SensorIDs_available.Add(Convert.ToInt32(myReader[S_DeviceTableColumn[0]]));
                        }

                        
                    }
                    else
                    {
                        Console.WriteLine("조회할 데이터가 없습니다.");
                    }
                }
                 
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
            catch (Exception ex) { MessageBox.Show(ex.Message, "에러 매시지"); }
            finally
            {
                if(myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
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
            catch
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
            /*if (Btn3_SensorLocation != null && DataTypesNow.Count < 1)
            {
                Btn3_SensorLocation = null;
            }*/
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
         /*   if (Btn3_SensorLocation != null && DataTypesNow.Count < 1)
            {
                Btn3_SensorLocation = null;
            }*/
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
            /*          if (Btn3_SensorLocation != null && DataTypesNow.Count < 1)
                      {
                          Btn3_SensorLocation = null;
                      }
          */

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

                if(DataTypesNow.Count != 4)
                {
                    for (int i = 0; i < Btn2_DataType.Length; i++)
                    {
                        if (Btn2_DataType[i].Image != btnClicked_small)
                        {
                            Btn2_DataType[i].Enabled = true;
                        }
                    }
                }


            }
            else
            {


                //4개 이상의 센서를 선택 못하게 If문으로 확인함.

                if (DataTypesNow.Count < 4)
                {
                    button.Image = btnClicked_small; //BackColor = Color.Chartreuse;
                    DataTypesNow.Add(button.Name);

                    if (DataTypesNow.Count == 4)
                    {

                        for (int i = 0; i < Btn2_DataType.Length; i++)
                        {
                            if (Btn2_DataType[i].Image != btnClicked_small)
                            {
                                Btn2_DataType[i].Enabled = false;
                            }
                        }
                    }
                    
                }


            }

            //4개 이상의 센서를 선택 못하게 If문으로 확인함.
            if (DataTypesNow.Count < 4)
            {
                if (allIDs.Count == 0)
                {
                    string GetAllIdSqlStr = $"SELECT {S_DeviceTableColumn[0]} FROM {S_DeviceTable} ORDER BY {S_DeviceTableColumn[0]};";
                    allIDs = GetColumnDataAsList(GetAllIdSqlStr, S_DeviceTableColumn[0]).Select(x => Convert.ToInt32(x)).ToList();
                }

                if (Btn3_SensorLocation == null || Btn3_SensorLocation.Length == 0)
                {
                    List<int> btn_addresses = new List<int>();

                    string sqlGetIDs = $"SELECT {S_DeviceTableColumn[0]} FROM {S_DeviceTable} ORDER BY {S_DeviceTableColumn[0]};";

                    btn_addresses = GetColumnDataAsList(sqlGetIDs, S_DeviceTableColumn[0]).Select(x => Convert.ToInt32(x)).ToList(); // 시각화 하려는 센서 ID 조회 및 배열에 ID번호 추가하기

                    int x_btn = Btn2_DataType[0].Left;
                    int y_btn = Btn2_DataType[Btn2_DataType.Length - 1].Bounds.Y + Btn2_DataType[Btn2_DataType.Length - 1].Height * 3 / 2;

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



                        string sqlStr = $"SELECT {S_DeviceTableColumn[2]}, {S_DeviceTableColumn[3]} FROM {S_DeviceTable} WHERE {S_DeviceTableColumn[0]} = {btn_addresses[index_btn3]}";
                        button1.Text = GetButtonText(sqlStr, btn_addresses[index_btn3]); //DeviceZoneLocInfo.ElementAt(index_btn3).Key + $"({devZoneLoc.Value[0]})" ;
                        button1.Font = new Font(button1.Font.FontFamily, 8);
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

                //Read-only and Clickable buttons
                if (DataTypesNow.Count >= 1)
                {
                    string getClickablesBtns = $"SELECT {S_DeviceTableColumn[0]} FROM {SensorUsage} WHERE ";
                    for (int i = 0; i < DataTypesNow.Count; i++)
                    {
                        getClickablesBtns += $" {DataTypesNow[i]} = 'YES' ";
                        if (DataTypesNow.Count > 1 && DataTypesNow.Count - 1 != i)
                        {
                            getClickablesBtns += $" AND ";
                        }
                    }

                    List<int> clickableIDs = GetColumnDataAsList(getClickablesBtns, S_DeviceTableColumn[0]).Select(x => Convert.ToInt32(x)).ToList();

                    for (int j = 0; j < allIDs.Count; j++)
                    {
                        if (clickableIDs.Contains(allIDs[j]))
                        {
                            Btn3_SensorLocation[j].Enabled = true;
                        }
                        else
                        {
                            Btn3_SensorLocation[j].Enabled = false;
                        }
                    }
                    button_show.Visible = false;

                }


                // Unclick the buttons
                IDs_now.Clear();
                //IDs_next.Clear();
                for (int i = 0; i < Btn3_SensorLocation.Length; i++)
                {
                    Btn3_SensorLocation[i].Image = btnUnClicked_small; //BackColor = Color.Transparent;
                }

            }

        }

        private string GetButtonText(string sqlStr, int button_index)
        {
            
            /*List<string> sLocations = GetColumnDataAsList(sLocSql, S_DeviceTableColumn[3]);
            var devZoneLoc = DeviceZoneLocInfo.Values;*/
            string btnTxt = "";

            /*for (int i = 0; i < DeviceZoneLocInfo.Count; i++)
            {
                List<string> devZoneN_loc = DeviceZoneLocInfo.ElementAt(i).Value;
                for (int i2 = 0; i2 < devZoneN_loc.Count; i2++)
                {
                    if (sLocations[button_index].Equals(devZoneN_loc[i2]))
                    {
                        btnTxt = DeviceZoneLocInfo.ElementAt(i).Key + $" ({sLocations[button_index]})";
                        return btnTxt;
                    }
                }
            }*/
            SqlCommand cmd = new SqlCommand(sqlStr, myConn);
            try
            {
                if(myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }
                using(SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        for(int i=0; i < r.FieldCount; i++)
                        {
                            if (i == 0 && r.FieldCount > 1)
                            {
                                btnTxt += r.GetValue(i).ToString() + " (";
                            }
                            else if(i != 0 && i == r.FieldCount - 1)
                            {
                                btnTxt += r.GetValue(i).ToString() + ")";
                            }
                            else
                            {
                                btnTxt += r.GetValue(i).ToString();
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if(myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }

            return btnTxt;
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
            DataQuery myDataQuery = new DataQuery();
            List<List<List<string[]>>> DataRetrieved_RT = new List<List<List<string[]>>>();
            DataRetrieved_RT = myDataQuery.RealTimeDBQuery(IDs_next, DataTypesNext, Sql_NamesNow);
            if (DataRetrieved_RT.Count == 0)
            {
                timer1.Stop();
                timer2.Stop();
                timer3_render.Stop();
            }
            else
            {

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
        }


        private void timer2_Tick(object sender, EventArgs e)
        {
            List<List<List<string[]>>> DataRetrieved_RT = new List<List<List<string[]>>>();
            DataRetrieved_RT = G_DataQuery.RealTimeDBQuery(IDs_next, DataTypesNext, Sql_NamesNow);
            if (DataRetrieved_RT.Count == 0)
            {
                timer2.Stop();
                timer3_render.Stop();
            }
            else
            {

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
                            if (DataTypesNext[index_DataType].Contains(SensorUsageColumn[1]) || DataTypesNext[index_DataType].Contains(SensorUsageColumn[2]))
                            {
                                RTDataArray[index_DataType][index_ID][0][nextDataIndex] = Convert.ToDouble(Convert.ToInt64(DataRetrieved_RT[index_DataType][index_ID][0][0]) / 100m);
                            }
                            else
                            {
                                RTDataArray[index_DataType][index_ID][0][nextDataIndex] = Convert.ToDouble(DataRetrieved_RT[index_DataType][index_ID][0][0]);
                            }

                            DateTime dtime = Convert.ToDateTime(DataRetrieved_RT[index_DataType][index_ID][0][1]);
                            RTDataArray[index_DataType][index_ID][1][nextDataIndex] = dtime.ToOADate();
                            Console.WriteLine($"\nnextDataIndex: {nextDataIndex}, Data: {RTDataArray[index_DataType][index_ID][0][nextDataIndex]} at {dtime.ToString("yyyy-MM-dd HH:mm:ss")}");

                            if (Convert.ToDouble(DataRetrieved_RT[index_DataType][index_ID][0][0]) > Convert.ToDouble(RT_Max[index_DataType][index_ID][0][0]))
                            {
                                if (DataTypesNext[index_DataType].Contains(SensorUsageColumn[1]) || DataTypesNext[index_DataType].Contains(SensorUsageColumn[2]))
                                {
                                    RT_Max[index_DataType][index_ID][0][0] = (Convert.ToInt64(DataRetrieved_RT[index_DataType][index_ID][0][0]) / 100m).ToString();
                                }
                                else
                                {
                                    RT_Max[index_DataType][index_ID][0][0] = DataRetrieved_RT[index_DataType][index_ID][0][0];
                                }
                                RT_Max[index_DataType][index_ID][1][0] = DataRetrieved_RT[index_DataType][index_ID][0][1];
                                string numberStrMax = RT_Max[index_DataType][index_ID][0][0];
                                Console.WriteLine($"numberStrMax: {numberStrMax}");
                                if (numberStrMax.Length == 2)
                                {
                                    Console.WriteLine($"I am the troubleMaker:  {numberStrMax}");
                                }
                                string maxLabel = (numberStrMax.Contains(".") == false && numberStrMax.Length > 3) ? numberStrMax.Insert(numberStrMax.Length - 3, ",") : numberStrMax;
                                //Console.WriteLine($"New Max: {RT_Max[index_DataType][index_ID][0][0]} at {RT_Max[index_DataType][index_ID][1][0]} ");
                                plottableAnnotationsMaxVal[index_DataType * IDs_next.Count + index_ID].label = maxLabel + " " + char.ConvertFromUtf32(0x2191);
                            }
                            if (Convert.ToDouble(DataRetrieved_RT[index_DataType][index_ID][0][0]) < Convert.ToDouble(RT_Min[index_DataType][index_ID][0][0]))
                            {
                                if (DataTypesNext[index_DataType].Contains(SensorUsageColumn[1]) || DataTypesNext[index_DataType].Contains(SensorUsageColumn[2]))
                                {
                                    RT_Min[index_DataType][index_ID][0][0] = (Convert.ToInt64(DataRetrieved_RT[index_DataType][index_ID][0][0]) / 100m).ToString();
                                }
                                else
                                {
                                    RT_Min[index_DataType][index_ID][0][0] = DataRetrieved_RT[index_DataType][index_ID][0][0];
                                }
                                RT_Min[index_DataType][index_ID][1][0] = DataRetrieved_RT[index_DataType][index_ID][0][1];
                                //Console.WriteLine($"New Max: {RT_Min[index_DataType][index_ID][0][0]} at {RT_Min[index_DataType][index_ID][1][0]} ");
                                string numberStrMin = RT_Min[index_DataType][index_ID][0][0];
                                Console.WriteLine($"numberStrMin: {numberStrMin}");
                                if (numberStrMin.Length == 2)
                                {
                                    Console.WriteLine($"I am the troubleMaker:  {numberStrMin}");
                                }
                                string minLabel = (numberStrMin.Contains(".") == false && numberStrMin.Length > 3) ? numberStrMin.Insert(numberStrMin.Length - 3, ",") : numberStrMin;
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

                    for (int i = 1; i < SensorUsageColumn.Count; i++)
                    {
                        if (MyDataTypes[index_DataType].Contains(SensorUsageColumn[i])) { titleName = SensorNames[i - 1]; }
                    }


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



                    }
                    pltStyler(MyDataTypes, index_DataType);
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


                        TableLayoutPrep(tableLayoutPanel, MyDataTypes);

                        // 최대값 표시를 위한 textbox 및 label 


                        int peak_yAxis = 50;//button_show.Bounds.Y + button_show.Bounds.Height + 2*button_show.Bounds.Height;
                        int label_yAxis = peak_yAxis - 24;

                        DrawRangeLimitChart(formsPlots, MyDataTypes);
                        for (int index_chart = 0; index_chart < MyDataTypes.Count; index_chart++)
                        {

                            List<List<double[]>> vs = new List<List<double[]>>();
                            RTDataArray.Add(vs);
                            List<List<string[]>> vs_max = new List<List<string[]>>();
                            List<List<string[]>> vs_min = new List<List<string[]>>();
                            RT_Max.Add(vs_max);
                            RT_Min.Add(vs_min);

                            
                            for(int i = 1; i < SensorUsageColumn.Count; i++)
                            {
                                if (MyDataTypes[index_chart].Contains(SensorUsageColumn[i])) { titleName = SensorNames[i-1]; }
                            }

                            for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
                            {
                                List<double[]> vs0 = new List<double[]>();
                                RTDataArray[index_chart].Add(vs0);

                                double[] vs1 = new double[100_000];
                                double[] vs2 = new double[100_000];
                                RTDataArray[index_chart][index_ID].Add(vs1);
                                RTDataArray[index_chart][index_ID].Add(vs2);

                                List<string[]> vs0_max = new List<string[]>();
                                List<string[]> vs0_min = new List<string[]>();
                                RT_Max[index_chart].Add(vs0_max);
                                RT_Min[index_chart].Add(vs0_min);

                                string[] vs1_max = new string[1];
                                string[] vs2_max = new string[1];
                                string[] vs1_min = new string[1];
                                string[] vs2_min = new string[1];


                                RT_Max[index_chart][index_ID].Add(vs1_max);
                                RT_Max[index_chart][index_ID].Add(vs2_max);

                                RT_Min[index_chart][index_ID].Add(vs1_min);
                                RT_Min[index_chart][index_ID].Add(vs2_min);


                                //Max values
                                DateTime dtime_min;
                                RT_Max[index_chart][index_ID][1][0] = MyData[index_chart][index_ID][0][1];// dtime_maxi.ToOADate();
                                dtime_min = DateTime.Parse(MyData[index_chart][index_ID][0][1]);
                                RT_Min[index_chart][index_ID][1][0] = MyData[index_chart][index_ID][0][1]; //dtime_min.ToOADate();

                                if (MyDataTypes[index_chart].Contains(SensorUsageColumn[1]) || MyDataTypes[index_chart].Contains(SensorUsageColumn[2]))
                                {
                                    RT_Max[index_chart][index_ID][0][0] = (Convert.ToInt64(MyData[index_chart][index_ID][0][0])/100m).ToString();
                                    //DateTime dtime_maxi = DateTime.Parse(MyData[index_DataType][index_ID][0][1]);
                                    //Min values
                                    RT_Min[index_chart][index_ID][0][0] = (Convert.ToInt64(MyData[index_chart][index_ID][0][0]) / 100m).ToString(); 
                                }
                                else
                                {
                                    RT_Max[index_chart][index_ID][0][0] = MyData[index_chart][index_ID][0][0];
                                    //DateTime dtime_maxi = DateTime.Parse(MyData[index_DataType][index_ID][0][1]);
                                    //Min values
                                    RT_Min[index_chart][index_ID][0][0] = MyData[index_chart][index_ID][0][0];
                                }
                                
                                /*RT_Max[index_DataType][index_ID][0].Add(Convert.ToDouble(MyData[index_DataType][index_ID][0][0]));
                                DateTime dtime_maxi = DateTime.Parse(MyData[index_DataType][index_ID][0][1]);
                                RT_Max[index_DataType][index_ID][1].Add(dtime_maxi.ToOADate());
                                
                                RT_Min[index_DataType][index_ID][0].Add(Convert.ToDouble(MyData[index_DataType][index_ID][0][0]));
                                DateTime dtime_min = DateTime.Parse(MyData[index_DataType][index_ID][0][1]);
                                RT_Min[index_DataType][index_ID][1].Add(dtime_min.ToOADate());*/

                                Console.WriteLine($"RT_Max {RT_Max[index_chart][index_ID][0][0]} , RT_Min  {RT_Min[index_chart][index_ID][0][0]} were initialized at {DateTime.Now.ToString("HH:mm:ss") }");
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
                                signalPlot = formsPlots[index_chart].plt.PlotSignal(RTDataArray[index_chart][index_ID][0], samplesPerDay, xs, label: Btn3_SensorLocation[index_ID].Text, color: colorset[index_ID]);

                                plts.Add(signalPlot);

                            }

                            pltStyler(MyDataTypes, index_chart);
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
                                string maxLabel = (numberStrMax.Contains(".") == false && numberStrMax.Length > 3) ? numberStrMax.Insert(numberStrMax.Length - 3, ",") : numberStrMax;
                                string minLabel = (numberStrMin.Contains(".") == false && numberStrMin.Length > 3) ? numberStrMin.Insert(numberStrMin.Length - 3, ",") : numberStrMin;

                                PlottableAnnotation pltAnnot = formsPlots[index_DataType].plt.PlotAnnotation(label: maxLabel + " " + char.ConvertFromUtf32(0x2191), -10, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);
                                PlottableAnnotation pltAnnot_min = formsPlots[index_DataType].plt.PlotAnnotation(label: minLabel + " " + char.ConvertFromUtf32(0x2193), -75, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);
                                //Console.WriteLine("Lbl: " + pltAnnot.label + ", vis: " + pltAnnot.visible + ", x: " + pltAnnot.xPixel + ", y: " + pltAnnot.yPixel);
                                plottableAnnotationsMaxVal.Add(pltAnnot);
                                plottableAnnotationsMinVal.Add(pltAnnot_min);
                                annotY += 25;
                            }
                        }
                        //Console.WriteLine("\n\n\n");
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




        /// <summary>
        /// 상한 및 하한 범위 차트 시각화해 주는 함수
        /// </summary>
        /// <param name="formsPlots">차트 리스트</param>
        /// <param name="sensor">센서 리스트</param>
        private void DrawRangeLimitChart(List<FormsPlot> formsPlots, List<string> sensor)
        {
            var data = RangeLimitData;
            var dict = RangeLimitData.Values;
            
            foreach(var item in dict)
            {
                for (int i = 0; i < formsPlots.Count; i++)
                {
                    if (i == formsPlots.Count)
                    {
                        break;
                    }
                    var item2 = item[sensor[i]].Values;

/*                    // manually define Y axis tick positions and labels
                    double[] yPositions = new double[4];
                    string[] yLabels = { "하한2", "하한1", "상한1", "상한2" };
                    int Y_index = 0;
                    foreach (var item3 in item2)
                    {
                        yPositions[Y_index] = item3;
                        Y_index += 1;  
                    }

                    formsPlots[i].plt.YTicks(yPositions, yLabels);

*/                    
                    List<long> data2 = new List<long>();
                    int j = 0;
                    foreach (var item3 in item2)
                    {
                        data2.Add(item3);
                        formsPlots[i].plt.PlotHLine(y: item3, lineStyle: LineStyle.Dash, lineWidth:2);



                        
                        










                        formsPlots[i].Render();
                        j += 1;
                    }
                }
                break;
            }

            //formsPlot.plt.PlotSignal(RTDataArray[index_chart][index_ID][0], samplesPerDay, xs, label: Btn3_SensorLocation[index_ID].Text, color: colorset[index_ID]);
            //throw new NotImplementedException();
        }
    }

}


