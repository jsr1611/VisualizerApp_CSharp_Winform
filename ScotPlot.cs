using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualizerApp_3
{
    public partial class ScotPlot : Form
    {

        //public ScotPlot() { }
        List<FormsPlot> formsPlots = new List<FormsPlot>();
        public string titleName = "";
        public double[] dataVals = new double[100_000];
        public double[] timeVals = new double[100_000];
        int nextDataIndex = 1;
        // = new PlottableSignalXYConst>();
        //public ScotPlot(List<List<List<string[]>>> dataArr, string whatToShow, List<int> IDs) 
        public ScotPlot()
        {
            InitializeComponent();

            List<int> IDs = new List<int>() { 1, 2 };
            string whatToShow = "temp";
            List<List<string[]>> dataArr = RealTimeDBQuery(IDs, whatToShow);
            //textBox1.Text = data[0][0][0].ToString() + data[0][0][1].ToString();

            int numOfElmnt = CountNumOfElmnt(dataArr, IDs, whatToShow); //데이터 개수 : number of Elements(temp)
            string[] dataVal = new string[numOfElmnt];
            string[] timeVal = new string[numOfElmnt];

            this.SetBounds(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height - 100);

            /*this.Controls.Add(panel1);
             this.Controls.Add(panel2);*/
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            panel2.Controls.Add(tableLayoutPanel);
            tableLayoutPanel.Dock = DockStyle.Fill;



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
                    dataArr2[i][j] = dataArr[i][j][0];
                    timeArr2[i][j] = dataArr[i][j][1];
                    //dataVal[j] = dataArr[0][0][j][0];
                }

                timeVal[0] = dataArr[0][0][1];
                //if(whatToShow.Contains("temp") || whatToShow.Contains("humid")) { }
                Console.WriteLine("num of data points: {0}", numOfElmnt);
                double[] ys = dataArr2[i].Select(x => double.Parse(x)).ToArray();
                DateTime[] timeData = timeArr2[i].Select(x => DateTime.Parse(x)).ToArray();
                double[] xs = timeData.Select(x => x.ToOADate()).ToArray();
                //DateTime dateTimes = DateTime.Parse(timeVal[0]);
                //formsPlots[i].plt.PlotScatter(xs, ys);  //formsPlots[i].plt.PlotSignal(ys, sampleRate: numOfElmnt, xOffset: xs[0]);                        
                var signalPlot = formsPlots[i].plt.PlotSignalXYConst(timeVals, dataVals, lineStyle: LineStyle.Dot);                                    // Signal Chart
                formsPlots[i].plt.Ticks(dateTimeX: true); //formsPlot1.

                //sig.minRenderIndex = 4000;
                //sig.maxRenderIndex = 5000;
                /*sig.fillType = FillType.FillBelow;
                sig.fillColor1 = Color.Blue;
                sig.gradientFillColor1 = Color.Transparent; */

                 if (whatToShow.Contains("temp")) { titleName = "온도(°C)"; }
                else if (whatToShow.Contains("humid")) { titleName = "습도(%)"; }
                else if (whatToShow.Contains("part03")) { titleName = "파티클(0.3μm)"; }
                else { titleName = "파티클(0.5μm)"; }

                formsPlots[i].plt.Title(titleName + " 센서 데이터 시각화"); // formsPlot1.
                formsPlots[i].plt.YLabel(titleName); // formsPlot1.
                formsPlots[i].plt.XLabel("시간");
                //formsPlots[i].plt.SaveFig(titleName +"_" + i.ToString() + "_" + DateTime.Now.ToString("MMdd_HHmm") + ".png");


            }
            datatimer1.Enabled = true;
            datatimer1.Start();

        }

       
        public int CountNumOfElmnt(List<List<string[]>> datalist, List<int> IDs, string flag)
        {
            if (flag.Contains("all"))
            {
                int cntr_tmp = 0; //임시 카운터변수
                int numOfElmnt = datalist[0].Count; // 데이터 개수 : number of Elements(temp, humid, part03, part05)
                                                    //시각화되는 데이터 개수를 동일하게 하기위해 최소 데이터 개수(Min num of elmnts) 계산하기
                for (int ind = 0; ind < datalist.Count; ind++)
                {
                    for (int ind2 = 0; ind2 < IDs.Count; ind2++)
                    {
                        if (numOfElmnt > datalist[ind][ind2].Length && datalist[ind][ind2].Length != 0)
                        {
                            numOfElmnt = datalist[ind][ind2].Length;
                        }
                        //Console.WriteLine("\n\n\nCOUNTERlength: "+graphDataAll[ind][ind2].Count.ToString());
                        cntr_tmp += 1;
                    }
                }
                return numOfElmnt;
            }
            else
            {
                int numOfElmnt = datalist[0].Count; // 데이터 개수 : number of Elements(temp, humid, part03, part05)
                //시각화되는 데이터 개수를 동일하게 하기위해 최소 데이터 개수(Min num of elmnts) 계산하기
                for (int ind = 0; ind < IDs.Count; ind++)
                {
                    if (numOfElmnt > datalist[ind].Count && datalist[ind].Count != 0)
                    {
                        numOfElmnt = datalist[ind].Count;
                    }
                    //Console.WriteLine("\n\n\nCOUNTERlength: "+graphDataAll[ind][ind2].Count.ToString());
                }
                return numOfElmnt;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            List<int> IDs_selected = new List<int>() { 1, 2 };
            string whatToQuery = "temp";
            List<List<string[]>> data = RealTimeDBQuery(IDs_selected, whatToQuery);
            

            List<string[]> data2 = new List<string[]>();
            List<string[]> time2 = new List<string[]>();
            
            for (var i = 0; i < data.Count; i++)
            {
                for(int j=0; j < data[i].Count; j++)
                {
                    data2.Add(new string[] { data[i][j][0] });
                    time2.Add(new string[] { data[i][j][1] });

                }
                
            }

            Console.WriteLine(data2.Count);
            Console.WriteLine(time2.Count);



            textBox1.Text = data[0][0][0].ToString() + data[0][0][1].ToString();
            //DateTime[] timeData = timeArr2[i].Select(x => DateTime.Parse(x)).ToArray();
            //double[] xs = timeData.Select(x => x.ToOADate()).ToArray();

            for (int i = 0; i < IDs_selected.Count; i++)
            {
                for(int j=0; j<data[i].Count; j++)
                {
                    double latestVal = double.Parse(data2[i][j]);
                    DateTime timeVal = DateTime.Parse(time2[i][j]);
                    double timeVal1 = timeVal.ToOADate();
                    timeVals[nextDataIndex] = timeVal1;
                    dataVals[nextDataIndex] = latestVal;
                    nextDataIndex += 1;
                }
                //formsPlots[i].plt.PlotSignalXYConst(timeVals, dataVals);
                
                //double[] ys = data2[i].Select(x => double.Parse(x)).ToArray();
                //double[] xs = time2[i].Select(x => x.ToOADate()).ToArray();
                formsPlots[i].plt.AxisAuto();
                formsPlots[i].Render();
            }
        }

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
            return DataArr;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}






