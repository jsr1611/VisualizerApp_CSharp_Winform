namespace VisualizerApp_3
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.datePicker2_end = new System.Windows.Forms.DateTimePicker();
            this.datePicker1_start = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button_show = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1_1H = new System.Windows.Forms.LinkLabel();
            this.linkLabel2_24H = new System.Windows.Forms.LinkLabel();
            this.linkLabel3_1w = new System.Windows.Forms.LinkLabel();
            this.linkLabel4_1m = new System.Windows.Forms.LinkLabel();
            this.checkedListBox1_DevicesList = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel0_30m = new System.Windows.Forms.LinkLabel();
            this.linkLabel5_RT = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 75);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gulim", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(89, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(363, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Clean Room 모니터링 대시보드";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Gulim", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(29, 109);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(302, 19);
            this.label7.TabIndex = 11;
            this.label7.Text = "1. 시각화 하려는 시간 간격 선택";
            // 
            // datePicker2_end
            // 
            this.datePicker2_end.CalendarFont = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker2_end.CustomFormat = "";
            this.datePicker2_end.Font = new System.Drawing.Font("Gulim", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker2_end.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker2_end.Location = new System.Drawing.Point(289, 191);
            this.datePicker2_end.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.datePicker2_end.Name = "datePicker2_end";
            this.datePicker2_end.Size = new System.Drawing.Size(190, 24);
            this.datePicker2_end.TabIndex = 40;
            // 
            // datePicker1_start
            // 
            this.datePicker1_start.CalendarFont = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker1_start.CustomFormat = "";
            this.datePicker1_start.Font = new System.Drawing.Font("Gulim", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker1_start.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker1_start.Location = new System.Drawing.Point(32, 191);
            this.datePicker1_start.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.datePicker1_start.Name = "datePicker1_start";
            this.datePicker1_start.Size = new System.Drawing.Size(190, 24);
            this.datePicker1_start.TabIndex = 41;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Gulim", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(233, 198);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 15);
            this.label8.TabIndex = 42;
            this.label8.Text = "부터";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Gulim", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(503, 198);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 15);
            this.label9.TabIndex = 43;
            this.label9.Text = "까지";
            // 
            // button_show
            // 
            this.button_show.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_show.Location = new System.Drawing.Point(32, 462);
            this.button_show.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_show.Name = "button_show";
            this.button_show.Size = new System.Drawing.Size(159, 59);
            this.button_show.TabIndex = 44;
            this.button_show.Text = "보기";
            this.button_show.UseVisualStyleBackColor = true;
            this.button_show.Click += new System.EventHandler(this.show_button_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton1.Location = new System.Drawing.Point(32, 299);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(58, 20);
            this.radioButton1.TabIndex = 47;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "온도";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton2.Location = new System.Drawing.Point(32, 326);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(58, 20);
            this.radioButton2.TabIndex = 48;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "습도";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton3.Location = new System.Drawing.Point(32, 354);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(87, 20);
            this.radioButton3.TabIndex = 49;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "파티클 1";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton4.Location = new System.Drawing.Point(32, 381);
            this.radioButton4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(87, 20);
            this.radioButton4.TabIndex = 50;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "파티클 2";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton5.Location = new System.Drawing.Point(32, 409);
            this.radioButton5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(58, 20);
            this.radioButton5.TabIndex = 51;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "모두";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Gulim", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(29, 256);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 19);
            this.label2.TabIndex = 52;
            this.label2.Text = "2. 구분 선택";
            // 
            // linkLabel1_1H
            // 
            this.linkLabel1_1H.AutoSize = true;
            this.linkLabel1_1H.Font = new System.Drawing.Font("Gulim", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.linkLabel1_1H.Location = new System.Drawing.Point(112, 155);
            this.linkLabel1_1H.Name = "linkLabel1_1H";
            this.linkLabel1_1H.Size = new System.Drawing.Size(80, 15);
            this.linkLabel1_1H.TabIndex = 54;
            this.linkLabel1_1H.TabStop = true;
            this.linkLabel1_1H.Text = "최근 1시간";
            this.linkLabel1_1H.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_1H_LinkClicked);
            // 
            // linkLabel2_24H
            // 
            this.linkLabel2_24H.AutoSize = true;
            this.linkLabel2_24H.Font = new System.Drawing.Font("Gulim", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.linkLabel2_24H.Location = new System.Drawing.Point(205, 155);
            this.linkLabel2_24H.Name = "linkLabel2_24H";
            this.linkLabel2_24H.Size = new System.Drawing.Size(88, 15);
            this.linkLabel2_24H.TabIndex = 55;
            this.linkLabel2_24H.TabStop = true;
            this.linkLabel2_24H.Text = "최근 24시간";
            this.linkLabel2_24H.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_24H_LinkClicked);
            // 
            // linkLabel3_1w
            // 
            this.linkLabel3_1w.AutoSize = true;
            this.linkLabel3_1w.Font = new System.Drawing.Font("Gulim", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.linkLabel3_1w.Location = new System.Drawing.Point(304, 155);
            this.linkLabel3_1w.Name = "linkLabel3_1w";
            this.linkLabel3_1w.Size = new System.Drawing.Size(80, 15);
            this.linkLabel3_1w.TabIndex = 56;
            this.linkLabel3_1w.TabStop = true;
            this.linkLabel3_1w.Text = "최근 1주일";
            this.linkLabel3_1w.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_1w_LinkClicked);
            // 
            // linkLabel4_1m
            // 
            this.linkLabel4_1m.AutoSize = true;
            this.linkLabel4_1m.Font = new System.Drawing.Font("Gulim", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.linkLabel4_1m.Location = new System.Drawing.Point(397, 155);
            this.linkLabel4_1m.Name = "linkLabel4_1m";
            this.linkLabel4_1m.Size = new System.Drawing.Size(80, 15);
            this.linkLabel4_1m.TabIndex = 57;
            this.linkLabel4_1m.TabStop = true;
            this.linkLabel4_1m.Text = "최근 1개월";
            this.linkLabel4_1m.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_1m_LinkClicked);
            // 
            // checkedListBox1_DevicesList
            // 
            this.checkedListBox1_DevicesList.CheckOnClick = true;
            this.checkedListBox1_DevicesList.FormattingEnabled = true;
            this.checkedListBox1_DevicesList.Location = new System.Drawing.Point(673, 189);
            this.checkedListBox1_DevicesList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkedListBox1_DevicesList.Name = "checkedListBox1_DevicesList";
            this.checkedListBox1_DevicesList.Size = new System.Drawing.Size(217, 284);
            this.checkedListBox1_DevicesList.TabIndex = 59;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Gulim", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(649, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(255, 19);
            this.label3.TabIndex = 60;
            this.label3.Text = "3. 시각화 하려는 센서 선택";
            // 
            // linkLabel0_30m
            // 
            this.linkLabel0_30m.AutoSize = true;
            this.linkLabel0_30m.Font = new System.Drawing.Font("Gulim", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.linkLabel0_30m.Location = new System.Drawing.Point(30, 155);
            this.linkLabel0_30m.Name = "linkLabel0_30m";
            this.linkLabel0_30m.Size = new System.Drawing.Size(73, 15);
            this.linkLabel0_30m.TabIndex = 58;
            this.linkLabel0_30m.TabStop = true;
            this.linkLabel0_30m.Text = "최근 30분";
            this.linkLabel0_30m.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel0_30m_LinkClicked);
            // 
            // linkLabel5_RT
            // 
            this.linkLabel5_RT.AutoSize = true;
            this.linkLabel5_RT.Font = new System.Drawing.Font("Gulim", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.linkLabel5_RT.Location = new System.Drawing.Point(488, 155);
            this.linkLabel5_RT.Name = "linkLabel5_RT";
            this.linkLabel5_RT.Size = new System.Drawing.Size(52, 15);
            this.linkLabel5_RT.TabIndex = 61;
            this.linkLabel5_RT.TabStop = true;
            this.linkLabel5_RT.Text = "실시간";
            this.linkLabel5_RT.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 598);
            this.Controls.Add(this.linkLabel5_RT);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkedListBox1_DevicesList);
            this.Controls.Add(this.linkLabel0_30m);
            this.Controls.Add(this.linkLabel4_1m);
            this.Controls.Add(this.linkLabel3_1w);
            this.Controls.Add(this.linkLabel2_24H);
            this.Controls.Add(this.linkLabel1_1H);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radioButton5);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.button_show);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.datePicker2_end);
            this.Controls.Add(this.datePicker1_start);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "메인화면";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button_show;
        internal System.Windows.Forms.DateTimePicker datePicker2_end;
        internal System.Windows.Forms.DateTimePicker datePicker1_start;
        internal System.Windows.Forms.Timer timer1;
        internal System.Windows.Forms.RadioButton radioButton2;
        internal System.Windows.Forms.RadioButton radioButton3;
        internal System.Windows.Forms.RadioButton radioButton4;
        internal System.Windows.Forms.RadioButton radioButton5;
        public System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1_1H;
        private System.Windows.Forms.LinkLabel linkLabel2_24H;
        private System.Windows.Forms.LinkLabel linkLabel3_1w;
        private System.Windows.Forms.LinkLabel linkLabel4_1m;
        private System.Windows.Forms.CheckedListBox checkedListBox1_DevicesList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel0_30m;
        private System.Windows.Forms.LinkLabel linkLabel5_RT;
    }
}

