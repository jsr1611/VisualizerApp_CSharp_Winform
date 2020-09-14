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
        static readonly object _locker = new object();
        public string startTime = "";
        public string endTime = "";

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
            /*timer1.Enabled = true;
            timer1.Start();*/
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*
            MyDataQuery myDataQuery = new MyDataQuery(); // Get data from SQL Server database
            myObjectList = myDataQuery.MyDataGetter(false, 0, 0);   // Get data - one row at a time

            textBox1_temp.Text = myObjectList[0];
            textBox2_humid.Text = myObjectList[1];
            textBox3_part03.Text = String.Format("{0:n0}", Convert.ToInt32(myObjectList[2]));
            textBox4_part05.Text = String.Format("{0:n0}", Convert.ToInt32(myObjectList[3]));
            */

        }

        /// <summary>
        /// Function to display results in form of chart for the selected time interval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void show_button_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || radioButton4.Checked || radioButton5.Checked) { 
                startTime = datePicker1_start.Value.ToString("yyyy-MM-dd HH:mm");
                endTime = datePicker2_end.Value.ToString("yyyy-MM-dd HH:mm");
                MyDataQuery myDataQuery = new MyDataQuery();
                if (datePicker1_start.Value > datePicker2_end.Value)
                {
                    MessageBox.Show("잘못된 날짜가 선택되었습니다. 날짜와 시간을 다시 선택해 보세요!", "에러 메시지");
                }
                else  {

                    var watch = System.Diagnostics.Stopwatch.StartNew();    //FOR DEBUGGING PURPOSE = FDP
                    List<List<List<string[]>>> DataGotten = new List<List<List<string[]>>>();
                    List<int> IDs = new List<int>();
                    string whatToShow = "";

                    if (radioButton1.Checked) {
                        whatToShow = "temp";
                        Tuple<List<int>, List<List<string[]>>> DataGottenTupleTemp = myDataQuery.MyDataGetter(whatToShow, startTime, endTime);
                        List<List<string[]>> DataGotten_temp = DataGottenTupleTemp.Item2;
                        IDs = DataGottenTupleTemp.Item1;
                        DataGotten.Add(DataGotten_temp);
                        
                    }
                    else if (radioButton2.Checked) {
                        whatToShow = "humid";
                        Tuple<List<int>, List<List<string[]>>> DataGottenTupleHumid = myDataQuery.MyDataGetter(whatToShow, startTime, endTime);
                        List<List<string[]>> DataGotten_humid = DataGottenTupleHumid.Item2;
                        IDs = DataGottenTupleHumid.Item1;
                        DataGotten.Add(DataGotten_humid);
                    }
                    else if (radioButton3.Checked) {
                        whatToShow = "part03";
                        Tuple<List<int>, List<List<string[]>>> DataGottenTuplePart03 = myDataQuery.MyDataGetter(whatToShow, startTime, endTime);
                        List<List<string[]>> DataGotten_part03 = DataGottenTuplePart03.Item2;
                        IDs = DataGottenTuplePart03.Item1;
                        DataGotten.Add(DataGotten_part03);
                    }
                    else if (radioButton4.Checked)
                    {
                        whatToShow = "part05";
                        Tuple<List<int>, List<List<string[]>>> DataGottenTuplePart05 = myDataQuery.MyDataGetter(whatToShow, startTime, endTime);
                        List<List<string[]>> DataGotten_part05 = DataGottenTuplePart05.Item2;
                        IDs = DataGottenTuplePart05.Item1;
                        DataGotten.Add(DataGotten_part05);

                    }
                    else if (radioButton5.Checked)
                    {
                        whatToShow = "all";
                        Tuple<List<int>, List<List<string[]>>> DataGottenTupleTemp = myDataQuery.MyDataGetter("temp", startTime, endTime);
                        List<List<string[]>> DataGotten_temp = DataGottenTupleTemp.Item2;
                        List<List<string[]>> DataGotten_humid = myDataQuery.MyDataGetter("humid", startTime, endTime).Item2;
                        List<List<string[]>> DataGotten_part03 = myDataQuery.MyDataGetter("part03", startTime, endTime).Item2;
                        List<List<string[]>> DataGotten_part05 = myDataQuery.MyDataGetter("part05", startTime, endTime).Item2;
                        IDs = DataGottenTupleTemp.Item1;
                        DataGotten.Add(DataGotten_temp);
                        DataGotten.Add(DataGotten_humid);
                        DataGotten.Add(DataGotten_part03);
                        DataGotten.Add(DataGotten_part05);
                    }


                    watch.Stop(); //FDP
                    Console.WriteLine("SQL서버에서 데이터를 불러오는 시간: " + watch.ElapsedMilliseconds.ToString() + " "+ "ms"); //FDP
                    var watch2 = System.Diagnostics.Stopwatch.StartNew(); //FDP
                    //Console.WriteLine("\n\nCHECK HERE:\nLen(): {0} {1} {2} {3} {4}\n\n", DataGotten.Count, DataGotten[0].Count, DataGotten[1].Count, DataGotten[2].Count, DataGotten[3].Count);
                    string[] timeInterval = { startTime, endTime };
                    ChartingForm form = new ChartingForm(timeInterval, DataGotten, IDs, whatToShow);   //send data in List<string[]> for to new winform
                    
                    form.Show(); //displaying the form in a seperate window FDP
                    watch2.Stop(); //stop the stopwatch to count time spent for displaying the charts FDP
                    Console.WriteLine("시각화 하는 시간: " + watch2.ElapsedMilliseconds.ToString() + " " + "ms"); //FDP
                    Console.WriteLine("\nPlotting: {0}", whatToShow);
                }
            }
            else
            {
                MessageBox.Show("시각화 하려는 것을 선텍해주세요!", "에러 메시지");
            }

        }


        private void Test2_button_Click(object sender, EventArgs e)
        {
            TestForm2 newTest2 = new TestForm2();
            newTest2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            startTime = datePicker1_start.Value.ToString("yyyy-MM-dd HH:mm");
            endTime = datePicker2_end.Value.ToString("yyyy-MM-dd HH:mm");
            MyDataQuery myDataQuery = new MyDataQuery();
            if (datePicker1_start.Value > datePicker2_end.Value)
            {
                MessageBox.Show("잘못된 날짜가 선택되었습니다. 날짜와 시간을 다시 선택해 보세요!", "에러 메시지");
            }
            else
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();    //FOR DEBUGGING PURPOSE = FDP
                //List<List<string[]>> DataGotten = myDataQuery.MyDataGetter("temp", startTime, endTime);
                watch.Stop(); //FDP
                Console.WriteLine("SQL서버에서 데이터를 불러오는 시간: " + watch.ElapsedMilliseconds.ToString() + " " + "ms"); //FDP
                var watch2 = System.Diagnostics.Stopwatch.StartNew(); //FDP
                //TestForm testF = new TestForm(DataGotten, startTime, endTime);
                //testF.Show();  //sending time and data in List<string[]> for to new winform
                               //displaying the form in a seperate window FDP
                watch2.Stop(); //stop the stopwatch to count time spent for displaying the charts FDP
                Console.WriteLine("시각화 하는 시간: " + watch2.ElapsedMilliseconds.ToString() + " " + "ms"); //FDP


            }


        }

    }

    public class MyDataQuery
    {
        public virtual List<string> MyDataGetter(bool lastFourRows, int d, int h)
        {
            List<string> arrList = new List<string>();
            string sql_temp = "";
            string sql_humid = "";
            string sql_part03 = "";
            string sql_part05 = "";

            try
            {
                SqlConnection myConnection = new SqlConnection(@"Data Source=DESKTOP-JQMGA3H;Initial Catalog=SensorDataDB;Integrated Security=True");
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

                for (int i=1; i<IDs.Count; i++)
                {
                    sql_temp = "SELECT TOP 1 * FROM DEV_TEMP_" + IDs[i].ToString()+ " ORDER BY DateAndTime DESC";
                    sql_humid = "SELECT TOP 1 * FROM DEV_HUMID_" + IDs[i].ToString() + " ORDER BY DateAndTime DESC";
                    sql_part03 = "SELECT TOP 1 * FROM DEV_PART03_" + IDs[i].ToString() + " ORDER BY DateAndTime DESC";
                    sql_part05 = "SELECT TOP 1 * FROM DEV_PART05_" + IDs[i].ToString() + " ORDER BY DateAndTime DESC";
                    string[] sql_arr = {sql_temp, sql_humid, sql_part03, sql_part05 };
                    foreach(var sql in sql_arr)
                    {
                        using (var cmd = new SqlCommand(sql, myConnection))
                        {
                            myConnection.Open();
                            using (var myReader = cmd.ExecuteReader())
                            {
                                while (myReader.Read())
                                {
                                    string myobj = myReader.GetValue(0).ToString();
                                    arrList.Add(myobj);
                                }
                                
                            }
                            myConnection.Close();
                        }
                        
                    }


                }
                
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
            
            return arrList;
        }

        /// <summary>
        /// GetData for the given period of startDate and endDate
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public virtual Tuple<List<int>, List<List<string[]>>> MyDataGetter(string what, string startDate, string endDate)
        {
            List<string[]> dev_temp_1 = new List<string[]>(); // 임시 센서 1번 데이터 저장기
            List<string[]> dev_temp_2 = new List<string[]>(); // 임시 센서 2번 데이터 저장기
            List<string[]> dev_temp_3 = new List<string[]>(); // 임시 센서 3번 데이터 저장기
            List<string[]> dev_temp_4 = new List<string[]>(); // 임시 센서 4번 데이터 저장기

            List<List<string[]>> arr_temp = new List<List<string[]>>();
            List<int> IDs = new List<int>(); // 센서 ID 저장을 위한 List<> 변수
            string sql = "";
            try
            {
                SqlConnection myConnection = new SqlConnection(@"Data Source=DESKTOP-JQMGA3H;Initial Catalog=SensorDataDB;Integrated Security=True");
                //센서 개수 확인하기
                string sql_getnum = "SELECT * FROM SensorDataDB.dbo.SENSOR_INFO a WHERE a.Usage = 'YES'";
                using (var cmd = new SqlCommand(sql_getnum, myConnection))
                {
                    myConnection.Open();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            string[] rowInfo = { myReader.GetValue(0).ToString(), myReader.GetValue(1).ToString(), myReader.GetValue(3).ToString()};
                            IDs.Add(Convert.ToInt32(rowInfo[0]));
                        }

                    }
                }
                myConnection.Close();
                //foreach(int item in IDs) { Console.WriteLine("IDs: {0}", item); }
                
                //센서 ID와 데이터 (온,습도,타티클03, 05), 그리고 산텍된 시간 간격(interval)에 따른 퀴리하기
                for (int i=0; i<IDs.Count; i++) 
                    { 
                    if(what.Contains("temp")) { sql = "select * from SensorDataDB.dbo.DEV_TEMP_" + IDs[i].ToString() + " a where CONVERT(datetime, DateAndTime) >= '" + startDate + "' and CONVERT(datetime, DateAndTime) <= '" + endDate + "' order by DateAndTime ASC";
                        //Console.WriteLine("\n\n\nNowThis: select * from SensorDataDB.dbo.DEV_TEMP_" + IDs[i].ToString() + " a where CONVERT(datetime, DateAndTime) >= '" + startDate + "' and CONVERT(datetime, DateAndTime) <= '" + endDate + "' order by DateAndTime ASC");
                    }
                    else if (what.Contains("humid")) { sql = "select * from SensorDataDB.dbo.DEV_HUMID_" + IDs[i].ToString() + " a where DateAndTime >= '" + startDate + "' and DateAndTime <= '" + endDate + "' order by DateAndTime ASC"; }
                    else if (what.Contains("part03")) { sql = "select * from SensorDataDB.dbo.DEV_PART03_" + IDs[i].ToString() + " a where DateAndTime >= '" + startDate + "' and DateAndTime <= '" + endDate + "' order by DateAndTime ASC"; }
                    else { sql = "select * from SensorDataDB.dbo.DEV_PART05_" + IDs[i].ToString() + " a where DateAndTime >= '" + startDate + "' and DateAndTime <= '" + endDate + "' order by DateAndTime ASC"; }
                    using (var cmd = new SqlCommand(sql, myConnection))
                    {
                        myConnection.Open();
                        using (var myReader = cmd.ExecuteReader())
                        {
                            int cnt = 0; //임시 카운터
                            while (myReader.Read())
                            {
                                cnt += 1;
                                //각 배열(array) 변수에 2가지 데이터가 들어가 있어서, 0과 1인덕스만 불러우면 된다. 
                                //0은 데이터, 1은 시간임.
                                string[] myObj = { myReader.GetValue(0).ToString(), myReader.GetValue(1).ToString() };
                                if (IDs[i] == 1) { dev_temp_1.Add(myObj); }
                                else if (IDs[i] == 2) { dev_temp_2.Add(myObj); }
                                else if (IDs[i] == 3) { dev_temp_3.Add(myObj); }
                                else if (IDs[i] == 4) { dev_temp_4.Add(myObj); }
                                //Console.WriteLine(cnt.ToString() + " " + myObj[0] +" "+ myObj[1]);
                            }
                        }
                        myConnection.Close();
                    }
                    //Console.WriteLine("\n For loop times: {0} {1} {2} {3} {4}\n", i, IDs[i], dev_temp_1.Count, dev_temp_2.Count, dev_temp_3.Count );
                }
                // 불러운 데이터들을 하나 하나 List<>에 집어넣는다. 
                arr_temp.Add(dev_temp_1); 
                arr_temp.Add(dev_temp_2);
                arr_temp.Add(dev_temp_3);
                arr_temp.Add(dev_temp_4);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
            //IDs = 센서 ID 집합, arr_temp = db 퀴리 결과
            return new Tuple<List<int>, List<List<string[]>>>(IDs, arr_temp);
        }
    }

}

