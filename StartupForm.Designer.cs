
namespace DataVisualizerApp
{
    partial class StartupForm
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
            this.label_Title_startup = new System.Windows.Forms.Label();
            this.label_title_ver_startup = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_Title_startup
            // 
            this.label_Title_startup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Title_startup.AutoSize = true;
            this.label_Title_startup.Font = new System.Drawing.Font("Gulim", 15F, System.Drawing.FontStyle.Bold);
            this.label_Title_startup.Location = new System.Drawing.Point(46, 196);
            this.label_Title_startup.Name = "label_Title_startup";
            this.label_Title_startup.Size = new System.Drawing.Size(374, 20);
            this.label_Title_startup.TabIndex = 3;
            this.label_Title_startup.Text = "Clean Room T.H.P. Monitoring System";
            this.label_Title_startup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_title_ver_startup
            // 
            this.label_title_ver_startup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_title_ver_startup.AutoSize = true;
            this.label_title_ver_startup.Font = new System.Drawing.Font("Gulim", 8F, System.Drawing.FontStyle.Bold);
            this.label_title_ver_startup.Location = new System.Drawing.Point(555, 204);
            this.label_title_ver_startup.Name = "label_title_ver_startup";
            this.label_title_ver_startup.Size = new System.Drawing.Size(53, 11);
            this.label_title_ver_startup.TabIndex = 5;
            this.label_title_ver_startup.Text = "ver. 0.9";
            this.label_title_ver_startup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label_title_ver_startup);
            this.Controls.Add(this.label_Title_startup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StartupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StartupForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Title_startup;
        private System.Windows.Forms.Label label_title_ver_startup;
    }
}