using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataVisualizerApp
{
    class DataQuery
    {

        public string dbServerAddress = "127.0.0.1";    //"10.1.55.174";
        public string dbName = "SensorDataDB";
        public string dbUID = "dlitdb01";
        public string dbPWD = "dlitdb01";
        public string connectionTimeout = "180";

        /// <summary>
        /// 데이터 쿼리 함수
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<List<List<string[]>>> DBQuery(string startDate, string endDate, List<int> IDs, List<string> whatToQuery)
        {

            

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

        public System.Data.DataSet GetTempValues()
        {
            SqlConnection myConnection = new SqlConnection($@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20");

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SELECT sensor_id, Temperature, dateandtime FROM( SELECT 1 AS sensor_id, AVG(CAST(Temperature AS DECIMAL(18, 2))) AS Temperature, SUBSTRING(dateandtime, 1,16) AS dateandtime FROM dev_temp_1 WHERE dateandtime BETWEEN '2020-01-07 12:29' AND '2021-01-08 12:29' GROUP BY SUBSTRING(dateandtime, 1, 16) UNION ALL SELECT 2 AS sensor_id, AVG(CAST(Temperature AS DECIMAL(18, 2))) AS Temperature, SUBSTRING(dateandtime, 1,16) AS dateandtime FROM dev_temp_2 WHERE dateandtime BETWEEN '2020-01-07 12:29' AND '2021-01-08 12:29' GROUP BY SUBSTRING(dateandtime, 1, 16) UNION ALL SELECT 3 AS sensor_id, AVG(CAST(Temperature AS DECIMAL(18, 2))) AS Temperature, SUBSTRING(dateandtime, 1,16) AS dateandtime FROM dev_temp_3 WHERE dateandtime BETWEEN '2020-01-07 12:29' AND '2021-01-08 12:29' GROUP BY SUBSTRING(dateandtime, 1, 16) )a ORDER BY dateandtime", myConnection);
            System.Data.DataSet ds = new System.Data.DataSet();

            ///conn.Open();
            da.Fill(ds);
            return ds;
        }


        /// <summary>
        /// sql쿼리문 입력 시 해당하는 데이터를 ds(dataset) 형태로 반환함
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public System.Data.DataSet GetValues(string sqlStr)
        {
            SqlConnection myConnection = new SqlConnection($@"Data Source={dbServerAddress};Connection Timeout={connectionTimeout};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20");

            /*string SQL_query = "SELECT sensor_id, " + whatToQuery + ", dateandtime " +
                "FROM( " +
                    "SELECT 1 AS sensor_id, " +
                    "AVG(CAST(" + whatToQuery + " AS DECIMAL(18, 2))) AS " + whatToQuery + "" +
                    ", SUBSTRING(dateandtime, 1,16) AS dateandtime " +
                    "FROM dev_temp_1 " +
                    "WHERE dateandtime BETWEEN '" + startTime + "' AND '" + endTime + "' " +
                    "GROUP BY SUBSTRING(dateandtime, 1, 16) " +
                "UNION ALL " +
                    "SELECT 2 AS sensor_id, " +
                    "AVG(CAST(" + whatToQuery + " AS DECIMAL(18, 2))) AS " + whatToQuery + "" +
                    ", SUBSTRING(dateandtime, 1,16) AS dateandtime " +
                    "FROM dev_temp_2 " +
                    "WHERE dateandtime BETWEEN '" + startTime + "' AND '" + endTime + "' " +
                    "GROUP BY SUBSTRING(dateandtime, 1, 16) " +
                "UNION ALL " +
                    "SELECT 3 AS sensor_id, " +
                    "AVG(CAST(" + whatToQuery + " AS DECIMAL(18, 2))) AS " + whatToQuery + "" +
                    ", SUBSTRING(dateandtime, 1,16) AS dateandtime " +
                    "FROM dev_temp_3 " +
                    "WHERE dateandtime BETWEEN '" + startTime + "' AND '" + endTime + "' " +
                    "GROUP BY SUBSTRING(dateandtime, 1, 16) )a " +
                "ORDER BY dateandtime";*/

            
            SqlDataAdapter da = new SqlDataAdapter();
            //da.SelectCommand.CommandTimeout = 180;
            da.SelectCommand = new SqlCommand(sqlStr, myConnection);

            System.Data.DataSet ds = new System.Data.DataSet();

            ///conn.Open();
            da.Fill(ds);
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
                    SqlConnection myConnection = new SqlConnection($@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20");

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = new SqlCommand(sql_head, myConnection);
                    System.Data.DataSet ds = new System.Data.DataSet();

                    ///conn.Open();
                    da.Fill(ds);


                    using (var cmd = new SqlCommand(sql_head, myConnection))
                    {
                        cmd.CommandTimeout = 0;
                        myConnection.Open();
                        Console.WriteLine("Connection opened");
                        using (var myReader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("Executing Reader() method...");
                            int i_sensorID = 0;
                            
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
                        myConnection.Close();
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
