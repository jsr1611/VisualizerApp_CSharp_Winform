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

namespace VisualizerApp_3
{
    public partial class MainForm : Form
    {
        public string startTime = "";
        public string endTime = "";
        /*public string prev_startTime;
        public string prev_endTime;
        public string currentSelection1;*/
        public List<string[]> pastSelections = new List<string[]>();
        public string[] currentSelection = new string[4];

        public string whatToShow;
        public List<string> myObjectList { get; set; }
        public MainForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.AutoScroll = true;
            datePicker1_start.Format = DateTimePickerFormat.Custom;
            datePicker1_start.CustomFormat = "yyyy-MM-dd HH:mm";
            datePicker2_end.Format = DateTimePickerFormat.Custom;
            datePicker2_end.CustomFormat = "yyyy-MM-dd HH:mm";

            // 시각화 하려는 센서 조회 및 CheckedListBox에서 선택할수 있게 표시
            try
            {
                SqlConnection myConnection = new SqlConnection(@"Data Source=DESKTOP-JIMMY;Initial Catalog=SensorDataDB;Integrated Security=True");
                string sql_getnum = "SELECT * FROM SensorDataDB.dbo.SENSOR_INFO a WHERE a.Usage = 'YES'";
                List<int> IDs = new List<int>();
                using (var cmd = new SqlCommand(sql_getnum, myConnection))
                {
                    myConnection.Open();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            string[] rowInfo = { myReader.GetValue(0).ToString(), myReader.GetValue(1).ToString(), myReader.GetValue(3).ToString() };
                            IDs.Add(Convert.ToInt32(rowInfo[0]));
                        }
                    }
                    myConnection.Close();
                }
                for (int i = 0; i < IDs.Count; i++)
                {
                    checkedListBox1_DevicesList.Items.Add("센서 " + IDs[i].ToString());
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
        /// <summary>
        /// Function to display results in form of chart for the selected time interval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //보기 버튼 누를 때에의 행위
        private void show_button_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || radioButton4.Checked)
            {
                startTime = datePicker1_start.Value.ToString("yyyy-MM-dd HH:mm");
                if (endTime.Contains("RT") == false) { endTime = datePicker2_end.Value.ToString("yyyy-MM-dd HH:mm"); }
                MyDataQuery myDataQuery = new MyDataQuery();
                if (datePicker1_start.Value > datePicker2_end.Value && endTime.Contains("RT") == false)
                {
                    MessageBox.Show("잘못된 날짜가 선택되었습니다. 확인해 보세요!", "에러 메시지");
                }
                else if (checkedListBox1_DevicesList.CheckedItems.Count < 1) { MessageBox.Show("조회할 센서가 선택되어 있지 않습니다! 최소 센서 1가지 선택하셔야 됩니다.", "에러 메시지"); }
                else
                {
                    if (checkedListBox1_DevicesList.CheckedItems.Count > 4) { MessageBox.Show("조회할 센서가 4개 이상 선택하셨습니다. 최대 센서 4개 까지 선택하실 수 있습니다.", "에러 메시지"); }
                    else {
                        var watch = System.Diagnostics.Stopwatch.StartNew();    //FOR DEBUGGING PURPOSE = FDP
                        List<List<List<string[]>>> DataGotten = new List<List<List<string[]>>>();
                        List<int> IDs = new List<int>();
                        whatToShow = "";
                        foreach (var index in checkedListBox1_DevicesList.CheckedIndices) {
                            int i = (int)index + 1;
                            IDs.Add(i);
                        }
                        if (radioButton1.Checked) {      whatToShow = "temp";   }
                        else if (radioButton2.Checked) { whatToShow = "humid";  }
                        else if (radioButton3.Checked) { whatToShow = "part03"; }
                        else if (radioButton4.Checked) { whatToShow = "part05"; }
                        if (endTime.Contains("RT") == false)
                        {
                            List<List<string[]>> DataGotten_temp = myDataQuery.MyDataGetter(whatToShow, startTime, endTime, IDs);
                            DataGotten.Add(DataGotten_temp);
                        }

                        /* else if (radioButton5.Checked)
                         {
                             whatToShow = "all";
                             if (endTime.Contains("RT") == false)
                             {
                                 List<List<string[]>> DataGotten_temp = myDataQuery.MyDataGetter("temp", startTime, endTime, IDs);
                                 List<List<string[]>> DataGotten_humid = myDataQuery.MyDataGetter("humid", startTime, endTime, IDs);
                                 List<List<string[]>> DataGotten_part03 = myDataQuery.MyDataGetter("part03", startTime, endTime, IDs);
                                 List<List<string[]>> DataGotten_part05 = myDataQuery.MyDataGetter("part05", startTime, endTime, IDs);


                                 DataGotten.Add(DataGotten_temp);
                                 DataGotten.Add(DataGotten_humid);
                                 DataGotten.Add(DataGotten_part03);
                                 DataGotten.Add(DataGotten_part05);
                             }

                         }*/
                        watch.Stop(); //FDP
                        Console.WriteLine("SQL서버에서 데이터를 불러오는 시간: " + watch.ElapsedMilliseconds.ToString() + " " + "ms"); //FDP
                        var watch2 = System.Diagnostics.Stopwatch.StartNew(); //FDP
                        string[] timeInterval = { startTime, endTime };
                        currentSelection = new string[] { startTime, endTime, whatToShow, IDs.Count.ToString() };
                        if (EqualityChecker(pastSelections, currentSelection) == false) {
                            MessageBox.Show("같은 장이 이미 열려 있습니다. 시간 간격, 센서 갯수 또는 데이터 구분을 다르게 선택해 보세요.", "에러 메시지");
                        }
                        else {
                            NewChartingForm formTest = new NewChartingForm(DataGotten, timeInterval, IDs, whatToShow);
                            pastSelections.Add(currentSelection);
                            formTest.Show();
                        }
                        //ChartingForm chartingForm = new ChartingForm(timeInterval, DataGotten, IDs, whatToShow); // string[] timeInterval, List<List<List<string[]>>> graphDataAll, List<int> ids, string whatToShow
                        //chartingForm.Show();
                        watch2.Stop(); //stop the stopwatch to count time spent for displaying the charts FDP
                        Console.WriteLine("시각화 하는 시간: " + watch2.ElapsedMilliseconds.ToString() + " " + "ms"); //FDP
                        Console.WriteLine("\nPlotting: {0}", whatToShow);
                    }
                }
            }
            else {
                MessageBox.Show("시각화 하려는 데이터 구분을 선텍해 주세요!", "에러 메시지");
            }
        }
        /// <summary>
        /// 똑같은 장이 1회 이상 열리지 않게 막는 함수.
        /// </summary>
        /// <param name="all"></param>
        /// <param name="one"></param>
        /// <returns></returns>
        private bool EqualityChecker(List<string[]> all, string[] one) {
            if (all.Count > 0) { 
                for (int i = 0; i < all.Count; i++) {
                        if (all[i][0].SequenceEqual(one[0]) && all[i][1].SequenceEqual(one[1]) && all[i][2].SequenceEqual(one[2]) && all[i][3].SequenceEqual(one[3]))
                        {
                            return false;
                        }
                }
                return true;
            }
            else { return true; }
        }
        //간편한 시간 간격 선택 시 다른색으로 표시해 주는 함수
        private void LinkLabelVisited(int num)
        {
            LinkLabel[] linkLabels = { linkLabel0_30m, linkLabel1_1H, linkLabel2_24H, linkLabel3_1w, linkLabel4_1m, linkLabel5_RT };
            for (int i = 0; i < linkLabels.Length; i++)
            {
                if (i == num)
                {
                    linkLabels[i].LinkVisited = true;
                }
                else
                {
                    linkLabels[i].LinkVisited = false;
                }
            }
        }
        //간편한 시간 간격 선택
        private void linkLabel0_30m_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabelVisited(0);
            startTime = DateTime.Now.AddMinutes(-30).ToString();
            datePicker1_start.Value = Convert.ToDateTime(startTime);
            endTime = DateTime.Now.ToString();
            datePicker2_end.Value = Convert.ToDateTime(endTime);
            Console.WriteLine(startTime + " " + endTime);
        }

        private void linkLabel1_1H_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabelVisited(1);
            startTime = DateTime.Now.AddHours(-1).ToString();
            datePicker1_start.Value = Convert.ToDateTime(startTime);
            endTime = DateTime.Now.ToString();
            datePicker2_end.Value = Convert.ToDateTime(endTime);
            Console.WriteLine(startTime + " " + endTime);
        }

        private void linkLabel2_24H_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabelVisited(2);
            startTime = DateTime.Now.AddDays(-1).ToString();
            datePicker1_start.Value = Convert.ToDateTime(startTime);
            endTime = DateTime.Now.ToString();
            datePicker2_end.Value = Convert.ToDateTime(endTime);
            Console.WriteLine(startTime + " " + endTime);
        }

        private void linkLabel3_1w_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabelVisited(3);
            startTime = DateTime.Now.AddDays(-7).ToString();
            datePicker1_start.Value = Convert.ToDateTime(startTime);
            endTime = DateTime.Now.ToString();
            datePicker2_end.Value = Convert.ToDateTime(endTime);
            Console.WriteLine(startTime + " " + endTime);
        }

        private void linkLabel4_1m_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabelVisited(4);
            startTime = DateTime.Now.AddMonths(-1).ToString();
            datePicker1_start.Value = Convert.ToDateTime(startTime);
            endTime = DateTime.Now.ToString();
            datePicker2_end.Value = Convert.ToDateTime(endTime);
            Console.WriteLine(startTime + " " + endTime);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabelVisited(5);
            datePicker1_start.Value = DateTime.Now;
            datePicker2_end.Value = DateTime.Now;
            endTime = "RT";

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
        public virtual List<List<string[]>> MyDataGetter(string what, string startDate, string endDate, List<int> IDs)
        {
            List<List<string[]>> DataArr = new List<List<string[]>>();
            for (int i=0; i<IDs.Count; i++)
            {
                DataArr.Add(new List<string[]>());
            }
            string sql = "";
            try
            {
                SqlConnection myConnection = new SqlConnection(@"Data Source=DESKTOP-JIMMY;Initial Catalog=SensorDataDB;Integrated Security=True");
                //센서 ID와 데이터 (온,습도,타티클03, 05), 그리고 산텍된 시간 간격(interval)에 따른 퀴리하기
                for (int i=0; i<IDs.Count; i++) 
                    { 
                    if(what.Contains("temp")) { sql = "select * from SensorDataDB.dbo.DEV_TEMP_" + IDs[i].ToString() + " a where CONVERT(datetime, DateAndTime) >= '" + startDate + "' and CONVERT(datetime, DateAndTime) <= '" + endDate + "' order by DateAndTime ASC";
                    }
                    else if (what.Contains("humid")) { sql = "select * from SensorDataDB.dbo.DEV_HUMID_" + IDs[i].ToString() + " a where DateAndTime >= '" + startDate + "' and DateAndTime <= '" + endDate + "' order by DateAndTime ASC"; }
                    else if (what.Contains("part03")) { sql = "select * from SensorDataDB.dbo.DEV_PART03_" + IDs[i].ToString() + " a where DateAndTime >= '" + startDate + "' and DateAndTime <= '" + endDate + "' order by DateAndTime ASC"; }
                    else { sql = "select * from SensorDataDB.dbo.DEV_PART05_" + IDs[i].ToString() + " a where DateAndTime >= '" + startDate + "' and DateAndTime <= '" + endDate + "' order by DateAndTime ASC"; }
                    using (var cmd = new SqlCommand(sql, myConnection))
                    {
                        myConnection.Open();
                        using (var myReader = cmd.ExecuteReader())
                        {
                            while (myReader.Read())
                            {
                                //각 배열(array) 변수에 2가지 데이터가 들어가 있어서, 0과 1인덕스만 불러우면 된다. 
                                //0은 데이터, 1은 시간임.
                                string[] myObj = { myReader.GetValue(0).ToString(), myReader.GetValue(1).ToString() };
                                DataArr[i].Add(myObj);
                            }
                        }
                        myConnection.Close();
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
            return new List<List<string[]>>(DataArr);
        }
    }

}

