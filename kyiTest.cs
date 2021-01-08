using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataVisualizerApp
{
    public partial class kyiTest : Form
    {

        public kyiTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataQuery myDataQuery = new DataQuery();
            List<string> MyDataTypes = new List<string>() { "Temperature", "Humidity", "Particle03", "Particle05" };
            List<int> MyIDs = new List<int>() { 1, 3 };
            DataQuery dataQuery = new DataQuery();
            
            //System.Data.DataSet GetValues(string startTime, string endTime, string whatToQuery, List<int> IDs)
            //List<>
            List<ScottPlot.FormsPlot> formsPlots = new List<ScottPlot.FormsPlot>() { formsPlot1, formsPlot2, formsPlot3, formsPlot4 };

            for (int j = 0; j < MyDataTypes.Count; j++)
            {
                System.Data.DataSet ds = dataQuery.GetValues("", "", MyDataTypes[j], MyIDs);
                for (int i = 0; i < MyIDs.Count; i++)
                {
                    double[] xs = ds.Tables[0].AsEnumerable().Where(r => r.Field<int>("sensor_id") == MyIDs[i]).Select(r => Convert.ToDateTime(r.Field<string>("dateandtime")).ToOADate()).ToArray();

                    double[] ys = ds.Tables[0].AsEnumerable().Where(r => r.Field<int>("sensor_id") == MyIDs[i]).Select(r => Convert.ToDouble(r.Field<decimal>(MyDataTypes[j]))).ToArray();
                    //formsPlot1.plt.PlotScatter(xs, ys);
                    formsPlots[j].plt.PlotSignalXYConst(xs, ys, label: MyDataTypes[j] + MyIDs[i]); // Btn3_SensorLocation[MyIDs[index_ID] - 1].Text, color: colorset[index_ID]             // Signal Chart
                    
                    
                }
                formsPlots[j].plt.Ticks(dateTimeX: true);
                formsPlots[j].plt.Legend();
                formsPlots[j].Render();
            }
            
        }



    }
}
