using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataVisualizerApp
{

    public class DataQuery
    {
        public string dbName { get; set; }

        public string SensorUsage { get; set; }

        public List<string> SensorUsageColumn { get; set; }

        public string S_DeviceTable { get; set; }

        public SqlConnection myConn { get; set; }

        public string sqlConStr { get; set; }

        public List<string> FourRangeColmn { get; }

        public DataQuery()
        {

        }

        public DataQuery(SqlConnection myConn, string dbName, string deviceTable, string sensorUsage, List<string> sensorUsageColumn, List<string> fourRangeColmn, string conStr)
        {
            this.myConn = myConn;
            this.dbName = dbName;
            S_DeviceTable = deviceTable;
            SensorUsage = sensorUsage;
            SensorUsageColumn = sensorUsageColumn;
            FourRangeColmn = fourRangeColmn;
            sqlConStr = conStr;
        }


        public System.Data.DataSet GetTempValues()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SELECT sensor_id, Temperature, dateandtime FROM( SELECT 1 AS sensor_id, AVG(CAST(Temperature AS DECIMAL(18, 2))) AS Temperature, SUBSTRING(dateandtime, 1,16) AS dateandtime FROM dev_temp_1 WHERE dateandtime BETWEEN '2020-01-07 12:29' AND '2021-01-08 12:29' GROUP BY SUBSTRING(dateandtime, 1, 16) UNION ALL SELECT 2 AS sensor_id, AVG(CAST(Temperature AS DECIMAL(18, 2))) AS Temperature, SUBSTRING(dateandtime, 1,16) AS dateandtime FROM dev_temp_2 WHERE dateandtime BETWEEN '2020-01-07 12:29' AND '2021-01-08 12:29' GROUP BY SUBSTRING(dateandtime, 1, 16) UNION ALL SELECT 3 AS sensor_id, AVG(CAST(Temperature AS DECIMAL(18, 2))) AS Temperature, SUBSTRING(dateandtime, 1,16) AS dateandtime FROM dev_temp_3 WHERE dateandtime BETWEEN '2020-01-07 12:29' AND '2021-01-08 12:29' GROUP BY SUBSTRING(dateandtime, 1, 16) )a ORDER BY dateandtime", myConn);
            System.Data.DataSet ds = new System.Data.DataSet();

            ///conn.Open();
            da.Fill(ds);
            return ds;
        }

        public System.Data.DataSet GetValuesFromDB(string startTime, string endTime, List<string> whatToQuery, List<int> IDs)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            System.Data.DataSet ds = new System.Data.DataSet();

            for (int index = 0; index < whatToQuery.Count; index++)
            {

                string sql_head = $"SELECT sensor_id, {whatToQuery[index]}, dateandtime FROM ( ";

                string sql_connector = " UNION ALL ";
                string sql_tail = ")a ORDER BY dateandtime";


                for (int i_sensorID = 0; i_sensorID < IDs.Count; i_sensorID++) // 1,2,3, ...
                {
                    sql_head += $" SELECT {IDs[i_sensorID]} as sensor_id, AVG(CAST({whatToQuery[index]} AS int)) AS {whatToQuery[index]}, SUBSTRING(dateandtime, 1, 16) AS dateandtime " +
                        $" FROM d_{whatToQuery[index].Substring(2)} WHERE {SensorUsageColumn[0]} = {IDs[i_sensorID]} AND dateandtime BETWEEN '{startTime}' AND '{endTime}' " +
                        $" GROUP BY SUBSTRING(dateandtime, 1, 16) ";

                    if (i_sensorID != (IDs.Count - 1)) { sql_head += sql_connector; }
                }


                sql_head += sql_tail;

                try
                {
                    if (myConn.State != System.Data.ConnectionState.Open)
                    {
                        myConn.Open();
                    }
                    da.SelectCommand = new SqlCommand(sql_head, myConn);

                    ///conn.Open();
                    da.Fill(ds, whatToQuery[index]);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return ds;
        }


        public (List<SortedDictionary<int, List<string>>>, List<SortedDictionary<int, List<string>>>) DBQuery2(string startDate, string endDate, List<int> IDs, List<string> whatToQuery)
        {


            //Console.WriteLine(devices[0].Temperature.Values.Count.ToString() + devices[0].Temperature.Times.Count);


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

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = new SqlCommand(sql_head, myConn);
                    System.Data.DataSet ds = new System.Data.DataSet();

                    ///conn.Open();
                    da.Fill(ds);


                    using (var cmd = new SqlCommand(sql_head, myConn))
                    {
                        cmd.CommandTimeout = 0;
                        myConn.Open();
                        Console.WriteLine("Connection opened");
                        using (var myReader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("Executing Reader() method...");
                            //int i_sensorID = 0;

                            while (myReader.Read())
                            {
                                Console.WriteLine("Reading data...");
                                string[] allDataRead = { myReader.GetValue(0).ToString(), myReader[sql_names[i_DataType]].ToString(), myReader["DateAndTime"].ToString() };

                                mapVals[i_DataType][Convert.ToInt32(allDataRead[0])].Add(allDataRead[1]);
                                mapTime[i_DataType][Convert.ToInt32(allDataRead[0])].Add(allDataRead[2]);

                                //devices1 = GetDeviceByDeviceId(devices, Convert.ToInt32(allDataRead[0]));


                                //devices[IDs_now[i_sensorID]].Temperature
                                //Console.WriteLine(sql_names[i_DataType] + " : " + allDataRead[1] + " " + allDataRead[2] + " " + allDataRead[0]);
                            }
                        }
                        myConn.Close();
                    }
                    Console.WriteLine("SQL 쿼리문: " + sql_head);
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


        /*
                private Device GetDeviceByDeviceId(Device[] devices, int deviceId)
                {
                    return devices.Where(r => r.ID == deviceId).FirstOrDefault();
                }*/

        /// <summary>
        /// 실시간 데이터 쿼리를 위한 쿼리 함수
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="tbName"></param>
        /// <returns="List<List<string[]>> DataArr "></returns>
        public List<List<List<string[]>>> RealTimeDBQuery(List<int> IDs, List<string> tbName)
        {
            List<List<List<string[]>>> DataArrRT = new List<List<List<string[]>>>();
            if (IDs.Count == 0 || tbName.Count == 0)
            {
                return DataArrRT;
            }
            else
            {

                /*DataSet ds = new DataSet();

                for (int ind = 0; ind < tbName.Count; ind++)
                {
                    DataArrRT.Add(new List<List<string[]>>());
                    
                    string queryTbName = $"d_{tbName[ind].Substring(2)}";
                    string sqlStr = "SELECT sensor_id, " + tbName[ind] + ", dateandtime FROM( ";
                    string unionStr = " UNION ALL "; // 테이블 연결하는 것
                    string sql_tail = " )a ";

                    for (int i = 0; i < IDs.Count; i++)
                    {
                        sqlStr += $"SELECT TOP 1  {IDs[i]} AS sensor_id, {tbName[ind]}, dateandtime " +
                                    $"FROM  {queryTbName} " +
                                    $"WHERE {SensorUsageColumn[0]} = {IDs[i]} " +
                                    " ORDER BY dateandtime DESC ";
                        if (IDs.Count > 1 && i != (IDs.Count - 1)) { sqlStr += unionStr; }

                    }

                    sqlStr += sql_tail;
                    sqlStr += " ORDER BY sensor_id ;";

                    using(SqlDataAdapter da = new SqlDataAdapter(sqlStr, myConn))
                    {
                        da.Fill(ds);
                    }

                }*/


                for (int index = 0; index < tbName.Count; index++)
                {
                    string queryTableName = $"d_{tbName[index].Substring(2)}";
                    string sql_head = "SELECT sensor_id, " + tbName[index] + ", dateandtime FROM( ";
                    string sql_connector = " UNION ALL "; // 테이블 연결하는 것
                    string sql_tail = " )a ORDER BY sensor_id";

                    for (int i = 0; i < IDs.Count; i++)
                    {
                        DataArrRT[index].Add(new List<string[]>());
                        sql_head += $"SELECT TOP 1  {IDs[i]} AS sensor_id, {tbName[index]}, dateandtime " +
                                    $"FROM  {queryTableName} " +
                                    $"WHERE {SensorUsageColumn[0]} = {IDs[i]} " +
                                    " ORDER BY dateandtime DESC ";
                        if (IDs.Count > 1 && i != (IDs.Count - 1)) { sql_head += sql_connector; }
                    }
                    sql_head += sql_tail;

                    /*sql_head = "SELECT sensor_id, c_tUsage as Temperature, dateandtime FROM (" +
                                    "SELECT TOP 1 2 AS sensor_id, c_tUsage, dateandtime " +
                                    "FROM d_tUsage ORDER BY dateandtime DESC)a " +
                                "ORDER BY sensor_id";*/
                    //Console.WriteLine("SQL RT query: " + sql_head);


                    var cmd = new SqlCommand(sql_head, myConn);
                    try
                    {
                        if (myConn.State != System.Data.ConnectionState.Open)
                        {
                            myConn.Open();
                        }
                        using (var myReader = cmd.ExecuteReader())
                        {
                            int i = 0;
                            while (myReader.Read())
                            {
                                DataArrRT[index][i].Add(new string[] { myReader[tbName[index]].ToString(), myReader.GetValue(myReader.FieldCount - 1).ToString() });
                                i += 1;
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("에러 메시지:\n" + ee.ToString());
                        //MessageBox.Show(ee.Message, "에러 메시지");

                    }
                    finally
                    {
                        if (myConn.State == System.Data.ConnectionState.Open)
                        {
                            myConn.Close();
                        }

                    }
                }
            }
            return DataArrRT;
        }


        /// <summary>
        /// 주어진 SQL쿼리를 위주로 데이터를 DataSet형테로 반환함.
        /// </summary>
        /// <param name="newSqlStr">SQL쿼리문</param>
        /// <returns></returns>
        public DataSet GetValuesFromDB(string newSqlStr)
        {
            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand(newSqlStr, myConn);
            if (myConn.State != ConnectionState.Open)
            {
                myConn.Open();
            }

            da.Fill(ds);

            return ds;
        }



        /// <summary>
        /// SQL 쿼리문 생성해주는 함수.
        /// </summary>
        /// <param name="tbName"></param>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public string GetSqlQueryStrFor(List<string> tbName, List<int> IDs)
        {
            string sqlStrNew = "WITH ";
            for (int ind = 0; ind < tbName.Count; ind++)
            {
                string queryTbName = $"d_{tbName[ind].Substring(2)}";
                sqlStrNew += $" {tbName[ind]} AS ( ";

                for (int i = 0; i < IDs.Count; i++)
                {
                    sqlStrNew += $" SELECT TOP 1 t_{ind}{i}.{SensorUsageColumn[0]} AS sensor_id, t_{ind}{i}.{tbName[ind]} AS {tbName[ind]}, t_{ind}{i}.DateAndTime as DateAndTime " +
                    $" FROM {queryTbName} t_{ind}{i} WHERE t_{ind}{i}.{SensorUsageColumn[0]} = {IDs[i]} ORDER BY DateAndTime DESC ";
                    if (i != IDs.Count - 1)
                    {
                        sqlStrNew += $" UNION ALL ";
                    }
                }

                sqlStrNew += " ) ";
                if (ind != tbName.Count - 1)
                {
                    sqlStrNew += ", ";
                }
                else
                {
                    string sqlStrNew_Head = $" SELECT {tbName[0]}.sensor_id ";
                    string sqlStrNew_Joiner = "";
                    string sqlStrNew_Tail = $", {tbName[0]}.DateAndTime FROM {tbName[0]} ";
                    for (int k = 0; k < tbName.Count; k++)
                    {
                        sqlStrNew_Head += $", {tbName[k]}.{tbName[k]} ";
                        if (k > 0)
                        {
                            sqlStrNew_Joiner += $" JOIN {tbName[k]} ON {tbName[k]}.sensor_id = {tbName[k - 1]}.sensor_id ";
                        }

                    }
                    sqlStrNew += (sqlStrNew_Head + sqlStrNew_Tail + sqlStrNew_Joiner);
                }


            }
            return sqlStrNew;
        }


        /// <summary>
        /// 0, 0, 0, 5, 0, 2, 0 => (Mode = 제일 자주 나오는 값) 0을 return해주는 함수
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="tbNames"></param>
        /// <param name="sampleNumber"></param>
        /// <returns></returns>
        public DataSet GetAvgData(List<int> IDs, List<string> tbNames)
        {
            string currTime = DateTime.Now.AddSeconds(-5).ToString("yyyy-MM-dd HH:mm:ss");
            if (IDs.Count == 0 || tbNames.Count == 0)
            {
                return new DataSet();
            }
            else
            {
                using (DataSet ds = new DataSet())
                {
                    for (int ind = 0; ind < tbNames.Count; ind++)
                    {
                        string queryTbName = $"d_{tbNames[ind].Substring(2)}";
                        string sqlStr = $"SELECT sensor_id, {tbNames[ind]} FROM( ";
                        string unionStr = " UNION ALL "; // 테이블 연결하는 것
                        string sql_tail = " )a ";

                        for (int i = 0; i < IDs.Count; i++)
                        {
                            sqlStr += $"SELECT sensor_id, {tbNames[ind]} FROM( ";
                            sqlStr += $" SELECT TOP 1 {IDs[i]} AS sensor_id, {tbNames[ind]} " +
                                        $" FROM  {queryTbName} " +
                                        $" WHERE {SensorUsageColumn[0]} = {IDs[i]} AND dateandtime > '{currTime}'" +
                                        $" GROUP BY {tbNames[ind]}, {SensorUsageColumn[0]} " +
                                        $" ORDER BY COUNT(*) DESC ) a_{IDs[i]} GROUP BY sensor_id, {tbNames[ind]} ";
                            if (IDs.Count > 1 && i != (IDs.Count - 1)) { sqlStr += unionStr; }

                        }

                        sqlStr += sql_tail;
                        sqlStr += " ORDER BY sensor_id ;";
                        using (SqlConnection myConn = new SqlConnection(sqlConStr))
                        {
                            if (myConn.State != ConnectionState.Open)
                            {
                                myConn.Open();
                            }
                            using (SqlDataAdapter da = new SqlDataAdapter(sqlStr, myConn))
                            {
                                da.Fill(ds, tbNames[ind]);
                            }
                        }
                    }
                    return ds;
                }
            }
        }

        public DataSet RealTimeDataQuery(List<int> IDs, List<string> tbName)
        {
            DataSet ds = new DataSet();
            string currTime = DateTime.Now.AddSeconds(-5).ToString("yyyy-MM-dd HH:mm:ss");
            if (IDs.Count == 0 || tbName.Count == 0)
            {
                return ds;
            }
            else
            {
                for (int ind = 0; ind < tbName.Count; ind++)
                {
                    string queryTbName = $"d_{tbName[ind].Substring(2)}";
                    string sqlStr = "SELECT sensor_id, " + tbName[ind] + ", dateandtime FROM( ";
                    string unionStr = " UNION ALL "; // 테이블 연결하는 것
                    string sql_tail = " )a ";

                    for (int i = 0; i < IDs.Count; i++)
                    {
                        sqlStr += $"SELECT TOP 1  {IDs[i]} AS sensor_id, {tbName[ind]}, dateandtime " +
                                    $"FROM  {queryTbName} " +
                                    $"WHERE {SensorUsageColumn[0]} = {IDs[i]} AND dateandtime > '{currTime}' " +
                                    " ORDER BY dateandtime DESC ";
                        if (IDs.Count > 1 && i != (IDs.Count - 1)) { sqlStr += unionStr; }

                    }

                    sqlStr += sql_tail;
                    sqlStr += " ORDER BY sensor_id ;";

                    using (SqlDataAdapter da = new SqlDataAdapter(sqlStr, myConn))
                    {
                        da.Fill(ds, tbName[ind]);
                    }

                }
            }
            return ds;
        }
    }


}
