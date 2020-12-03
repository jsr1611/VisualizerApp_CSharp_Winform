namespace DataVisualizerApp
{
    partial class MainForm
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
            this.datePicker2_end = new System.Windows.Forms.DateTimePicker();
            this.datePicker1_start = new System.Windows.Forms.DateTimePicker();
            this.label_between = new System.Windows.Forms.Label();
            this.panel1_menu = new System.Windows.Forms.Panel();
            this.button1_chartRT = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1_numRT = new System.Windows.Forms.Button();
            this.label1_mainHeader = new System.Windows.Forms.Label();
            this.button1_datepicker = new System.Windows.Forms.Button();
            this.button1_24h = new System.Windows.Forms.Button();
            this.button1_realtime = new System.Windows.Forms.Button();
            this.panel2_ChartArea = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3_render = new System.Windows.Forms.Timer(this.components);
            this.panel1_menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // datePicker2_end
            // 
            this.datePicker2_end.CalendarFont = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker2_end.CustomFormat = "";
            this.datePicker2_end.Font = new System.Drawing.Font("Gulim", 12F);
            this.datePicker2_end.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker2_end.Location = new System.Drawing.Point(212, 181);
            this.datePicker2_end.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.datePicker2_end.Name = "datePicker2_end";
            this.datePicker2_end.Size = new System.Drawing.Size(164, 26);
            this.datePicker2_end.TabIndex = 40;
            this.datePicker2_end.Visible = false;
            // 
            // datePicker1_start
            // 
            this.datePicker1_start.CalendarFont = new System.Drawing.Font("Gulim", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker1_start.CustomFormat = "";
            this.datePicker1_start.Font = new System.Drawing.Font("Gulim", 12F);
            this.datePicker1_start.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker1_start.Location = new System.Drawing.Point(15, 181);
            this.datePicker1_start.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.datePicker1_start.Name = "datePicker1_start";
            this.datePicker1_start.Size = new System.Drawing.Size(164, 26);
            this.datePicker1_start.TabIndex = 41;
            this.datePicker1_start.Visible = false;
            // 
            // label_between
            // 
            this.label_between.AutoSize = true;
            this.label_between.Font = new System.Drawing.Font("Gulim", 12F);
            this.label_between.Location = new System.Drawing.Point(186, 186);
            this.label_between.Name = "label_between";
            this.label_between.Size = new System.Drawing.Size(20, 16);
            this.label_between.TabIndex = 42;
            this.label_between.Text = "~";
            this.label_between.Visible = false;
            // 
            // panel1_menu
            // 
            this.panel1_menu.BackColor = System.Drawing.Color.Transparent;
            this.panel1_menu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1_menu.Controls.Add(this.button1_chartRT);
            this.panel1_menu.Controls.Add(this.comboBox1);
            this.panel1_menu.Controls.Add(this.button1_numRT);
            this.panel1_menu.Controls.Add(this.label1_mainHeader);
            this.panel1_menu.Controls.Add(this.button1_datepicker);
            this.panel1_menu.Controls.Add(this.button1_24h);
            this.panel1_menu.Controls.Add(this.button1_realtime);
            this.panel1_menu.Controls.Add(this.datePicker1_start);
            this.panel1_menu.Controls.Add(this.datePicker2_end);
            this.panel1_menu.Controls.Add(this.label_between);
            this.panel1_menu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1_menu.Location = new System.Drawing.Point(0, 0);
            this.panel1_menu.Name = "panel1_menu";
            this.panel1_menu.Size = new System.Drawing.Size(415, 920);
            this.panel1_menu.TabIndex = 62;
            // 
            // button1_chartRT
            // 
            this.button1_chartRT.Location = new System.Drawing.Point(107, 168);
            this.button1_chartRT.Name = "button1_chartRT";
            this.button1_chartRT.Size = new System.Drawing.Size(90, 40);
            this.button1_chartRT.TabIndex = 1;
            this.button1_chartRT.Text = "차트형";
            this.button1_chartRT.UseVisualStyleBackColor = true;
            this.button1_chartRT.Visible = false;
            this.button1_chartRT.Click += new System.EventHandler(this.button1_chartRT_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Gulim", 15F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Line Chart",
            "Scatter Chart",
            "Bar Chart"});
            this.comboBox1.Location = new System.Drawing.Point(15, 531);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(363, 28);
            this.comboBox1.TabIndex = 78;
            this.comboBox1.Visible = false;
            // 
            // button1_numRT
            // 
            this.button1_numRT.Location = new System.Drawing.Point(12, 168);
            this.button1_numRT.Name = "button1_numRT";
            this.button1_numRT.Size = new System.Drawing.Size(90, 40);
            this.button1_numRT.TabIndex = 0;
            this.button1_numRT.Text = "숫자형";
            this.button1_numRT.UseVisualStyleBackColor = true;
            this.button1_numRT.Visible = false;
            this.button1_numRT.Click += new System.EventHandler(this.button1_numRT_Click);
            // 
            // label1_mainHeader
            // 
            this.label1_mainHeader.AutoSize = true;
            this.label1_mainHeader.BackColor = System.Drawing.Color.Transparent;
            this.label1_mainHeader.Font = new System.Drawing.Font("Gulim", 18F, System.Drawing.FontStyle.Bold);
            this.label1_mainHeader.Location = new System.Drawing.Point(39, 36);
            this.label1_mainHeader.Name = "label1_mainHeader";
            this.label1_mainHeader.Size = new System.Drawing.Size(312, 24);
            this.label1_mainHeader.TabIndex = 74;
            this.label1_mainHeader.Text = "클린 룸 모니터링 컨트롤러";
            this.label1_mainHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1_datepicker
            // 
            this.button1_datepicker.Font = new System.Drawing.Font("Gulim", 15F);
            this.button1_datepicker.Location = new System.Drawing.Point(258, 90);
            this.button1_datepicker.Name = "button1_datepicker";
            this.button1_datepicker.Size = new System.Drawing.Size(120, 65);
            this.button1_datepicker.TabIndex = 65;
            this.button1_datepicker.Text = "기간설정";
            this.button1_datepicker.UseVisualStyleBackColor = true;
            this.button1_datepicker.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1_24h
            // 
            this.button1_24h.Font = new System.Drawing.Font("Gulim", 15F);
            this.button1_24h.Location = new System.Drawing.Point(135, 90);
            this.button1_24h.Name = "button1_24h";
            this.button1_24h.Size = new System.Drawing.Size(120, 65);
            this.button1_24h.TabIndex = 64;
            this.button1_24h.Text = "24시간";
            this.button1_24h.UseVisualStyleBackColor = true;
            this.button1_24h.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1_realtime
            // 
            this.button1_realtime.Font = new System.Drawing.Font("Gulim", 15F);
            this.button1_realtime.Location = new System.Drawing.Point(12, 90);
            this.button1_realtime.Name = "button1_realtime";
            this.button1_realtime.Size = new System.Drawing.Size(120, 65);
            this.button1_realtime.TabIndex = 63;
            this.button1_realtime.Text = "실시간";
            this.button1_realtime.UseVisualStyleBackColor = true;
            this.button1_realtime.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2_ChartArea
            // 
            this.panel2_ChartArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2_ChartArea.Location = new System.Drawing.Point(415, 0);
            this.panel2_ChartArea.Name = "panel2_ChartArea";
            this.panel2_ChartArea.Size = new System.Drawing.Size(1127, 920);
            this.panel2_ChartArea.TabIndex = 63;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3_render
            // 
            this.timer3_render.Interval = 1000;
            this.timer3_render.Tick += new System.EventHandler(this.timer3_render_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1542, 920);
            this.Controls.Add(this.panel2_ChartArea);
            this.Controls.Add(this.panel1_menu);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "모니터링";
            this.panel1_menu.ResumeLayout(false);
            this.panel1_menu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label_between;
        internal System.Windows.Forms.DateTimePicker datePicker2_end;
        internal System.Windows.Forms.DateTimePicker datePicker1_start;
        public System.Windows.Forms.Panel panel1_menu;
        private System.Windows.Forms.Button button1_datepicker;
        private System.Windows.Forms.Button button1_24h;
        private System.Windows.Forms.Button button1_realtime;
        private System.Windows.Forms.Label label1_mainHeader;
        private System.Windows.Forms.Panel panel2_ChartArea;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1_chartRT;
        private System.Windows.Forms.Button button1_numRT;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3_render;
    }
}

