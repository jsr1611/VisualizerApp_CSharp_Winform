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
            this.panel1_header = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.datePicker2_end = new System.Windows.Forms.DateTimePicker();
            this.datePicker1_start = new System.Windows.Forms.DateTimePicker();
            this.label1_from = new System.Windows.Forms.Label();
            this.label2_end = new System.Windows.Forms.Label();
            this.button_show = new System.Windows.Forms.Button();
            this.panel2_menu = new System.Windows.Forms.Panel();
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
            this.panel1_header.SuspendLayout();
            this.panel2_menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1_header
            // 
            this.panel1_header.BackColor = System.Drawing.Color.Transparent;
            this.panel1_header.Controls.Add(this.label1);
            this.panel1_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1_header.Location = new System.Drawing.Point(0, 0);
            this.panel1_header.Name = "panel1_header";
            this.panel1_header.Size = new System.Drawing.Size(402, 36);
            this.panel1_header.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Gulim", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(33, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "클린 룸 모니터링 컨트롤러";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // datePicker2_end
            // 
            this.datePicker2_end.CalendarFont = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker2_end.CustomFormat = "";
            this.datePicker2_end.Font = new System.Drawing.Font("Gulim", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker2_end.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker2_end.Location = new System.Drawing.Point(214, 72);
            this.datePicker2_end.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.datePicker2_end.Name = "datePicker2_end";
            this.datePicker2_end.Size = new System.Drawing.Size(147, 23);
            this.datePicker2_end.TabIndex = 40;
            this.datePicker2_end.Visible = false;
            // 
            // datePicker1_start
            // 
            this.datePicker1_start.CalendarFont = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker1_start.CustomFormat = "";
            this.datePicker1_start.Font = new System.Drawing.Font("Gulim", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.datePicker1_start.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker1_start.Location = new System.Drawing.Point(25, 72);
            this.datePicker1_start.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.datePicker1_start.Name = "datePicker1_start";
            this.datePicker1_start.Size = new System.Drawing.Size(152, 23);
            this.datePicker1_start.TabIndex = 41;
            this.datePicker1_start.Visible = false;
            // 
            // label1_from
            // 
            this.label1_from.AutoSize = true;
            this.label1_from.Font = new System.Drawing.Font("Gulim", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1_from.Location = new System.Drawing.Point(179, 77);
            this.label1_from.Name = "label1_from";
            this.label1_from.Size = new System.Drawing.Size(35, 14);
            this.label1_from.TabIndex = 42;
            this.label1_from.Text = "부터";
            this.label1_from.Visible = false;
            // 
            // label2_end
            // 
            this.label2_end.AutoSize = true;
            this.label2_end.Font = new System.Drawing.Font("Gulim", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2_end.Location = new System.Drawing.Point(363, 77);
            this.label2_end.Name = "label2_end";
            this.label2_end.Size = new System.Drawing.Size(35, 14);
            this.label2_end.TabIndex = 43;
            this.label2_end.Text = "까지";
            this.label2_end.Visible = false;
            // 
            // button_show
            // 
            this.button_show.Font = new System.Drawing.Font("Gulim", 12F);
            this.button_show.Location = new System.Drawing.Point(114, 246);
            this.button_show.Name = "button_show";
            this.button_show.Size = new System.Drawing.Size(172, 40);
            this.button_show.TabIndex = 44;
            this.button_show.Text = "확인";
            this.button_show.UseVisualStyleBackColor = true;
            this.button_show.Click += new System.EventHandler(this.show_button_Click);
            // 
            // panel2_menu
            // 
            this.panel2_menu.BackColor = System.Drawing.Color.Transparent;
            this.panel2_menu.Controls.Add(this.button3_solbi4);
            this.panel2_menu.Controls.Add(this.button3_solbi3);
            this.panel2_menu.Controls.Add(this.button3_solbi2);
            this.panel2_menu.Controls.Add(this.button3_solbi1);
            this.panel2_menu.Controls.Add(this.button2_part05);
            this.panel2_menu.Controls.Add(this.button2_part03);
            this.panel2_menu.Controls.Add(this.button2_humid);
            this.panel2_menu.Controls.Add(this.button2_temp);
            this.panel2_menu.Controls.Add(this.button1_datepicker);
            this.panel2_menu.Controls.Add(this.button1_24h);
            this.panel2_menu.Controls.Add(this.button1_realtime);
            this.panel2_menu.Controls.Add(this.datePicker1_start);
            this.panel2_menu.Controls.Add(this.datePicker2_end);
            this.panel2_menu.Controls.Add(this.label1_from);
            this.panel2_menu.Controls.Add(this.label2_end);
            this.panel2_menu.Controls.Add(this.button_show);
            this.panel2_menu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2_menu.Location = new System.Drawing.Point(0, 36);
            this.panel2_menu.Name = "panel2_menu";
            this.panel2_menu.Size = new System.Drawing.Size(402, 318);
            this.panel2_menu.TabIndex = 62;
            // 
            // button3_solbi4
            // 
            this.button3_solbi4.Location = new System.Drawing.Point(292, 183);
            this.button3_solbi4.Name = "button3_solbi4";
            this.button3_solbi4.Size = new System.Drawing.Size(83, 35);
            this.button3_solbi4.TabIndex = 73;
            this.button3_solbi4.Text = "설비 4";
            this.button3_solbi4.UseVisualStyleBackColor = true;
            this.button3_solbi4.Click += new System.EventHandler(this.button10__solbi4_Click);
            // 
            // button3_solbi3
            // 
            this.button3_solbi3.Location = new System.Drawing.Point(203, 183);
            this.button3_solbi3.Name = "button3_solbi3";
            this.button3_solbi3.Size = new System.Drawing.Size(83, 35);
            this.button3_solbi3.TabIndex = 72;
            this.button3_solbi3.Text = "설비 3";
            this.button3_solbi3.UseVisualStyleBackColor = true;
            this.button3_solbi3.Click += new System.EventHandler(this.button9_solbi3_Click);
            // 
            // button3_solbi2
            // 
            this.button3_solbi2.Location = new System.Drawing.Point(114, 183);
            this.button3_solbi2.Name = "button3_solbi2";
            this.button3_solbi2.Size = new System.Drawing.Size(83, 35);
            this.button3_solbi2.TabIndex = 71;
            this.button3_solbi2.Text = "설비 2";
            this.button3_solbi2.UseVisualStyleBackColor = true;
            this.button3_solbi2.Click += new System.EventHandler(this.button8_solbi2_Click);
            // 
            // button3_solbi1
            // 
            this.button3_solbi1.Location = new System.Drawing.Point(25, 183);
            this.button3_solbi1.Name = "button3_solbi1";
            this.button3_solbi1.Size = new System.Drawing.Size(83, 35);
            this.button3_solbi1.TabIndex = 70;
            this.button3_solbi1.Text = "설비 1";
            this.button3_solbi1.UseVisualStyleBackColor = true;
            this.button3_solbi1.Click += new System.EventHandler(this.button7_solbi1_Click);
            // 
            // button2_part05
            // 
            this.button2_part05.Location = new System.Drawing.Point(292, 113);
            this.button2_part05.Name = "button2_part05";
            this.button2_part05.Size = new System.Drawing.Size(83, 35);
            this.button2_part05.TabIndex = 69;
            this.button2_part05.Text = "파티클(0.5)";
            this.button2_part05.UseVisualStyleBackColor = true;
            this.button2_part05.Click += new System.EventHandler(this.button6_part05_Click);
            // 
            // button2_part03
            // 
            this.button2_part03.Location = new System.Drawing.Point(203, 113);
            this.button2_part03.Name = "button2_part03";
            this.button2_part03.Size = new System.Drawing.Size(83, 35);
            this.button2_part03.TabIndex = 68;
            this.button2_part03.Text = "파티클(0.3)";
            this.button2_part03.UseVisualStyleBackColor = true;
            this.button2_part03.Click += new System.EventHandler(this.button6_part03_Click);
            // 
            // button2_humid
            // 
            this.button2_humid.Location = new System.Drawing.Point(114, 113);
            this.button2_humid.Name = "button2_humid";
            this.button2_humid.Size = new System.Drawing.Size(83, 35);
            this.button2_humid.TabIndex = 67;
            this.button2_humid.Text = "습도";
            this.button2_humid.UseVisualStyleBackColor = true;
            this.button2_humid.Click += new System.EventHandler(this.button5_humid_Click);
            // 
            // button2_temp
            // 
            this.button2_temp.Location = new System.Drawing.Point(25, 113);
            this.button2_temp.Name = "button2_temp";
            this.button2_temp.Size = new System.Drawing.Size(83, 35);
            this.button2_temp.TabIndex = 66;
            this.button2_temp.Text = "온도";
            this.button2_temp.UseVisualStyleBackColor = true;
            this.button2_temp.Click += new System.EventHandler(this.button4_temp_Click);
            // 
            // button1_datepicker
            // 
            this.button1_datepicker.Location = new System.Drawing.Point(249, 22);
            this.button1_datepicker.Name = "button1_datepicker";
            this.button1_datepicker.Size = new System.Drawing.Size(106, 35);
            this.button1_datepicker.TabIndex = 65;
            this.button1_datepicker.Text = "기간설정";
            this.button1_datepicker.UseVisualStyleBackColor = true;
            this.button1_datepicker.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1_24h
            // 
            this.button1_24h.Location = new System.Drawing.Point(137, 22);
            this.button1_24h.Name = "button1_24h";
            this.button1_24h.Size = new System.Drawing.Size(106, 35);
            this.button1_24h.TabIndex = 64;
            this.button1_24h.Text = "24시간";
            this.button1_24h.UseVisualStyleBackColor = true;
            this.button1_24h.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1_realtime
            // 
            this.button1_realtime.Location = new System.Drawing.Point(25, 22);
            this.button1_realtime.Name = "button1_realtime";
            this.button1_realtime.Size = new System.Drawing.Size(106, 35);
            this.button1_realtime.TabIndex = 63;
            this.button1_realtime.Text = "실시간";
            this.button1_realtime.UseVisualStyleBackColor = true;
            this.button1_realtime.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 354);
            this.Controls.Add(this.panel2_menu);
            this.Controls.Add(this.panel1_header);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "메인화면";
            this.panel1_header.ResumeLayout(false);
            this.panel1_header.PerformLayout();
            this.panel2_menu.ResumeLayout(false);
            this.panel2_menu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1_header;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label1_from;
        private System.Windows.Forms.Label label2_end;
        private System.Windows.Forms.Button button_show;
        internal System.Windows.Forms.DateTimePicker datePicker2_end;
        internal System.Windows.Forms.DateTimePicker datePicker1_start;
        public System.Windows.Forms.Panel panel2_menu;
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
    }
}

