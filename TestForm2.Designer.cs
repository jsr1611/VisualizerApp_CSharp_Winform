namespace VisualizerApp_3
{
    partial class TestForm2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.cartesianChart2 = new LiveCharts.WinForms.CartesianChart();
            this.cartesianChart3 = new LiveCharts.WinForms.CartesianChart();
            this.cartesianChart4 = new LiveCharts.WinForms.CartesianChart();
            this.SuspendLayout();
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Location = new System.Drawing.Point(46, 43);
            this.cartesianChart1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(608, 356);
            this.cartesianChart1.TabIndex = 0;
            this.cartesianChart1.Text = "cartesianChart1";
            // 
            // cartesianChart2
            // 
            this.cartesianChart2.Location = new System.Drawing.Point(683, 43);
            this.cartesianChart2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cartesianChart2.Name = "cartesianChart2";
            this.cartesianChart2.Size = new System.Drawing.Size(608, 356);
            this.cartesianChart2.TabIndex = 0;
            this.cartesianChart2.Text = "cartesianChart1";
            // 
            // cartesianChart3
            // 
            this.cartesianChart3.Location = new System.Drawing.Point(46, 422);
            this.cartesianChart3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cartesianChart3.Name = "cartesianChart3";
            this.cartesianChart3.Size = new System.Drawing.Size(608, 356);
            this.cartesianChart3.TabIndex = 0;
            this.cartesianChart3.Text = "cartesianChart1";
            // 
            // cartesianChart4
            // 
            this.cartesianChart4.Location = new System.Drawing.Point(683, 422);
            this.cartesianChart4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cartesianChart4.Name = "cartesianChart4";
            this.cartesianChart4.Size = new System.Drawing.Size(608, 356);
            this.cartesianChart4.TabIndex = 0;
            this.cartesianChart4.Text = "cartesianChart1";
            // 
            // TestForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1288, 824);
            this.Controls.Add(this.cartesianChart4);
            this.Controls.Add(this.cartesianChart2);
            this.Controls.Add(this.cartesianChart3);
            this.Controls.Add(this.cartesianChart1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TestForm2";
            this.Text = "TestForm2";
            this.ResumeLayout(false);

        }

        #endregion

        private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private LiveCharts.WinForms.CartesianChart cartesianChart2;
        private LiveCharts.WinForms.CartesianChart cartesianChart3;
        private LiveCharts.WinForms.CartesianChart cartesianChart4;
    }
}