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
    public partial class Form1 : Form
    {
        public string startTime = "";
        public string endTime = "";

        public List<string[]> myObjectList;
        public Form1()
        {
            InitializeComponent();

            this.AutoScroll = true;
            datePicker1_start.Format = DateTimePickerFormat.Custom;
            datePicker1_start.CustomFormat = "yyyy-MM-dd HH:mm";
            datePicker2_end.Format = DateTimePickerFormat.Custom;
            datePicker2_end.CustomFormat = "yyyy-MM-dd HH:mm";

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MyDataQuery myDataQuery = new MyDataQuery(); // Get data from SQL Server database
            myObjectList = myDataQuery.MyDataGetter(false, 0, 0);   // Get data - one row at a time

            textBox1_temp.Text = myObjectList[0][1];
            textBox2_humid.Text = myObjectList[0][2];
            textBox3_part03.Text = myObjectList[0][3];
            textBox4_part05.Text = myObjectList[0][4];
        }

        /// <summary>
        /// Function to display results in form of chart for the selected time interval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            startTime = datePicker1_start.Value.ToString("yyyy-MM-dd HH:mm");
            endTime = datePicker2_end.Value.ToString("yyyy-MM-dd HH:mm");
            MyDataQuery myDataQuery = new MyDataQuery();
            if (datePicker1_start.Value > datePicker2_end.Value)
            {
                MessageBox.Show("Wrong dates are selected. Please try again!", "Error message");
            }
            else  {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " ");
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Console.WriteLine("Time elapsed: ");
            List<string[]> DataGotten = myDataQuery.MyDataGetter(startTime, endTime);
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds.ToString() + " " + DateTime.Now.ToString("HH:mm:ss.fff"));
            ChartingForm form = new ChartingForm(startTime, endTime, DataGotten);
            form.Show();
            }

        }
    }

    public class MyDataQuery
    {
        public virtual List<string[]> MyDataGetter(bool lastFourRows, int d, int h)
        {
            List<string[]> arrList = new List<string[]>();
            string sql = "";
            try
            {
                SqlConnection myConnection = new SqlConnection(@"Data Source=DESKTOP-JQMGA3H;Initial Catalog=MyDatabase01;Integrated Security=True");
                if (lastFourRows == true)
                {
                    sql = "SELECT TOP 4 * FROM SensorData ORDER BY DateAndTime DESC";
                }
                else
                {
                    if (d == 0 && h > 0) { sql = "SELECT * FROM SensorData WHERE CONVERT(datetime, SensorData.DateAndTime) > getdate()-" + h.ToString() + " ORDER BY DateAndTime DESC"; }
                    else if (d > 1 && h == 0) { sql = "SELECT * FROM SensorData WHERE CONVERT(datetime, SensorData.DateAndTime) > getdate()-" + d.ToString() + " ORDER BY DateAndTime DESC"; }
                    else if (d == 0 && h == 0) { sql = "SELECT TOP 1 * FROM SensorData ORDER BY DateAndTime DESC"; }
                }
                using (var cmd = new SqlCommand(sql, myConnection))
                {
                    myConnection.Open();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            string[] myObj = { myReader.GetValue(0).ToString(), myReader.GetValue(1).ToString(), myReader.GetValue(2).ToString(), myReader.GetValue(3).ToString(), myReader.GetValue(4).ToString(), myReader.GetValue(5).ToString() };
                            arrList.Add(myObj);
                        }
                    }
                }
                myConnection.Close();
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
        public virtual List<string[]> MyDataGetter(string startDate, string endDate)
        {
            
            List<string[]> arrList = new List<string[]>();
            string sql = "";
            try
            {
                //Console.WriteLine("startDate.ToString() + endDate.ToString(): " + startDate.ToString() + " " + endDate.ToString());
                SqlConnection myConnection = new SqlConnection(@"Data Source=DESKTOP-JQMGA3H;Initial Catalog=MyDatabase01;Integrated Security=True");
                sql = "select * from MyDatabase01.dbo.SensorData a where DateAndTime >= '" + startDate + "' and DateAndTime <= '" + endDate + "' order by DateAndTime ASC";
                //Console.WriteLine("SQL: " + sql);
                using (var cmd = new SqlCommand(sql, myConnection))
                {
                    string[] myObj = {"0", "0", "0", "0", "0", "0"};
                    myConnection.Open();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            string[] myObj2 = { myReader.GetValue(0).ToString(), myReader.GetValue(1).ToString(), myReader.GetValue(2).ToString(), myReader.GetValue(3).ToString(), myReader.GetValue(4).ToString(), myReader.GetValue(5).ToString() };
                            arrList.Add(myObj2);
                            /*
                            myObj[0] = myReader.GetValue(0).ToString();
                            myObj[1] = myReader.GetValue(1).ToString();
                            myObj[2] = myReader.GetValue(2).ToString();
                            myObj[3] = myReader.GetValue(3).ToString();
                            myObj[4] = myReader.GetValue(4).ToString();
                            myObj[5] = myReader.GetValue(5).ToString();
                            */
                        }
                    }
                }
                myConnection.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
            return arrList;
        }
    }

}

