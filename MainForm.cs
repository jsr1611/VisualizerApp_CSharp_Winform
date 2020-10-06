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
        public List<string> myObjectList { get; set; }
        public MainForm()
        {
            InitializeComponent();
            //this.SetBounds(0, 0, Screen.PrimaryScreen.Bounds.Width-10, Screen.PrimaryScreen.Bounds.Height-40);
            
            this.AutoScroll = true;
            datePicker1_start.Format = DateTimePickerFormat.Custom;
            datePicker1_start.CustomFormat = "yyyy-MM-dd HH:mm";
            datePicker2_end.Format = DateTimePickerFormat.Custom;
            datePicker2_end.CustomFormat = "yyyy-MM-dd HH:mm";
            Btn1_time = new Button[] { button1_realtime, button1_24h, button1_datepicker };
            Btn2_data = new Button[] { button2_temp, button2_humid, button2_part03, button2_part05 };
            Btn3_address = new Button[] { button3_solbi1, button3_solbi2, button3_solbi3, button3_solbi4 };
            foreach(var button in Btn3_address) 
                { button.Enabled = false; }
            IDs_AvailCheck(); // 시각화 하려는 센서 ID 조회 및 배열에 ID번호 추가하기
        }
        /// <summary>
        /// Function to display results in form of chart for the selected time interval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //보기 버튼 누를 때에의 행위
        private void show_button_Click(object sender, EventArgs e) {
            
            if (button1_realtime.BackColor != Color.Transparent || button1_24h.BackColor != Color.Transparent || button1_datepicker.BackColor != Color.Transparent) {
                MyDataQuery myDataQuery = new MyDataQuery();
                List<List<List<string[]>>> DataRetrieved_general = new List<List<List<string[]>>>();
                string[] timeInterval = { startTime, endTime };
                if (button1_realtime.BackColor != Color.Transparent && IDs_selected.Count > 0)
                {
                    Console.WriteLine("Now RealTime");
                    timeInterval[0] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    timeInterval[1] = "RT";
                    List<List<List<string[]>>> DataRetrieved_RT = new List<List<List<string[]>>>();
                    List<List<string[]>> rtData = myDataQuery.RealTimeDBQuery(IDs_selected, whatToShow);
                    DataRetrieved_RT.Add(rtData);
                    NewChartingForm RTChart = new NewChartingForm(DataRetrieved_RT, timeInterval, IDs_selected, whatToShow);
                    RTChart.Show();
                }
                else if (button1_24h.BackColor != Color.Transparent && IDs_selected.Count > 0)
                {
                    Console.WriteLine("Now 24H");
                    startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm");
                    endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    DataRetrieved_general.Add(myDataQuery.DBQuery(whatToShow, startTime, endTime, IDs_selected));
                    ScotPlot GeneralChart = new ScotPlot(DataRetrieved_general, whatToShow, IDs_selected);
                    //ChartingForm GeneralChart = new ChartingForm(DataRetrieved_general, timeInterval, IDs_selected, whatToShow);
                    //NewChartingForm GeneralChart = new NewChartingForm(DataRetrieved_general, timeInterval, IDs_selected, whatToShow);
                    GeneralChart.Show();
                }
                else if (button1_datepicker.BackColor != Color.Transparent && IDs_selected.Count > 0)
                {
                    Console.WriteLine("Now from " + startTime + " to " + endTime);
                    if (datePicker1_start.Value < datePicker2_end.Value) {
                        startTime = datePicker1_start.Value.ToString("yyyy-MM-dd HH:mm");
                        endTime = datePicker2_end.Value.ToString("yyyy-MM-dd HH:mm");
                        Console.WriteLine(startTime + " " + endTime);
                        DataRetrieved_general.Add(myDataQuery.DBQuery(whatToShow, startTime, endTime, IDs_selected));
                      
                        //////////////////// new Chart ScotPlot ////////////////////////////////
                        ScotPlot GeneralChart = new ScotPlot(DataRetrieved_general, whatToShow, IDs_selected);
                        //ChartingForm GeneralChart = new ChartingForm(DataRetrieved_general, timeInterval, IDs_selected, whatToShow);
                        //NewChartingForm GeneralChart = new NewChartingForm(DataRetrieved_general, timeInterval, IDs_selected, whatToShow);
                        GeneralChart.Show();
                    }
                    else {
                        MessageBox.Show("잘못된 날짜가 선택되었습니다. 확인해 보세요!", "에러 메시지");
                    }
                }
                else
                {
                    MessageBox.Show("센서 설치 주소를 선택하지 않으셨습니다.", "에러 매시지");
                }

            }

            else { MessageBox.Show("시간을 선택하지 않으셨습니다. 실시간 또는 기간 설정을 해 주세요!", "에러 메시지"); }

            if (IDs_selected.Count < 5) { }
            else {
                var watch = System.Diagnostics.Stopwatch.StartNew();    //timer for sql read time measurement
                whatToShow = "";
                //currentSelection = new string[] { startTime, endTime, whatToShow, IDs_selected.Count.ToString() };
                watch.Stop();
                Console.WriteLine("SQL서버에서 데이터를 불러오는 시간: " + watch.ElapsedMilliseconds.ToString() + " " + "ms"); //FDP
                var watch2 = System.Diagnostics.Stopwatch.StartNew();

                //NewChartingForm formTest = new NewChartingForm(DataGotten, timeInterval, IDs, whatToShow); // 차트폼 생성
                //formTest.Show(); // 차트 시각화
                //ChartingForm chartingForm = new ChartingForm(timeInterval, DataRetrieved_general, IDs, whatToShow);
                // chartingForm.Show();
                //UserControl_Main userControl1 = new UserControl_Main(timeInterval, DataGotten, IDs, whatToShow);
                //userControl1.Dock = DockStyle.Fill;
                //panel3_chart.Controls.Add(userControl1);
                //userControl1.Visible = true;
                //pastSelections.Add(currentSelection);

                watch2.Stop(); //stop the stopwatch to count time spent for displaying the charts FDP
                Console.WriteLine("시각화 하는 시간: " + watch2.ElapsedMilliseconds.ToString() + " " + "ms"); //FDP
            }
            foreach(var item in IDs_selected) {
                Console.WriteLine("IDs selected: {0} ",item);
            }
        
        }
        private void IDs_AvailCheck()
        {
            IDs_available.Clear();
            try
            {
                SqlConnection myConnection = new SqlConnection(@"Data Source=10.1.55.174;Initial Catalog=SensorDataDB;User id=dlitdb;Password=dlitdb; Min Pool Size=20");
                string sql_getIDs = "SELECT * FROM SensorDataDB.dbo.SENSOR_INFO a WHERE a.Usage = 'YES'";

                using (var cmd = new SqlCommand(sql_getIDs, myConnection)) {
                    myConnection.Open();
                    using (var myReader = cmd.ExecuteReader()) {
                        if (myReader.HasRows) {
                            while (myReader.Read()) {
                                string[] rowInfo = { myReader.GetValue(0).ToString(), myReader.GetValue(1).ToString(), myReader.GetValue(3).ToString() };
                                Btn3_address[Convert.ToInt32(rowInfo[0]) - 1].Enabled = true;
                                IDs_available.Add(Convert.ToInt32(rowInfo[0]));
                            }
                        }
                        else {
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
        //간편한 시간 간격 선택 시 button.BackColor를 다른색으로 표시해 주는 함수
        private void buttonSelector(Button[] btnNames, int index, Color color)
        {
            for (int i = 0; i < btnNames.Length; i++)
            {
                if (i == index) {
                    btnNames[i].BackColor = color;

                }
                else {
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
            if (IDs_available.Contains(1) == false) { 
            MessageBox.Show("조회 불가한 센서 ID입니다.");
            }
            else
            {
                if (IDs_selected.Contains(1) == false) // button3_solbi1.BackColor == Color.Transparent
                {
                    IDs_selected.Add(1);
                    button3_solbi1.BackColor = Color.Chartreuse;
                }
                else {
                    IDs_selected.Remove(1);
                    button3_solbi1.BackColor = Color.Transparent; }
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
                else {
                    IDs_selected.Remove(2);
                    button3_solbi2.BackColor = Color.Transparent; }
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
                else {
                    IDs_selected.Remove(3); 
                    button3_solbi3.BackColor = Color.Transparent; }
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
                else {
                    IDs_selected.Remove(4);
                    button3_solbi4.BackColor = Color.Transparent; }
            }
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
        public virtual List<List<string[]>> DBQuery(string what, string startDate, string endDate, List<int> IDs)
        {
            List<List<string[]>> DataArr = new List<List<string[]>>();
            for (int i=0; i<IDs.Count; i++)
            {
                DataArr.Add(new List<string[]>());
            }
            string sql = "";
            try
            {
                //SqlConnection myConnection = new SqlConnection(@"Data Source=DESKTOP-DLIT\SQLEXPRESS;Initial Catalog=SensorDataDB;Integrated Security=True");
                SqlConnection myConnection = new SqlConnection(@"Data Source=10.1.55.174;Initial Catalog=SensorDataDB;User id=dlitdb;Password=dlitdb; Min Pool Size=20");

                //센서 ID와 데이터 (온,습도,타티클03, 05), 그리고 산텍된 시간 간격(interval)에 따른 퀴리하기
                for (int i=0; i<IDs.Count; i++) 
                    { 
                    if(what.Contains("temp")) { sql = "select * from SensorDataDB.dbo.DEV_TEMP_" + IDs[i].ToString() + " a where CONVERT(datetime, DateAndTime) >= '" + startDate + "' and CONVERT(datetime, DateAndTime) <= '" + endDate + "' order by DateAndTime ASC";
                    }
                    else if (what.Contains("humid")) { sql = "select * from SensorDataDB.dbo.DEV_HUMID_" + IDs[i].ToString() + " a where DateAndTime >= '" + startDate + "' and DateAndTime <= '" + endDate + "' order by DateAndTime ASC"; }
                    else if (what.Contains("part03")) { sql = "select * from SensorDataDB.dbo.DEV_PART03_" + IDs[i].ToString() + " a where DateAndTime >= '" + startDate + "' and DateAndTime <= '" + endDate + "' order by DateAndTime ASC"; }
                    else { sql = "select * from SensorDataDB.dbo.DEV_PART05_" + IDs[i].ToString() + " a where DateAndTime >= '" + startDate + "' and DateAndTime <= '" + endDate + "' order by DateAndTime ASC"; }
                    Console.WriteLine(sql);
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

