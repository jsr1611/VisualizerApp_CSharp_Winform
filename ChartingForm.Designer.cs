namespace VisualizerApp_3
{
    partial class ChartingForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cartesianChart1_temp = new LiveCharts.WinForms.CartesianChart();
            this.cartesianChart2_humid = new LiveCharts.WinForms.CartesianChart();
            this.cartesianChart4_part05 = new LiveCharts.WinForms.CartesianChart();
            this.cartesianChart3_part03 = new LiveCharts.WinForms.CartesianChart();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(69, 76);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(108, 21);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(69, 125);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(108, 21);
            this.textBox2.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "시작:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "끝:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gulim", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(28, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "과거 데이터";
            // 
            // cartesianChart1_temp
            // 
            this.cartesianChart1_temp.BackColor = System.Drawing.Color.Transparent;
            this.cartesianChart1_temp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cartesianChart1_temp.Location = new System.Drawing.Point(235, 28);
            this.cartesianChart1_temp.Name = "cartesianChart1_temp";
            this.cartesianChart1_temp.Size = new System.Drawing.Size(509, 273);
            this.cartesianChart1_temp.TabIndex = 5;
            this.cartesianChart1_temp.Text = "cartesianChart1_temp";
            // 
            // cartesianChart2_humid
            // 
            this.cartesianChart2_humid.BackColor = System.Drawing.Color.Transparent;
            this.cartesianChart2_humid.Location = new System.Drawing.Point(781, 28);
            this.cartesianChart2_humid.Name = "cartesianChart2_humid";
            this.cartesianChart2_humid.Size = new System.Drawing.Size(509, 273);
            this.cartesianChart2_humid.TabIndex = 6;
            this.cartesianChart2_humid.Text = "cartesianChart2_humid";
            // 
            // cartesianChart4_part05
            // 
            this.cartesianChart4_part05.BackColor = System.Drawing.Color.Transparent;
            this.cartesianChart4_part05.Location = new System.Drawing.Point(781, 340);
            this.cartesianChart4_part05.Name = "cartesianChart4_part05";
            this.cartesianChart4_part05.Size = new System.Drawing.Size(509, 273);
            this.cartesianChart4_part05.TabIndex = 8;
            this.cartesianChart4_part05.Text = "cartesianChart4_part05";
            // 
            // cartesianChart3_part03
            // 
            this.cartesianChart3_part03.BackColor = System.Drawing.Color.Transparent;
            this.cartesianChart3_part03.Location = new System.Drawing.Point(235, 340);
            this.cartesianChart3_part03.Name = "cartesianChart3_part03";
            this.cartesianChart3_part03.Size = new System.Drawing.Size(509, 273);
            this.cartesianChart3_part03.TabIndex = 7;
            this.cartesianChart3_part03.Text = "cartesianChart3_part03";
            // 
            // ChartingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 639);
            this.Controls.Add(this.cartesianChart4_part05);
            this.Controls.Add(this.cartesianChart3_part03);
            this.Controls.Add(this.cartesianChart2_humid);
            this.Controls.Add(this.cartesianChart1_temp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "ChartingForm";
            this.Text = "시각화 화면";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        internal LiveCharts.WinForms.CartesianChart cartesianChart1_temp;
        internal LiveCharts.WinForms.CartesianChart cartesianChart2_humid;
        internal LiveCharts.WinForms.CartesianChart cartesianChart4_part05;
        internal LiveCharts.WinForms.CartesianChart cartesianChart3_part03;
    }
}