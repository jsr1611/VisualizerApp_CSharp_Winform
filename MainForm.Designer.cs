namespace VisualizerApp
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
            this.label1_from = new System.Windows.Forms.Label();
            this.label2_end = new System.Windows.Forms.Label();
            this.button_show = new System.Windows.Forms.Button();
            this.panel1_menu = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel4peakVal = new System.Windows.Forms.Panel();
            this.label1_mainHeader = new System.Windows.Forms.Label();
            this.button3_solbi4 = new System.Windows.Forms.Button();
            this.button3_solbi3 = new System.Windows.Forms.Button();
            this.button3_solbi2 = new System.Windows.Forms.Button();
            this.button3_solbi1 = new System.Windows.Forms.Button();
            this.button2_part05 = new System.Windows.Forms.Button();
            this.button2_part03 = new System.Windows.Forms.Button();
            this.button2_humid = new System.Windows.Forms.Button();
            this.button2_temp = new System.Windows.Forms.Button();
            this.button1_datepicker = new System.Windows.Forms.Button();
            this.button1_24h = new System.Windows.Forms.Button();
            this.button1_realtime = new System.Windows.Forms.Button();
            this.panel2_ChartArea = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1_menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // datePicker2_end
            // 
            this.datePicker2_end.CalendarFont = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker2_end.CustomFormat = "";
            this.datePicker2_end.Font = new System.Drawing.Font("Gulim", 12F);
            this.datePicker2_end.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker2_end.Location = new System.Drawing.Point(201, 181);
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
            this.datePicker1_start.Location = new System.Drawing.Point(4, 181);
            this.datePicker1_start.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.datePicker1_start.Name = "datePicker1_start";
            this.datePicker1_start.Size = new System.Drawing.Size(164, 26);
            this.datePicker1_start.TabIndex = 41;
            this.datePicker1_start.Visible = false;
            // 
            // label1_from
            // 
            this.label1_from.AutoSize = true;
            this.label1_from.Font = new System.Drawing.Font("Gulim", 12F);
            this.label1_from.Location = new System.Drawing.Point(163, 186);
            this.label1_from.Name = "label1_from";
            this.label1_from.Size = new System.Drawing.Size(40, 16);
            this.label1_from.TabIndex = 42;
            this.label1_from.Text = "부터";
            this.label1_from.Visible = false;
            // 
            // label2_end
            // 
            this.label2_end.AutoSize = true;
            this.label2_end.Font = new System.Drawing.Font("Gulim", 12F);
            this.label2_end.Location = new System.Drawing.Point(363, 186);
            this.label2_end.Name = "label2_end";
            this.label2_end.Size = new System.Drawing.Size(40, 16);
            this.label2_end.TabIndex = 43;
            this.label2_end.Text = "까지";
            this.label2_end.Visible = false;
            // 
            // button_show
            // 
            this.button_show.Font = new System.Drawing.Font("Gulim", 15F);
            this.button_show.Location = new System.Drawing.Point(104, 461);
            this.button_show.Name = "button_show";
            this.button_show.Size = new System.Drawing.Size(182, 64);
            this.button_show.TabIndex = 44;
            this.button_show.Text = "확인";
            this.button_show.UseVisualStyleBackColor = true;
            this.button_show.Click += new System.EventHandler(this.show_button_Click);
            // 
            // panel1_menu
            // 
            this.panel1_menu.BackColor = System.Drawing.Color.Transparent;
            this.panel1_menu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1_menu.Controls.Add(this.comboBox1);
            this.panel1_menu.Controls.Add(this.panel4peakVal);
            this.panel1_menu.Controls.Add(this.label1_mainHeader);
            this.panel1_menu.Controls.Add(this.button3_solbi4);
            this.panel1_menu.Controls.Add(this.button3_solbi3);
            this.panel1_menu.Controls.Add(this.button3_solbi2);
            this.panel1_menu.Controls.Add(this.button3_solbi1);
            this.panel1_menu.Controls.Add(this.button2_part05);
            this.panel1_menu.Controls.Add(this.button2_part03);
            this.panel1_menu.Controls.Add(this.button2_humid);
            this.panel1_menu.Controls.Add(this.button2_temp);
            this.panel1_menu.Controls.Add(this.button1_datepicker);
            this.panel1_menu.Controls.Add(this.button1_24h);
            this.panel1_menu.Controls.Add(this.button1_realtime);
            this.panel1_menu.Controls.Add(this.datePicker1_start);
            this.panel1_menu.Controls.Add(this.datePicker2_end);
            this.panel1_menu.Controls.Add(this.label1_from);
            this.panel1_menu.Controls.Add(this.label2_end);
            this.panel1_menu.Controls.Add(this.button_show);
            this.panel1_menu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1_menu.Location = new System.Drawing.Point(0, 0);
            this.panel1_menu.Name = "panel1_menu";
            this.panel1_menu.Size = new System.Drawing.Size(400, 920);
            this.panel1_menu.TabIndex = 62;
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
            // 
            // panel4peakVal
            // 
            this.panel4peakVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel4peakVal.BackColor = System.Drawing.Color.Transparent;
            this.panel4peakVal.Location = new System.Drawing.Point(0, 569);
            this.panel4peakVal.Name = "panel4peakVal";
            this.panel4peakVal.Size = new System.Drawing.Size(394, 349);
            this.panel4peakVal.TabIndex = 77;
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
            // button3_solbi4
            // 
            this.button3_solbi4.Font = new System.Drawing.Font("Gulim", 15F);
            this.button3_solbi4.Location = new System.Drawing.Point(288, 364);
            this.button3_solbi4.Name = "button3_solbi4";
            this.button3_solbi4.Size = new System.Drawing.Size(90, 65);
            this.button3_solbi4.TabIndex = 73;
            this.button3_solbi4.Text = "설비 4";
            this.button3_solbi4.UseVisualStyleBackColor = true;
            this.button3_solbi4.Click += new System.EventHandler(this.button10__solbi4_Click);
            // 
            // button3_solbi3
            // 
            this.button3_solbi3.Font = new System.Drawing.Font("Gulim", 15F);
            this.button3_solbi3.Location = new System.Drawing.Point(196, 364);
            this.button3_solbi3.Name = "button3_solbi3";
            this.button3_solbi3.Size = new System.Drawing.Size(90, 65);
            this.button3_solbi3.TabIndex = 72;
            this.button3_solbi3.Text = "설비 3";
            this.button3_solbi3.UseVisualStyleBackColor = true;
            this.button3_solbi3.Click += new System.EventHandler(this.button9_solbi3_Click);
            // 
            // button3_solbi2
            // 
            this.button3_solbi2.Font = new System.Drawing.Font("Gulim", 15F);
            this.button3_solbi2.Location = new System.Drawing.Point(104, 364);
            this.button3_solbi2.Name = "button3_solbi2";
            this.button3_solbi2.Size = new System.Drawing.Size(90, 65);
            this.button3_solbi2.TabIndex = 71;
            this.button3_solbi2.Text = "설비 2";
            this.button3_solbi2.UseVisualStyleBackColor = true;
            this.button3_solbi2.Click += new System.EventHandler(this.button8_solbi2_Click);
            // 
            // button3_solbi1
            // 
            this.button3_solbi1.Font = new System.Drawing.Font("Gulim", 15F);
            this.button3_solbi1.Location = new System.Drawing.Point(12, 364);
            this.button3_solbi1.Name = "button3_solbi1";
            this.button3_solbi1.Size = new System.Drawing.Size(90, 65);
            this.button3_solbi1.TabIndex = 70;
            this.button3_solbi1.Text = "설비 1";
            this.button3_solbi1.UseVisualStyleBackColor = true;
            this.button3_solbi1.Click += new System.EventHandler(this.button7_solbi1_Click);
            // 
            // button2_part05
            // 
            this.button2_part05.Font = new System.Drawing.Font("Gulim", 15F);
            this.button2_part05.Location = new System.Drawing.Point(288, 238);
            this.button2_part05.Name = "button2_part05";
            this.button2_part05.Size = new System.Drawing.Size(90, 65);
            this.button2_part05.TabIndex = 69;
            this.button2_part05.Text = "파티클(0.5)";
            this.button2_part05.UseVisualStyleBackColor = true;
            this.button2_part05.Click += new System.EventHandler(this.button6_part05_Click);
            // 
            // button2_part03
            // 
            this.button2_part03.Font = new System.Drawing.Font("Gulim", 15F);
            this.button2_part03.Location = new System.Drawing.Point(196, 238);
            this.button2_part03.Name = "button2_part03";
            this.button2_part03.Size = new System.Drawing.Size(90, 65);
            this.button2_part03.TabIndex = 68;
            this.button2_part03.Text = "파티클(0.3)";
            this.button2_part03.UseVisualStyleBackColor = true;
            this.button2_part03.Click += new System.EventHandler(this.button6_part03_Click);
            // 
            // button2_humid
            // 
            this.button2_humid.Font = new System.Drawing.Font("Gulim", 15F);
            this.button2_humid.Location = new System.Drawing.Point(104, 238);
            this.button2_humid.Name = "button2_humid";
            this.button2_humid.Size = new System.Drawing.Size(90, 65);
            this.button2_humid.TabIndex = 67;
            this.button2_humid.Text = "습도";
            this.button2_humid.UseVisualStyleBackColor = true;
            this.button2_humid.Click += new System.EventHandler(this.button5_humid_Click);
            // 
            // button2_temp
            // 
            this.button2_temp.Font = new System.Drawing.Font("Gulim", 15F);
            this.button2_temp.Location = new System.Drawing.Point(12, 238);
            this.button2_temp.Name = "button2_temp";
            this.button2_temp.Size = new System.Drawing.Size(90, 65);
            this.button2_temp.TabIndex = 66;
            this.button2_temp.Text = "온도";
            this.button2_temp.UseVisualStyleBackColor = true;
            this.button2_temp.Click += new System.EventHandler(this.button4_temp_Click);
            // 
            // button1_datepicker
            // 
            this.button1_datepicker.Font = new System.Drawing.Font("Gulim", 15F);
            this.button1_datepicker.Location = new System.Drawing.Point(258, 93);
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
            this.button1_24h.Location = new System.Drawing.Point(135, 93);
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
            this.button1_realtime.Location = new System.Drawing.Point(12, 93);
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
            this.panel2_ChartArea.Location = new System.Drawing.Point(400, 0);
            this.panel2_ChartArea.Name = "panel2_ChartArea";
            this.panel2_ChartArea.Size = new System.Drawing.Size(1142, 920);
            this.panel2_ChartArea.TabIndex = 63;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
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
            this.Text = "메인화면";
            this.panel1_menu.ResumeLayout(false);
            this.panel1_menu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1_from;
        private System.Windows.Forms.Label label2_end;
        private System.Windows.Forms.Button button_show;
        internal System.Windows.Forms.DateTimePicker datePicker2_end;
        internal System.Windows.Forms.DateTimePicker datePicker1_start;
        public System.Windows.Forms.Panel panel1_menu;
        private System.Windows.Forms.Button button1_datepicker;
        private System.Windows.Forms.Button button1_24h;
        private System.Windows.Forms.Button button1_realtime;
        private System.Windows.Forms.Button button2_part05;
        private System.Windows.Forms.Button button2_part03;
        private System.Windows.Forms.Button button2_humid;
        private System.Windows.Forms.Button button2_temp;
        private System.Windows.Forms.Button button3_solbi4;
        private System.Windows.Forms.Button button3_solbi3;
        private System.Windows.Forms.Button button3_solbi2;
        private System.Windows.Forms.Button button3_solbi1;
        private System.Windows.Forms.Label label1_mainHeader;
        private System.Windows.Forms.Panel panel2_ChartArea;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel4peakVal;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

