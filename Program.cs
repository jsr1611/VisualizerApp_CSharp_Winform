using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataVisualizerApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            /*Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Start();
            Application.Run(new StartupForm());
            timer.Stop();
            */
            Application.Run(new MainForm());
        }
    }
}
