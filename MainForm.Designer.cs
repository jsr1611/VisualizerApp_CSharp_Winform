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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.datePicker2_end = new System.Windows.Forms.DateTimePicker();
            this.datePicker1_start = new System.Windows.Forms.DateTimePicker();
            this.label_between = new System.Windows.Forms.Label();
            this.panel1_menu = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Column0_No = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Column1_SensorName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column2_Zone = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Column3_Location = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Column4_info = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Column5_usage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_pressure = new System.Windows.Forms.Button();
            this.button11_chartRT = new System.Windows.Forms.Button();
            this.button_particle = new System.Windows.Forms.Button();
            this.button11_numRT = new System.Windows.Forms.Button();
            this.label1_mainHeader = new System.Windows.Forms.Label();
            this.button1_datepicker = new System.Windows.Forms.Button();
            this.button1_24h = new System.Windows.Forms.Button();
            this.button1_realtime = new System.Windows.Forms.Button();
            this.panel2_ChartArea = new System.Windows.Forms.Panel();
            this.label_title_ver = new System.Windows.Forms.Label();
            this.panel_logo = new System.Windows.Forms.Panel();
            this.pictureBox_logo = new System.Windows.Forms.PictureBox();
            this.label_Title_main = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3_render = new System.Windows.Forms.Timer(this.components);
            this.panel1_menu.SuspendLayout();
            this.panel2_ChartArea.SuspendLayout();
            this.panel_logo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_logo)).BeginInit();
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
            this.panel1_menu.Controls.Add(this.listView1);
            this.panel1_menu.Controls.Add(this.button_pressure);
            this.panel1_menu.Controls.Add(this.button11_chartRT);
            this.panel1_menu.Controls.Add(this.button_particle);
            this.panel1_menu.Controls.Add(this.button11_numRT);
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
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Column0_No,
            this.Column1_SensorName,
            this.column2_Zone,
            this.Column3_Location,
            this.Column4_info,
            this.Column5_usage});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 609);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(397, 315);
            this.listView1.TabIndex = 75;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // Column0_No
            // 
            this.Column0_No.Text = "ID";
            this.Column0_No.Width = 30;
            // 
            // Column1_SensorName
            // 
            this.Column1_SensorName.Text = "센서이름";
            // 
            // column2_Zone
            // 
            this.column2_Zone.Text = "존";
            this.column2_Zone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column2_Zone.Width = 50;
            // 
            // Column3_Location
            // 
            this.Column3_Location.Text = "위치";
            this.Column3_Location.Width = 80;
            // 
            // Column4_info
            // 
            this.Column4_info.Text = "상세";
            this.Column4_info.Width = 130;
            // 
            // Column5_usage
            // 
            this.Column5_usage.Text = "수집";
            this.Column5_usage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_pressure
            // 
            this.button_pressure.FlatAppearance.BorderSize = 0;
            this.button_pressure.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_pressure.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_pressure.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button_pressure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_pressure.Image = ((System.Drawing.Image)(resources.GetObject("button_pressure.Image")));
            this.button_pressure.Location = new System.Drawing.Point(207, 212);
            this.button_pressure.Name = "button_pressure";
            this.button_pressure.Size = new System.Drawing.Size(90, 60);
            this.button_pressure.TabIndex = 1;
            this.button_pressure.Text = "차압 센서";
            this.button_pressure.UseVisualStyleBackColor = true;
            this.button_pressure.Visible = false;
            this.button_pressure.Click += new System.EventHandler(this.button_particleORpressure_Click);
            // 
            // button11_chartRT
            // 
            this.button11_chartRT.FlatAppearance.BorderSize = 0;
            this.button11_chartRT.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button11_chartRT.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button11_chartRT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button11_chartRT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11_chartRT.Image = ((System.Drawing.Image)(resources.GetObject("button11_chartRT.Image")));
            this.button11_chartRT.Location = new System.Drawing.Point(112, 157);
            this.button11_chartRT.Name = "button11_chartRT";
            this.button11_chartRT.Size = new System.Drawing.Size(90, 60);
            this.button11_chartRT.TabIndex = 1;
            this.button11_chartRT.Text = "차트형";
            this.button11_chartRT.UseVisualStyleBackColor = true;
            this.button11_chartRT.Visible = false;
            this.button11_chartRT.Click += new System.EventHandler(this.button11_Chart_Click);
            // 
            // button_particle
            // 
            this.button_particle.FlatAppearance.BorderSize = 0;
            this.button_particle.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_particle.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_particle.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button_particle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_particle.Image = ((System.Drawing.Image)(resources.GetObject("button_particle.Image")));
            this.button_particle.Location = new System.Drawing.Point(112, 212);
            this.button_particle.Name = "button_particle";
            this.button_particle.Size = new System.Drawing.Size(90, 60);
            this.button_particle.TabIndex = 0;
            this.button_particle.Text = "파티클 센서";
            this.button_particle.UseVisualStyleBackColor = true;
            this.button_particle.Visible = false;
            this.button_particle.Click += new System.EventHandler(this.button_particleORpressure_Click);
            // 
            // button11_numRT
            // 
            this.button11_numRT.FlatAppearance.BorderSize = 0;
            this.button11_numRT.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button11_numRT.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button11_numRT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button11_numRT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11_numRT.Image = ((System.Drawing.Image)(resources.GetObject("button11_numRT.Image")));
            this.button11_numRT.Location = new System.Drawing.Point(17, 157);
            this.button11_numRT.Name = "button11_numRT";
            this.button11_numRT.Size = new System.Drawing.Size(90, 60);
            this.button11_numRT.TabIndex = 0;
            this.button11_numRT.Text = "숫자형";
            this.button11_numRT.UseVisualStyleBackColor = true;
            this.button11_numRT.Visible = false;
            this.button11_numRT.Click += new System.EventHandler(this.button11_Chart_Click);
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
            this.button1_datepicker.FlatAppearance.BorderSize = 0;
            this.button1_datepicker.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button1_datepicker.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button1_datepicker.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button1_datepicker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1_datepicker.Font = new System.Drawing.Font("Gulim", 15F);
            this.button1_datepicker.Image = ((System.Drawing.Image)(resources.GetObject("button1_datepicker.Image")));
            this.button1_datepicker.Location = new System.Drawing.Point(258, 90);
            this.button1_datepicker.Name = "button1_datepicker";
            this.button1_datepicker.Size = new System.Drawing.Size(120, 65);
            this.button1_datepicker.TabIndex = 65;
            this.button1_datepicker.Text = "기간설정";
            this.button1_datepicker.UseVisualStyleBackColor = true;
            this.button1_datepicker.Click += new System.EventHandler(this.button1_Click);
            // 
            // button1_24h
            // 
            this.button1_24h.FlatAppearance.BorderSize = 0;
            this.button1_24h.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button1_24h.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button1_24h.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button1_24h.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1_24h.Font = new System.Drawing.Font("Gulim", 15F);
            this.button1_24h.Image = ((System.Drawing.Image)(resources.GetObject("button1_24h.Image")));
            this.button1_24h.Location = new System.Drawing.Point(135, 90);
            this.button1_24h.Name = "button1_24h";
            this.button1_24h.Size = new System.Drawing.Size(120, 65);
            this.button1_24h.TabIndex = 64;
            this.button1_24h.Text = "24시간";
            this.button1_24h.UseVisualStyleBackColor = true;
            this.button1_24h.Click += new System.EventHandler(this.button1_Click);
            // 
            // button1_realtime
            // 
            this.button1_realtime.FlatAppearance.BorderSize = 0;
            this.button1_realtime.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button1_realtime.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button1_realtime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1_realtime.Font = new System.Drawing.Font("Gulim", 15F);
            this.button1_realtime.Image = ((System.Drawing.Image)(resources.GetObject("button1_realtime.Image")));
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
            this.panel2_ChartArea.Controls.Add(this.label_title_ver);
            this.panel2_ChartArea.Controls.Add(this.panel_logo);
            this.panel2_ChartArea.Controls.Add(this.label_Title_main);
            this.panel2_ChartArea.Cursor = System.Windows.Forms.Cursors.Default;
            this.panel2_ChartArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2_ChartArea.Location = new System.Drawing.Point(415, 0);
            this.panel2_ChartArea.Name = "panel2_ChartArea";
            this.panel2_ChartArea.Size = new System.Drawing.Size(1127, 920);
            this.panel2_ChartArea.TabIndex = 63;
            // 
            // label_title_ver
            // 
            this.label_title_ver.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_title_ver.AutoSize = true;
            this.label_title_ver.Font = new System.Drawing.Font("Gulim", 15F, System.Drawing.FontStyle.Bold);
            this.label_title_ver.Location = new System.Drawing.Point(984, 204);
            this.label_title_ver.Name = "label_title_ver";
            this.label_title_ver.Size = new System.Drawing.Size(86, 20);
            this.label_title_ver.TabIndex = 4;
            this.label_title_ver.Text = "ver. 0.9";
            this.label_title_ver.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel_logo
            // 
            this.panel_logo.Controls.Add(this.pictureBox_logo);
            this.panel_logo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_logo.Location = new System.Drawing.Point(0, 820);
            this.panel_logo.Name = "panel_logo";
            this.panel_logo.Size = new System.Drawing.Size(1127, 100);
            this.panel_logo.TabIndex = 3;
            // 
            // pictureBox_logo
            // 
            this.pictureBox_logo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pictureBox_logo.Image = global::DataVisualizerApp.Properties.Resources.logo;
            this.pictureBox_logo.Location = new System.Drawing.Point(885, 34);
            this.pictureBox_logo.Name = "pictureBox_logo";
            this.pictureBox_logo.Size = new System.Drawing.Size(219, 44);
            this.pictureBox_logo.TabIndex = 2;
            this.pictureBox_logo.TabStop = false;
            // 
            // label_Title_main
            // 
            this.label_Title_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Title_main.AutoSize = true;
            this.label_Title_main.Font = new System.Drawing.Font("Gulim", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Title_main.Location = new System.Drawing.Point(209, 187);
            this.label_Title_main.Name = "label_Title_main";
            this.label_Title_main.Size = new System.Drawing.Size(769, 40);
            this.label_Title_main.TabIndex = 2;
            this.label_Title_main.Text = "Clean Room T.H.P. Monitoring System";
            this.label_Title_main.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // timer2
            // 
            this.timer2.Interval = 3000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3_render
            // 
            this.timer3_render.Interval = 5000;
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
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "모니터링";
            this.panel1_menu.ResumeLayout(false);
            this.panel1_menu.PerformLayout();
            this.panel2_ChartArea.ResumeLayout(false);
            this.panel2_ChartArea.PerformLayout();
            this.panel_logo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_logo)).EndInit();
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
        private System.Windows.Forms.Button button11_chartRT;
        private System.Windows.Forms.Button button11_numRT;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Column0_No;
        private System.Windows.Forms.ColumnHeader Column1_SensorName;
        private System.Windows.Forms.ColumnHeader Column3_Location;
        private System.Windows.Forms.ColumnHeader Column4_info;
        private System.Windows.Forms.ColumnHeader Column5_usage;
        private System.Windows.Forms.Label label_Title_main;
        private System.Windows.Forms.Panel panel_logo;
        private System.Windows.Forms.PictureBox pictureBox_logo;
        private System.Windows.Forms.Label label_title_ver;
        private System.Windows.Forms.ColumnHeader column2_Zone;
        public System.Windows.Forms.Timer timer2;
        public System.Windows.Forms.Timer timer3_render;
        private System.Windows.Forms.Button button_pressure;
        private System.Windows.Forms.Button button_particle;
    }
}

