using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ParticleDataVisualizerApp
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

        public int RTLimitTime { get; set; }
        public int AvgLimitTime { get; set; }


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


        /// <summary>
        /// 0, 0, 0, 5, 0, 2, 0 => (Mode = 제일 자주 나오는 값) 0을 return해주는 함수
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="tbNames"></param>
        /// <param name="sampleNumber"></param>
        /// <returns></returns>
        public DataSet GetAvgData(List<int> IDs, List<string> tbNames)
        {
            string currTime = DateTime.Now.AddMinutes(-AvgLimitTime).ToString("yyyy-MM-dd HH:mm:ss");
            if (IDs.Count == 0 || tbNames.Count == 0)
            {
                return new DataSet();
            }
            else
            {
                using (DataSet ds = new DataSet())
                {


                    SqlTransaction transaction;

                    for (int ind = 0; ind < tbNames.Count; ind++)
                    {
                        string queryTbName = $"d_{tbNames[ind].Substring(2)}";
                        string sqlStr = $"SELECT sensor_id, AVG(CONVERT(int, {tbNames[ind]})) AS {tbNames[ind]}  FROM( ";
                        string unionStr = " UNION ALL "; // 테이블 연결하는 것

                        for (int i = 0; i < IDs.Count; i++)
                        {

                            sqlStr += $" SELECT {SensorUsageColumn[0]} AS sensor_id, {tbNames[ind]} " +
                                        $" FROM  {queryTbName} WHERE {SensorUsageColumn[0]} = {IDs[i]} AND dateandtime > '{currTime}'";
                            if (!tbNames[ind].Equals(SensorUsageColumn[1]) && !tbNames[ind].Equals(SensorUsageColumn[2]))
                            {
                                sqlStr += $" AND {tbNames[ind]} > 0 ";
                            }
                            if (IDs.Count > 1 && i != (IDs.Count - 1)) { sqlStr += unionStr; }

                        }

                        sqlStr += $" )a GROUP BY sensor_id " +
                        " ORDER BY sensor_id ;";

                        try
                        {
                            using (SqlConnection myConn = new SqlConnection(sqlConStr))
                            {
                                myConn.Open();
                                transaction = myConn.BeginTransaction();
                                using (SqlDataAdapter da = new SqlDataAdapter(sqlStr, myConn))
                                {
                                    da.SelectCommand.Transaction = transaction;
                                    da.Fill(ds, tbNames[ind]);
                                    transaction.Commit();
                                }
                            }
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            Console.WriteLine("Exception occurred:\n" + ex.Message);

                        }
                    }
                    return ds;
                }
            }
        }

        public DataSet RealTimeDataQuery(List<int> IDs, List<string> tbNames)
        {

            string currTime = DateTime.Now.AddSeconds(-RTLimitTime).ToString("yyyy-MM-dd HH:mm:ss");
            if (IDs.Count == 0 || tbNames.Count == 0)
            {
                return new DataSet();
            }
            else
            {
                using (DataSet ds = new DataSet())
                {
                    string sqlStr = "";
                    using (SqlConnection conn = new SqlConnection(sqlConStr))
                    {
                        try
                        {
                            conn.Open();

                            for (int ind = 0; ind < tbNames.Count; ind++)
                            {
                                try
                                {
                                    SqlTransaction RTDQ_transaction = conn.BeginTransaction();
                                    string queryTbName = $"d_{tbNames[ind].Substring(2)}";
                                    sqlStr = "SELECT sensor_id, " + tbNames[ind] + ", dateandtime FROM( ";
                                    string unionStr = " UNION ALL "; // 테이블 연결하는 것
                                    string sql_tail = " )a ";

                                    for (int i = 0; i < IDs.Count; i++)
                                    {
                                        sqlStr += $"SELECT TOP 1 {SensorUsageColumn[0]} AS sensor_id, {tbNames[ind]}, dateandtime " +
                                                    $"FROM  {queryTbName} " +
                                                    $"WHERE {SensorUsageColumn[0]} = {IDs[i]} AND dateandtime > '{currTime}' ";
                                        /*if (!tbNames[ind].Equals(SensorUsageColumn[1]) && !tbNames[ind].Equals(SensorUsageColumn[2]))
                                        {
                                            sqlStr += $" AND {tbNames[ind]} > 0 ";
                                        }*/

                                        if (IDs.Count > 1 && i != (IDs.Count - 1)) { sqlStr += unionStr; }

                                    }

                                    sqlStr += sql_tail;
                                    sqlStr += " ORDER BY sensor_id ;";

                                    using (SqlDataAdapter sda = new SqlDataAdapter(sqlStr, conn))
                                    {
                                        sda.SelectCommand.Transaction = RTDQ_transaction;
                                        sda.Fill(ds, tbNames[ind]);
                                        RTDQ_transaction.Commit();
                                    }
                                    RTDQ_transaction.Dispose();
                                }
                                catch (System.Exception ex)
                                {
                                    Console.WriteLine("Exception occurred:\n" + ex.Message + " " + ex.StackTrace);
                                    Console.WriteLine($"SqlQuery skipped:\n{sqlStr}\n");
                                }
                            }

                        }
                        catch (Exception ex2)
                        {
                            Console.WriteLine($"DB Connection Error: {ex2.Message}. {ex2.StackTrace}");
                         
                        }

                    }
                    return ds;
                }
            }
        }
    }


}
