namespace VisualizerApp_3
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1_temp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2_humid = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox4_part05 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox3_part03 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.datePicker2_end = new System.Windows.Forms.DateTimePicker();
            this.datePicker1_start = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button_show = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(704, 60);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gulim", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(153, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(323, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Clean Room Monitoring Dashboard";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textBox1_temp
            // 
            this.textBox1_temp.Location = new System.Drawing.Point(125, 114);
            this.textBox1_temp.Name = "textBox1_temp";
            this.textBox1_temp.Size = new System.Drawing.Size(100, 21);
            this.textBox1_temp.TabIndex = 2;
            this.textBox1_temp.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Temperature(C)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(12, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(197, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Sensor Data Real-Time";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Humidity(%)";
            // 
            // textBox2_humid
            // 
            this.textBox2_humid.Location = new System.Drawing.Point(125, 153);
            this.textBox2_humid.Name = "textBox2_humid";
            this.textBox2_humid.Size = new System.Drawing.Size(100, 21);
            this.textBox2_humid.TabIndex = 5;
            this.textBox2_humid.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "Particle(0.5um)";
            // 
            // textBox4_part05
            // 
            this.textBox4_part05.Location = new System.Drawing.Point(125, 233);
            this.textBox4_part05.Name = "textBox4_part05";
            this.textBox4_part05.Size = new System.Drawing.Size(100, 21);
            this.textBox4_part05.TabIndex = 9;
            this.textBox4_part05.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 198);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "Particle(0.3um)";
            // 
            // textBox3_part03
            // 
            this.textBox3_part03.Location = new System.Drawing.Point(125, 194);
            this.textBox3_part03.Name = "textBox3_part03";
            this.textBox3_part03.Size = new System.Drawing.Size(100, 21);
            this.textBox3_part03.TabIndex = 7;
            this.textBox3_part03.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(344, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(184, 16);
            this.label7.TabIndex = 11;
            this.label7.Text = "Check Hisotircal Data";
            // 
            // datePicker2_end
            // 
            this.datePicker2_end.CalendarFont = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker2_end.CustomFormat = "";
            this.datePicker2_end.Font = new System.Drawing.Font("Gulim", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker2_end.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker2_end.Location = new System.Drawing.Point(539, 118);
            this.datePicker2_end.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.datePicker2_end.Name = "datePicker2_end";
            this.datePicker2_end.Size = new System.Drawing.Size(132, 20);
            this.datePicker2_end.TabIndex = 40;
            // 
            // datePicker1_start
            // 
            this.datePicker1_start.CalendarFont = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker1_start.CustomFormat = "";
            this.datePicker1_start.Font = new System.Drawing.Font("Gulim", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker1_start.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker1_start.Location = new System.Drawing.Point(357, 118);
            this.datePicker1_start.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.datePicker1_start.Name = "datePicker1_start";
            this.datePicker1_start.Size = new System.Drawing.Size(132, 20);
            this.datePicker1_start.TabIndex = 41;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(314, 123);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 12);
            this.label8.TabIndex = 42;
            this.label8.Text = "From";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(513, 123);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 12);
            this.label9.TabIndex = 43;
            this.label9.Text = "To";
            // 
            // button_show
            // 
            this.button_show.Location = new System.Drawing.Point(470, 169);
            this.button_show.Name = "button_show";
            this.button_show.Size = new System.Drawing.Size(75, 23);
            this.button_show.TabIndex = 44;
            this.button_show.Text = "Show";
            this.button_show.UseVisualStyleBackColor = true;
            this.button_show.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 289);
            this.Controls.Add(this.button_show);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.datePicker2_end);
            this.Controls.Add(this.datePicker1_start);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox4_part05);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox3_part03);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox2_humid);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1_temp);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Main window";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1_temp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2_humid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox4_part05;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox3_part03;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button_show;
        internal System.Windows.Forms.DateTimePicker datePicker2_end;
        internal System.Windows.Forms.DateTimePicker datePicker1_start;
    }
}

