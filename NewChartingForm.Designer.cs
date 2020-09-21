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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2_endTime = new System.Windows.Forms.TextBox();
            this.textBox1_startTime = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Gulim", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(520, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 18);
            this.label3.TabIndex = 9;
            this.label3.Text = "까지";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Gulim", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(321, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "부터";
            // 
            // textBox2_endTime
            // 
            this.textBox2_endTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2_endTime.Font = new System.Drawing.Font("Gulim", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox2_endTime.Location = new System.Drawing.Point(378, 9);
            this.textBox2_endTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox2_endTime.Name = "textBox2_endTime";
            this.textBox2_endTime.ReadOnly = true;
            this.textBox2_endTime.Size = new System.Drawing.Size(132, 20);
            this.textBox2_endTime.TabIndex = 7;
            this.textBox2_endTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox1_startTime
            // 
            this.textBox1_startTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1_startTime.Font = new System.Drawing.Font("Gulim", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox1_startTime.Location = new System.Drawing.Point(179, 9);
            this.textBox1_startTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox1_startTime.Name = "textBox1_startTime";
            this.textBox1_startTime.ReadOnly = true;
            this.textBox1_startTime.Size = new System.Drawing.Size(132, 20);
            this.textBox1_startTime.TabIndex = 6;
            this.textBox1_startTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gulim", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "선택한 간격 시간: ";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // NewChartingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 637);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2_endTime);
            this.Controls.Add(this.textBox1_startTime);
            this.Controls.Add(this.label1);
            this.Name = "NewChartingForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2_endTime;
        private System.Windows.Forms.TextBox textBox1_startTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
    }
}