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
    public partial class StartupForm : Form
    {
        public StartupForm()
        {
            InitializeComponent();
            label_Title_startup.Left = this.Width / 2 - label_Title_startup.Width / 2;
            label_title_ver_startup.Left = label_Title_startup.Right + 10;
        }
    }
}
