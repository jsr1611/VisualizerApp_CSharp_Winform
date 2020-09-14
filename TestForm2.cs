using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts;

namespace VisualizerApp_3
{
    public partial class TestForm2 : Form
    {
        public TestForm2()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new ScatterSeries
                {
                    Title = "Series C",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometry = DefaultGeometries.Triangle,
                    StrokeThickness = 2,
                    Fill = System.Windows.Media.Brushes.Transparent
                }
            };
            /*
            ,
                new ScatterSeries
                {
                    Title = "Series B",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometry = DefaultGeometries.Diamond
                },*/
            var r = new Random();

            foreach (var series in SeriesCollection)
            {
                for (var i = 0; i < 20; i++)
                {
                    series.Values.Add(new ObservablePoint(r.NextDouble() * 10, r.NextDouble() * 10));
                }
            }

            cartesianChart1.Series = SeriesCollection;
            cartesianChart1.LegendLocation = LegendLocation.Bottom;
        }

        public SeriesCollection SeriesCollection { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            var r = new Random();

            foreach (var values in SeriesCollection.Select(x => x.Values))
            {
                for (var i = 0; i < 20; i++)
                {
                    ((ObservablePoint)values[i]).X = r.NextDouble() * 10;
                    ((ObservablePoint)values[i]).Y = r.NextDouble() * 10;
                }
            }

        }
    
    }
}
