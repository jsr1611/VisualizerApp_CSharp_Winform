using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public string titleName = "";
        public ScotPlot(List<List<List<string[]>>> dataArr, string whatToShow, List<int> IDs) 
        {
            InitializeComponent();

            string[] dataVal = new string[dataArr[0][0].Count];
            string[] timeVal = new string[1];
            this.SetBounds(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height - 100);
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            this.Controls.Add(tableLayoutPanel);
            List<FormsPlot> formsPlots = new List<FormsPlot>();
            string[][] dataArr2 = new string[IDs.Count][]; //dataArr[0][0].Count
            for(int i=0; i<IDs.Count; i++) { dataArr2[i] = new string[dataArr[0][0].Count]; }



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

                for (int j = 0; j < dataVal.Length; j++)
                {
                    dataArr2[i][j] = dataArr[0][i][j][0];
                    //dataVal[j] = dataArr[0][0][j][0];
                }

                timeVal[0] = dataArr[0][0][0][1];
                //if(whatToShow.Contains("temp") || whatToShow.Contains("humid")) { }
                
                double[] data = dataArr2[i].Select(x => double.Parse(x)).ToArray();
                DateTime dateTimes = DateTime.Parse(timeVal[0]);
                formsPlots[i].plt.PlotSignal(data, sampleRate: data.Length, xOffset: dateTimes.ToOADate()); //formsPlot1.
                formsPlots[i].plt.Ticks(dateTimeX: true); //formsPlot1.

                //sig.minRenderIndex = 4000;
                //sig.maxRenderIndex = 5000;
                /*sig.fillType = FillType.FillBelow;
                sig.fillColor1 = Color.Blue;
                sig.gradientFillColor1 = Color.Transparent;*/

                if (whatToShow.Contains("temp")) { titleName = "온도(°C)"; }
                else if (whatToShow.Contains("humid")) { titleName = "습도(%)"; }
                else if (whatToShow.Contains("part03")) { titleName = "파티클(0.3μm)"; }
                else { titleName = "파티클(0.5μm)"; }

                formsPlots[i].plt.Title(titleName + " 센서 데이터 시각화"); // formsPlot1.
                formsPlots[i].plt.YLabel(titleName); // formsPlot1.
                formsPlots[i].plt.XLabel("시간");
                formsPlots[i].plt.SaveFig("PlotTypes_Signal_PlotGradientFillRange" + DateTime.Now.ToString("HHmmss") + ".png");
                formsPlots[i].Render();

            }
            
        }
    }
}
