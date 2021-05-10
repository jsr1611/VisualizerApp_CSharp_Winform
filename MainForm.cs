using ScottPlot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DataSet = System.Data.DataSet;

namespace ParticleDataVisualizerApp
{
    public partial class MainForm : Form
    {
        public string dbServerAddress = "";
        public string dbName = "";
        public string dbUID = "";
        public string dbPWD = "";
        public SqlConnection myConn { get; set; }
        public string sqlConStr { get; set; }

        public string S_DeviceTable { get; set; }
        public List<string> S_DeviceTableColumn { get; set; }
        public List<string> S_FourRangeColmn { get; }

        public string SensorUsage { get; set; }

        public List<string> SensorUsageColumn { get; set; }

        public List<string> SensorNames { get; set; }
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
        public Dictionary<string, List<string>> DeviceZoneLocInfo = new Dictionary<string, List<string>>();

        public List<int> IDs_now = new List<int>();
        public List<int> IDs_next = new List<int>();
        public List<int> allIDs = new List<int>();

        public List<string> DataTypesNow = new List<string>();
        public List<string> DataTypesNext = new List<string>();
        public string titleName = "";
        public List<List<TextBox>> RT_textBoxes = new List<List<TextBox>>();

        public Color[] colorset { get; set; }
        public bool digital_flag { get; set; }

        public List<List<List<double[]>>> RTDataArray = new List<List<List<double[]>>>();



        public DataSet AvgData { get; set; }
        public Thread UpdAvgDataThread;

        public List<List<List<string>>> RT_Max3 = new List<List<List<string>>>();
        public List<List<List<string>>> RT_Min3 = new List<List<List<string>>>();

        public Dictionary<string, List<long>> RangeLimitData { get; set; }
        public List<string> RangeNames { get; set; }
        public int nextDataIndex = 1;
        public List<FormsPlot> formsPlots { get; set; }

        public List<List<PlottableSignal>> plt_list = new List<List<PlottableSignal>>();
        public List<List<int>> newNextDataIndex = new List<List<int>>();
        public bool data_ok = false;

        public PlottableSignal signalPlot;
        public List<List<PlottableAnnotation>> plottableAnnotationsMaxVal2 = new List<List<PlottableAnnotation>>();
        public List<List<PlottableAnnotation>> plottableAnnotationsMinVal2 = new List<List<PlottableAnnotation>>();

        private static Thread progressbarThread;
        public Bitmap btnClicked_big = ParticleDataVisualizerApp.Properties.Resources._05;
        public Bitmap btnUnClicked_big = ParticleDataVisualizerApp.Properties.Resources._04;
        public Bitmap btnClicked_small = ParticleDataVisualizerApp.Properties.Resources.btn_sm7;
        public Bitmap btnUnClicked_small = ParticleDataVisualizerApp.Properties.Resources.btn_sm6;

        public DataQuery G_DataQuery { get; set; }



        public MainForm()
        {
            InitializeComponent();



            ////////////////////////////////////////////////////////


            // ini 읽기 //////
            IniFile ini = new IniFile();

            ini.Load(AppInfo.StartupPath + "\\" + "Setting.ini");

            string D_IP = ini["DBSetting"]["IP"].ToString();
            string D_SERVERNAME = ini["DBSetting"]["SERVERNAME"].ToString();
            string D_NAME = ini["DBSetting"]["DBNAME"].ToString();
            string D_TABLENAME = ini["DBSetting"]["DEVICETABLE"].ToString();
            string D_USAGETABLENAME = ini["DBSetting"]["USAGETABLE"].ToString();
            string D_ID = ini["DBSetting"]["ID"].ToString();
            string D_PW = ini["DBSetting"]["PW"].ToString();
            string D_RTLimitTime = ini["DBSetting"]["RTLIMITTIME"].ToString();
            string D_AVGLimitTime = ini["DBSetting"]["AVGLIMITTIME"].ToString();


            string R_RangeHigh2 = ini["RangeLimitTable"]["RangeHigh2"].ToString();
            string R_RangeHigh1 = ini["RangeLimitTable"]["RangeHigh1"].ToString();
            string R_RangeLow1 = ini["RangeLimitTable"]["RangeLow1"].ToString();
            string R_RangeLow2 = ini["RangeLimitTable"]["RangeLow2"].ToString();



            ////////////////////////////////////////////////////////





            // Initialize DB access variables
            dbServerAddress = D_SERVERNAME; //"localhost\\SQLEXPRESS"; //"127.0.0.1";    //"10.1.55.174";
            dbName = D_NAME; // "SensorData2"; //"SensorDataNewDB";
            dbUID = D_ID; // "admin";
            dbPWD = D_PW; // "admin";
            sqlConStr = $@"Data Source={dbServerAddress};Initial Catalog={dbName}; Integrated Security=True";
            myConn = new SqlConnection(sqlConStr); // ; Integrated Security=True ");

            try
            {
                myConn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while SQL connection: {ex.Message}. {ex.StackTrace}");
                dbServerAddress = D_IP;
                sqlConStr = $@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20";
                myConn = new SqlConnection(sqlConStr);
                try
                {
                    myConn.Open();
                    Console.WriteLine("Connection Successful!");
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"Error while SQL connection 2: {ex2.Message}. {ex2.StackTrace}");
                    dbServerAddress = "192.168.11.209";
                    dbName = dbServerAddress + @"\SQLEXPRESS";
                    sqlConStr = $@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20";
                    myConn = new SqlConnection(sqlConStr);
                    try
                    {
                        myConn.Open();
                        Console.WriteLine("Connection Successful!");
                    }
                    catch (Exception ex3) { Console.WriteLine($"Error while SQL connection 2: {ex3.Message}. {ex3.StackTrace}"); }
                        
                }
            }

            S_DeviceTable = D_TABLENAME; // "SENSOR_INFO";

            SensorUsage = D_USAGETABLENAME; // "SensorUsage";
            SensorUsageColumn = new List<string>();
            S_DeviceTableColumn = GetTableColumnNames(S_DeviceTable);

            SensorUsageColumn = GetTableColumnNames(SensorUsage);


            S_FourRangeColmn = new List<string>() { R_RangeHigh2, R_RangeHigh1, R_RangeLow1, R_RangeLow2 };// { "higherLimit2", "higherLimit1", "lowerLimit1", "lowerLimit2" };

            RangeNames = new List<string>() { "상한2", "상한1", "하한1", "하한2" };
            SensorNames = new List<string>(); // "온도(°C)", "습도(%)", "파티클(0.3μm)", "파티클(0.5μm)", "파티클(1.0μm)", "파티클(2.5μm)", "파티클(5.0μm)", "파티클(10.0μm)" };
            for (int i = 1; i < SensorUsageColumn.Count; i++)
            {
                if (SensorUsageColumn[i].Contains("t"))
                {
                    SensorNames.Add("온도(°C)");
                }
                else if (SensorUsageColumn[i].Contains("h"))
                {
                    SensorNames.Add("습도(%)");
                }
                else if (SensorUsageColumn[i].Contains("p"))
                {
                    string a = SensorUsageColumn[i];
                    string b = string.Empty;
                    for (int j = 0; j < a.Length; j++)
                    {
                        if (Char.IsDigit(a[j]))
                            b += a[j];
                    }
                    int indeks = b.Length - 1;
                    /*if(b.Length >= 2)
                    {
                        indeks = b.Length - 2;
                    }*/
                    string pname = $"파티클 ({b.Insert(indeks, ".")}μm)";
                    SensorNames.Add(pname);
                }

            }
            //RangeLimitData = new Dictionary<int, Dictionary<string, long>>();


