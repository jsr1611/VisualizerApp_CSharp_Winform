namespace VisualizerApp_3
{
    partial class NewChartingForm
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
            this.label3_upto = new System.Windows.Forms.Label();
            this.label2_from = new System.Windows.Forms.Label();
            this.textBox2_endTime = new System.Windows.Forms.TextBox();
            this.textBox1_startTime = new System.Windows.Forms.TextBox();
            this.label1_selectedTimeInter = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3_upto
            // 
            this.label3_upto.AutoSize = true;
            this.label3_upto.Font = new System.Drawing.Font("Gulim", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3_upto.Location = new System.Drawing.Point(539, 9);
            this.label3_upto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3_upto.Name = "label3_upto";
            this.label3_upto.Size = new System.Drawing.Size(44, 18);
            this.label3_upto.TabIndex = 19;
            this.label3_upto.Text = "까지";
            // 
            // label2_from
            // 
            this.label2_from.AutoSize = true;
            this.label2_from.Font = new System.Drawing.Font("Gulim", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2_from.Location = new System.Drawing.Point(340, 9);
            this.label2_from.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2_from.Name = "label2_from";
            this.label2_from.Size = new System.Drawing.Size(44, 18);
            this.label2_from.TabIndex = 18;
            this.label2_from.Text = "부터";
            // 
            // textBox2_endTime
            // 
            this.textBox2_endTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2_endTime.Font = new System.Drawing.Font("Gulim", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox2_endTime.Location = new System.Drawing.Point(397, 9);
            this.textBox2_endTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox2_endTime.Name = "textBox2_endTime";
            this.textBox2_endTime.ReadOnly = true;
            this.textBox2_endTime.Size = new System.Drawing.Size(132, 20);
            this.textBox2_endTime.TabIndex = 17;
            this.textBox2_endTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox1_startTime
            // 
            this.textBox1_startTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1_startTime.Font = new System.Drawing.Font("Gulim", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox1_startTime.Location = new System.Drawing.Point(198, 9);
            this.textBox1_startTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox1_startTime.Name = "textBox1_startTime";
            this.textBox1_startTime.ReadOnly = true;
            this.textBox1_startTime.Size = new System.Drawing.Size(132, 20);
            this.textBox1_startTime.TabIndex = 16;
            this.textBox1_startTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1_selectedTimeInter
            // 
            this.label1_selectedTimeInter.AutoSize = true;
            this.label1_selectedTimeInter.Font = new System.Drawing.Font("Gulim", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1_selectedTimeInter.Location = new System.Drawing.Point(32, 9);
            this.label1_selectedTimeInter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1_selectedTimeInter.Name = "label1_selectedTimeInter";
            this.label1_selectedTimeInter.Size = new System.Drawing.Size(169, 18);
            this.label1_selectedTimeInter.TabIndex = 15;
            this.label1_selectedTimeInter.Text = "선택한 간격 시간: ";
            // 
            // NewChartingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 637);
            this.Controls.Add(this.label3_upto);
            this.Controls.Add(this.label2_from);
            this.Controls.Add(this.textBox2_endTime);
            this.Controls.Add(this.textBox1_startTime);
            this.Controls.Add(this.label1_selectedTimeInter);
            this.Name = "NewChartingForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3_upto;
        private System.Windows.Forms.Label label2_from;
        private System.Windows.Forms.TextBox textBox2_endTime;
        private System.Windows.Forms.TextBox textBox1_startTime;
        private System.Windows.Forms.Label label1_selectedTimeInter;
    }
}