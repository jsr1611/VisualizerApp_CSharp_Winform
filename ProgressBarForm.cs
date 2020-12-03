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
    public partial class ProgressBarForm : Form
    {
        public ProgressBarForm()
        {
            InitializeComponent();
        }
    }
}







/*public void DoWork(IProgress<int> progress)
        {
            // This method is executed in the context of another thread (different than the main UI thread), so use only thread-safe code
            while(true) 
            //for (int j = 0; j < total; j++)
            {
            //    Calculate(j);
                // Use progress to notify UI thread that progress has changed
                //if (progress != null)
                  //  progress.Report((j + 1) * 100 / total);
            }
        }
        private async void progressWork()
        {
            //progressBar1.Maximum = 100;
            //progressBar1.Step = 1;

            var progress = new Progress<int>(v =>
            {
                // This lambda is executed in context of UI thread,
                // so it can safely update form controls
                //progressBar1.Value = v;
            });

            // Run operation in another thread
            await Task.Run(() => DoWork(progress));

            // TODO: Do something after all calculations
            //label_loading.Text = "DONE.";
            //this.Close();

        }*/