            DeviceZoneLocInfo = new Dictionary<string, List<string>>();
            string getZoneLocation = $"SELECT {S_DeviceTableColumn[2]}, COUNT(*) FROM {S_DeviceTable} GROUP BY {S_DeviceTableColumn[2]};";
            List<string> sZones = GetColumnDataAsList("string", getZoneLocation, S_DeviceTableColumn[2]);
            for (int i = 0; i < sZones.Count; i++)
            {
                string getLocations = $"SELECT {S_DeviceTableColumn[3]} FROM {S_DeviceTable} WHERE {S_DeviceTableColumn[2]} = '{sZones[i]}';";
                List<string> sLocations = GetColumnDataAsList("string", getLocations, S_DeviceTableColumn[3]);
                DeviceZoneLocInfo.Add(sZones[i], sLocations);
            }








            //this.SetBounds(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height - 50);
            this.WindowState = FormWindowState.Maximized;
            this.AutoScroll = true;
            datePicker1_start.Format = DateTimePickerFormat.Custom;
            datePicker1_start.CustomFormat = "yyyy-MM-dd HH:mm";
            datePicker2_end.Format = DateTimePickerFormat.Custom;
            datePicker2_end.CustomFormat = "yyyy-MM-dd HH:mm";

            colorset = new Color[] { Color.Black, Color.DarkOrange, Color.Blue, Color.Green, Color.Brown, Color.Yellow, Color.Purple, Color.Red, Color.Azure, Color.Chocolate, Color.DarkCyan, Color.Gold, Color.Gray, Color.GreenYellow, Color.Ivory };
            //Console.WriteLine(colorset.Length);



            //CreateButtonsForSensors();
            Btn2_DataType = new Button[SensorUsageColumn.Count - 1];

            int btn_X = button1_realtime.Bounds.X;
            int btn_Y = button1_realtime.Bounds.Y + button1_realtime.Bounds.Height * 2;


            Btn2_DataType = GenerateButtonsHere(Btn2_DataType, btn2_data_Click, SensorUsageColumn, SensorNames, btn_X, btn_Y);








            G_DataQuery = new DataQuery(myConn, dbName, S_DeviceTable, SensorUsage, SensorUsageColumn, S_FourRangeColmn, sqlConStr);
            G_DataQuery.RTLimitTime = Convert.ToInt32(D_RTLimitTime);
            G_DataQuery.AvgLimitTime = Convert.ToInt32(D_AVGLimitTime);





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

        private Button[] GenerateButtonsHere(Button[] Btns, EventHandler btn_data_Click, List<string> buttonNames, List<string> buttonTxt, int btn_X, int btn_Y)
        {
            for (int btn2_index = 0; btn2_index < Btns.Length; btn2_index++)
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

                button.Name = $"{buttonNames[btn2_index + 1]}";
                button.Text = buttonTxt[btn2_index];
                button.Font = new Font(button.Font.FontFamily, 12);
                button.SetBounds(btn_X, btn_Y, button1_realtime.Bounds.Width * 3 / 4, button1_realtime.Bounds.Height);
                btn_X += ((button1_datepicker.Bounds.X + button1_datepicker.Bounds.Width) / 4);
                if (button.Right + (btn_X - button.Right) + button.Width >= panel1_menu.Right)
                {
                    btn_X = button1_realtime.Bounds.X;
                    btn_Y += button.Height;
                }
                button.Click += new EventHandler(btn_data_Click);
                panel1_menu.Controls.Add(button);
                button.Visible = false;
                Btns[btn2_index] = button;

            }

            return Btns;
        }





        /// <summary>
        /// 테이블 특정 Column의 데이터를 반환함.
        /// </summary>
        /// <param name="dataType">데이터 타입</param>
        /// <param name="sqlStr">SQL쿼리문</param>
        /// <param name="ColumnName">테이블명</param>
        /// <returns></returns>
        private List<string> GetColumnDataAsList(string dataType, string sqlStr, string ColumnName)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            List<string> res = new List<string>();

