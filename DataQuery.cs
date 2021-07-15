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
        /// <summary>
        /// 데이터저장되는 테이블의 Column명들의 리스트
        /// [0]: DateAndTime, [1]: sDateTime, [2]: sID, [3]: sCode, [4]: sDataValue, [5]:Remarks
        /// </summary>
        public List<string> dataTableColumns { get; set; }
        public string dataTable { get; set; }


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

        public System.Data.DataSet GetValuesFromDB(string startTime, string endTime, List<string> tbNames, List<int> IDs)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            System.Data.DataSet ds = new System.Data.DataSet();

            string data_type = $"int";

            for (int ind = 0; ind < tbNames.Count; ind++)
            {

                if (tbNames[ind].Equals(SensorUsageColumn[1]) || tbNames[ind].Equals(SensorUsageColumn[2]))
                {
                    data_type = "decimal(10,2)";
                }
                else
                {
                    data_type = $"int";
                }

                string sql_head = $"SELECT {dataTableColumns[1]},{dataTableColumns[2]}, CAST({dataTableColumns[4]} AS {data_type}) AS {dataTableColumns[4]} " +
                    $" FROM [{dbName}].[dbo].[{dataTable}] WHERE {dataTableColumns[3]} = '{tbNames[ind]}' " +
                    $" AND {dataTableColumns[1]} BETWEEN '{startTime}' AND '{endTime}'" +
                    $" AND {dataTableColumns[2]} IN ({string.Join(",", IDs.ToArray())}) ORDER BY {dataTableColumns[2]} ASC";

                Console.WriteLine("\n\n\n" + sql_head);

                try
                {
                    if (myConn.State != System.Data.ConnectionState.Open)
                    {
                        myConn.Open();
                    }
                    da.SelectCommand = new SqlCommand(sql_head, myConn);

                    ///conn.Open();
                    da.Fill(ds, tbNames[ind]);
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
            string currTime = DateTime.Now.AddMinutes(AvgLimitTime).ToString("yyyy-MM-dd HH:mm:ss.fff");
            if (IDs.Count == 0 || tbNames.Count == 0)
            {
                return new DataSet();
            }
            else
            {
                using (DataSet ds = new DataSet())
                {


                    SqlTransaction transaction;
                    string queryTbName = $"{dbName[0]}_DATATABLE";
                    string data_type = $"int";
                    for (int ind = 0; ind < tbNames.Count; ind++)
                    {
                        
                        if (tbNames[ind].Equals(SensorUsageColumn[1]) || tbNames[ind].Equals(SensorUsageColumn[2]))
                        {
                            data_type = "decimal(10,2)";
                        }
                        else
                        {
                            data_type = $"int";
                        }
                        string sqlStr = $"SELECT CAST(AVG(CAST({dataTableColumns[4]} AS {data_type})) AS {data_type}) AS {dataTableColumns[4]}, {dataTableColumns[2]} " +
                            $" FROM [{dbName}].[dbo].[{queryTbName}] " +
                            $" WHERE {dataTableColumns[1]} > DATEADD(MI, {AvgLimitTime}, GETDATE()) " +
                            $" AND {dataTableColumns[3]} = '{tbNames[ind]}' " +
                            $" AND {dataTableColumns[2]} IN ({string.Join(",", IDs.ToArray())}) GROUP BY {dataTableColumns[2]} ORDER BY {dataTableColumns[2]};";


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

            string currTime = DateTime.Now.AddSeconds(RTLimitTime).ToString("yyyy-MM-dd HH:mm:ss.fff");
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
                            string data_type = $"int";

                            for (int ind = 0; ind < tbNames.Count; ind++)
                            {
                                try
                                {
                                    SqlTransaction RTDQ_transaction = conn.BeginTransaction();
                                    string queryTbName = $"{dbName[0]}_DATATABLE";
                                    //string queryTbName = $"{dbName[0]}_DATATABLE";
                                    if (tbNames[ind].Equals(SensorUsageColumn[1]) || tbNames[ind].Equals(SensorUsageColumn[2]))
                                    {
                                        data_type = "decimal(10,2)";
                                    }
                                    else
                                    {
                                        data_type = $"int";
                                    }


                                    sqlStr = $"SELECT * FROM(SELECT TOP {IDs.Count} {dataTableColumns[1]},{dataTableColumns[2]}, CAST({dataTableColumns[4]} AS {data_type}) AS {dataTableColumns[4]} " +
                                        $"FROM [{dbName}].[dbo].[{queryTbName}] WHERE {dataTableColumns[3]} = '{tbNames[ind]}' and {dataTableColumns[1]} > DATEADD(SS, {RTLimitTime}, GETDATE()) AND {dataTableColumns[2]} IN ({string.Join(",", IDs.ToArray())}) " +
                                        $" ORDER BY {dataTableColumns[1]} DESC) a ORDER BY a.{dataTableColumns[2]};";
                                   
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