            SqlCommand cmd = new SqlCommand(sqlStr, myConn);
            try
            {
                if (myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(ds);
                try
                {
                    if (dataType.Equals("string"))
                    {
                        res = ds.Tables[0].AsEnumerable().Select(x => x.Field<string>(ColumnName)).ToList();
                    }
                    else if (dataType.Equals("int"))
                    {
                        res = ds.Tables[0].AsEnumerable().Select(x => x.Field<int>(ColumnName).ToString()).ToList();
                    }
                    else if (dataType.Equals("double"))
                    {
                        res = ds.Tables[0].AsEnumerable().Select(x => x.Field<double>(ColumnName).ToString()).ToList();
                    }
                    else
                    {
                        return res;
                    }

                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Could not get the data type. Sorry.");

                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return res;
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







        /// <summary>
        /// 차트 배경 색갈 세팅함: FormsPlot figure background color setter
        /// </summary>
        /// <param name="MyDataTypes"></param>
        /// <param name="index"></param>
        private void pltStyler(List<string> MyDataTypes, int index, string chartTitle)
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
            formsPlots[index].plt.Title(chartTitle + "                           data", fontSize: 24);
            //formsPlots[index].plt.Title.RtlTranslateAlignment = 
            formsPlots[index].plt.YLabel(chartTitle, fontSize: 20);
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
                        formsPlot.Name = MyDataTypes[index_column * tableLayoutPanel.RowCount + index_row]; //"formPlot " + index_column * tableLayoutPanel.RowCount + index_row;
                        tableLayoutPanel.Controls.Add(panel, index_row, index_column);
                        formsPlots.Add(formsPlot);
                        panel.Controls.Add(formsPlot);
                    }
                }
            }

        }


        private void DrawAnnotationBackground(int index_DataType, List<PlottableSignalXYConst<double, double>> MyIDs)
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
            double target = 0;
            if (items.Length > 0)
            {
                target = items[0];
                // max val
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
                else // min val
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
            }
            return (index, target);
        }

        private void AnnotateMinMax(int index_DataType, List<string> maxVals, List<string> minVals)
        {
            int annotY = -10 - 25 * (maxVals.Count - 1);
            string maxLabel = "";
            string minLabel = "";
            for (int index_ID = 0; index_ID < maxVals.Count; index_ID++)
            {
                maxLabel = maxVals.Count > index_ID ? maxVals[index_ID] + " " : " ";
                minLabel = minVals.Count > index_ID ? minVals[index_ID] + " " : " ";
                formsPlots[index_DataType].plt.PlotAnnotation(maxLabel + char.ConvertFromUtf32(0x2191), -10, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);
                formsPlots[index_DataType].plt.PlotAnnotation(minLabel + char.ConvertFromUtf32(0x2193), -75, annotY, fontSize: 12, fontColor: colorset[index_ID], fillAlpha: 1, lineWidth: 0, fillColor: Color.White);
                annotY += 25;
            }
        }









        public void AnnotationBackground(List<List<PlottableSignal>> MyFormPlots, List<string> MyDataTypes, List<int> MyIDs)
        {
            for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
            {
                switch (plt_list[index_DataType].Count)
                {
                    case 4:
                        formsPlots[index_DataType].plt.PlotAnnotation(label: "ANN", -10, -10, fontSize: 60, fontColor: Color.White, fillColor: Color.White, fillAlpha: 1);
                        break;
                    case 3:
                        formsPlots[index_DataType].plt.PlotAnnotation(label: "ANN", -10, -10, fontSize: 50, fontColor: Color.White, fillColor: Color.White, fillAlpha: 1);
                        break;
                    case 2:
                        formsPlots[index_DataType].plt.PlotAnnotation(label: "ANNOTA", -10, -10, fontSize: 30, fontColor: Color.White, fillColor: Color.White, fillAlpha: 1);
                        break;
                    case 1:
                        formsPlots[index_DataType].plt.PlotAnnotation(label: "ANNOTATION BOX", -10, -10, fontSize: 13, fontColor: Color.White, fillColor: Color.White, fillAlpha: 1);
                        break;
                    default:
                        // do nothing
                        break;
                }

            }
        }


        /*

                private void MouseHover()
                {
                    var plottables = formsPlots[0].plt.GetPlottables();
                    var signalPlot = (ScottPlot.PlottableSignal)plottables[0];
                    var highlightSignal = (ScottPlot.PlottableSignal)plottables[1];
                    var highlightText = (ScottPlot.PlottableSignal)plottables[2];


                    // get mouse position on the screen
                    Point mouseLoc = new Point(Cursor.Position.X, Cursor.Position.Y);

                    //modify it to be mouse position on the ScottPlot
                    mouseLoc.X -= this.PointToScreen(formsPlots[0].Location).X;
                    mouseLoc.Y -= this.PointToScreen(formsPlots[0].Location).Y;


                    //PointF mousePos = formsPlots[0].plt.CoordinateFromPixelY(mouseLoc.Y);

                    int closestIndex = 0;
                    double closestDistance = double.PositiveInfinity;
                    for(int i=0; i < signalPlot.ys.Length; i++)
                    {
                        double dx = mouseLoc.X - formsPlots[0].plt.CoordinateToPixel(signalPlot. xs[i], 0).X;
                        double dy = mouseLoc.Y - formsPlots[0].plt.CoordinateToPixel(0, signalPlot.ys[i]).Y;
                        double distance = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                        if (closestIndex < 0)
                        {
                            closestDistance = distance;
                        }
                        else if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestIndex = i;
                        }
                    }
                }
        */



        /// <summary>
        /// Progress Bar
        /// </summary>
        private static void WaitForm()
        {
            //Application.Run(new ProgressBarForm());
            ProgressBarForm progressBarForm = new ProgressBarForm();
            progressBarForm.ShowDialog();
        }








        //Starting Point for Visualization



        //확인 (시각화) 버튼 누를 때에의 행위
        /// <summary>
        /// Function to display results in form of chart for the selected time interval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_show_Click(object sender, EventArgs e)
        {
            //List<List<List<string[]>>> DataRetrieved_general = new List<List<List<string[]>>>();
            DataSet MyData;

            RangeLimitData = new Dictionary<string, List<long>>();
            timer1.Enabled = false;
            timer2.Enabled = false;
            IDs_now.Sort();
            timer3_render.Enabled = false;


            Dictionary<string, bool> RangeLimitChecker = IfRangeLimitsSame(DataTypesNow);

            for (int i = 0; i < RangeLimitChecker.Count; i++)
            {
                if (RangeLimitChecker.Values.ElementAt(i))
                {
                    RangeLimitData.Add(RangeLimitChecker.Keys.ElementAt(i), GetRangeLimitData(IDs_now, RangeLimitChecker.Keys.ElementAt(i)));
                }

            }



            string[] startEndTime = { "", "" };

            // 실시간 버튼 눌렀을 때
            if (button1_realtime.Image == btnClicked_big)// BackColor != Color.Transparent
            {
                startEndTime[0] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                startEndTime[1] = "RT";

                MyData = G_DataQuery.RealTimeDataQuery(IDs_now, DataTypesNow);
                //List<List<List<string[]>>> realTimeData = G_DataQuery.RealTimeDBQuery(IDs_now, DataTypesNow, Sql_NamesNow);
                IDs_next = new List<int>(IDs_now);
                DataTypesNext = new List<string>(DataTypesNow);

                if (MyData.Tables.Count > 0) //(realTimeData.Count != 0)
                {
                    //ScotPlot(ds, DataTypesNext, IDs_next, true);
                    ScotPlot(MyData, DataTypesNext, IDs_next, true);
                }

            }

            //24시간 버튼 눌렀을 때
            else if (button1_24h.Image == btnClicked_big) //BackColor != Color.Transparent
            {
                startEndTime[0] = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm");
                startEndTime[1] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                progressbarThread = new Thread(new ThreadStart(WaitForm));
                progressbarThread.Start();

                MyData = G_DataQuery.GetValuesFromDB(startEndTime[0], startEndTime[1], DataTypesNow, IDs_now);
                ScotPlot(MyData, DataTypesNow, IDs_now, false);

            }
            // 기간 설정 버튼 눌렀을 때
            else
            {
                if (datePicker1_start.Value < datePicker2_end.Value)
                {
                    startEndTime[0] = datePicker1_start.Value.ToString("yyyy-MM-dd HH:mm");
                    startEndTime[1] = datePicker2_end.Value.ToString("yyyy-MM-dd HH:mm");
                    progressbarThread = new Thread(new ThreadStart(WaitForm));
                    progressbarThread.Start();
                    /*
                                        MyData = G_DataQuery.GetValuesFromDB(startEndTime[0], startEndTime[1], DataTypesNow, IDs_now);

                                        ScotPlot(MyData, DataTypesNow, IDs_now, false);
                    */
                    MyData = G_DataQuery.GetValuesFromDB(startEndTime[0], startEndTime[1], DataTypesNow, IDs_now);
                    ScotPlot(MyData, DataTypesNow, IDs_now, false);
                    //ScotPlot3(DataTypesNow, Sql_NamesNow, IDs_now, startEndTime, false);

                }
                else
                {
                    MessageBox.Show("잘못된 날짜가 선택되었습니다. 확인해 보세요!", "에러 메시지");
                }
            }

        }





        // Entry point for data visualization

        /// <summary>
        /// 실시간 시각화 
        /// </summary>
        /// <param name="MyData"></param>
        /// <param name="MyDataTypes"></param>
        /// <param name="MyIDs"></param>
        /// <param name="RT_flag"></param>
        private void ScotPlot(DataSet MyData, List<string> MyDataTypes, List<int> MyIDs, bool RT_flag)
        {
            panel2_ChartArea.Controls.Clear();
            plt_list.Clear();
            nextDataIndex = 0;
            //newNextDataIndex = new List<List<int>>();
            RTDataArray.Clear();
            RT_textBoxes.Clear();



            //int numOfElmnt = 1;

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            panel2_ChartArea.Controls.Add(tableLayoutPanel);


            if (digital_flag)
            {
                try
                {
                    timer1.Start();

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
                                label.SetBounds(panel.Bounds.Width / 2 - 200, 20, 400, 50);
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
                                    textBox1.SetBounds(panel.Bounds.Width / 2 - 350, 100 + yBound, 700, 70);
                                    textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                                    textBox1.Font = new Font(textBox1.Font.FontFamily, 50);
                                    textBox1.BackColor = this.BackColor;
                                    textBox1.BorderStyle = BorderStyle.None;

                                    for (int k = 1; k < SensorUsageColumn.Count; k++)
                                    {
                                        if (MyDataTypes[boxIndex].Equals(SensorUsageColumn[k]))
                                        {
                                            textBox1.Name = SensorNames[k - 1];
                                            break;
                                        }
                                    }

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
            else
            {

                formsPlots = new List<FormsPlot>();
                plottableAnnotationsMaxVal2.Clear();
                plottableAnnotationsMinVal2.Clear();


                TableLayoutPrep(tableLayoutPanel, MyDataTypes);

                string chartTitle = "";
                DrawRangeLimitChart(formsPlots, MyDataTypes);


                if (!RT_flag)
                {
                    List<List<PlottableSignalXYConst<double, double>>> MyPltList = new List<List<PlottableSignalXYConst<double, double>>>();
                    PlottableSignalXYConst<double, double> plottableXYConst;
                    for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
                    {

                        //plt_list.Add(new List<PlottableSignal>());
                        plottableAnnotationsMaxVal2.Add(new List<PlottableAnnotation>());
                        plottableAnnotationsMinVal2.Add(new List<PlottableAnnotation>());
                        MyPltList.Add(new List<PlottableSignalXYConst<double, double>>());



                        for (int i = 1; i < SensorUsageColumn.Count; i++)
                        {
                            if (MyDataTypes[index_DataType].Contains(SensorUsageColumn[i]))
                            {
                                chartTitle = SensorNames[i - 1]; break;
                            }
                        }


                        List<string> maxValues = new List<string>();
                        List<string> minValues = new List<string>();

                        if (MyData.Tables.Count > 0 && MyData.Tables[index_DataType].Rows.Count > 0)
                        {
                            double[] xs_time;
                            double[] ys_data;
                            for (int i = 0; i < MyIDs.Count; i++)
                            {
                                bool sensor_idExists = MyData.Tables[index_DataType].AsEnumerable().Any(row => MyIDs[i] == row.Field<int>("sensor_id"));
                                if (sensor_idExists)
                                {
                                    List<double> myData = new List<double>();
                                    if (MyDataTypes[index_DataType].Equals(SensorUsageColumn[1]) || MyDataTypes[index_DataType].Equals(SensorUsageColumn[2]))
                                    {
                                        myData = MyData.Tables[index_DataType].AsEnumerable().Where(r => r.Field<int>("sensor_id") == MyIDs[i]).Select(r => Convert.ToDouble(r.Field<int>(MyDataTypes[index_DataType])) / 100d).ToList();
                                    }
                                    else
                                    {
                                        myData = MyData.Tables[index_DataType].AsEnumerable().Where(r => r.Field<int>("sensor_id") == MyIDs[i]).Select(r => Convert.ToDouble(r.Field<int>(MyDataTypes[index_DataType]))).ToList();
                                    }
                                    xs_time = MyData.Tables[index_DataType].AsEnumerable().Where(r => r.Field<int>("sensor_id") == MyIDs[i]).Select(r => Convert.ToDateTime(r.Field<string>("dateandtime")).ToOADate()).ToArray();
                                    ys_data = myData.ToArray();


                                    var (indexOfMax, max) = minMaxIndex(ys_data, true);
                                    var (indexOfMin, min) = minMaxIndex(ys_data, false);

                                    if (MyDataTypes[index_DataType].Equals(SensorUsageColumn[1]) || MyDataTypes[index_DataType].Equals(SensorUsageColumn[2]))
                                    {
                                        maxValues.Add(max.ToString("F", CultureInfo.InvariantCulture));
                                        minValues.Add(min.ToString("F", CultureInfo.InvariantCulture));
                                    }
                                    else
                                    {
                                        maxValues.Add(String.Format("{0:n0}", max));
                                        minValues.Add(String.Format("{0:n0}", min));
                                    }
                                    plottableXYConst = formsPlots[index_DataType].plt.PlotSignalXYConst(xs_time, ys_data, label: Btn3_SensorLocation[MyIDs[i] - 1].Text, color: colorset[i]); //              // Signal Chart
                                    MyPltList[index_DataType].Add(plottableXYConst);
                                }
                                else
                                {
                                    Console.WriteLine("1. Skipped this chart settings because there is no data to show.");

                                }
                            }

                            pltStyler(MyDataTypes, index_DataType, chartTitle);

                            DrawAnnotationBackground(index_DataType, MyPltList[index_DataType]);
                            AnnotateMinMax(index_DataType, maxValues, minValues);
                            formsPlots[index_DataType].plt.Title(chartTitle + $"                           {maxValues[maxValues.Count - 1]}");
                        }
                        else
                        {
                            //continue;
                            /*emptyTableCounter += 1;
                            if (formsPlots.Count == emptyTableCounter)
                            {
                                MessageBox.Show("조회된 데이터가 없습니다.", "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                progressbarThread.Abort();
                                progressbarThread = null;
                                break;
                            }*/

                        }
                    }

                    for (int index_DataType = 0; index_DataType < MyDataTypes.Count; index_DataType++)
                    {
                        formsPlots[index_DataType].Render();
                    }
                    progressbarThread.Abort();
                    progressbarThread = null;
                }
                else
                {
                    RT_Max3.Clear();
                    RT_Min3.Clear();

                    timer1.Stop();
                    timer2.Stop();
                    timer3_render.Stop();
                    AvgData = G_DataQuery.GetAvgData(MyIDs, MyDataTypes);
                    // working on here
                    try
                    {

                        DateTime dtime_min = DateTime.Now;
                        for (int index_chart = 0; index_chart < MyDataTypes.Count; index_chart++)
                        {
                            plt_list.Add(new List<PlottableSignal>());
                            //newNextDataIndex.Add(new List<int>());

                            RTDataArray.Add(new List<List<double[]>>());

                            // new 
                            RT_Max3.Add(new List<List<string>>());
                            RT_Min3.Add(new List<List<string>>());
                            //


                            for (int i = 1; i < SensorUsageColumn.Count; i++)
                            {
                                if (MyDataTypes[index_chart].Contains(SensorUsageColumn[i])) { chartTitle = SensorNames[i - 1]; }
                            }

                            for (int index_ID = 0; index_ID < MyIDs.Count; index_ID++)
                            {
                                //newNextDataIndex[index_chart].Add(0);
                                RTDataArray[index_chart].Add(new List<double[]>());

                                RTDataArray[index_chart][index_ID].Add(new double[100_000]);
                                RTDataArray[index_chart][index_ID].Add(new double[100_000]);

                                // new
                                RT_Max3[index_chart].Add(new List<string>());
                                RT_Min3[index_chart].Add(new List<string>());
                                RT_Max3[index_chart][index_ID].Add("0");
                                RT_Max3[index_chart][index_ID].Add("0");

                                RT_Min3[index_chart][index_ID].Add("0");
                                RT_Min3[index_chart][index_ID].Add("0");
                                //

                                if (MyData.Tables[index_chart].Rows.Count > index_ID)
                                {

                                    ////////////////////////////////////////////////////////////

                                    //nextDataIndex = newNextDataIndex[index_chart][index_ID];
                                    // check data (row) index exists

                                    //dtime_min = DateTime.Parse(MyData.Tables[index_chart].Rows[index_ID].Field<string>("DateAndTime").ToString());

                                    RT_Max3[index_chart][index_ID][1] = MyData.Tables[index_chart].Rows[index_ID].Field<string>("DateAndTime").ToString();
                                    RT_Min3[index_chart][index_ID][1] = MyData.Tables[index_chart].Rows[index_ID].Field<string>("DateAndTime").ToString();


                                    if (MyDataTypes[index_chart].Contains(SensorUsageColumn[1]) || MyDataTypes[index_chart].Contains(SensorUsageColumn[2]))
                                    {
                                        RT_Max3[index_chart][index_ID][0] = (Convert.ToInt64(MyData.Tables[index_chart].Rows[index_ID].Field<string>(MyDataTypes[index_chart])) / 100m).ToString();
                                        RT_Min3[index_chart][index_ID][0] = (Convert.ToInt64(MyData.Tables[index_chart].Rows[index_ID].Field<string>(MyDataTypes[index_chart])) / 100m).ToString();

                                        double dblVal = Convert.ToInt64(MyData.Tables[index_chart].Rows[index_ID].Field<string>(MyDataTypes[index_chart])) / 100.0D;
                                        double dblAvg = AvgData.Tables[index_chart].Rows.Count > index_ID ? Convert.ToInt64(AvgData.Tables[index_chart].Rows[index_ID].Field<int>(MyDataTypes[index_chart])) / 100.0D : double.NaN;

                                        RTDataArray[index_chart][index_ID][0][nextDataIndex] = (dblAvg != 0) && ((dblVal - dblAvg) >= 1 || (dblVal - dblAvg) <= -1) ? dblAvg : dblVal;
                                        RTDataArray[index_chart][index_ID][1][nextDataIndex] = dtime_min.ToOADate();
                                    }
                                    else
                                    {
                                        RT_Max3[index_chart][index_ID][0] = MyData.Tables[index_chart].Rows[index_ID].Field<string>(MyDataTypes[index_chart]).ToString();
                                        RT_Min3[index_chart][index_ID][0] = MyData.Tables[index_chart].Rows[index_ID].Field<string>(MyDataTypes[index_chart]).ToString();

                                        Int64 intVal = Convert.ToInt64(MyData.Tables[index_chart].Rows[index_ID].Field<string>(MyDataTypes[index_chart]));
                                        double intAvg = AvgData.Tables[index_chart].Rows.Count > index_ID ? Convert.ToInt64(AvgData.Tables[index_chart].Rows[index_ID].Field<int>(MyDataTypes[index_chart])) : double.NaN;


                                        RTDataArray[index_chart][index_ID][0][nextDataIndex] = (intAvg != 0) && (intAvg * 2 <= intVal || intVal <= intAvg / 2) ? intAvg : intVal;
                                        RTDataArray[index_chart][index_ID][1][nextDataIndex] = dtime_min.ToOADate();
                                    }
                                    //nextDataIndex += -1;

                                }
                                else
                                {
                                    if (nextDataIndex > 0)
                                    {
                                        RTDataArray[index_chart][index_ID][0][nextDataIndex] = RTDataArray[index_chart][index_ID][0][nextDataIndex - 1];
                                        RTDataArray[index_chart][index_ID][1][nextDataIndex] = RTDataArray[index_chart][index_ID][1][nextDataIndex - 1];
                                    }

                                    Console.WriteLine("1. Old data was re-used because there is no data to show.");
                                }


                                double xs = dtime_min.ToOADate();

                                double samplesPerDay = TimeSpan.TicksPerDay / (TimeSpan.TicksPerMinute);

                                //MyIDs[index_ID] - 51
                                signalPlot = formsPlots[index_chart].plt.PlotSignal(RTDataArray[index_chart][index_ID][0], samplesPerDay, xs, label: Btn3_SensorLocation[index_ID].Text, color: colorset[index_ID]);


                                plt_list[index_chart].Add(signalPlot);
                                plt_list[index_chart][index_ID].maxRenderIndex = nextDataIndex;
                            }

                            pltStyler(MyDataTypes, index_chart, chartTitle);

                        }

                        AnnotationBackground(plt_list, MyDataTypes, MyIDs);


                        PlottableAnnotation pltAnnot;
                        PlottableAnnotation pltAnnot_min;
                        string numberStrMax = " ";
                        string numberStrMin = " ";


                        // Plot Annotations separately to put them above the charts.
                        for (int index_chart = 0; index_chart < MyDataTypes.Count; index_chart++)
                        {
                            plottableAnnotationsMaxVal2.Add(new List<PlottableAnnotation>());
                            plottableAnnotationsMinVal2.Add(new List<PlottableAnnotation>());
                            int annotY = -10 - 23 * (plt_list[index_chart].Count - 1);
                            for (int i = 0; i < MyIDs.Count; i++)
                            {
                                string maxLabel = " ";
                                string minLabel = " ";

                                pltAnnot = formsPlots[index_chart].plt.PlotAnnotation(label: maxLabel + char.ConvertFromUtf32(0x2191), -10, annotY, fontSize: 12, fontColor: colorset[i], fillAlpha: 0, lineWidth: 0, fillColor: Color.White);
                                pltAnnot_min = formsPlots[index_chart].plt.PlotAnnotation(label: minLabel + char.ConvertFromUtf32(0x2193), -75, annotY, fontSize: 12, fontColor: colorset[i], fillAlpha: 0, lineWidth: 0, fillColor: Color.White);

                                if (MyData.Tables[index_chart].Rows.Count > i)
                                {

                                    numberStrMax = RT_Max3[index_chart][i][0];
                                    numberStrMin = RT_Min3[index_chart][i][0];


                                    maxLabel = (numberStrMax.Contains(".") == false && numberStrMax.Length > 3) ? numberStrMax.Insert(numberStrMax.Length - 3, ",") : numberStrMax;
                                    minLabel = (numberStrMin.Contains(".") == false && numberStrMin.Length > 3) ? numberStrMin.Insert(numberStrMin.Length - 3, ",") : numberStrMin;


                                    pltAnnot.label = maxLabel + " " + char.ConvertFromUtf32(0x2191);
                                    pltAnnot_min.label = minLabel + " " + char.ConvertFromUtf32(0x2193);

                                }
                                else
                                {
                                    Console.WriteLine("2. Skipped this annotation settings because there is no data to show.");
                                }
                                annotY += 23;
                                plottableAnnotationsMaxVal2[index_chart].Add(pltAnnot);
                                plottableAnnotationsMinVal2[index_chart].Add(pltAnnot_min);

                            }
                            formsPlots[index_chart].Render();
                        }
                        nextDataIndex += 1;

                        timer2.Start();
                        timer3_render.Start();


                        //timer_RT(MyDataTypes, MyIDs);




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


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            //List<List<List<string[]>>> DataRetrieved_RT = new List<List<List<string[]>>>();
            //DataRetrieved_RT = G_DataQuery.RealTimeDBQuery(IDs_next, DataTypesNext);
            DataSet MyData = G_DataQuery.RealTimeDataQuery(IDs_next, DataTypesNext);
            if (MyData.Tables.Count == 0)
            {
                timer1.Stop();
                timer2.Stop();
                timer3_render.Stop();
            }
            else
            {
                timer2.Stop();
                timer3_render.Stop();
                string printedData = "_";
                try
                {
                    for (int index_ID = 0; index_ID < RT_textBoxes.Count; index_ID++)
                    {
                        for (int index_chart = 0; index_chart < RT_textBoxes[index_ID].Count; index_chart++) //whatToShow2
                        {
                            for (int k = 0; k < SensorUsageColumn.Count; k++)
                            {
                                if (DataTypesNext[index_chart].Equals(SensorUsageColumn[k]))
                                {
                                    printedData = "_";
                                    if (MyData.Tables[index_chart].Rows.Count == RT_textBoxes.Count)
                                    {

                                        if (DataTypesNext[index_chart].Equals(SensorUsageColumn[1]) || DataTypesNext[index_chart].Equals(SensorUsageColumn[2]))
                                        {
                                            printedData = (Convert.ToDouble(MyData.Tables[index_chart].Rows[index_ID].Field<string>(DataTypesNext[index_chart])) / 100d).ToString();
                                        }
                                        else
                                        {
                                            printedData = String.Concat(MyData.Tables[index_chart].Rows[index_ID].Field<string>(DataTypesNext[index_chart]).Where(c => !Char.IsWhiteSpace(c)));
                                        }
                                    }
                                    else { Console.WriteLine("Skipped number chart data settings = No Data"); }
                                    RT_textBoxes[index_ID][index_chart].Text = printedData + " " + RT_textBoxes[index_ID][index_chart].Name;

                                }
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
            DataSet MyData = G_DataQuery.RealTimeDataQuery(IDs_next, DataTypesNext);
            DataSet averageData = AvgData.Copy();
            if (nextDataIndex <= 1)
            {
                UpdAvgDataThread = new Thread(UpdAvgData);
                UpdAvgDataThread.Start();
            }


            if (MyData.Tables.Count == 0)//(DataRetrieved_RT.Count == 0)
            {
            }
            else
            {
                string chartTitle = "";
                double dblVal = double.NaN;
                double avgVal = double.NaN;
                double oldVal = double.NaN;
                double finalVal = double.NaN;
                string now = DateTime.Now.ToString("HH:mm:ss");
                DateTime resetTime = Convert.ToDateTime(now);
                try
                {
                    if (resetTime > Convert.ToDateTime("23:59:00") && resetTime <= Convert.ToDateTime("23:59:59") || nextDataIndex + 1 == 100_000)
                    {
                        Console.WriteLine("Timer Reset Successful. Charts reset.");
                        nextDataIndex = 0;
                        timer2.Stop();
                        timer3_render.Stop();
                        ScotPlot(MyData, DataTypesNext, IDs_next, true);
                    }
                    for (int index_chart = 0; index_chart < DataTypesNext.Count; index_chart++)
                    {
                        for (int i = 1; i < SensorUsageColumn.Count; i++)
                        {
                            if (DataTypesNext[index_chart].Contains(SensorUsageColumn[i])) { chartTitle = SensorNames[i - 1]; break; }
                        }

                        for (int index_ID = 0; index_ID < IDs_next.Count; index_ID++)
                        {
                            if (MyData.Tables[index_chart].Rows.Count > index_ID)
                            {

                                if (DataTypesNext[index_chart].Contains(SensorUsageColumn[1]) || DataTypesNext[index_chart].Contains(SensorUsageColumn[2]))
                                {
                                    oldVal = RTDataArray[index_chart][index_ID][0][nextDataIndex - 1];
                                    dblVal = Convert.ToDouble(MyData.Tables[index_chart].Rows[index_ID].Field<string>(DataTypesNext[index_chart])) / 100d;
                                    avgVal = averageData.Tables[index_chart].Rows.Count > index_ID ? (Convert.ToDouble(averageData.Tables[index_chart].Rows[index_ID].Field<int>(DataTypesNext[index_chart])) / 100d) : oldVal;

                                    finalVal = (dblVal >= (avgVal + 1.0) || dblVal <= (avgVal - 1.0)) ? avgVal : dblVal;
                                    RTDataArray[index_chart][index_ID][0][nextDataIndex] = finalVal;

                                }
                                else
                                {
                                    oldVal = RTDataArray[index_chart][index_ID][0][nextDataIndex - 1];
                                    dblVal = Convert.ToDouble(MyData.Tables[index_chart].Rows[index_ID].Field<string>(DataTypesNext[index_chart]));
                                    avgVal = averageData.Tables[index_chart].Rows.Count > index_ID ? Convert.ToDouble(averageData.Tables[index_chart].Rows[index_ID].Field<int>(DataTypesNext[index_chart])) : oldVal;
                                    finalVal = (dblVal >= avgVal * 2.0 || dblVal <= avgVal / 2.0) ? avgVal : dblVal;
                                    RTDataArray[index_chart][index_ID][0][nextDataIndex] = finalVal;

                                }

                                DateTime dtime = DateTime.Parse(MyData.Tables[index_chart].Rows[index_ID].Field<string>("DateAndTime").ToString());
                                RTDataArray[index_chart][index_ID][1][nextDataIndex] = dtime.ToOADate();


                                if (finalVal > Convert.ToDouble(RT_Max3[index_chart][index_ID][0]))
                                {
                                    RT_Max3[index_chart][index_ID][0] = finalVal.ToString();
                                    RT_Max3[index_chart][index_ID][1] = MyData.Tables[index_chart].Rows[index_ID].Field<string>("DateAndTime");
                                }

                                if (finalVal < Convert.ToDouble(RT_Min3[index_chart][index_ID][0]))
                                {
                                    RT_Min3[index_chart][index_ID][0] = finalVal.ToString();
                                    RT_Min3[index_chart][index_ID][1] = MyData.Tables[index_chart].Rows[index_ID].Field<string>("DateAndTime");
                                }
                                string currentVal = finalVal.ToString();
                                string displayCurrVal = ""; //

                                if (DataTypesNext[index_chart].Contains(SensorUsageColumn[1]) || DataTypesNext[index_chart].Contains(SensorUsageColumn[2]))
                                {
                                    displayCurrVal = currentVal.Length > 2 && !currentVal.Contains(".") ? currentVal.Insert(2, ".") : currentVal;
                                }
                                else
                                {
                                    displayCurrVal = (currentVal.Length > 3) ? currentVal.Insert(currentVal.Length - 3, ",") : currentVal;
                                }

                                formsPlots[index_chart].plt.Title(chartTitle + $"                           {displayCurrVal}", fontSize: 24);
                                Console.WriteLine($"2.  chartID:{index_chart}, sensor:{index_ID}: currVal: {displayCurrVal}");

                                string numberStrMax = RT_Max3[index_chart][index_ID][0];
                                string maxLabel = (numberStrMax.Contains(".") == false && numberStrMax.Length > 3) ? numberStrMax.Insert(numberStrMax.Length - 3, ",") : numberStrMax;
                                plottableAnnotationsMaxVal2[index_chart][index_ID].label = maxLabel + " " + char.ConvertFromUtf32(0x2191);

                                string numberStrMin = RT_Min3[index_chart][index_ID][0];
                                string minLabel = (numberStrMin.Contains(".") == false && numberStrMin.Length > 3) ? numberStrMin.Insert(numberStrMin.Length - 3, ",") : numberStrMin;

                                plottableAnnotationsMinVal2[index_chart][index_ID].label = minLabel + " " + char.ConvertFromUtf32(0x2193);
                            }
                            else
                            {

                                if (nextDataIndex > 0)
                                {
                                    RTDataArray[index_chart][index_ID][0][nextDataIndex] = RTDataArray[index_chart][index_ID][0][nextDataIndex - 1];
                                    RTDataArray[index_chart][index_ID][1][nextDataIndex] = RTDataArray[index_chart][index_ID][1][nextDataIndex - 1];
                                }
                                else
                                {

                                    RTDataArray[index_chart][index_ID][0][nextDataIndex] = double.NaN;
                                    RTDataArray[index_chart][index_ID][1][nextDataIndex] = double.NaN;

                                }

                            }
                            /////////////////////////////////////////////////

                        }
                        for (int pltIndex = 0; pltIndex < plt_list[index_chart].Count; pltIndex++)
                        {
                            plt_list[index_chart][pltIndex].maxRenderIndex = nextDataIndex;
                        }
                    }
                    for (int formPltIndex = 0; formPltIndex < formsPlots.Count; formPltIndex++)
                    {
                        formsPlots[formPltIndex].plt.AxisAuto();
                        formsPlots[formPltIndex].Render();
                    }


                }
                catch (Exception ex)
                {
                    throw new Exception("Error: " + ex.Message + "\n" + ex.StackTrace);
                }

                //////////////////////////////////////////////////////////
                nextDataIndex += 1;
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
                for (int formPltIndex = 0; formPltIndex < formsPlots.Count; formPltIndex++)
                {
                    formsPlots[formPltIndex].plt.AxisAuto();
                    formsPlots[formPltIndex].Render();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.StackTrace);
            }
        }



        private void UpdAvgData()
        {
            while (true)
            {
                AvgData = G_DataQuery.GetAvgData(IDs_next, DataTypesNext);
                Thread.Sleep(1000);
            }
        }





        private List<long> GetRangeLimitData(List<int> sensor_Id, string tableName)
        {
            List<long> response = new List<long>();
            string ifRangesSameStr = $"SELECT {S_FourRangeColmn[0]}, {S_FourRangeColmn[1]}, {S_FourRangeColmn[2]}, {S_FourRangeColmn[3]}, COUNT(*) occurrences " +
                                        $"FROM {tableName} WHERE ";
            for (int j = 0; j < sensor_Id.Count; j++)
            {
                ifRangesSameStr += $" {S_DeviceTableColumn[0]} = {sensor_Id[j]} ";
                if (j != IDs_now.Count - 1)
                {
                    ifRangesSameStr += $" or ";
                }
            }
            ifRangesSameStr += $" GROUP BY {S_FourRangeColmn[0]}, {S_FourRangeColmn[1]}, {S_FourRangeColmn[2]}, {S_FourRangeColmn[3]} " +
                                    $"HAVING COUNT(*) = {sensor_Id.Count}; ";


            SqlCommand cmd = new SqlCommand(ifRangesSameStr, myConn);
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
                        if (r.FieldCount == S_FourRangeColmn.Count + 1)
                        {
                            //bool res = Convert.ToInt32(r.GetValue(r.FieldCount - 1)) == IDs_now.Count;
                            for (int k = 0; k < r.FieldCount - 1; k++)
                            {
                                if (tableName.Equals(SensorUsageColumn[1]) || tableName.Equals(SensorUsageColumn[2]))
                                {
                                    response.Add(Convert.ToInt64(Convert.ToDecimal(r.GetValue(k)) * 100));
                                }
                                else
                                {
                                    response.Add(Convert.ToInt64(r.GetValue(k)));
                                }
                            }

                            break;
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
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return response;
        }




        private Dictionary<string, bool> IfRangeLimitsSame(List<string> dataTypesNow)
        {
            Dictionary<string, bool> response = new Dictionary<string, bool>();
            List<int> RangeLimitSame = new List<int>();
            for (int i = 0; i < dataTypesNow.Count; i++)
            {
                string ifRangesSameStr = $"SELECT {S_FourRangeColmn[0]}, {S_FourRangeColmn[1]}, {S_FourRangeColmn[2]}, {S_FourRangeColmn[3]}, COUNT(*) occurrences " +
                                        $"FROM {dataTypesNow[i]} WHERE ";
                for (int j = 0; j < IDs_now.Count; j++)
                {
                    ifRangesSameStr += $" {S_DeviceTableColumn[0]} = {IDs_now[j]} ";
                    if (j != IDs_now.Count - 1)
                    {
                        ifRangesSameStr += $" or ";
                    }
                }
                ifRangesSameStr += $" GROUP BY {S_FourRangeColmn[0]}, {S_FourRangeColmn[1]}, {S_FourRangeColmn[2]}, {S_FourRangeColmn[3]} " +
                                        $"HAVING COUNT(*) = {IDs_now.Count}; ";

                SqlCommand cmd = new SqlCommand(ifRangesSameStr, myConn);
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
                            if (r.FieldCount == S_FourRangeColmn.Count + 1)
                            {
                                bool res = Convert.ToInt32(r.GetValue(r.FieldCount - 1)) == IDs_now.Count;
                                response.Add(dataTypesNow[i], res);
                                break;
                            }
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }

            }
            return response;
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


        private void DisableButtons(Button[] btns)
        {
            for (int i = 0; i < btns.Length; i++)
            {
                btns[i].Enabled = false;
            }

        }





        /// <summary>
        /// real time data visualization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Button thisBtn = (Button)sender;
            thisBtn.Image = thisBtn.Image == btnClicked_big ? btnUnClicked_big : btnClicked_big;

            if (thisBtn.Image == btnClicked_big)
            {
                if (thisBtn == button1_realtime)
                {
                    button1_24h.Image = btnUnClicked_big;
                    button1_datepicker.Image = btnUnClicked_big;

                    button11_numRT.Visible = true;
                    button11_chartRT.Visible = true;
                    datePicker1_start.Visible = false;
                    datePicker2_end.Visible = false;
                    label_between.Visible = false;
                }
                else if (thisBtn == button1_24h)
                {
                    button1_realtime.Image = btnUnClicked_big;
                    button1_datepicker.Image = btnUnClicked_big;

                    button11_numRT.Visible = false;
                    button11_chartRT.Visible = false;

                    datePicker1_start.Visible = false;
                    datePicker2_end.Visible = false;
                    label_between.Visible = false;
                    datePicker1_start.Value = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
                    datePicker2_end.Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

                }
                else if (thisBtn == button1_datepicker)
                {
                    button1_realtime.Image = btnUnClicked_big;
                    button1_24h.Image = btnUnClicked_big;

                    button11_numRT.Visible = false;
                    button11_chartRT.Visible = false;

                    label_between.Visible = true;
                    datePicker1_start.Visible = true;
                    datePicker2_end.Visible = true;

                    datePicker1_start.Value = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
                    datePicker2_end.Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));


                }
            }
            else
            {

                button11_numRT.Visible = false;
                button11_chartRT.Visible = false;
            }

            clearHighlighting(new Button[] { button11_chartRT, button11_numRT }, "small");
            HandleBtn2nBtn3(thisBtn);
            button_show.Visible = false;
            DataTypesNow.Clear();
            IDs_now.Clear();

        }


        private void button11_Chart_Click(object sender, EventArgs e)
        {
            Button thisBtn = (Button)sender;

            thisBtn.Image = thisBtn.Image == btnClicked_small ? btnUnClicked_small : btnClicked_small;

            if (thisBtn.Image == btnClicked_small)
            {
                if (thisBtn == button11_chartRT)
                {
                    button11_numRT.Image = btnUnClicked_small; // BackColor = Color.Transparent;
                    digital_flag = false;
                }
                else
                {
                    digital_flag = true;
                    button11_chartRT.Image = btnUnClicked_small;

                }
            }
            else
            {
            }

            HandleBtn2nBtn3(new Button());
            button_show.Visible = false;
            DataTypesNow.Clear();
            IDs_now.Clear();
        }


        private void btn2_data_Click(object sender, EventArgs e)
        {


            Button button = (Button)sender; // receive clicked button properties
            button.Image = button.Image == btnUnClicked_small ? btnClicked_small : btnUnClicked_small;

            if (button.Image == btnUnClicked_small) //BackColor == Color.Transparent
            {
                // unClicking the button 
                if (DataTypesNow.Count > 0)
                {
                    DataTypesNow.Remove(button.Name);
                }

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
                if (DataTypesNow.Count != 4)
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



            //check if other buttons are clickable
            else
            {

                if (allIDs.Count == 0)
                {
                    string GetAllIdSqlStr = $"SELECT {S_DeviceTableColumn[0]} FROM {S_DeviceTable} ORDER BY {S_DeviceTableColumn[0]};";
                    allIDs = GetColumnDataAsList("int", GetAllIdSqlStr, S_DeviceTableColumn[0]).Select(x => Convert.ToInt32(x)).ToList();
                }

                //4개 이상의 센서를 선택 못하게 If문으로 확인함.

                if (DataTypesNow.Count <= 4)
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

                    if (Btn3_SensorLocation == null || Btn3_SensorLocation.Length == 0)
                    {


                        Btn3_SensorLocation = CreateButtonsForSensorIds(allIDs, btn3_address_Click, Btn2_DataType, false);


                        button_show.Image = btnUnClicked_big;
                        button_show.SetBounds(panel1_menu.Bounds.Width / 2 - button1_realtime.Bounds.Width / 2,
                                              Btn3_SensorLocation[Btn3_SensorLocation.Length - 1].Bounds.Y + Btn3_SensorLocation[Btn3_SensorLocation.Length - 1].Bounds.Height * 3 / 2,
                                              button1_realtime.Bounds.Width,
                                              button1_realtime.Bounds.Height
                                              );
                        listView1.SetBounds(panel1_menu.Bounds.X,
                                    button_show.Bounds.Y + 200,
                                    panel1_menu.Bounds.Width - 15,
                                    panel1_menu.Bounds.Height - button_show.Bounds.Y + 200);

                    }


                    if (Btn3_SensorLocation.Length != 0 && Btn3_SensorLocation[0].Visible == false)
                    {

                        foreach (var btn in Btn3_SensorLocation)
                        {
                            Console.WriteLine($"btn.Visible: {btn.Visible}");
                            btn.Visible = true;
                        }
                    }

                }
                else
                {
                    button.Image = btnUnClicked_small;
                }
                //Show Read-only and Clickable buttons

            }

            button_show.Visible = false;

            ShowClickableSensorDeviceButtons(DataTypesNow, SensorUsage, Btn3_SensorLocation);
            clearHighlighting(Btn3_SensorLocation, "small");

            // Unclick the Sensor Device buttons
            IDs_now.Clear(); // 선텍되어 있는 센서 장비 버튼 리스트 지우기 


        }



        /// <summary>
        /// 센서장비를 카리기는 버튼 생성
        /// </summary>
        private Button[] CreateButtonsForSensorIds(List<int> btn_addresses, EventHandler btn_address_Click, Button[] btn2, bool flag_p)
        {

            int x_btn = btn2[0].Left;
            int y_btn = btn2[btn2.Length - 1].Bounds.Y + btn2[btn2.Length - 1].Height * 3 / 2;

            Button[] btns = new Button[btn_addresses.Count]; // Btn3_SensorLocation
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
                                    btn2[0].Bounds.Width,
                                    btn2[0].Bounds.Height);
                if (button1_datepicker.Bounds.X <= x_btn)
                {
                    x_btn = btn2[0].Bounds.X;
                    y_btn += btn2[0].Bounds.Y - (button1_24h.Bounds.Y + button1_24h.Bounds.Height);
                }
                else
                {
                    x_btn += btn2[1].Bounds.X - btn2[0].Bounds.X;
                }



                string sqlStr = $"SELECT {S_DeviceTableColumn[2]}, {S_DeviceTableColumn[3]} FROM {S_DeviceTable} WHERE {S_DeviceTableColumn[0]} = {btn_addresses[index_btn3]}";

                button1.Text = GetButtonText(sqlStr, btn_addresses[index_btn3]); //DeviceZoneLocInfo.ElementAt(index_btn3).Key + $"({devZoneLoc.Value[0]})" ;
                button1.Font = new Font(button1.Font.FontFamily, 8);
                button1.Name = btn_addresses[index_btn3].ToString();
                //button1.FlatStyle = FlatStyle.Flat;
                button1.Click += new EventHandler(btn_address_Click);
                panel1_menu.Controls.Add(button1);
                button1.Visible = false;
                btns[index_btn3] = button1;

            }


            button_show.Text = "확인";
            button_show.Font = new Font(button_show.Font.FontFamily, 15);
            button_show.Click += new EventHandler(this.button_show_Click);
            panel1_menu.Controls.Add(button_show);
            button_show.Visible = false;

            return btns;
        }



        /// <summary>
        /// 사용여부에 맞게 버튼을 선텍할수 있게 해주는 함수
        /// </summary>
        /// <param name="dataTypesNow"></param>
        /// <param name="sensorUsageTable"></param>
        /// <param name="btn3"></param>
        private void ShowClickableSensorDeviceButtons(List<string> dataTypesNow, string sensorUsageTable, Button[] btn3)
        {
            if (DataTypesNow.Count >= 1)
            {
                string ClickablesBtnsSqlStr = $"SELECT {S_DeviceTableColumn[0]} FROM {sensorUsageTable} WHERE ";
                for (int i = 0; i < DataTypesNow.Count; i++)
                {
                    ClickablesBtnsSqlStr += $" {DataTypesNow[i]} = 'YES' ";
                    if (DataTypesNow.Count > 1 && DataTypesNow.Count - 1 != i)
                    {
                        ClickablesBtnsSqlStr += $" OR ";
                    }
                }
                Console.Write(ClickablesBtnsSqlStr);
                List<int> clickableIDs = GetColumnDataAsList("int", ClickablesBtnsSqlStr, S_DeviceTableColumn[0]).Select(x => Convert.ToInt32(x)).ToList();

                DisableButtons(btn3);
                for (int j = 0; j < clickableIDs.Count; j++)
                {
                    btn3[j].Enabled = true;
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
                if (myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        for (int i = 0; i < r.FieldCount; i++)
                        {
                            if (i == 0 && r.FieldCount > 1)
                            {
                                btnTxt += r.GetValue(i).ToString() + " (";
                            }
                            else if (i != 0 && i == r.FieldCount - 1)
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
                if (myConn.State == ConnectionState.Open)
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



        /// <summary>
        /// 상한 및 하한 범위 차트 시각화해 주는 함수
        /// </summary>
        /// <param name="formsPlots">차트 리스트</param>
        /// <param name="sensorNames">센서 리스트</param>
        private void DrawRangeLimitChart(List<FormsPlot> formsPlots, List<string> sensorNames)
        {
            var dict = RangeLimitData.Values.AsEnumerable().ToList();
            var targetSensorNames = RangeLimitData.Keys.AsEnumerable().ToList();
            int k = 0;
            for (int i = 0; i < formsPlots.Count; i++)
            {

                if (targetSensorNames.Contains(formsPlots[i].Name))
                {

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

                    for (int j = 0; j < dict[k].Count; j++)
                    {
                        if (targetSensorNames[k].Equals(SensorUsageColumn[1]) || targetSensorNames[k].Equals(SensorUsageColumn[2]))
                        {
                            formsPlots[i].plt.PlotHLine(y: Convert.ToDouble(dict[k][j]) / 100d, lineStyle: LineStyle.Dash, lineWidth: 2);
                        }
                        else
                        {
                            formsPlots[i].plt.PlotHLine(y: Convert.ToDouble(dict[k][j]), lineStyle: LineStyle.Dash, lineWidth: 2);
                        }
                        formsPlots[i].Render();
                    }
                    k += 1;
                }
            }
            //formsPlot.plt.PlotSignal(RTDataArray[index_chart][index_ID][0], samplesPerDay, xs, label: Btn3_SensorLocation[index_ID].Text, color: colorset[index_ID]);
            //throw new NotImplementedException();
        }

        /*
                private void button_particleORpressure_Click(object sender, EventArgs e)
                {
                    Button thisBtn = (Button)sender;

                    if (thisBtn == button_particle)
                    {
                        button_pressure.Image = btnUnClicked_small;
                        thisBtn.Image = thisBtn.Image == btnClicked_small ? btnUnClicked_small : btnClicked_small;
                        clearHighlighting(Btn2_DataType_p, "small");
                        clearHighlighting(Btn3_SensorLocation_p, "small");

                    }
                    else // if(thisBtn == button_particle)
                    {
                        button_particle.Image = btnUnClicked_small;
                        thisBtn.Image = thisBtn.Image == btnClicked_small ? btnUnClicked_small : btnClicked_small;
                        clearHighlighting(Btn2_DataType, "small");
                        clearHighlighting(Btn3_SensorLocation, "small");
                    }

                    if (thisBtn.Image == btnClicked_small)
                        HandleBtn2nBtn3(thisBtn);
                    else
                        HandleBtn2nBtn3(new Button());

                    button_show.Visible = false;
                    DataTypesNow.Clear();
                    IDs_now.Clear();

                }
        */




        private void HandleBtn2nBtn3(Button thisBtn)
        {

            if (Btn2_DataType != null)
            {
                if (thisBtn != button1_realtime)
                {
                    foreach (var btn in Btn2_DataType)
                    {
                        btn.Visible = true;
                        btn.Image = btnUnClicked_small;
                        btn.Enabled = true;
                    }
                }
                else
                {
                    foreach (var btn in Btn2_DataType)
                    {
                        btn.Visible = false;
                        btn.Image = btnUnClicked_small;
                        btn.Enabled = true;
                    }
                }


                if (Btn3_SensorLocation != null)
                {
                    foreach (var btn in Btn3_SensorLocation)
                    {
                        btn.Visible = false;
                        btn.Image = btnUnClicked_small;
                        btn.Enabled = true;
                    }
                }
            }

        }

        public class AppInfo
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            private static extern int GetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);
            private static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);
            public static string StartupPath
            {
                get
                {
                    StringBuilder stringBuilder = new StringBuilder(260);
                    GetModuleFileName(NullHandleRef, stringBuilder, stringBuilder.Capacity);
                    return Path.GetDirectoryName(stringBuilder.ToString());
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Application.Exit();
            System.Windows.Forms.Application.ExitThread(); // .Exit();
                
        }
    }
}
